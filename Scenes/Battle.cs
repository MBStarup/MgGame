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
    internal class loadContent : Scene
    {
        new public float LoadAmount { get => (float)completedLoadTasks / loadTasks + base.LoadAmount / loadTasks; }

        private int completedLoadTasks;
        private int loadTasks = 1; //The setup, corresponds to the completedLoadTasks increment at the bottom of the load method

        private PokeMan friendlyPokeMan;
        private PokeMan enemyPokeMan;
        private Texture2D background;

        private List<Button> currentButtons;
        private List<Button> newButtons;
        private Song song;
        private Player p;
        private string currentmessage = "";

        private Rectangle friendlyRect;
        private Rectangle enemyRect;
        private Rectangle textRect;
        private Rectangle buttonRect;
        private int pokemanOffset;

        private Button escapeButton;
        private Button fightButton;
        private Button[] moveButtons;

        public loadContent(string xmlPath, Player player)
        {
            pokemanOffset = PokeManGame.SceenSize.x;
            p = player;
            textRect = new Rectangle(PokeManGame.SceenSize.x / 100 * 10, PokeManGame.SceenSize.y / 100 * 70, PokeManGame.SceenSize.x / 100 * 80, PokeManGame.SceenSize.y / 100 * 25);
            buttonRect = new Rectangle(textRect.X, textRect.Y + textRect.Height / 2, textRect.Width, textRect.Height / 3);

            friendlyPokeMan = p.Party[0];
            enemyPokeMan = new PokeMan(new System.Random().Next(1, 4), 5); //Big NO! NO!, hard coded that we have 3 pokemans, should probably be stored in the gameworld data of the area, but so should the path to the sprites probably also, maybe even the textures them self, but we didn't have time for that soooo ¯\_(ツ)_/¯

            Content.RootDirectory = "Content";
            beginLoad(xmlPath);
            init();
        }

        private async void beginLoad(string xmlPath)
        {
            loadTasks += 4;//Manually counted at design-time, no time to add cool way to programatically count this, definetly "nice to have"-feature that could be added

            this.song = Content.Load<Song>("Assets/Battle/Music/battlemusic");
            completedLoadTasks += 1;

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
            background = arr[0];
            completedLoadTasks += 1;

            //frienly
            doc = new XmlDocument();
            doc.Load("../../../Content/Xml/PocketMan.xml");
            node = doc.DocumentElement.SelectSingleNode("/PokeMans");

            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == friendlyPokeMan.id).SelectSingleNode("Animations");
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
            friendlyPokeMan.Sprite = LoadedTextures.ToArray();
            completedLoadTasks += 1;

            //enemy
            doc = new XmlDocument();
            doc.Load("../../../Content/Xml/PocketMan.xml");
            node = doc.DocumentElement.SelectSingleNode("/PokeMans");

            node = node.Cast<XmlNode>().First(a => int.Parse(a.Attributes["id"].Value) == enemyPokeMan.id).SelectSingleNode("Animations");
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
            enemyPokeMan.Sprite = LoadedTextures.ToArray();
            completedLoadTasks += 1;

            completedLoadTasks += 1; //Setup Done!
        }

        public override void Update()
        {
            if (!(this.LoadAmount < 1))
            {
                newButtons.Clear();
                newButtons.AddRange(currentButtons);

                if (MediaPlayer.State == MediaState.Stopped) //Start song
                {
                    MediaPlayer.Play(song);
                }

                if (pokemanOffset > 0)
                {
                    enemyRect = new Rectangle(PokeManGame.SceenSize.x / 100 * 55 - pokemanOffset, PokeManGame.SceenSize.y / 100 * 10, PokeManGame.SceenSize.x / 100 * 33, PokeManGame.SceenSize.y / 100 * 33);
                    friendlyRect = new Rectangle(PokeManGame.SceenSize.x / 100 * 13 + pokemanOffset, PokeManGame.SceenSize.y / 100 * 40, PokeManGame.SceenSize.x / 100 * 33, PokeManGame.SceenSize.y / 100 * 33);
                    pokemanOffset -= 100;
                    currentmessage = $"You've incountered a wild {enemyPokeMan.nickname}";
                }
                else
                {
                    if (!(friendlyPokeMan.Alive && enemyPokeMan.Alive))
                    {
                        currentmessage = friendlyPokeMan.Alive ? $"You won! Congratulations!" : $"You lost...";
                        newButtons.RemoveRange(1, currentButtons.Count - 1);
                        newButtons[0] = escapeButton;
                        newButtons[0].Text = "Click here to continue";
                    }

                    foreach (Button button in currentButtons) //Update buttons
                    {
                        button.Update();
                    }
                }

                currentButtons.Clear();
                currentButtons.AddRange(newButtons);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.LoadAmount < 1)
            {
                spriteBatch.DrawString(PokeManGame.Font, "Loading Assets: " + (int)(LoadAmount * 100) + "%", Vector2.Zero, Color.Green);
            }
            else
            {
                Rectangle temp;
                Texture2D shadow = Content.Load<Texture2D>("Assets/Battle/Background/shadow");

                spriteBatch.Draw(background, new Rectangle(0, 0, PokeManGame.SceenSize.x, PokeManGame.SceenSize.y), Color.White);

                temp = friendlyRect;
                temp.Height = (int)(temp.Height * 0.2);
                temp.Y += friendlyRect.Height - temp.Height;
                spriteBatch.Draw(shadow, temp, Color.Black);

                temp = enemyRect;
                temp.Height = (int)(temp.Height * 0.2);
                temp.Y += enemyRect.Height - temp.Height;
                spriteBatch.Draw(shadow, temp, Color.Black);

                spriteBatch.Draw(friendlyPokeMan.Sprite, friendlyRect, Color.White);
                spriteBatch.Draw(enemyPokeMan.Sprite, enemyRect, Color.White);

                spriteBatch.Draw(background, textRect, Color.Black);

                spriteBatch.DrawString(PokeManGame.Font, $"Enemy Hp: = {enemyPokeMan.hp}", enemyRect.Location.ToVector2(), Color.White);
                spriteBatch.DrawString(PokeManGame.Font, $"Your Hp: = {friendlyPokeMan.hp}", friendlyRect.Location.ToVector2(), Color.White);

                var textLocation = textRect.Location.ToVector2();
                textLocation.X += (textRect.Width - PokeManGame.Font.MeasureString(currentmessage).X) / 2;
                textLocation.Y += (textRect.Height / 3 - PokeManGame.Font.MeasureString(currentmessage).Y) / 2; ;
                spriteBatch.DrawString(PokeManGame.Font, currentmessage, textLocation, Color.White);

                try
                {
                    int buttonWidth = PokeManGame.SceenSize.x / 5;
                    int spaceWidth = 0;
                    if (buttonWidth * currentButtons.Count > buttonRect.Width)
                    {
                        buttonWidth = buttonRect.Width / currentButtons.Count;
                    }
                    else
                    {
                        spaceWidth = buttonRect.Width / currentButtons.Count - buttonWidth;
                    }

                    int i = 0;
                    foreach (Button button in currentButtons)
                    {
                        button.Rectangle = buttonRect; //Assign y coord and height
                        button.Rectangle.X += (i++ * (buttonWidth + spaceWidth)) + (spaceWidth / 2); //Assign x coord
                        button.Rectangle.Width = buttonWidth; //Assign width
                        button.Draw(spriteBatch);
                    }
                }
                catch (DivideByZeroException e) { } //Doesn't draw if currentButtons.Count == 0
            }
        }

        private void doTurn(Move move)
        {
            currentmessage = "";
            (friendlyPokeMan.SpeedStat >= enemyPokeMan.SpeedStat ? (Action)PlayerFirst : EnemyFirst)();
            newButtons.Clear();
            newButtons.Add(fightButton);
            newButtons.Add(escapeButton);

            void PlayerFirst()
            {
                friendlyPokeMan.Attack(enemyPokeMan, move);
                if (friendlyPokeMan.Alive && enemyPokeMan.Alive)
                {
                    currentmessage += $"You used {move} and did {enemyPokeMan.tookdmg} dmg";
                    currentmessage += ", and " + Environment.NewLine;
                    int moveIndex = new System.Random().Next(enemyPokeMan.moves.Where(a => a != null).Count());
                    enemyPokeMan.Attack(friendlyPokeMan, enemyPokeMan.moves[moveIndex]);
                    currentmessage += $"Enemy used {enemyPokeMan.moves[moveIndex]} and did {friendlyPokeMan.tookdmg} dmg";
                }
            }

            void EnemyFirst()
            {
                int moveIndex = new System.Random().Next(enemyPokeMan.moves.Where(a => a != null).Count());
                enemyPokeMan.Attack(friendlyPokeMan, enemyPokeMan.moves[moveIndex]);
                if (friendlyPokeMan.Alive && enemyPokeMan.Alive)
                {
                    currentmessage += $"Enemy used {enemyPokeMan.moves[moveIndex]} and did {friendlyPokeMan.tookdmg} dmg";
                    currentmessage += ", and " + Environment.NewLine;
                    friendlyPokeMan.Attack(enemyPokeMan, move);
                    currentmessage += $"You used {move} and did {enemyPokeMan.tookdmg} dmg";
                }
            }
        }

        private void init()
        {
            // De forskellige knapper som spilleren nok skal kunne bruge i kamp scenen

            fightButton = new Button(PokeManGame.ButtonTexture, PokeManGame.Font, text: "Fight"/*, position: new Point(1000, 900)*/);
            escapeButton = new Button(PokeManGame.ButtonTexture, PokeManGame.Font, text: "Run"/*, position: new Point(1100, 950)*/);
            escapeButton.Click += (object o, EventArgs e) => Close();
            fightButton.Click += (object sender, EventArgs e) =>
            {
                newButtons.Clear();
                foreach (Button b in moveButtons)
                {
                    newButtons.Add(b);
                }
            };

            newButtons = new List<Button>() { fightButton, escapeButton };
            currentButtons = new List<Button>(newButtons);

            initMoves();
        }

        private void initMoves() //might need to be called seperate from Init() if we add pokemon switching
        {
            var moves = friendlyPokeMan.moves.Where(a => a != null).ToArray();
            moveButtons = new Button[moves.Count()];
            for (int i = 0; i < moves.Length; i++)
            {
                var currMove = moves[i];
                moveButtons[i] = new Button(PokeManGame.ButtonTexture, PokeManGame.Font, text: currMove.Name/*, position: new Point(1100, 850 + (50 * i))*/);
                moveButtons[i].Click += (object o, EventArgs e) => doTurn(currMove);
            }
        }

        public override void Close()
        {
            MediaPlayer.Stop();
            base.Close();
        }
    }
}