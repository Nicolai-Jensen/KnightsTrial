using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    internal class Icicle : GameObject
    {
        //Fields

        private float rotation;

        //Properties
        public override Rectangle CollisionBox
        {
            get
            {
                if (rotation == 1.55f || rotation == -1.55f)
                {
                     return new Rectangle(

                        (int)(position.X - SpriteSize.X),
                        (int)(position.Y - SpriteSize.Y / 4),
                        (int)SpriteSize.Y, (int)SpriteSize.X);

                }
                else
                {
                    return new Rectangle(

                        (int)(position.X - SpriteSize.X / 2),
                        (int)(position.Y - SpriteSize.Y / 2),
                        (int)SpriteSize.X, (int)SpriteSize.Y);
                }
            }
        }

        //Constructors

        public Icicle(Vector2 inputVelocity, Vector2 position, float rotation)
        {
            GameState.InstantiateGameObject(this);
            this.position = position;
            speed = 200f;
            scale = 2f;
            velocity = inputVelocity;
            this.rotation = rotation;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            objectSprites = new Texture2D[1];

            objectSprites[0] = content.Load<Texture2D>("BringerOfDeath/Icicle");

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[0], position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0.5f);
        }

        public override void OnCollision(GameObject other)
        {
            
        }
    }
}
