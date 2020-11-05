using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    public class Button : Component
    {
        private MouseState _currentMouse;
        private SpriteFont _font;
        private bool _isHovering;
        private MouseState _previousMouse;
        private Texture2D _texture;

        public event EventHandler Click;

        public bool Clicked { get; private set; }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }

       public Rectangle Rectangle
        {
            get
            {
                // Sets Button size
                // Original   return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
                return new Rectangle((int)Position.X, (int)Position.Y, 100, 50);
            }
        }

        // Constructor for button
        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.White;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            // Resets from hover colour
            var colour = Color.Green;
            // changes colour when mouse is hovering
            if (_isHovering)
                colour = Color.Gray;

            // draws the button outline
            spriteBatch.Draw(_texture, Rectangle, colour);

            // Draws the buttons label and sets it in the middle of the button
            var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
            var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);
            spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);

            // no idea ???????????
            // Original
            //if (!string.IsNullOrEmpty(Text))
            //{
            //    var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
            //    var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);
            //    spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            //}
        }

        public override void Update()
        {
            // ??????????
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            //defines the mouse size
            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
            // continusly resets hovering to false
            _isHovering = false;

            // if mouse is hovering over button
            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                // all works somehow, executes the buttons code when pressed, code is in Main private void QuitButton_Click(object sender, System.EventArgs e)
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                //theese are wonky and causes dobbelt clicks
                //if (_currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
                //if (_previousMouse.LeftButton == ButtonState.Pressed)
                // if (_currentMouse.LeftButton == ButtonState.Pressed)

                {
                    Click?.Invoke(this, new EventArgs());
                    //if (Click != null)
                    //{
                    //    Click(this, new EventArgs());
                    //}
                }
            }
        }
    }
}