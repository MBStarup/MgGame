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

        //State
        private State _currentState;

        private State _nextstate;

        public void ChangeState(State state)
        {
            _nextstate = state;
        }

        //

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


            currentScene = new SceneStartMenu("Battle1.xml");
            //currentScene = new Area("Home.xml");

            mc.LoadAssets(this.Content, "MainChar.xml");

            // loads font FontTextBox
            font = Content.Load<SpriteFont>("Assets/FontTextBox");
            Texture2D buttonTexture = Content.Load<Texture2D>("Assets/EmptyButton");

            // Creates a new button
            Button testButton = new Button(buttonTexture, font, text: "Test", position: new Point(200, 950));

            // links the button to the code, (auto creates the method)
            testButton.Click += TestButton_Click;
            // example example.Click +=

            // Creates a new button
            var quitButton = new Button(buttonTexture, font, text: "quit", position: new Point(1700, 950));

            quitButton.Click += QuitButton_Click;

            var startGameButton = new Button(buttonTexture, font, text: "Start Game(scene)", position: new Point(400, 950));

            // links the button to the code, (auto creates the method)
            startGameButton.Click += StartGameButton_Click;

            // adds buttons to list
            _GameComponents = new List<Component>()
            {
               testButton,
               quitButton,
               startGameButton
            };
            //State
            //_currentState = new StateMenu(this, _graphics.GraphicsDevice, Content);
            //
        }

        private void StartGameButton_Click(object sender, System.EventArgs e)
        {
            //toDraw.Remove(currentScene);
            currentScene = new PickScene("Battle1.xml");
            //toDraw.Add(currentScene);
        }

        private void QuitButton_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

        private void TestButton_Click(object sender, System.EventArgs e)
        {
            //toDraw.Remove(currentScene);
            currentScene = new Battle("Battle1.xml");
            //toDraw.Add(currentScene);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState state = Keyboard.GetState();

            currentScene.Update();

            if (state.IsKeyDown(Keys.Right))
            {
                currAnimIndex = 1;
            }
            else
            {
                currAnimIndex = 0;
            }

            foreach (var component in _GameComponents)
                component.Update();

            ////State
            //_currentState.Update();

            //if (_nextstate != null)
            //{
            //    _currentState = _nextstate;
            //    _nextstate = null;
            //}
            //

            cam.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            {

                currentScene.Draw(_spriteBatch, cam);
                _spriteBatch.Draw(mc.Animations[currAnimIndex], mc.Position, null, Color.White, 0f, new Vector2(((Texture2D)mc.Animations[currAnimIndex]).Width / 2, ((Texture2D)mc.Animations[currAnimIndex]).Height), 0.5f, SpriteEffects.None, 0f);

                //foreach (IDisplayable displayable in toDraw)
                //{
                //    displayable.Draw(_spriteBatch, cam);
                //}

                

                _spriteBatch.DrawString(font, "Welcome", new Vector2(50, 50), Color.White);

                foreach (var component in _GameComponents)
                    component.Draw(_spriteBatch, cam);
                //State
                //_currentState.Draw( _spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}