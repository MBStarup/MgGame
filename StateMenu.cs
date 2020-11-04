﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;

namespace PokeMan
{
    public class StateMenu : State
    {
        private List<Component> _components;
        public StateMenu(PokeManGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = content.Load<Texture2D>("Assets/EmptyButton");
            var buttonFont = _content.Load<SpriteFont>("Assets/FontTextBox");


            var startGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "Start Game (state)",
            };

            startGameButton.Click += StartGameButton_Click;

            _components = new List<Component>()
            {
                startGameButton,
            };
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new StatePick(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // spriteBatch.Begin();

            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            //spriteBatch.End();
        }



        public override void Update(GameTime gameTime)
        {

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }

        }
    }


}
