using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Xml;

namespace PokeMan
{
    public enum ElementEnum
    {
        Normal,
        Fire,
        Water,
        Grass
    }

    [Serializable]
    public class Move : IDisplayable
    {
        private ElementEnum element;
        private int power;

        public SpriteAnimation Animation;

        public void DoMove(PokeMan user, PokeMan enemy)
        {
            //enemy.TakeDmg((int)(power * GetElementMultiplier(user.Element, element, enemy.Element)));
            // https://bulbapedia.bulbagarden.net/wiki/Damage
            enemy.TakeDmg((int)(((((((2*user.lvl)/5)+2)*power*user.AttackStat/enemy.DefenceStat)/50)+2)* GetElementMultiplier(user.Element, element, enemy.Element)));
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

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(Animation, Vector2.Zero, Color.White);
        }
    }
}