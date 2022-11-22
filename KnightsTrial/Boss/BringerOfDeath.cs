using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    internal class BringerOfDeath : Boss
    {
        //Fields
        private Vector2 playerPosition;
        private float behaviourTimer;
        private Random rndBehaviour = new Random();
        private int randomTimeCount;
        private bool animationReset;

        //Properties

        //Constructors
        public BringerOfDeath()
        {
            GameState.InstantiateGameObject(this);
            health = 2500;
            speed = 120f;
            velocity = new Vector2(1, 0);
            position = new Vector2(200, 200);
            randomTimeCount = 3;
            animationReset = true;

            walkAnimation = new Texture2D[8];
            magicAnimation = new Texture2D[9];
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            for (int i = 0; i < walkAnimation.Length; i++)
            {
                walkAnimation[i] = content.Load<Texture2D>($"BringerOfDeath/Bosswalk_{i}");
            }
            for (int i = 0; i < magicAnimation.Length; i++)
            {
                magicAnimation[i] = content.Load<Texture2D>($"BringerOfDeath/Bringer-of-Death_Cast_{i+1}");
            }

            objectSprites = walkAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            playerPosition = GetPlayer().Position;
            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height);

            MovementBehaviour(gameTime);
            Move(gameTime);
            Animate(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (playerPosition.X < position.X)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, 3f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, 3f, SpriteEffects.FlipHorizontally, 1f);
            }
        }

        /// <summary>
        /// Gets the player object from the gameObject list in GameWorld.
        /// If there are no player objects in the list, the methods returns null.
        /// </summary>
        /// <returns>The player object</returns>
        private Player GetPlayer()
        {
            //loops through the gameObject list untill it finds the player, then returns it. 
            foreach (GameObject go in GameState.gameObject)
            {
                if (go is Player)
                {
                    return (Player)go;
                }
            }
            //if no player object is found, returns null.
            return null;
        }

        private void MovementBehaviour(GameTime gameTime)
        {
            behaviourTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (behaviourTimer < randomTimeCount)
            {
                objectSprites = walkAnimation;
                origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height);
                FollowPlayer();
            }
            else if (behaviourTimer > randomTimeCount && behaviourTimer < randomTimeCount + 1)
            {
                if (animationReset)
                {
                    animationTime = 0;
                    animationReset = false;
                }

                objectSprites = magicAnimation;
                origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height);
                velocity = new(0, 0);
            }
            else if (behaviourTimer > randomTimeCount + 1)
            {
                randomTimeCount = rndBehaviour.Next(1, 6);
                behaviourTimer = 0;
                animationReset = true;
            }
        }

        private void FollowPlayer()
        {
            Vector2 outputVelocity = playerPosition - position;
            outputVelocity.Normalize();
            velocity = outputVelocity;
        }


        //-----------------------ATTACKS & SPELLS-----------------------------


        public void SwingAttack(GameTime gameTime)
        {
            //Normal attack
        }
        public void MagicAttack(GameTime gameTime)
        {
            // Ranged attack
        }
        public void AOEAttack(GameTime gameTime)
        {
            // Area of effect attack.
        }
        public void PhaseShiftAttack(GameTime gameTime)
        {

        }
    }
}
