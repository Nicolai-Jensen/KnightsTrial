using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    /// <summary>
    /// the blueprint of a button, when instantiated it just needs a texture, and position.
    /// </summary>
    internal class Button : Component
    {
        //Fields

        //MouseStates to make sure it only presses 1 time.
        private MouseState currentMouse;
        private MouseState previousMouse;

        private Texture2D[] _texture;
        private Vector2 position;

        protected float animationTime;
        protected float animationSpeed;

        //Properties

        /// <summary>
        /// A event when you click the buttons.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// A Rectangle for the buttons, to determine if the mouse is intersecting the button.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, _texture[0].Width, _texture[0].Height);
            }
        }

        //Constructors
        public Button(Texture2D[] texture, Vector2 posValue)
        {
            _texture = texture;

            position = posValue;

        }
        //Methods

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            //mouse Rectangle used to see if the mouse is intersecting with the button.
            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
            {
                Animate(gameTime);

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                animationTime = 0;
                animationSpeed = 100;
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture[(int)animationTime], position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            //if (!string.IsNullOrEmpty(Text))
            //{
            //    float x = (Rectangle.X + (Rectangle.Width / 2)) - (buttonFont.MeasureString(Text).X / 2);
            //    float y = (Rectangle.Y + (Rectangle.Height / 2)) - (buttonFont.MeasureString(Text).Y / 2);

            //    spriteBatch.DrawString(buttonFont, Text, new Vector2(x, y), PenColour);
            //}
        }
        protected void Animate(GameTime gameTime)
        {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds * animationSpeed;

            if (animationTime > _texture.Length)
            {
                animationTime = 20;
                animationSpeed = 0;
            }
        }
    }
}
