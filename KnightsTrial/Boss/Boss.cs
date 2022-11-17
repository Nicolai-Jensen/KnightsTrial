using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial.Boss
{
    internal abstract class Boss : GameObject
    {
        //Fields
        protected int health;
        protected float damageMultiplier;
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
