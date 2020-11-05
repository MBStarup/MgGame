using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        private PokeMan firePokeman;
        private Rectangle firePos;
        private PokeMan waterPokeman;
        private Rectangle waterPos;
        private PokeMan grassPokeman;
        private Rectangle grassPos;
        private Texture2D Background;
        private Textbox textbox;
        private List<Component> _GameComponents;
        private SpriteBatch _spriteBatch;


        private SpriteFont font;

        public PickScene(string xmlPath)
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










            var pickFireButton = new Button(Content.Load<Texture2D>("Assets/EmptyButton"), font)
            {
                Position = new Vector2(400, 950),
                Text = "Pick Flamer",
            };
            // links the button to the code, (auto creates the method)
            pickFireButton.Click += PickFireButton_Click;

            var pickWaterButton = new Button(Content.Load<Texture2D>("Assets/EmptyButton"), font)
            {
                Position = new Vector2(400, 950),
                Text = "Pick Bubbly",
            };
            // links the button to the code, (auto creates the method)
            pickWaterButton.Click += PickWaterButton_Click;

            var pickGrassButton = new Button(Content.Load<Texture2D>("Assets/EmptyButton"), font)
            {
                Position = new Vector2(400, 950),
                Text = "Pick Leaflutter",
            };
            // links the button to the code, (auto creates the method)
            pickGrassButton.Click += PickGrassButton_Click;



            // adds buttons to list
            _GameComponents = new List<Component>()
            {
               
               pickFireButton
            };







            localLoadAmount = 1f;
        }

        private void PickGrassButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PickWaterButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PickFireButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //protected override void Update(GameTime gameTime)
        //{
        //    foreach (var component in _GameComponents)
        //        component.Update(gameTime);
        //}


        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (this.LoadAmount < 1)
            {
                spriteBatch.DrawString(font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
            }
            else
            {
                firePos = new Rectangle(camera.Width * 2 / 6, camera.Height * 2 / 3-600, 500, 500);
                waterPos = new Rectangle(camera.Width * 2 / 6+500, camera.Height * 2 / 3 - 600, 500, 500);

                grassPos = new Rectangle(camera.Width * 2 / 6-500, camera.Height * 2 / 3 - 600, 500, 500);



                spriteBatch.Draw(Background, new Rectangle(0, 0, camera.Width, camera.Height), Color.Black);

                spriteBatch.Draw(firePokeman.Sprite, firePos, Color.White);
               spriteBatch.Draw(waterPokeman.Sprite, waterPos, Color.White);
                spriteBatch.Draw(grassPokeman.Sprite, grassPos, Color.White);



                //foreach (var component in _GameComponents)
                //    component.Draw(gameTime, _spriteBatch);

            }
            base.Draw(spriteBatch, camera);

            


        }
    }
}



 