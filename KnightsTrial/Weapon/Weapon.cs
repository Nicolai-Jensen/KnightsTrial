using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
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
            foreach (GameObject go in GameWorld.gameObject)
            {

                if (go is Player)
                {

                    return go.Position;
                }
            }

            return new Vector2(position.X, -100);
        }
        protected Vector2 Direction(Vector2 playerPosition)
        {
            Vector2 direction;
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = mouseState.Position.ToVector2();

            direction = mousePosition - playerPosition;
            direction.Normalize();
            rotation = (float)Math.Atan2(mousePosition.Y - position.Y, mousePosition.X - position.X) + 1.4f;

            return direction;
        }

        public override void OnCollision(GameObject other)
        {

        }
    }
}
