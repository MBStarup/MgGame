//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Diagnostics;

//namespace PokeMan
//{
//    public class StatePick : State
//    {
//        private List<Component> _components;
//        private Texture2D background;

//        private string type;

//        public PokeMan FriendlyPokeMan { get; }

//        public StatePick(PokeManGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
//        {
//            FriendlyPokeMan = new PokeMan();
//            FriendlyPokeMan.id = 1;

//            var buttonTexture = content.Load<Texture2D>("Assets/EmptyButton");
//            var buttonFont = _content.Load<SpriteFont>("Assets/FontTextBox");

//            background = content.Load<Texture2D>("Assets/Battle/Background/1");

//            Checktype();

//            var startGameButton = new Button(buttonTexture, buttonFont)
//            {
//                Position = new Vector2(300, 250),
//                Text = "Return to Menu",
//            };
//// De forskellige knapper som spilleren nok skal kunne bruge i kamp scenen
//            var fightButton = new Button(buttonTexture, buttonFont)
//            {
//                Position = new Vector2(1000, 500),
//                Text = "Fight",
//            };

//            var bagButton = new Button(buttonTexture, buttonFont)
//            {
//                Position = new Vector2(1100, 500),
//                Text = "Bag",
//            };

//            var pokemanButton = new Button(buttonTexture, buttonFont)
//            {
//                Position = new Vector2(1000, 550),
//                Text = "Pokeman",
//            };

//            var cowardButton = new Button(buttonTexture, buttonFont)
//            {
//                Position = new Vector2(1100, 550),
//                Text = "Run",
//            };

//            startGameButton.Click += StartGameButton_Click;
//            fightButton.Click += fightButton_Click;
//            bagButton.Click += bagButton_Click;
//            pokemanButton.Click += pokemanButton_Click;
//            cowardButton.Click += cowardButton_Click;

//            _components = new List<Component>()
//            {
//                startGameButton,
//                fightButton,
//                bagButton,
//                pokemanButton,
//                cowardButton,

//            };

//            void StartGameButton_Click(object sender, EventArgs e)
//            {
//                _game.ChangeState(new StateMenu(_game, _graphicsDevice, _content));
//            }
//            // Knappen fight har andre knapper i sig når man klikker på den
//            void fightButton_Click(object sender, EventArgs e)
//            {
//                var move1Button = new Button(buttonTexture, buttonFont)
//                {
//                    Position = new Vector2(1100, 550),
//                    Text = "hak",
//                };

//                var move2Button = new Button(buttonTexture, buttonFont)
//                {
//                    Position = new Vector2(1100, 600),
//                    Text = "slash",
//                };

//                var move3Button = new Button(buttonTexture, buttonFont)
//                {
//                    Position = new Vector2(1100, 650),
//                    Text = "poke",
//                };

//                var moveSpecialButton = new Button(buttonTexture, buttonFont)
//                {
//                    Position = new Vector2(1100, 700),

//                    Text = (type),
//                };
//                move1Button.Click += move1Button_Click;
//                move2Button.Click += move2Button_Click;
//                move3Button.Click += move3Button_Click;
//                moveSpecialButton.Click += moveSpecialButton_Click;
//                _components = new List<Component>()
//            {
//               move1Button,
//               move2Button,
//               move3Button,
//               moveSpecialButton,
//            };
//                void move1Button_Click(object sender, EventArgs e)
//                {
//                }
//                void move2Button_Click(object sender, EventArgs e)
//                {
//                }
//                void move3Button_Click(object sender, EventArgs e)
//                {
//                }
//                void moveSpecialButton_Click(object sender, EventArgs e)
//                {
//                    if (FriendlyPokeMan.id == 1)
//                    {
//                        Debug.WriteLine("Leaflutter is a" + type);
//                    }
//                }
//            }

//            void bagButton_Click(object sender, EventArgs e)
//            {
//            }
//            void pokemanButton_Click(object sender, EventArgs e)
//            {
//            }
//            void cowardButton_Click(object sender, EventArgs e)
//            {
//                // Tager spilleren tilbage til startmenuen, bare en placeholder
//                _game.ChangeState(new StateMenu(_game, _graphicsDevice, _content));
//            }

//            // Tjekker hvilken type pokeman man har, så pokemanens special attack passer til typen
//            void Checktype()
//            {
//                if (FriendlyPokeMan.id == 1)
//                {
//                    type = "grass";
//                }
//                else if (FriendlyPokeMan.id == 2)
//                {
//                    type = "water";

//                }
//                else if (FriendlyPokeMan.id == 3)
//                {
//                    type = "fire";
//                }

//                else
//                {
//                    type = "normal";
//                }
//            }
//        }

//        public override void Draw( SpriteBatch spriteBatch)
//        {
//            //spriteBatch.Begin();

//            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.1f);

//            foreach (var component in _components)
//            {
//                component.Draw( spriteBatch);
//            }

//            //spriteBatch.End();
//        }

//        public override void Update()
//        {
//            foreach (var component in _components)
//            {
//                component.Update();
//            }
//        }
//    }
//}