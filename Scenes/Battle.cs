using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace PokeMan
{
    internal class Battle : Scene
    {
        new public float LoadAmount { get => base.LoadAmount * localLoadAmount; }
        private float localLoadAmount;
        private PokeMan FriendlyPokeMan;
        private PokeMan EnemyPokeMan;
        private Texture2D Background;
        private Textbox textbox;

        private string currentMoveName;
        private ElementEnum currentMoveElement;


        private Rectangle Friendly;
        private Rectangle Enemy;
        private List<Component> _components;
        private SpriteFont font;
        private string type;
        private string enemyhpText;
        private string hpText;
        private int pos = PokeManGame.SceenSize.x * 2 / 3;
        private int posE;
        private Rectangle FriendlyShadow;
        private Rectangle EnemyShadow;
        private Move move;
        public Song song;
        private Player p;

        private bool playerHasAttacked;

        private bool playerDead;
        private bool enemyDead;

        public Battle(string xmlPath, Player player)
        {
            p = player;
            var buttonTexture = Content.Load<Texture2D>("Assets/EmptyButton");
            var buttonFont = Content.Load<SpriteFont>("Assets/FontTextBox");

            FriendlyPokeMan = p.Party[0];
            //FriendlyPokeMan.id = 1;

            EnemyPokeMan = new PokeMan(1, 5);

            Content.RootDirectory = "Content";
            font = Content.Load<SpriteFont>("Assets/FontTextBox");
            Checktype();
            LoadContent(xmlPath);
            BattleButtons();

            void CheckPlayerStatus()
            {
                if (FriendlyPokeMan.hp <= 0)
                {
                    playerDead = true;
                }
            }
            void CheckEnemyStatus()
            {
                if (EnemyPokeMan.hp <= 0)
                {
                    enemyDead = true;
                }
            }



            void BattleButtons()
            {
                // De forskellige knapper som spilleren nok skal kunne bruge i kamp scenen

                Button fightButton = new Button(buttonTexture, font, text: "Fight", position: new Point(1000, 900));

                Button bagButton = new Button(buttonTexture, font, text: "Bag", position: new Point(1100, 900));

                Button pokemanButton = new Button(buttonTexture, font, text: "Pokeman", position: new Point(1000, 950));

                Button cowardButton = new Button(buttonTexture, font, text: "Run", position: new Point(1100, 950));

                //startGameButton.Click += StartGameButton_Click;
                fightButton.Click += fightButton_Click;
                bagButton.Click += bagButton_Click;
                pokemanButton.Click += pokemanButton_Click;
                cowardButton.Click += cowardButton_Click;

                _components = new List<Component>()
            {
               // startGameButton,
                fightButton,
                bagButton,
                pokemanButton,
                cowardButton,
            };

            // Knappen fight har andre knapper i sig når man klikker på den
            void fightButton_Click(object sender, EventArgs e)
            {

                    Move chosenmove = new Move();
                _components = new List<Component>();
                int i = 0;
                foreach (Move move in FriendlyPokeMan.moves)
                {
                    if (move != null)
                    {
                            
                        Button moveButton = new Button(buttonTexture, font, text: move.Name, position: new Point(1100, 850 + (50 * i)));
                            moveButton.Click += (object o, EventArgs e) =>
                            {
                                CheckPlayerStatus();
                                if (playerDead == false)
                                {
                                    currentMoveName = move.Name;
                                   // currentMoveElement = move.Element;
                                    
                                    FriendlyPokeMan.Attack(EnemyPokeMan, move);

                                    playerHasAttacked = true;
                                }
                                else
                                {
                                    throw new NotImplementedException("You dead");
                                }
                                CheckEnemyStatus();
                                if (playerHasAttacked == true && playerDead == false && enemyDead == false)
                                {
                                    EnemyPokeMan.Attack(FriendlyPokeMan, move);

                                    playerHasAttacked = false;
                                }
                                else if (playerHasAttacked == true && enemyDead == true)
                                    {
                                        throw new NotImplementedException("Enemy dead");
                                        playerHasAttacked = false;
                                    }
                                    else
                                    {
                                        BattleButtons();
                                        playerHasAttacked = false;
                                    }
                                
                                BattleButtons();
                            };
                        _components.Add(moveButton);
                        i++;
                    }
                    else
                        break; 
                }
            }

                void bagButton_Click(object sender, EventArgs e)
                {
                }
                void pokemanButton_Click(object sender, EventArgs e)
                {
                    //FriendlyPokeMan = Area.p.party[1];
                }
                void cowardButton_Click(object sender, EventArgs e)
                {
                    // Returns the player to area scene if they have higher speed
                    if (FriendlyPokeMan.SpeedStat >= EnemyPokeMan.SpeedStat)
                    {
                        PokeManGame.Scenes.Pop();

                    }
                    else
                    {
                            //message cant run
                    }


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

            //enemy
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

       


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.LoadAmount < 1)
            {
                spriteBatch.DrawString(font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
            }
            else
            {
                Texture2D shadow = Content.Load<Texture2D>("Assets/Battle/Background/shadow");

                Friendly = new Rectangle(pos, PokeManGame.SceenSize.y * 2 / 3 - 50, PokeManGame.SceenSize.x / 3, PokeManGame.SceenSize.y / 3);
                //Enemy = new Rectangle(PokeManGame.SceenSize.x * 2 / 3, PokeManGame.SceenSize.y, PokeManGame.SceenSize.x / 3, PokeManGame.SceenSize.y / 3);
                Enemy = new Rectangle(posE, 0, PokeManGame.SceenSize.x / 3, PokeManGame.SceenSize.y / 3);

                spriteBatch.Draw(Background, new Rectangle(0, 0, PokeManGame.SceenSize.x, PokeManGame.SceenSize.y), Color.White);

                FriendlyShadow = new Rectangle(pos, PokeManGame.SceenSize.y * 2 / 3 + 100, PokeManGame.SceenSize.x / 3, PokeManGame.SceenSize.y / 3);

                EnemyShadow = new Rectangle(posE, 100, PokeManGame.SceenSize.x / 3, PokeManGame.SceenSize.y / 3);

                spriteBatch.Draw(shadow, FriendlyShadow, Color.Black);

                spriteBatch.Draw(shadow, EnemyShadow, Color.Black);

                if (pos > 100 && posE < 1200)
                {
                    SlideIn();

                    spriteBatch.DrawString(font, "FIGHT!", new Vector2(540, 500), Color.Black);
                }

                spriteBatch.Draw(FriendlyPokeMan.Sprite, Friendly, Color.White);
                spriteBatch.Draw(EnemyPokeMan.Sprite, Enemy, Color.White);

               spriteBatch.DrawString(font, $"Your Hp: = {hpText}", new Vector2(50, 200), Color.White);

                spriteBatch.DrawString(font, $"Dmg Taken: = {FriendlyPokeMan.tookdmg}", new Vector2(50, 250), Color.White);
                spriteBatch.DrawString(font, $"AttackStat: = {FriendlyPokeMan.AttackStat}", new Vector2(50, 300), Color.White);
                spriteBatch.DrawString(font, $"speed : {FriendlyPokeMan.SpeedStat}", new Vector2(50, 350), Color.White);



                spriteBatch.DrawString(font, $"Enemy Hp: = {enemyhpText}", new Vector2(50, 50), Color.White);

                spriteBatch.DrawString(font, $"Dmg Taken: = {EnemyPokeMan.tookdmg}", new Vector2(50, 100), Color.White);

                spriteBatch.DrawString(font, $"Enemy AttackStat: = {EnemyPokeMan.AttackStat}", new Vector2(50, 150), Color.White);

                spriteBatch.DrawString(font, $"enemy speed : {EnemyPokeMan.SpeedStat}", new Vector2(50, 400), Color.White);

                //spriteBatch.DrawString(font, $"enemy speed : {EnemyPokeMan.SpeedStat}", new Vector2(50, 400), Color.White);



                foreach (var component in _components)
                {
                    component.Draw(spriteBatch);
                }
            }
        }

        private void Checktype()
        {
            if (FriendlyPokeMan.id == 1)
            {
                type = "grass";
            }
            else if (FriendlyPokeMan.id == 2)
            {
                type = "fire";
            }
            else if (FriendlyPokeMan.id == 3)
            {
                type = "water";
            }
            else
            {
                type = "normal";
            }
        }

        private void SlideIn()
        {
            pos -= 15;

            posE += 15;
        }

        public override void Update()
        {
            foreach (var component in _components)
            {
                component.Update();
            }

            enemyhpText = EnemyPokeMan.hp.ToString();
            hpText = FriendlyPokeMan.hp.ToString();
        }
    }
}