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

    public class Move
    {
        private ElementEnum element;
        private int power;

        public void DoMove(Character user, Character enemy)
        {
            enemy.TakeDmg((int)(power * GetElementMultiplier(user.Element, element, enemy.Element)));
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
    }
}