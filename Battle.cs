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
    internal class Battle : Scene
    {
        new public float LoadAmount { get => base.LoadAmount * localLoadAmount; }
        private float localLoadAmount;
        private PokeMan FriendlyPokeMan;
        private PokeMan EnemyPokeMan;
        private Texture2D Background;
        private Textbox textbox;

        private Rectangle Friendly;
        private Rectangle Enemy;

        private SpriteFont font;

        public Battle(string xmlPath)
        {
            FriendlyPokeMan = new PokeMan();
            FriendlyPokeMan.id = 1;

            EnemyPokeMan = new PokeMan();
            EnemyPokeMan.id = 2;

            Content.RootDirectory = "Content";
            font = Content.Load<SpriteFont>("Assets/FontTextBox");
            LoadContent(xmlPath);
        }

        public async void LoadContent(string xmlPath)
        {
            Thread.Sleep(0000);
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

            //frienly
            doc = new XmlDocument();
            doc.Load("../../../Content/Xml/PocketMan.xml");
            node = doc.DocumentElement.SelectSingleNode("/PokeMans");

            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == FriendlyPokeMan.id).SelectSingleNode("Animations");
            node = node.Cast<XmlNode>().First(a => a.Attributes["name"].Value == "Back");

            count = node.ChildNodes.Count;
            paths = new string[count];

            path = node.Attributes["path"].Value;
            i = 0;
            foreach (XmlNode frame in node)
            {
                paths[i++] = ($"{path}{frame.InnerText}");
            }

            LoadedTextures = await LoadAssets<Texture2D>(paths);
            FriendlyPokeMan.Sprite = LoadedTextures.ToArray();

            localLoadAmount = 2f / 3;

            //enmy
            doc = new XmlDocument();
            doc.Load("../../../Content/Xml/PocketMan.xml");
            node = doc.DocumentElement.SelectSingleNode("/PokeMans");

            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == EnemyPokeMan.id).SelectSingleNode("Animations");
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
            EnemyPokeMan.Sprite = LoadedTextures.ToArray();

            localLoadAmount = 1f;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (this.LoadAmount < 1)
            {
                spriteBatch.DrawString(font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
            }
            else
            {
                Friendly = new Rectangle(0, camera.Height * 2 / 3, camera.Width / 3, camera.Height / 3);
                Enemy = new Rectangle(camera.Width * 2 / 3, camera.Height, camera.Width / 3, camera.Height / 3);
                Enemy = new Rectangle(camera.Width * 2 / 3, 0, camera.Width / 3, camera.Height / 3);

                spriteBatch.Draw(Background, new Rectangle(0, 0, camera.Width, camera.Height), Color.Black);

                spriteBatch.Draw(FriendlyPokeMan.Sprite, Friendly, Color.White);
                spriteBatch.Draw(EnemyPokeMan.Sprite, Enemy, Color.White);
            }
            base.Draw(spriteBatch, camera);
        }
    }
}