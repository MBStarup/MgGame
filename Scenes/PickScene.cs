using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace PokeMan
{
    internal class PickScene : Scene
    {
        new public float LoadAmount { get => base.LoadAmount * localLoadAmount; }

        private float localLoadAmount;

        private Texture2D background;
        private Texture2D buttonTexture = PokeManGame.ButtonTexture;
        private SpriteFont font = PokeManGame.Font;

        private static readonly PokeMan[] choices = new PokeMan[]
        {
            new PokeMan(1, 2),
            new PokeMan(2, 2),
            new PokeMan(3, 2),
            new PokeMan(3, 2)
        };

        private Button[] buttons = new Button[choices.Length];

        public PickScene(PokeManGame game, string xmlPath) : base(game)
        {
            Content.RootDirectory = "Content";
            LoadContent(xmlPath);
        }

        public async void LoadContent(string xmlPath)
        {
            //font = Content.Load<SpriteFont>("Assets/FontTextBox");
            //buttonTexture = Content.Load<Texture2D>("Assets/EmptyButton");

            //Loads Background sprites based off xml doc
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/" + xmlPath);
            var node = doc.DocumentElement.SelectSingleNode("/Battle");
            var path = node.Attributes["path"].Value;

            int count = node.ChildNodes.Count;
            string[] paths = new string[count];

            int i = 0;
            foreach (XmlNode sprite in node)
            {
                paths[i++] = ($"{path}{sprite.InnerText}");
            }
            IEnumerable<Texture2D> LoadedTextures = await LoadAssets<Texture2D>(paths);
            var arr = LoadedTextures.ToArray();
            background = arr[0];

            //Load pokeman assets
            count = choices.Length;
            i = 0;
            foreach (PokeMan pokeMan in choices)
            {
                doc.Load("../../../Content/Xml/PocketMan.xml");
                node = doc.DocumentElement.SelectSingleNode("/PokeMans");

                node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == pokeMan.id).SelectSingleNode("Animations");
                node = node.Cast<XmlNode>().First(a => a.Attributes["name"].Value == "Front");

                count = node.ChildNodes.Count;
                paths = new string[count];

                path = node.Attributes["path"].Value;
                i = 0;
                foreach (XmlNode frame in node)
                {
                    paths[i++] = ($"{path}{frame.InnerText}");
                }

                LoadedTextures = await LoadAssets<Texture2D>(paths);

                pokeMan.Sprite = LoadedTextures.ToArray();

                localLoadAmount = ++i / count;
            }

            //make buttons

            i = 0;
            foreach (PokeMan pokeMan in choices)
            {
                var button = new Button(buttonTexture, font, spriteColor: Color.Transparent, penColour: Color.Green);
                button.Click += OnPokemonClick;
                buttons[i++] = button;
            }

            var testButton = new Button(buttonTexture, font, text: "Test", position: new Point(1500, 700));

            testButton.Click += TestButton_Click;

            localLoadAmount = 1f;
        }

        private void OnPokemonClick(object sender, EventArgs e)
        {
            PokeMan.playerPokemen.Add(new PokeMan(1, 5));
            PokeManGame.Scenes.Pop();
        }

        private void TestButton_Click(object sender, System.EventArgs e)
        {
            PokeManGame.Scenes.Pop();
        }

        public override void Update()
        {
            foreach (Button button in buttons)
                button.Update();
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (this.LoadAmount < 1)
            {
                spriteBatch.DrawString(font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
            }
            else
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, camera.Width, camera.Height), Color.Green);

                float buttonSpaceRatio = 0.5f / 1;
                float buttonWidth = camera.Width / (buttons.Length * (buttonSpaceRatio + 1));

                int i = 0;
                foreach (Button b in buttons)
                {
                    var rec = new Rectangle((int)((i * (buttonSpaceRatio + 1) * buttonWidth) + ((buttonSpaceRatio * buttonWidth) / 2)), (int)(buttonWidth / 2), (int)buttonWidth, (int)buttonWidth);
                    b.Rectangle = rec;
                    spriteBatch.Draw(choices[i].Sprite, rec, Color.White);
                    i++;
                    b.Draw(spriteBatch, camera);
                }
            }
            base.Draw(spriteBatch, camera);
        }
    }
}