using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    internal class Camera
    {
        private (int x, int y) position;

        public SpriteBatch SpriteBatch { get; }

        public Camera(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }

        public void Draw()
        {
            SpriteBatch.Begin();
            {
                //SpriteBatch.Draw();
            }
            SpriteBatch.End();
        }
    }
}