using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial.Weapon
{
    internal class Weapon : GameObject
    {
        //Fields

        protected float rotation;
        protected bool enoughStamina;
        protected bool useArsenalAnimation;

        //Properties

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        //Constructors

        //Methods
        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        protected void UseArsenal(GameTime gameTime)
        {

        }

        protected Vector2 ReturnPlayerPostition()
        {
            return Vector2.Zero;
        }
        protected Vector2 DirectionClosestEnemy(Vector2 playerPosition)
        {
            return playerPosition;
        }

        public override void OnCollision(GameObject other)
        {

        }
    }
}
