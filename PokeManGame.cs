using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
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
        private Scene currentScene;
        private int currAnimIndex = 0;
        private Camera cam;

        private List<IDisplayable> toDraw = new List<IDisplayable>();

        private SpriteFont font;

        private List<Component> _GameComponents;

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

            // window size
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            //_graphics.IsFullScreen = true;

            toDraw.Add(currentScene);

            cam = new Camera(_graphics);
            cam.position = new Vector2(0, 0);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            currentScene = new Area("Home.xml");
            mc.LoadAssets(this.Content, "MainChar.xml");

            // loads font FontTextBox
            font = Content.Load<SpriteFont>("Assets/FontTextBox");

            // Creates a new button
            var testButton = new Button(Content.Load<Texture2D>("Assets/EmptyButton"), font)
            {
                Position = new Vector2(200, 950),
                Text = "Test",
            };
            // links the button to the code, (auto creates the method)
            testButton.Click += RandomButton_Click;
            // example example.Click +=

            // Creates a new button
            var quitButton = new Button(Content.Load<Texture2D>("Assets/EmptyButton"), font)
            {
                Position = new Vector2(1700, 950),
                Text = "quit",
            };

            quitButton.Click += QuitButton_Click;

            // adds buttons to list
            _GameComponents = new List<Component>()
            {
               testButton,
               quitButton
            };
        }

        private void QuitButton_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

        private void RandomButton_Click(object sender, System.EventArgs e)
        {
            toDraw.Remove(currentScene);
            currentScene = new Battle("Battle1.xml");
            toDraw.Add(currentScene);
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

            foreach (var component in _GameComponents)
                component.Update(gameTime);

            cam.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            _spriteBatch.Draw(mc.Animations[currAnimIndex], mc.Position, null, Color.White, 0f, new Vector2(((Texture2D)mc.Animations[currAnimIndex]).Width / 2, ((Texture2D)mc.Animations[currAnimIndex]).Height), 0.5f, SpriteEffects.None, 0f);

            foreach (IDisplayable displayable in toDraw)
            {
                displayable.Draw(_spriteBatch, cam);
            }

            //
            _spriteBatch.DrawString(font, "Welcome", new Vector2(50, 50), Color.White);

            foreach (var component in _GameComponents)
                component.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}