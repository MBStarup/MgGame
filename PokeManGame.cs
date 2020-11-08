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

        public static Stack<Scene> Scenes = new Stack<Scene>();

        //private Player mc;
        private Camera cam;

        public static SpriteFont Font;
        public static Texture2D ButtonTexture;

       
      

        //private List<Component> _GameComponents;

        //private Scene _currentState;

        //private Scene _nextstate;

        // void ChangeState(Scene scene)
        //{
        //    _nextstate = scene;
        //}

        public PokeManGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Services = base.Services;

        }

        protected override void Initialize()
        {
            //mc = new Player((0, _graphics.PreferredBackBufferHeight));

            base.Initialize();

            // window size
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
          //  _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            cam = new Camera(_graphics);
            //cam.position = new Vector2(0, 0);

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //mc.LoadAssets(this.Content, "MainChar.xml");

            // loads static game sprites
            Font = Content.Load<SpriteFont>("Assets/FontTextBox");
            ButtonTexture = Content.Load<Texture2D>("Assets/EmptyButton");

            Scenes.Push(new Area( "Home.xml"));
            Scenes.Push(new PickScene( "Battle1.xml"));
            Scenes.Push(new StartMenuScene());

            // Creates a new button
            //var quitButton = new Button(ButtonTexture, Font, text: "quit", position: new Point(1700, 950));

            //quitButton.Click += QuitButton_Click;

            // adds buttons to list
            //_GameComponents = new List<Component>()
            //{
            //   quitButton,
            //};
            //_currentState = new StateMenu(this);
        }




        private void QuitButton_Click(object sender, System.EventArgs e)
        {
            Exit();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Scenes.Peek().Update();

            //_currentState.Update();

            //foreach (var component in _GameComponents)
            //    component.Update();

            //if (_nextstate != null)
            //{
            //    _currentState = _nextstate;
            //    _nextstate = null;
            //}

            cam.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            {
                Scenes.Peek().Draw(_spriteBatch, cam);

                //_currentState.Draw(_spriteBatch, cam);

                //foreach (IDisplayable displayable in toDraw)
                //{
                //    displayable.Draw(_spriteBatch, cam);
                //}

                //foreach (var component in _GameComponents)
                //    component.Draw(_spriteBatch, cam);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}