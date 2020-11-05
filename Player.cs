using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace PokeMan
{
    public class Player : Component
    {
        public SpriteAnimation[] Animations;
        public int AnimationIndex;
        private PlayerData data = new PlayerData();
        public Vector2 Position { get => new Vector2(data.Position.x, data.Position.y); }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(Animations[AnimationIndex], camera.WorldToScreen(Position), Color.White);
        }

        public Player((int x, int y) position)
        {
            data.Position = position;
        }

        public void LoadAssets(ContentManager contMan, string xmlPath)
        {
            //Loads animations sprites based off xml doc
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/" + xmlPath);

            data.Name = doc.SelectSingleNode("/Character")?.Attributes["name"].Value;

            var node = doc.DocumentElement.SelectSingleNode("/Character/Animations");

            Animations = new SpriteAnimation[node.ChildNodes.Count];

            int i = 0;
            foreach (XmlNode n in node.SelectNodes("Animation"))
            {
                int j = 0;
                var t = new Texture2D[n.ChildNodes.Count];

                foreach (XmlNode m in n.SelectNodes("Frame"))
                {
                    t[j++] = contMan.Load<Texture2D>($"{n.Attributes["path"].Value}{m.InnerText}");
                }
                Animations[i++] = t;
            }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class PlayerData
    {
        public (int x, int y) Position;
        public string Name;
        public PokeMan[] Party;
    }
}