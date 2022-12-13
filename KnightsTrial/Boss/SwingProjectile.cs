using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightsTrial
{
    /// <summary>
    /// A class for the swing attack collisionbox used by the boss.
    /// </summary>
    internal class SwingProjectile : GameObject
    {
        //Fields

        private int damageValue;
        private Vector2 position;
        private bool hasDamaged;
        private float timer;


        //Properties
        public override Rectangle CollisionBox
        {
            get
            {
                if (!BringerOfDeath.isFacingLeft)
                {
                    return new Rectangle((int)(position.X), (int)(position.Y - 50), 300, 100);
                }
                else
                {
                    return new Rectangle((int)(position.X - 300), (int)(position.Y - 50), 300, 100);
                }
                    
            }
        }

        //Constructor
        public SwingProjectile(Vector2 posValue)
        {
            GameState.InstantiateGameObject(this);

            this.position = posValue;
            scale = 1f;
            damageValue = 25;
            hasDamaged = false;
        }
        //Methods
        public override void LoadContent(ContentManager content)
        {

            blockSound = content.Load<SoundEffect>("SoundEffects/BlockSound");
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > 0.3f)
                toBeRemoved = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Loops through the gameObject list in GameState to find and return the Player object.
        /// If no Player object is found, returns null.
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
        /// An override of the OnCollision method for the SwingProjectile object.
        /// Controls what happen when the object collides with other objects.
        /// HealthModified is a backup method in player that disables collision for a short while incase our first attempt at getting an attack to only hit once doesn't go through
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {
            //Checks that the condidtions are met to be able to hit the player, That the attack hasn't hit before and the player is in a normal state
            if (other is Player && hasDamaged == false && !Player.godMode)
            {
                hasDamaged = true;

                //checks if the player is blocking and not dodging
                if (Player.Blocking == true && Player.Dodging != true && GetPlayer().HealthModified == false)
                {
                    //Damages the health if the player doesn't have enough stamina value
                    if (GetPlayer().Stamina < damageValue)
                    {
                        GetPlayer().Health -= damageValue;
                        GetPlayer().HealthModified = true;
                    }

                    //Damages the stamina instead of the health of the player and makes a block sound effect
                    GetPlayer().Stamina -= damageValue * 2;
                    GetPlayer().HealthModified = true;
                    SoundEffectInstance blockSoundInstance = blockSound.CreateInstance();
                    blockSoundInstance.Volume = 0.5f;
                    blockSoundInstance.Play();
                }

                //Checks if the player is neither blocking or dodging and damages the player
                if (Player.Blocking == false && Player.Dodging == false && GetPlayer().HealthModified == false)
                {
                    GetPlayer().Health -= damageValue;
                    GetPlayer().HealthModified = true;
                }

                toBeRemoved = true;
            }
        }
    }
}
