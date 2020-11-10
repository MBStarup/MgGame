using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;

namespace PokeMan
{
    public class StartMenuScene : Scene
    {
        private List<Component> _components;

        private Texture2D background;
        private Texture2D buttonTexture;
        private SpriteFont font;

        public StartMenuScene()
        {
            background = Content.Load<Texture2D>("Assets/StartMenu/StartMenuBG");
            font = PokeManGame.Font;
            buttonTexture = PokeManGame.ButtonTexture;

            //Vector2 startGameStateSize = buttonFont.MeasureString("Start Game (state)");
            //var startGameButton = new Button(buttonTexture, buttonFont)
            //{
            //    Position = new Vector2((1920 / 2) - (startGameStateSize.X / 2), 250),
            //    Text = "Start Game (state)",
            //};

            var startGameButton = new Button(buttonTexture, font, text: "Start Game (state)", position: new Point(100, 800), width: 250);
            startGameButton.Click += StartGameButton_Click;

            _components = new List<Component>()
            {
                startGameButton
            };
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            PokeManGame.Scenes.Pop();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

            spriteBatch.DrawString(font, "Welcome to the world of PokeMan!", new Vector2(50, 50), Color.White);

            foreach (var component in _components)
            {
                component.Draw(spriteBatch);
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