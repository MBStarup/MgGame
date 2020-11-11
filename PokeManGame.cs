using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace PokeMan
{
    public class PokeManGame : Game
    {
        new public static GameServiceContainer Services;
        public GraphicsDeviceManager Graphics;
        private SpriteBatch _spriteBatch;
        public static Stack<Scene> Scenes = new Stack<Scene>();
        public static SpriteFont Font;
        public static Texture2D ButtonTexture;

        public static (int x, int y) SceenSize;

        public PokeManGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Services = base.Services;
        }

        protected override void Initialize()
        {
            SceenSize = (GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);

            // window size
            Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;

            Graphics.IsFullScreen = true;

#if DEBUG

            Graphics.IsFullScreen = false;
#endif
            Graphics.ApplyChanges();

            MediaPlayer.Volume = 0.1f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //mc.LoadAssets(this.Content, "MainChar.xml");

            // loads static game sprites
            Font = Content.Load<SpriteFont>("Assets/FontTextBox");
            ButtonTexture = Content.Load<Texture2D>("Assets/EmptyButton");


            // adds scenes to stack to preload them
            var a = new Area("Sprites.xml");
            Scenes.Push(a);
            Scenes.Push(new PickScene("Battle1.xml", a.Player));
            Scenes.Push(new StartMenuScene());

        }

      
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            Scenes.Peek().Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // draws the scene on top of stack
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            {
                Scenes.Peek().Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    //
    // picture bg https://chrystianadach.artstation.com/projects/XBzk4Y
    // soundtrack menu + overworld track 1: Bip bop https://soundcloud.com/joshuamclean17/sets/free-music-pack-5-chiptune-2
    // button from https://karwisch.itch.io/pxui-basic

}