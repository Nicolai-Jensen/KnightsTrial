using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace KnightsTrial
{
    internal abstract class GameBoss : GameObject
    {
        //Fields
        protected int health;
        protected float damageMultiplier;

        protected Texture2D[] walkAnimation;
        protected Texture2D[] swingAnimation;
        protected Texture2D[] magicAnimation;
        protected Texture2D[] aoeAnimation;
        protected Texture2D[] deathAnimation;

        protected bool heavyHit = false;
        protected bool hitCooldown = false;
        protected bool hitFeedback = false;
        protected bool gotHit = false;
        protected float feedbackTimer;
        protected float hitCooldownTimer;

        Weapon previousHit;
        Weapon currentHit;

        //Properties

        //Constructors

        //Methods

        /// <summary>
        /// An override of the OnCollision method for the GameBoss object.
        /// Controls what happen when the object collides with other objects.
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {

            //if (other is Weapon)
            //{
            //    currentHit = (Weapon)other;

            //    if (currentHit != previousHit)
            //    {
            //        health -= Weapon.DamageValue;
            //        previousHit = (Weapon)other;
            //    }


            //    if (currentHit == previousHit)
            //    {
            //        health -= 0;
            //    }

            //}

            //Checks if the boss has made contact with a light attack from the player and utilizes the currentHit/previousHit variables to make sure the light attack can only hit once per attack
            if (other is HeroWeapon && this is BringerOfDeath && this.color == Color.White)
            {
                currentHit = (HeroWeapon)other;

                //Damages the bosses health and sets gotHit to true activating the damage feedback method
                if (currentHit != previousHit)
                {
                    health -= HeroWeapon.DamageValue;
                    previousHit = (HeroWeapon)other;
                    gotHit = true;
                }

                //If the current light attack is the same light attack as the the one he just got hit by it will do nothing
                if (currentHit == previousHit)
                {
                    health -= 0;
                }
            }

            //The collision check for the players heavy attack, this collision activates HeavyDamaged which is a method that makes the boss invulnerable to more heavy attack for a limited time
            if ((other is HiddenHitBox || other is HiddenHitBox2) && heavyHit == false && this is BringerOfDeath && this.color == Color.White)
            {
                health -= HiddenHitBox.DamageValue;
                gotHit = true;
                heavyHit = true;
            }
        }

        /// <summary>
        /// A method used to make sure the boss is only hit once by an attack and not multiple times or frames by activating/deavtivating a bool which starts a timer
        /// </summary>
        /// <param name="gameTime"></param>
        public void HeavyDamaged(GameTime gameTime)
        {
            if (heavyHit == true)
            {
                hitCooldown = true;
                if (hitCooldown == true)
                {
                }
                hitCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (hitCooldownTimer >= 1f)
                {
                    hitCooldown = false;
                    hitCooldownTimer = 0;
                    heavyHit = false;
                }

            }
        }

        /// <summary>
        /// Visualizes damage by changing the color to red. and back to white after 0.1 second.
        /// </summary>
        /// <param name="gameTime"></param>
        public void DamagedFeedBack(GameTime gameTime)
        {
            if (gotHit == true)
            {
                hitFeedback = true;
                if (hitFeedback == true)
                {
                    color = Color.Red;
                }
                feedbackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (feedbackTimer >= 0.1f)
                {
                    hitFeedback = false;
                    color = Color.White;
                    feedbackTimer = 0;
                    gotHit = false;
                }

            }

        }


        public void CheckPhase(GameTime gameTime)
        {
            //Do something
        }
        public void Death()
        {
            //Do something
        }

    }
}
