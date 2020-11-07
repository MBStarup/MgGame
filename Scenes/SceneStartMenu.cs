//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Xml;

//namespace PokeMan
//{
//    internal class SceneStartMenu : Scene
//    {
//        new public float LoadAmount { get => base.LoadAmount * localLoadAmount; }
//        private float localLoadAmount;

//        private Scene currentScene;

//        private Texture2D Background;
//        private Textbox textbox;
//        private List<Component> _GameComponents = new List<Component>();
//        private SpriteBatch _spriteBatch;

//        private SpriteFont font;

//        public SceneStartMenu(string xmlPath)
//        {
//            Content.RootDirectory = "Content";
//            font = Content.Load<SpriteFont>("Assets/FontTextBox");
//            LoadContent(xmlPath);
//        }

//        public async void LoadContent(string xmlPath)
//        {
//            //Loads Background sprites based off xml doc
//            XmlDocument doc = new XmlDocument();
//            doc.Load("../../../Content/Xml/" + xmlPath);
//            var node = doc.DocumentElement.SelectSingleNode("/Battle");
//            var path = node.Attributes["path"].Value;

//            int count = node.ChildNodes.Count;
//            string[] paths = new string[count];

//            int i = 0;
//            foreach (XmlNode sprite in node)
//            {
//                paths[i++] = ($"{path}{sprite.InnerText}");
//            }
//            IEnumerable<Texture2D> LoadedTextures = await LoadAssets<Texture2D>(paths);
//            var arr = LoadedTextures.ToArray();
//            Background = arr[0];

//            localLoadAmount = 1f / 3;

//            localLoadAmount = 2f / 3;

//            LoadedTextures = await LoadAssets<Texture2D>(paths);

//            // loads font FontTextBox
//            font = Content.Load<SpriteFont>("Assets/FontTextBox");
//            Texture2D buttonTexture = Content.Load<Texture2D>("Assets/EmptyButton");

//            // Creates a new button
//            Button testButton = new Button(buttonTexture, font, text: "Test", position: new Point(200, 950));

//            // links the button to the code, (auto creates the method)
//            testButton.Click += TestButton_Click;
//            // example example.Click +=

//            var startGameButton = new Button(buttonTexture, font, text: "Start Game(scene)", position: new Point(400, 600));

//            // links the button to the code, (auto creates the method)
//            startGameButton.Click += StartGameButton_Click;

//            // adds buttons to list
//            _GameComponents = new List<Component>()
//            {
//               testButton,

//               startGameButton
//            };
//            //State
//            //_currentState = new StateMenu(this, _graphics.GraphicsDevice, Content);
//            //

//            localLoadAmount = 1f;
//        }

//        private void StartGameButton_Click(object sender, System.EventArgs e)
//        {
//            //toDraw.Remove(currentScene);
//            currentScene = new PickScene("Battle1.xml");
//            //toDraw.Add(currentScene);
//        }

//        private void TestButton_Click(object sender, System.EventArgs e)
//        {
//            //toDraw.Remove(currentScene);
//            currentScene = new Battle("Battle1.xml");
//            //toDraw.Add(currentScene);
//        }

//        public override void Update()
//        {
//            //currentScene.Update();
//            foreach (var component in _GameComponents)
//                component.Update();

//        }

//        public override void Draw(SpriteBatch spriteBatch, Camera camera)
//        {
//            if (this.LoadAmount < 1)
//            {
//                spriteBatch.DrawString(font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
//            }
//            else
//            {
//                spriteBatch.Draw(Background, new Rectangle(0, 0, camera.Width, camera.Height), Color.White);

//                //_spriteBatch.DrawString(font, "Welcome", new Vector2(50, 50), Color.White);

//                foreach (var component in _GameComponents)
//                    component.Draw(spriteBatch, camera);
//            }

//            base.Draw(spriteBatch, camera);
//        }
//    }
//}