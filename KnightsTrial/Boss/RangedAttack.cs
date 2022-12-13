using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;

namespace KnightsTrial
{
    /// <summary>
    /// A class for the RangedAttack object used as a magic attack by the boss.
    /// </summary>
    class RangedAttack : GameObject
    {

        //Fields

        //The Sprite thats used for the attack
        private Texture2D[] sprite;

        //A rotatin is needed as this attack changes direction
        private float rotation;

        //As an attacck it needs a damageValue for when it hits a player
        private int damageValue = 20;

        //A couple bools and a timer to change the state of the attack
        private bool homing = false;
        private float homingTimer;
        private bool hasDamaged;

        //Properties

        //This Override is to change the hotbox to be more fair since this is a rotating object
        protected override Vector2 SpriteSize
        {
            get { return new Vector2(sprite[0].Width * scale / 4, sprite[0].Height * scale / 4); }
        }

        //Its CollisionBox is Drawn slightly differently than others by dividing and multiplying the spriteSizes
        public override Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(

                    (int)(position.X - SpriteSize.X / 5 * 2),
                    (int)(position.Y - SpriteSize.Y / 4),
                    (int)SpriteSize.X, (int)SpriteSize.Y);
            }
        }

        //Constructors
        /// <summary>
        /// The Constructor for the attack
        /// </summary>
        /// <param name="position">The Position in which the attack is spawned at</param>
        public RangedAttack(Vector2 position)
        {
            //Sets various base variables needed for the object to be drawn out properly
            this.position = position;
            rotation = 0f;
            speed = 180f;
            scale = 2f;
            animationSpeed = 60f;

            //Sets the 2 states of the attack when it is constructed so that each time one is spawned it works the same way 
            homing = true;
            hasDamaged = false;
        }

        //Methods
        /// <summary>
        /// This Method loads in information and code needed for the game, there is nothing in this one but it is needed anyway as Gameobject has it abstract
        /// </summary>
        /// <param name="content">Parameter given by the monogame framework for our use</param>
        public override void LoadContent(ContentManager content)
        {
            //Instantiates the texture array for the attack
            sprite = new Texture2D[60];

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < sprite.Length; i++)
            {
                sprite[i] = content.Load<Texture2D>($"BringerOfDeath/RangedAttack/1_{i}");
            }

            //objectSprites is the base array Texture used in Draw and it is set during load to give the first animation
            objectSprites = sprite;

            //Sets the origin of the object to a certain spot so it rotates properly and lines up with the collisionBox
            origin = new Vector2(objectSprites[0].Width / 5 * 2, objectSprites[0].Height / 2);

            //Loads SoundEffects used
            blockSound = content.Load<SoundEffect>("SoundEffects/BlockSound");
        }
        /// <summary>
        /// The Method for Drawing out a sprite to the screen, this method is an override for the vitual one in GameObject and is called in GameWorld/GameState
        /// </summary>
        /// <param name="spriteBatch">Parameter given by the Monogame framework for the use of sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
        }
        /// <summary>
        /// This Update Method constantly loops throughout the program aslong as it is running, other methods we want to be looped are called inside this one
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
            TravelTime(gameTime);
        }

        /// <summary>
        /// A method used to control when the homing effect of the attack ends
        /// </summary>
        /// <param name="gameTime"></param>
        public void TravelTime(GameTime gameTime)
        {
            //While the homing state is true its velocity is constantly changed to the direction of the player
            if (homing == true)
            {
                velocity = Direction(ReturnPlayerPostition());
            }

            //After 2 seconds the homing effect is deactivated
            homingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (homingTimer > 2f)
            {
                homing = false;
                homingTimer = 0;
            }
        }

        /// <summary>
        /// A method that determines what happens when the attack comes into contact with another object
        /// </summary>
        /// <param name="other">The object that the attack has collided with</param>
        public override void OnCollision(GameObject other)
        {
            //Checks if the other object is the player and if its still in its damaging state
            if (other is Player && hasDamaged == false && !Player.godMode)
            {
                //Sets its state to having already contacted and damaged so the attack can no longer hit (Stops it from hitting every frame)
                hasDamaged = true;

                //Checks if the player is holding block and isn't dodging
                if (Player.Blocking == true && Player.Dodging != true)
                {
                    //Checks if the player has the stamina to block the attack and damages health instead if he doesn't
                    if (GetPlayer().Stamina < damageValue)
                    {
                        //Damages the player
                        GetPlayer().Health -= damageValue;
                        GetPlayer().HealthModified = true;
                    }
                    //damage done to the players stamina is double of the attacks DamageValue
                    GetPlayer().Stamina -= damageValue * 2;
                    SoundEffectInstance blockSoundInstance = blockSound.CreateInstance();
                    blockSoundInstance.Volume = 0.5f;
                    blockSoundInstance.Play();
                }

                //Checks if the player is neither dodging nor blocking
                if (Player.Blocking == false && Player.Dodging == false)
                {
                    //Damages the player
                    GetPlayer().Health -= damageValue;
                    GetPlayer().HealthModified = true;
                }
            }
        }

        /// <summary>
        /// A method used to get the states or properties of the player from the gameobjects list
        /// </summary>
        /// <returns></returns>
        private Player GetPlayer()
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

        /// <summary>
        /// A method that tracks the direction of the player compared to the current object
        /// </summary>
        /// <param name="playerPosition">playerPosition is a vector2 that used as the position to face</param>
        /// <returns></returns>
        protected Vector2 Direction(Vector2 playerPosition)
        {
            //Makes a Vector2 variable
            Vector2 direction;

            //Determines the direction of the Vector2 "playerPosition" from the objects position. Then normalizes it so its speed is constant
            direction = playerPosition - position;
            direction.Normalize();

            //Rotates the object to the same direction as the velocity 
            rotation = (float)Math.Atan2(position.Y - playerPosition.Y, position.X - playerPosition.X) - 0.1f;

            return direction;
        }

        /// <summary>
        /// A method for finding the position of the player
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

    }
}
