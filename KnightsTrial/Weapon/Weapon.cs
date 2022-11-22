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
        protected float scale;

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
            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        protected void UseArsenal(GameTime gameTime)
        {

        }

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
