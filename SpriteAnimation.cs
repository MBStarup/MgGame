﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeMan
{
    public class SpriteAnimation : IDisplayable
    {
        private Texture2D[] Frames;
        private ulong currIndex;
        public int Length { get => Frames.Length; }

        /// <summary>
        /// How many frames each texture is repeated, lover numbers means faster animantions
        /// </summary>
        public uint InverseSpeed = 10;

        public SpriteAnimation(Texture2D[] frames)
        {
            Frames = frames;
        }

        public SpriteAnimation(Texture2D[] frames, uint inverseSpeed)
        {
            Frames = frames;
            InverseSpeed = inverseSpeed;
        }

        public static implicit operator Texture2D(SpriteAnimation a)
        {
            return a.GetNextFrame();
        }

        public static implicit operator SpriteAnimation(Texture2D[] t)
        {
            return new SpriteAnimation(t);
        }

        public Texture2D GetNextFrame()
        {
            return Frames[(++currIndex % ((ulong)Frames.Length * InverseSpeed)) / InverseSpeed];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this, Vector2.Zero, Color.White);
        }
    }
}