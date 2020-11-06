using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;

namespace PokeMan
{
    public class StateMenu : Scene
    {
        private List<Component> _components;
        private Texture2D background;

        private SpriteFont font;


        public StateMenu(PokeManGame game) : base(game)
        {
            background = Content.Load<Texture2D>("Assets/StartMenu/StartMenuBG");
            font = Content.Load<SpriteFont>("Assets/FontTextBox");

            var buttonTexture = Content.Load<Texture2D>("Assets/EmptyButton");
            var buttonFont = Content.Load<SpriteFont>("Assets/FontTextBox");

            //Vector2 startGameStateSize = buttonFont.MeasureString("Start Game (state)");
            //var startGameButton = new Button(buttonTexture, buttonFont)
            //{
            //    Position = new Vector2((1920 / 2) - (startGameStateSize.X / 2), 250),
            //    Text = "Start Game (state)",
            //};

            var startBattleButton = new Button(buttonTexture, buttonFont, text: "Start Battle (state)", position: new Point(400, 800));
            var startGameButton = new Button(buttonTexture, buttonFont, text: "Start Game (state)", position: new Point(100, 800));

            startGameButton.Click += StartGameButton_Click;
            startBattleButton.Click += StartBattleButton_Click;

            _components = new List<Component>()
            {
                startBattleButton,
                startGameButton
            };
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new StatePickPokeMan(_game));

        }

        private void StartBattleButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new StatePick(_game));

        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            // spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

            spriteBatch.DrawString(font, "Welcome to the world of PokeMan!", new Vector2(50, 50), Color.White);

            foreach (var component in _components)
            {
                component.Draw(spriteBatch, camera);
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