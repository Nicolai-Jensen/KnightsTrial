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
            scale = 2f;
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
        }

        public override void Update(GameTime gameTime)
        {
            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);

            Move(gameTime);
            Animate(gameTime);
            CheckForRemove();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (objectSprites == explosionSprites)
            {

                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 1f);
                scale = 4f;
            }
            else
            {

                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 1f);
                scale = 2f;
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Beware && !hasCollided)
            {
                hasCollided = true;
                animationTime = 0;
                velocity = Vector2.Zero;
                objectSprites = explosionSprites;
                other.ToBeRemoved = true;
            }

            if (other is Player && objectSprites == explosionSprites && hasCollided)
            {
                hasCollided = false;


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
