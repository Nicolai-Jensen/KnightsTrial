using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Weapon hitBoss = new Weapon();
        protected SoundEffect swingSound;
        protected SoundEffect magicSound;
        protected SoundEffect aoeSound;
        protected SoundEffect damagedSound;
        //Properties

        //Constructors

        //Methods
        public override void OnCollision(GameObject other)
        {

            if (other is Weapon && other != hitBoss)
            {
                health -= Weapon.DamageValue;
                hitBoss = (Weapon)other;
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
