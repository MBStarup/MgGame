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

namespace Zel
{
    internal class Character
    {
        public SpriteAnimation[] Animations;

        public Vector2 Position;
        public Quaternion Rotation;
        private string name;

        public Character(Vector2 position)
        {
            Position = position;
        }

        public void LoadContent(ContentManager contMan, string xmlPath)
        {
            //Loads animations sprites based off xml doc
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/" + xmlPath);

            name = doc.SelectSingleNode("/Character")?.Attributes["name"].Value;

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
    }
}