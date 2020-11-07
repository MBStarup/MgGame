using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    public abstract class State
    {
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected PokeManGame _game;

        // Constructor
        public State(PokeManGame game, ContentManager content)
        {
            _game = game;
           
            _content = content;
        }



        public abstract void Draw(SpriteBatch spriteBatch, Camera camera);




        public abstract void Update();



     




    }






}

