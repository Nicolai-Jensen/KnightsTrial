using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

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

        //Texture array for the buttons.
        private Texture2D[] _texture;

        private Vector2 position;

        //Used for buttons with animation.
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

            //If the mouse hovers over the buttons, they begin to animate.
            if (mouseRectangle.Intersects(Rectangle))
            {
                Animate(gameTime);

                //If the buttons is clicked. begin the Click event.
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
            //If the mouse leaves the buttons rectangle, the animation resets back to sprite[0]
            else
            {
                animationTime = 0;
                animationSpeed = 100;
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //If the player is dead, the resume button is not being drawn.
            if (_texture == PauseState.resumeButtonAnimation && Player.dead || _texture == PauseState.resumeButtonAnimation && !GameState.isBossAlive)
            { }

            //The Godmode Buttons is not animated, so it needs a new draw method.
            else if(_texture == MenuState.currentGodMode)
                spriteBatch.Draw(_texture[0], position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            else
                spriteBatch.Draw(_texture[(int)animationTime], position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
        /// <summary>
        /// Animates the buttons but only loops throu the animation once. 
        /// </summary>
        /// <param name="gameTime"></param>
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
