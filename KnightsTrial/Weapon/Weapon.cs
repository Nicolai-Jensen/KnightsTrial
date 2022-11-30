using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;

namespace KnightsTrial
{
    internal class Weapon : GameObject
    {
        //Fields

        //The field values used by all weapons are protected here
        protected float rotation;
        protected static int damageValue;

        //Properties

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public static int DamageValue
        {
            get { return damageValue; }
        }

        //Constructors
        //There is no constructor for this superclass

        //Methods

        /// <summary>
        /// This Method loads in information and code needed for the game, there is nothing in this one but it is needed anyway as Gameobject has it abstract
        /// </summary>
        /// <param name="content">Parameter given by the monogame framework for our use</param>
        public override void LoadContent(ContentManager content)
        {
            
        }

        /// <summary>
        /// This Update Method constantly loops throughout the program aslong as it is running, other methods we want to be looped are called inside this one
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Method used by all weapoons to find the position of the PLayer
        /// </summary>
        /// <returns></returns>
        protected Vector2 ReturnPlayerPostition()
        {
            foreach (GameObject go in GameState.gameObject)
            {

                if (go is Player)
                {

                    return go.Position;
                }
            }

            return new Vector2(position.X, -100);
        }

        /// <summary>
        /// Method used to set the animationtime (place in the array) of the player before swaping player animations, this is used because otherwise swapping arrays from an array with 5 spot to 2 spots may result in Null
        /// </summary>
        /// <param name="timeValue">timeValue in this method is the spot in the array or the players "animationTime" variable</param>
        protected void SetPlayerAnimationTime(float timeValue)
        {
            foreach (GameObject go in GameState.gameObject)
            {
                if (go is Player)
                {
                    go.AnimationTime = timeValue;
                }
            }
        }

        /// <summary>
        /// Method used to spend stamina when attacking
        /// </summary>
        /// <param name="value">Value is the amount of stamina that is taken from the player</param>
        protected void SetPlayerStamina(int value)
        {

            GetPlayer().Stamina -= value;
          
        }

        /// <summary>
        /// Method used to change the players speed value as when attacking he needs to commit to his positioning
        /// </summary>
        /// <param name="speedValue">speedValue is the value the players speed is set too</param>
        protected void SetPlayerSpeed(float speedValue)
        {
            foreach (GameObject go in GameState.gameObject)
            {
                if (go is Player)
                {
                    go.Speed = speedValue;                 
                }
            }          
        }

        /// <summary>
        /// This Method is used to determine the direction of the weapon objects
        /// </summary>
        /// <param name="playerPosition">While it can be any position, we called it playerPosition as inserting "ReturnPlayerPosition" method giver den resultat vi vil have</param>
        /// <returns></returns>
        protected Vector2 Direction(Vector2 playerPosition)
        {
            Vector2 direction;
            //Usses a static class we made to convert Points to vector2s
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = mouseState.Position.ToVector2();

            //We get the correct direction by Subtracting the player Position with the Mouse Point, same goes for Rotation
            direction = mousePosition - playerPosition;
            direction.Normalize();
            rotation = (float)Math.Atan2(mousePosition.Y - position.Y, mousePosition.X - position.X) - 0.1f;

            return direction;
        }

        /// <summary>
        /// The OnCollision method is used for collision if it has to react in anyway, however most of our collision is done in the bosses collision
        /// </summary>
        /// <param name="other">other is the object colliding with this object</param>
        public override void OnCollision(GameObject other)
        {

        }

        /// <summary>
        /// This Method gets the player and lets you use the players properties as long as they are public
        /// </summary>
        /// <returns></returns>
        protected Player GetPlayer()
        {
            //loops through the gameObject list untill it finds the player, then returns it. 
            foreach (GameObject go in GameState.gameObject)
            {
                if (go is Player)
                {
                    return (Player)go;
                }
            }
            //if no player object is found, returns null.
            return null;
        }


    }
}
