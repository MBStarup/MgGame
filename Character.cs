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
    public class Character
    {
        public SpriteAnimation[] Animations;

        public Vector2 Position;
        public Quaternion Rotation;
        private string name;

        private int health;

        public ElementEnum Element;

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

        public void Attack(Character enemy, Move move)
        {
            move.DoMove(this, enemy);
        }

        internal void TakeDmg(int dmg)
        {
            health = Math.Clamp(health - dmg, 0, Int32.MaxValue);

            if (health == 0)
                this.Die();
        }

        private void Die()
        {
            throw new NotImplementedException("Your PokeMan died!");
        }
    }
}