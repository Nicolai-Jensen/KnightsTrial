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
    internal class Fireball : GameObject
    {
        //Fields
        private int damageValue;
        private Vector2 playerPosition;
        private Texture2D[] explosionSprites;
        private bool hasCollided;
        private Beware telegraph;
        //Properties

        //Constructors

        public Fireball(Vector2 currentPlayerPosition, Vector2 spawnPosition, Vector2 projectileDirection)
        {
            playerPosition = currentPlayerPosition;
            GameState.InstantiateGameObject(this);
            position = spawnPosition;
            speed = 800f;
            velocity = projectileDirection;
            hasCollided = false;
            telegraph = new Beware(playerPosition);
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            objectSprites = new Texture2D[4];
            explosionSprites = new Texture2D[7];

            for(int i = 0; i < objectSprites.Length; i++)
            {
                objectSprites[i] = content.Load<Texture2D>($"BringerOfDeath/Fireball/Fireball{i+1}");
            }

            for (int i = 0; i < explosionSprites.Length; i++)
            {
                explosionSprites[i] = content.Load<Texture2D>($"BringerOfDeath/Fireball/Explosion{i + 1}");
            }

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
            CheckForRemove();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, 2f, SpriteEffects.None, 1f);

            if (objectSprites == explosionSprites)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, 4f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, 2f, SpriteEffects.None, 1f);
            }
        }

        public override void OnCollision(GameObject other)
        {

            if (Player.Blocking == true)
            {
                if (other is Player && !hasCollided)
                {
                    hasCollided = true;
                    animationTime = 0;
                    velocity = Vector2.Zero;
                    objectSprites = explosionSprites;
                }
            }

            if (Player.Dodging == false)
            {
                if (other is Player && !hasCollided)
                {
                    hasCollided = true;
                    animationTime = 0;
                    velocity = Vector2.Zero;
                    objectSprites = explosionSprites;
                }


                if (other is Beware && !hasCollided)
                {
                    hasCollided = true;
                    animationTime = 0;
                    velocity = Vector2.Zero;
                    objectSprites = explosionSprites;
                    other.ToBeRemoved = true;

                }

            }
        }

        private void CheckForRemove()
        {
            if (objectSprites[(int)animationTime] == explosionSprites[6])
            {
                toBeRemoved = true;
            }
        }
    }
}
