using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace PokeMan
{
    internal class PickScene : Scene
    {
        new public float LoadAmount { get => (float)completedLoadTasks / loadTasks + base.LoadAmount / loadTasks; }

        private int completedLoadTasks;
        private int loadTasks;

        private Texture2D background;
        private Texture2D buttonTexture = PokeManGame.ButtonTexture;
        private SpriteFont font = PokeManGame.Font;

        private static readonly PokeMan[] choices = new PokeMan[]
        {
            new PokeMan(1, 2),
            new PokeMan(2, 2),
            new PokeMan(3, 2)
        };

        private Button[] buttons = new Button[choices.Length];

        public PickScene(string xmlPath)
        {
            Content.RootDirectory = "Content";
            LoadContent(xmlPath);
        }

        public async void LoadContent(string xmlPath)
        {
            loadTasks = choices.Length;

            //Loads Background sprites based off xml doc
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/" + xmlPath);
            var node = doc.DocumentElement.SelectSingleNode("/Battle");
            var path = node.Attributes["path"].Value;

            int count = node.ChildNodes.Count;
            loadTasks += count;
            string[] paths = new string[count];

            int i = 0;
            foreach (XmlNode sprite in node)
            {
                paths[i++] = ($"{path}{sprite.InnerText}");
                completedLoadTasks += 1;
            }
            IEnumerable<Texture2D> LoadedTextures = await LoadAssets<Texture2D>(paths);
            var arr = LoadedTextures.ToArray();
            background = arr[0];

            //Load pokeman assets
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

                completedLoadTasks += 1;
            }

            //make buttons

            i = 0;
            foreach (PokeMan pokeMan in choices)
            {
                var button = new Button(buttonTexture, font, spriteColor: Color.Transparent, penColour: Color.Green);

                button.Click += (object o, EventArgs e) => { Area.p.party[0] = pokeMan; PokeManGame.Scenes.Pop(); };
                buttons[i++] = button;
            }

            completedLoadTasks = loadTasks;
        }

        public override void Update()
        {
            foreach (Button button in buttons)
                button.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.LoadAmount < 1)
            {
                spriteBatch.DrawString(font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
            }
            else
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, PokeManGame.SceenSize.x, PokeManGame.SceenSize.y), Color.Green);

                float buttonSpaceRatio = 0.5f / 1;
                float buttonWidth = PokeManGame.SceenSize.x / (buttons.Length * (buttonSpaceRatio + 1));

                int i = 0;
                foreach (Button b in buttons)
                {
                    var rec = new Rectangle((int)((i * (buttonSpaceRatio + 1) * buttonWidth) + ((buttonSpaceRatio * buttonWidth) / 2)), (int)(buttonWidth / 2), (int)buttonWidth, (int)buttonWidth);
                    b.Rectangle = rec;
                    spriteBatch.Draw(choices[i].Sprite, rec, Color.White);
                    i++;
                    b.Draw(spriteBatch);
                }
            }
            base.Draw(spriteBatch);
        }
    }
}