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
    internal class BringerOfDeath : GameBoss
    {
        //Fields
        private Vector2 playerPosition;
        private float behaviourTimer;
        private Random rndBehaviour = new Random();
        private int randomTimeCount;
        private bool animationReset;

        //Properties
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public override Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(

                    (int)(position.X - SpriteSize.X / 2),
                    (int)(position.Y - SpriteSize.Y),
                    (int)SpriteSize.X, (int)SpriteSize.Y);
            }
        }

        //Constructors
        public BringerOfDeath()
        {
            //GameState.InstantiateGameObject(this);
            health = 2500;
            speed = 100f;
            velocity = new Vector2(1, 0);
            position = new Vector2(200, 200);
            randomTimeCount = 3;
            animationReset = true;
            scale = 3f;

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

            MovementBehaviour(gameTime);
            Move(gameTime);
            Animate(gameTime);
            SetOrigin();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (playerPosition.X < position.X)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0.5f);
            }
            else
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, scale, SpriteEffects.FlipHorizontally, 0.5f);
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
                velocity = new(0, 0);
            }
            else if (behaviourTimer > randomTimeCount + 1)
            {
                AttackBehaviour(gameTime);
                randomTimeCount = rndBehaviour.Next(1, 6);
                behaviourTimer = 0;
                animationReset = true;
            }
        }

        private void SetOrigin()
        {
            if (objectSprites == walkAnimation)
            {
                origin = new Vector2(walkAnimation[0].Width / 2, walkAnimation[0].Height);
            }
            else if (objectSprites == magicAnimation)
            {
                origin = new Vector2(magicAnimation[0].Width / 2, magicAnimation[0].Height);
            }
            else
            {
                origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height);
            }
        }

        private void FollowPlayer()
        {
            Vector2 outputVelocity = playerPosition - position;
            outputVelocity.Normalize();
            velocity = outputVelocity;
        }


        //-----------------------ATTACKS & SPELLS-----------------------------

        private void AttackBehaviour(GameTime gameTime)
        {
            int randomAttack = rndBehaviour.Next(1, 3);

            switch (randomAttack)
            {
                case 1:
                    RainOfFire(gameTime);
                    break;

                case 2:
                    IcicleWall(gameTime);
                    break;

                default:
                    break;
            }
        }

        public void SwingAttack(GameTime gameTime)
        {
            //Normal attack
        }
        public void RainOfFire(GameTime gameTime)
        {
            Fireball rangedProjektile = new Fireball(playerPosition, new Vector2(playerPosition.X, playerPosition.Y - 1080), new Vector2(0, 1));
        }
        public void IcicleWall(GameTime gameTime)
        {

            int icicleDirection = rndBehaviour.Next(1, 5);

            switch (icicleDirection)
            {
                case 1:
                    //From right of screen

                    int icicleWallHoleRight = rndBehaviour.Next(1, 17);

                    for (int i = 0; i < 16; i++)
                    {
                        if (i != icicleWallHoleRight)
                        {
                            if (i != icicleWallHoleRight + 1)
                            {
                                Icicle icicleWallUnit = new Icicle(new Vector2(-1, 0), new Vector2(1920, i * 68), 1.55f);
                            }
                        }
                    }
                    break;

                case 2:
                    //From left of screen

                    int icicleWallHoleLeft = rndBehaviour.Next(1, 17);

                    for (int i = 0; i < 16; i++)
                    {
                        if (i != icicleWallHoleLeft)
                        {
                            if (i != icicleWallHoleLeft + 1)
                            {
                                Icicle icicleWallUnit = new Icicle(new Vector2(1, 0), new Vector2(0, i * 68), -1.55f);
                            }
                        }
                    }
                    break;

                case 3:
                    //From bottom of screen.

                    int icicleWallHoleBottom = rndBehaviour.Next(1, 30);

                    for (int i = 0; i < 29; i++)
                    {
                        if (i != icicleWallHoleBottom)
                        {
                            if (i != icicleWallHoleBottom + 1)
                            {
                                Icicle icicleWallUnit = new Icicle(new Vector2(0, -1), new Vector2(i * 68, 1080), 3.15f);
                            }
                        }
                    }
                    break;

                case 4:
                    //From top of screen.

                    int icicleWallHoleTop = rndBehaviour.Next(1, 30);

                    for (int i = 0; i < 29; i++)
                    {
                        if (i != icicleWallHoleTop)
                        {
                            if (i != icicleWallHoleTop + 1)
                            {
                                Icicle icicleWallUnit = new Icicle(new Vector2(0, 1), new Vector2(i * 68, 0), 0f);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public void PhaseShiftAttack(GameTime gameTime)
        {
            // Attack to run when boss enters a new phase.
        }
    }
}
