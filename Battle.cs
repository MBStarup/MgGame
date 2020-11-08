using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;

namespace PokeMan
{
    public class Battle : Scene
    {
        new public float LoadAmount { get => base.LoadAmount * localLoadAmount; }
        private float localLoadAmount;
        private PokeMan FriendlyPokeMan;
        private PokeMan EnemyPokeMan;
        private Texture2D Background;
        private Textbox textbox;

        private Rectangle Friendly;
        private Rectangle Enemy;

        private SpriteFont font;

        

        private List<Component> _components;
        private Texture2D background;

        private string type;
        public int pos = 1200;
        
        public int posenemy = 100;


        public Battle(string xmlPath)
        {

            FriendlyPokeMan = new PokeMan();
            FriendlyPokeMan.id = 1;

            EnemyPokeMan = new PokeMan();
            EnemyPokeMan.id = 2;

            Content.RootDirectory = "Content";
            font = Content.Load<SpriteFont>("Assets/FontTextBox");
            LoadContent(xmlPath);

            font = Content.Load<SpriteFont>("Assets/FontTextBox");

            FriendlyPokeMan = new PokeMan();
            FriendlyPokeMan.id = 1;

            var buttonTexture = Content.Load<Texture2D>("Assets/EmptyButton");
            var buttonFont = Content.Load<SpriteFont>("Assets/FontTextBox");

            background = Content.Load<Texture2D>("Assets/Battle/Background/1");

            Checktype();

            Button startGameButton = new Button(buttonTexture, font, text: "Return to Menu", position: new Point(300, 250));

            // De forskellige knapper som spilleren nok skal kunne bruge i kamp scenen

            Button fightButton = new Button(buttonTexture, font, text: "Fight", position: new Point(1000, 900));

            Button bagButton = new Button(buttonTexture, font, text: "Bag", position: new Point(1100, 900));

            Button pokemanButton = new Button(buttonTexture, font, text: "Pokeman", position: new Point(1000, 950));

            Button cowardButton = new Button(buttonTexture, font, text: "Run", position: new Point(1100, 950));

            startGameButton.Click += StartGameButton_Click;
            fightButton.Click += fightButton_Click;
            bagButton.Click += bagButton_Click;
            pokemanButton.Click += pokemanButton_Click;
            cowardButton.Click += cowardButton_Click;

            _components = new List<Component>()
            {
                startGameButton,
                fightButton,
                bagButton,
                pokemanButton,
                cowardButton,
            };

            void StartGameButton_Click(object sender, EventArgs e)
            {
                PokeManGame.Scenes.Pop();
            }
            // Knappen fight har andre knapper i sig når man klikker på den
            void fightButton_Click(object sender, EventArgs e)
            {
                Button move1Button = new Button(buttonTexture, font, text: "hak", position: new Point(1100, 550));

                Button move2Button = new Button(buttonTexture, font, text: "slash", position: new Point(1100, 600));

                Button move3Button = new Button(buttonTexture, font, text: "poke", position: new Point(1100, 650));

                Button moveSpecialButton = new Button(buttonTexture, font, text: (type), position: new Point(1100, 700));

                move1Button.Click += move1Button_Click;
                move2Button.Click += move2Button_Click;
                move3Button.Click += move3Button_Click;
                moveSpecialButton.Click += moveSpecialButton_Click;
                _components = new List<Component>()
            {
               move1Button,
               move2Button,
               move3Button,
               moveSpecialButton,
            };
                void move1Button_Click(object sender, EventArgs e)
                {
                }
                void move2Button_Click(object sender, EventArgs e)
                {
                }
                void move3Button_Click(object sender, EventArgs e)
                {
                }
                void moveSpecialButton_Click(object sender, EventArgs e)
                {
                    if (FriendlyPokeMan.id == 1)
                    {
                        Debug.WriteLine("Leaflutter is a" + type);
                    }
                }
            }

            void bagButton_Click(object sender, EventArgs e)
            {
            }
            void pokemanButton_Click(object sender, EventArgs e)
            {
            }
            void cowardButton_Click(object sender, EventArgs e)
            {
                // Tager spilleren tilbage til startmenuen, bare en placeholder
                // _game.ChangeState(new StateMenu(_game));
            }

            // Tjekker hvilken type pokeman man har, så pokemanens special attack passer til typen
            void Checktype()
            {
                if (FriendlyPokeMan.id == 1)
                {
                    type = "grass";
                }
                else if (FriendlyPokeMan.id == 2)
                {
                    type = "water";
                }
                else if (FriendlyPokeMan.id == 3)
                {
                    type = "fire";
                }
                else
                {
                    type = "normal";
                }
            }
        }


