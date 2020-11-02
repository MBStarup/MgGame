using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokeMan
{
    internal class Scene : IDisplayable
    {
        public virtual float LoadAmount { get { return loadAmount; } } //should be atomic, so no need to lock https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables#atomicity-of-variable-references

        private float loadAmount;

        protected ContentManager Content = new ContentManager(PokeManGame.Services);

        public Scene()
        {
        }

        public async Task<IEnumerable<T>> LoadAssets<T>(IEnumerable<string> AssetPaths)
        {
            var count = AssetPaths.Count();
            T[] result = new T[count];
            void LoadAllAssets()
            {
                int i = 0;

                foreach (string path in AssetPaths)
                {
                    T a = Content.Load<T>(path);

                    result[i++] = a;

                    loadAmount = (float)i / (float)count; //Should be atomic, see earlier comment
                }
                loadAmount = 1;
                return;
            }
            await Task.Run(LoadAllAssets);
            return result;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Camera camera)
        {
        }
    }
}