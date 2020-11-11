using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    public class Camera
    {
        public Vector2 position;
        public Vector2 offset;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Camera()
        {
            Width = PokeManGame.SceenSize.x;
            Height = PokeManGame.SceenSize.y;
            offset = new Vector2(-Width, -Height);
        }

        public Vector2 WorldToScreen(Vector2 worldCoords)
        {
            return worldCoords - position - offset / 2;
        }

       
        public Vector2 ScreenToWorld(Vector2 screenCoords)
        {
            return screenCoords + position + offset / 2;
        }

        public void Update()
        {
        }
    }
}