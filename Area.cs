using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PokeMan
{
    internal class Area : Scene
    {
        new public float LoadAmount;
        private SpriteAnimation[] Animations;

        public Area(string xmlPath)
        {
        }

        public async Task LoadContent(string xmlPath)
        {
            //Loads sprites based off xml doc
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/Areas/" + xmlPath);

            var node = doc.DocumentElement.SelectSingleNode("/Tiles");

            Animations = new SpriteAnimation[node.ChildNodes.Count];

            int i = 0;
            foreach (XmlNode n in node.SelectNodes("Animation"))
            {
                Task<SpriteAnimation> loading = LoadAnimation(n);
                Animations[i++] = await loading;
            }
        }

        private async Task<SpriteAnimation> LoadAnimation(XmlNode n)
        {
            int i = 0;
            string[] paths = new string[n.ChildNodes.Count];

            foreach (XmlNode m in n.SelectNodes("Frame"))
            {
                paths[i++] = ($"{n.Attributes["path"].Value}{m.InnerText}");
            }

            IEnumerable<Texture2D> a = await LoadAssets<Texture2D>(paths);
            return new SpriteAnimation(a.ToArray());
        }
    }
}