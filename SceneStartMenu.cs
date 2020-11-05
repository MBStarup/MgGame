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
    internal class SceneStartMenu : Scene
    {
        new public float LoadAmount { get => base.LoadAmount * localLoadAmount; }
        private float localLoadAmount;
        private Texture2D Background;
        private Textbox textbox;
        private List<Component> _GameComponents = new List<Component>();
        private SpriteBatch _spriteBatch;

        private SpriteFont font;

        public SceneStartMenu(string xmlPath)
        {
           

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

           
            localLoadAmount = 2f / 3;

           


           

            LoadedTextures = await LoadAssets<Texture2D>(paths);
        


            // adds buttons to list
            _GameComponents = new List<Component>()
            {
              
            };

            localLoadAmount = 1f;
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
                spriteBatch.DrawString(font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
            }
            else
            {
              
                spriteBatch.Draw(Background, new Rectangle(0, 0, camera.Width, camera.Height), Color.Black);

              

                foreach (var component in _GameComponents)
                    component.Draw(spriteBatch, camera);
            }
            base.Draw(spriteBatch, camera);
        }
    }
}