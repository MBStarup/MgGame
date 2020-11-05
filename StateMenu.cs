//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Design.Serialization;
//using System.Text;

//namespace PokeMan
//{
//    public class StateMenu : State
//    {
//        private List<Component> _components;
//        public StateMenu(PokeManGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
//        {
//            var buttonTexture = content.Load<Texture2D>("Assets/EmptyButton");
//            var buttonFont = _content.Load<SpriteFont>("Assets/FontTextBox");

//            Vector2 startGameStateSize = buttonFont.MeasureString("Start Game (state)");
//            var startGameButton = new Button(buttonTexture, buttonFont)
//            {
//                Position = new Vector2((1920/2)-(startGameStateSize.X/2), 250),
//                Text = "Start Game (state)",
//            };

//            startGameButton.Click += StartGameButton_Click;

//            _components = new List<Component>()
//            {
//                startGameButton,
//            };
//        }

//        private void StartGameButton_Click(object sender, EventArgs e)
//        {
//            _game.ChangeState(new StatePick(_game, _graphicsDevice, _content));

//        }

//        public override void Draw( SpriteBatch spriteBatch)
//        {
//            // spriteBatch.Begin();

//            foreach (var component in _components)
//            {
//                component.Draw( spriteBatch);
//            }
//            //spriteBatch.End();
//        }

//        public override void Update()
//        {
//            foreach (var component in _components)
//            {
//                component.Update();
//            }

//        }
//    }

//}