        public async void LoadContent(string xmlPath)
        {
            //Loads Background sprites based off xml doc
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Content/Xml/" + xmlPath);
            var node = doc.DocumentElement.SelectSingleNode("/Battle");
            var path = node.Attributes["path"].Value;

          



            int count = node.ChildNodes.Count;
            string[] paths = new string[count];

            int i = 0;
            foreach (XmlNode sprite in node)
            {
                paths[i++] = ($"{path}{sprite.InnerText}");
            }
            IEnumerable<Texture2D> LoadedTextures = await LoadAssets<Texture2D>(paths);
            var arr = LoadedTextures.ToArray();
            Background = arr[0];

            localLoadAmount = 1f / 3;

            //frienly
            doc = new XmlDocument();
            doc.Load("../../../Content/Xml/PocketMan.xml");
            node = doc.DocumentElement.SelectSingleNode("/PokeMans");

            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == FriendlyPokeMan.id).SelectSingleNode("Animations");
            node = node.Cast<XmlNode>().First(a => a.Attributes["name"].Value == "Back");

            count = node.ChildNodes.Count;
            paths = new string[count];

            path = node.Attributes["path"].Value;
            i = 0;
            foreach (XmlNode frame in node)
            {
                paths[i++] = ($"{path}{frame.InnerText}");
            }

            LoadedTextures = await LoadAssets<Texture2D>(paths);
            FriendlyPokeMan.Sprite = LoadedTextures.ToArray();

            localLoadAmount = 2f / 3;

           // enmy
            doc = new XmlDocument();
            doc.Load("../../../Content/Xml/PocketMan.xml");
            node = doc.DocumentElement.SelectSingleNode("/PokeMans");

            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == EnemyPokeMan.id).SelectSingleNode("Animations");
            node = node.Cast<XmlNode>().First(a => a.Attributes["name"].Value == "Front");

            count = node.ChildNodes.Count;
            paths = new string[count];

            path = node.Attributes["path"].Value;
            i = 0;
            foreach (XmlNode frame in node)
            {
                paths[i++] = ($"{path}{frame.InnerText}");
            }

            LoadedTextures = await LoadAssets<Texture2D>(paths);
            EnemyPokeMan.Sprite = LoadedTextures.ToArray();

            localLoadAmount = 1f;











        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Friendly = new Rectangle(pos, camera.Height * 2 / 3, camera.Width / 3, camera.Height / 3);
            //Enemy = new Rectangle(camera.Width * 2 / 3, camera.Height, camera.Width / 3, camera.Height / 3);
            Enemy = new Rectangle(posenemy, 0, camera.Width / 3, camera.Height / 3);
           

            if (this.LoadAmount < 1)
            {
                spriteBatch.DrawString(font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
            }
            else
            {

               spriteBatch.Draw(Background, new Rectangle(0, 0, camera.Width, camera.Height), Color.Black);
                spriteBatch.Draw(FriendlyPokeMan.Sprite, Friendly, Color.White);

                spriteBatch.Draw(EnemyPokeMan.Sprite, Enemy, Color.White);
              
            }



            //spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.1f);

            foreach (var component in _components)
            {
                component.Draw(spriteBatch, camera);
            }
            base.Draw(spriteBatch, camera);
        }
        public override void Update()
        {


            if (pos > 100)
                pos -= 10;

            if (posenemy < 1200)
                posenemy += 10;
            //else if (Friendly.X = 0)
            //    Friendly.Y--;


            foreach (var component in _components)
            {
                component.Update();
            }
            
            base.Update();
        }

    }
    
}