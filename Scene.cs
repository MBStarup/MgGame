using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PokeMan
{
    internal class Scene : IDisplayable
    {
        public virtual float LoadAmount { get { return loadAmount; } } //should be atomic, so no need to lock https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables#atomicity-of-variable-references

        private float loadAmount;

        protected ContentManager Content = new ContentManager(PokeManGame.Services);

        public Texture2D Texture => throw new NotImplementedException();

        public Scene()
        {
        }

        public IEnumerable<T> LoadAssets<T>(IEnumerable<string> AssetPaths)
        {
            var count = AssetPaths.Count();
            T[] result = new T[count];

            void LoadAllAssets()
            {
                lock (AssetPaths) //kinda redundant lock, since AssetPath only exists in this constructor
                {
                    int i = 0;

                    foreach (string path in AssetPaths)
                    {
                        T a = Content.Load<T>(path);

                        result[i++] = a;

                        loadAmount = (float)i / (float)count; //Should be atomic, see earlier comment
                    }
                    loadAmount = 1;
                }
            }

            new Thread(new ThreadStart(LoadAllAssets)).Start();

            while (loadAmount < 1) { }

            return result;
        }
    }
}