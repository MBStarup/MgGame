using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace PokeMan
{
    public class StateBattle : Scene
    {
        private List<Component> _components;
        private Texture2D background;
        private SpriteFont font;

        private string type;

        public PokeMan FriendlyPokeMan { get; }

        public StateBattle(PokeManGame game) : base(game)
        {
            font = Content.Load<SpriteFont>("Assets/FontTextBox");

            FriendlyPokeMan = new PokeMan();
            FriendlyPokeMan.id = 1;

            var buttonTexture = Content.Load<Texture2D>("Assets/EmptyButton");
            var buttonFont = Content.Load<SpriteFont>("Assets/FontTextBox");

            background = Content.Load<Texture2D>("Assets/Battle/Background/1");

            Checktype();

            Button startGameButton = new Button(buttonTexture, font, text: "Return to Menu", position: new Point(300, 250));

            // De forskellige knapper som spilleren nok skal kunne bruge i kamp scenen

            Button fightButton = new Button(buttonTexture, font, text: "Fight", position: new Point(1000, 500));

            Button bagButton = new Button(buttonTexture, font, text: "Bag", position: new Point(1100, 500));

            Button pokemanButton = new Button(buttonTexture, font, text: "Pokeman", position: new Point(1000, 550));

            Button cowardButton = new Button(buttonTexture, font, text: "Run", position: new Point(1100, 550));

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
                //_game.ChangeState(new StateMenu(_game));
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

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            //spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.1f);

            foreach (var component in _components)
            {
                component.Draw(spriteBatch, camera);
            }

            //spriteBatch.End();
        }

        public override void Update()
        {
            foreach (var component in _components)
            {
                component.Update();
            }
        }
    }
}