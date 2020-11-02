using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace PokeMan
{
    public class PokeManGame : Game
    {
        new public static GameServiceContainer Services;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D texture;
        private Player mc;
        private Area area;
        private Scene currentScene;
        private int currAnimIndex = 0;
        private Camera cam;

        public PokeManGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Services = base.Services;
        }

        protected override void Initialize()
        {
            mc = new Player((0, _graphics.PreferredBackBufferHeight));

            base.Initialize();

            cam = new Camera(_graphics);
            cam.position = new Vector2(0, 0);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            currentScene = area = new Area("Home.xml");
            mc.LoadAssets(this.Content, "MainChar.xml");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Right))
            {
                currAnimIndex = 1;
            }
            else
            {
                currAnimIndex = 0;
            }

            cam.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            currentScene.Draw(_spriteBatch, cam);

            _spriteBatch.Draw(mc.Animations[currAnimIndex], mc.Position, null, Color.White, 0f, new Vector2(((Texture2D)mc.Animations[currAnimIndex]).Width / 2, ((Texture2D)mc.Animations[currAnimIndex]).Height), 0.5f, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}