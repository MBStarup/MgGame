using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Xml;

namespace PokeMan
{
    public class StatePick : Scene
    {
        new public float LoadAmount { get => base.LoadAmount * localLoadAmount; }

        private float localLoadAmount;
        private PokeMan firePokeman;
        private Rectangle firePos;
        private PokeMan waterPokeman;
        private Rectangle waterPos;
        private PokeMan grassPokeman;
        private Rectangle grassPos;
        private Texture2D Background;
        private List<Component> _GameComponents = new List<Component>();
        private SpriteBatch _spriteBatch;

        private Button pickFireButton;
        private Button pickWaterButton;
        private Button pickGrassButton;

        private SpriteFont font;

        private PokeMan selectedPokeman;


        public StatePick(PokeManGame game, string xmlPath) : base(game)
        {

            firePokeman = new PokeMan();
            firePokeman.id = 2;

            waterPokeman = new PokeMan();
            waterPokeman.id = 3;

            grassPokeman = new PokeMan();
            grassPokeman.id = 1;

            Content.RootDirectory = "Content";
            font = Content.Load<SpriteFont>("Assets/FontTextBox");
            LoadContent(xmlPath);

        }

        public async void LoadContent(string xmlPath)
        {
            font = Content.Load<SpriteFont>("Assets/FontTextBox");
            Texture2D buttonTexture = Content.Load<Texture2D>("Assets/EmptyButton");

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
            Background = arr[0];

            localLoadAmount = 1f / 3;

            //firePokeman
            doc.Load("../../../Content/Xml/PocketMan.xml");
            node = doc.DocumentElement.SelectSingleNode("/PokeMans");

            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == firePokeman.id).SelectSingleNode("Animations");
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
            firePokeman.Sprite = LoadedTextures.ToArray();

            localLoadAmount = 2f / 3;

            //waterPokeman

            node = doc.DocumentElement.SelectSingleNode("/PokeMans");

            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == waterPokeman.id).SelectSingleNode("Animations");
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
            waterPokeman.Sprite = LoadedTextures.ToArray();

            //grassPokeman

            node = doc.DocumentElement.SelectSingleNode("/PokeMans");

            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == grassPokeman.id).SelectSingleNode("Animations");
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
            grassPokeman.Sprite = LoadedTextures.ToArray();

            Texture2D ButtonSprite = Content.Load<Texture2D>("Assets/EmptyButton");

            pickFireButton = new Button(ButtonSprite, font, "Pick Flamer", spriteColor: Color.Transparent, penColour: Color.Green);

            // links the button to the code, (auto creates the method)
            pickFireButton.Click += PickFireButton_Click;

            pickWaterButton = new Button(ButtonSprite, font, "Pick Bubbly", spriteColor: Color.Transparent, penColour: Color.Green);

            // links the button to the code, (auto creates the method)
            pickWaterButton.Click += PickWaterButton_Click;

            pickGrassButton = new Button(ButtonSprite, font, "Pick Leaflutter", spriteColor: Color.Transparent, penColour: Color.Green);

            // links the button to the code, (auto creates the method)
            pickGrassButton.Click += PickGrassButton_Click;

            var testButton = new Button(buttonTexture, font, text: "Test", position: new Point(1500, 700));

            testButton.Click += TestButton_Click;
            // adds buttons to list
            _GameComponents = new List<Component>()
            {
               pickFireButton,
               pickGrassButton,
               pickWaterButton,
               testButton,
            };

            localLoadAmount = 1f;
        }


        private void PickGrassButton_Click(object sender, EventArgs e)
        {
            //selectedPokeman = new PokeMan(1, 1);
            PokeMan.playerPokemen.Add(new PokeMan(1, 5));

            //throw new NotImplementedException();
            //Debug.WriteLine(selectedPokeman);
        }

        private void PickWaterButton_Click(object sender, EventArgs e)
        {
            //selectedPokeman = new PokeMan(3, 1);
            //throw new NotImplementedException();
            //Debug.WriteLine(selectedPokeman);
            PokeMan.playerPokemen.Add(new PokeMan(3, 5));
        }

        private void PickFireButton_Click(object sender, EventArgs e)
        {
            //selectedPokeman = new PokeMan(2, 1);
            //throw new NotImplementedException();
            //Debug.WriteLine(selectedPokeman);
            PokeMan.playerPokemen.Add(new PokeMan(2, 5));
        }

        private void TestButton_Click(object sender, System.EventArgs e)
        {
        }

        public override void Update()
        {

            foreach (var component in _GameComponents)
                component.Update();

        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {


            if (this.LoadAmount < 1)
            {
                _spriteBatch.DrawString(font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
            }
            else
            {
                firePos = new Rectangle(camera.Width * 2 / 6, camera.Height * 2 / 3 - 600, 500, 500);
                waterPos = new Rectangle(camera.Width * 2 / 6 + 500, camera.Height * 2 / 3 - 600, 500, 500);

                grassPos = new Rectangle(camera.Width * 2 / 6 - 500, camera.Height * 2 / 3 - 600, 500, 500);

                spriteBatch.Draw(Background, new Rectangle(0, 0, camera.Width, camera.Height), Color.Black);

                spriteBatch.Draw(firePokeman.Sprite, firePos, Color.White);
                spriteBatch.Draw(waterPokeman.Sprite, waterPos, Color.White);
                spriteBatch.Draw(grassPokeman.Sprite, grassPos, Color.White);

                pickFireButton.Rectangle = firePos;
                pickGrassButton.Rectangle = waterPos;
                pickWaterButton.Rectangle = grassPos;

                foreach (var component in _GameComponents)
                    component.Draw(spriteBatch, camera);
            }
            base.Draw(spriteBatch, camera);
        }

     
    }
}
