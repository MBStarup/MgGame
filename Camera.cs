using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    public class Camera
    {
        public Vector2 position;
        public Vector2 offset;

        public Camera(GraphicsDeviceManager g)
        {
            offset = new Vector2(-g.GraphicsDevice.Viewport.Width, -g.GraphicsDevice.Viewport.Height);
        }

        public Vector2 WorldToScreen(Vector2 worldCoords)
        {
            return worldCoords - position - offset / 2;
        }

        public Vector2 ScreenToWorld(Vector2 screenCoords)
        {
            return screenCoords + position + offset / 2;
        }
    }
}