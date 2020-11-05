using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    // auto creates the 2 main methods for subclass
    public abstract class Component
    {
        public abstract void Draw( SpriteBatch spriteBatch);



        public abstract void Update();

    }
}
