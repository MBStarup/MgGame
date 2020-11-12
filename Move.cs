using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Xml;
using EnumBuilder;
using Microsoft.Xna.Framework.Content;

namespace PokeMan
{
    [Serializable]
    public class Move : IDisplayable
    {
        public int LoadAmount = 0;

        private ElementEnum element;
        private string name;
        private int power;
        private int accuracy;
        private int id;

        public SpriteAnimation Animation;

        public string Name { get => name; private set => name = value; }
        public int Power { get => power; private set => power = value; }
        public int Accuracy { get => accuracy; private set => accuracy = value; }
        /// <summary>
        /// Creates a new move for use in battle, using the Moves.XML file
        /// </summary>
        /// <param name="id">ID: identifies the specific move that the user wants to use</param>
        public Move(int id)
        {
            this.id = id;
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/Moves.xml");
            var node = doc.DocumentElement.SelectSingleNode("/Moves");
            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == id);

            Name = node.Attributes["name"].Value;
            Power = int.Parse(node.Attributes["power"].Value);
            Accuracy = int.Parse(node.Attributes["accuracy"].Value);
            element = (ElementEnum)Enum.Parse(typeof(ElementEnum), node.Attributes["element"].Value);
        }

        public void LoadAssets(ContentManager contMan)
        {
            //Loads animation sprites based off xml doc
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/Moves.xml");

            var node = doc.DocumentElement.SelectSingleNode("/Moves");
            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == id);
            node = node.SelectSingleNode("Animation");

            int j = 0;
            var t = new Texture2D[node.ChildNodes.Count];

            foreach (XmlNode m in node.SelectNodes("Frame"))
            {
                t[j++] = contMan.Load<Texture2D>($"{node.Attributes["path"].Value}{m.InnerText}");
            }
            Animation = new SpriteAnimation(t, uint.Parse(node.Attributes["inverseSpeed"].Value));

            LoadAmount = 1;
        }

        /// <summary>
        /// Makes an attack against "enemy" pokeman with "user" pokeman
        /// Calc based on https://bulbapedia.bulbagarden.net/wiki/Damage
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enemy"></param>
        public void DoMove(PokeMan user, PokeMan enemy)
        {
            //enemy.TakeDmg((int)(power * GetElementMultiplier(user.Element, element, enemy.Element)));
            // https://bulbapedia.bulbagarden.net/wiki/Damage
            enemy.TakeDmg((int)(((((((2 * user.lvl) / 5) + 2) * Power * user.AttackStat / enemy.DefenceStat) / 50) + 2) * GetElementMultiplier(user.Element, element, enemy.Element)));
        }

        /// <summary>
        /// Calculates the dmg based on the users pokemans "attacking" stat, the "move" number and the "defending" pokemans stats
        /// </summary>
        /// <param name="attacking"></param>
        /// <param name="move"></param>
        /// <param name="defending"></param>
        /// <returns></returns>
        private static float GetElementMultiplier(ElementEnum attacking, ElementEnum move, ElementEnum defending)
        {
            float result = 1f;

            if (attacking == move)
                result *= 2f;

            //Loads weaknesses based off xml doc
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/Elements.xml");

            var node = doc.DocumentElement.SelectSingleNode("/Elements");

            foreach (XmlNode n in node.SelectNodes("Element"))
            {
                if ((ElementEnum)Enum.Parse(typeof(ElementEnum), n.Attributes["name"].Value, true) == defending)
                {
                    foreach (XmlNode m in n.SelectNodes("Weakness"))
                    {
                        if ((ElementEnum)Enum.Parse(typeof(ElementEnum), m.Attributes["name"].Value, true) == move)
                            result *= 2f;
                    }
                }

                if ((ElementEnum)Enum.Parse(typeof(ElementEnum), n.Attributes["name"].Value, true) == move)
                {
                    foreach (XmlNode m in n.SelectNodes("Weakness"))
                    {
                        if ((ElementEnum)Enum.Parse(typeof(ElementEnum), m.Attributes["name"].Value, true) == defending)
                            result *= 0.5f;
                    }
                }
            }

            return result;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Animation, Vector2.Zero, Color.White);
        }

        /// <summary>
        /// Stores this moves name as a string (to display in battle scene after a move)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}