using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    public class StatePick : State
    {
        

        private List<Component> _components;
        private Texture2D background;


        public StatePick(PokeManGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = content.Load<Texture2D>("Assets/EmptyButton");
            var buttonFont = _content.Load<SpriteFont>("Assets/FontTextBox");

            background = content.Load<Texture2D>("Assets/Battle/Background/1");

            var startGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "Return to Menu",
            };

            startGameButton.Click += StartGameButton_Click;

            _components = new List<Component>()
            {
                startGameButton,

            };



        }




        private void StartGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new StateMenu(_game, _graphicsDevice, _content));

        }

        public override void Draw( SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.1f);


            foreach (var component in _components)
            {
                component.Draw( spriteBatch);
            }



            //spriteBatch.End();
        }



        public override void Update()
        {

            foreach (var component in _components)
            {
                component.Update();
            }
        }
    }
}
