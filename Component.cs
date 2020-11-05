using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    public abstract class Component : IDisplayable, IUpdatable
    {
        public abstract void Draw(SpriteBatch spriteBatch, Camera camera);

        public abstract void Update();
    }
}