using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Xml;
using EnumBuilder;

namespace PokeMan
{
    [Serializable]
    public class Move : IDisplayable
    {
        private ElementEnum element;
        private string name;
        private int power;
        private int accuracy;
        private XmlDocument doc = new XmlDocument();

        public SpriteAnimation Animation;

        public string Name { get => name; private set => name = value; }
        public int Power { get => power; private set => power = value; }
        public int Accuracy { get => accuracy; private set => accuracy = value; }

        public Move(int id)
        {
            doc.Load("../../../Content/Xml/Moves.xml");
            var node = doc.DocumentElement.SelectSingleNode("/Moves");
            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == id);

            Name = node.Attributes["name"].Value;
            Power = int.Parse(node.Attributes["power"].Value);
            Accuracy = int.Parse(node.Attributes["accuracy"].Value);
            element = (ElementEnum)Enum.Parse(typeof(ElementEnum), node.Attributes["element"].Value);
        }

        public void DoMove(PokeMan user, PokeMan enemy)
        {
            //enemy.TakeDmg((int)(power * GetElementMultiplier(user.Element, element, enemy.Element)));
            // https://bulbapedia.bulbagarden.net/wiki/Damage
            enemy.TakeDmg((int)(((((((2 * user.lvl) / 5) + 2) * Power * user.AttackStat / enemy.DefenceStat) / 50) + 2) * GetElementMultiplier(user.Element, element, enemy.Element)));
        }

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
    }
}