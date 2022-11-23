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

        protected SoundEffect swingSound;
        protected SoundEffect magicSound;
        protected SoundEffect aoeSound;
        protected SoundEffect damagedSound;
        //Properties

        //Constructors

        //Methods
        public override void OnCollision(GameObject other)
        {
            //Do something
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
