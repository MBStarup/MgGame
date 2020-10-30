using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection.PortableExecutable;

namespace PokeMan
{
    public class PokeManGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D texture;
        private Character mc;
        private int currAnimIndex = 0;

        public PokeManGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mc = new Character(new Vector2(0f, _graphics.PreferredBackBufferHeight));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mc.LoadContent(this.Content, "MainChar.xml");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState state = Keyboard.GetState();

            // TODO: Add your update logic here
            if (state.IsKeyDown(Keys.Right))
            {
                currAnimIndex = 1;
            }
            else
            {
                currAnimIndex = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            _spriteBatch.Draw(mc.Animations[currAnimIndex], mc.Position, null, Color.White, 0f, new Vector2(((Texture2D)mc.Animations[currAnimIndex]).Width / 2, ((Texture2D)mc.Animations[currAnimIndex]).Height), 0.5f, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}