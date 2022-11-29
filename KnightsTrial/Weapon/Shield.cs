using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightsTrial
{
    internal class Shield : Weapon
    {
        //Fields

        //Variables used for the mouse to later avtivate single presses/releases
        private MouseState currentMouse;
        private MouseState previousMouse;

        //Properties

        //Constructors

        /// <summary>
        /// The Constructor for shield
        /// </summary>
        /// <param name="sprite">The Texture that the shield uses</param>
        /// <param name="position">The position of the shield</param>
        public Shield(Texture2D sprite, Vector2 position)
        {
            objectSprites = new Texture2D[1];
            objectSprites[0] = sprite;
            scale = 0.1f;
            this.position = position;
        }

        //Methods

        /// <summary>
        /// This Method loads in information and code needed for the game, there is nothing in this one but it is needed anyway as Gameobject has it abstract
        /// </summary>
        /// <param name="content">Parameter given by the monogame framework for our use</param>
        public override void LoadContent(ContentManager content)
        {
            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
        }

        /// <summary>
        /// This Update Method constantly loops throughout the program aslong as it is running, other methods we want to be looped are called inside this one
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public override void Update(GameTime gameTime)
        {
            //constantly updates the state of the mouse buttons 
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            HoldBlock(gameTime);
            RemoveShield();
        }

        /// <summary>
        /// The Method for Drawing out a sprite to the screen, this method is an override for the vitual one in GameObject and is called in GameWorld/GameState
        /// </summary>
        /// <param name="spriteBatch">Parameter given by the Monogame framework for the use of sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.Aqua * 0.4f, 0, origin, scale, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// This Method is played as long as the right mouse buttons is held down, it keeps the blocking bool on the player active
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public void HoldBlock(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                position = ReturnPlayerPostition();

                SetPlayerSpeed(0f);
                Player.Blocking = true;
            }
        }

        /// <summary>
        /// This Method is used to remove the shield and reactive the players movement/stamina
        /// </summary>
        public void RemoveShield()
        {
            if (currentMouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
            {
                ToBeRemoved = true;
                Player.Blocking = false;
                Player.RegenStamina = true;
                SetPlayerSpeed(200f);
            }
        }

    }
}
