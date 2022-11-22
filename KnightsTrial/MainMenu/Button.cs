﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    internal class Button : Component
    {
        //Fields
        private MouseState currentMouse;
        private MouseState previousMouse;

        private SpriteFont buttonFont;

        private bool isHovering;

        private Texture2D[] _texture;

        protected float animationTime;
        protected float animationSpeed;

        //Properties
        public event EventHandler Click;

        public bool Clicked { get; set; }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture[0].Width, _texture[0].Height);
            }
        }
        public string Text { get; set; }

        //Constructors
        public Button(Texture2D[] texture)
        {
            _texture = texture;

            PenColour = Color.Black;

            animationSpeed = 50;

        }
        //Methods
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;

            spriteBatch.Draw(_texture[(int)animationTime], Rectangle, Color.White);

            //if (!string.IsNullOrEmpty(Text))
            //{
            //    float x = (Rectangle.X + (Rectangle.Width / 2)) - (buttonFont.MeasureString(Text).X / 2);
            //    float y = (Rectangle.Y + (Rectangle.Height / 2)) - (buttonFont.MeasureString(Text).Y / 2);

            //    spriteBatch.DrawString(buttonFont, Text, new Vector2(x, y), PenColour);
            //}
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;
                Animate(gameTime);

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
        protected void Animate(GameTime gameTime)
        {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds * animationSpeed;

            if (animationTime > _texture.Length)
            {
                animationTime = 0;
            }
        }
    }
}