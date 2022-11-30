using KnightsTrial.Boss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Text;
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
        private bool canRollAttack;
        private bool attackAnimationActive;
        public static bool isFacingLeft;
        private bool enterPhase;
        private float phaseAttackTimer;
        private bool canEnterPhase1;
        private bool canEnterPhase2;
        private bool canEnterPhase3;
        private bool canRockPhase;

        private Vector2 playerPosTemp;

        private int randomAttack;

        //Properties
        public Color Color { get; }
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

                    (int)(position.X - SpriteSize.X / 3),
                    (int)(position.Y - SpriteSize.Y),
                    (int)SpriteSize.X / 2, (int)SpriteSize.Y);
            }
        }

        //Constructors
        public BringerOfDeath()
        {
            health = 2500;
            speed = 100f;
            velocity = new Vector2(1, 0);
            position = new Vector2(200, 200);
            randomTimeCount = 3;
            animationReset = true;
            scale = 3f;
            color = Color.White;
            canRollAttack = true;
            attackAnimationActive = false;
            enterPhase = false;
            GameState.isBossAlive = true;
            canEnterPhase1 = true;
            canEnterPhase2 = true;
            canEnterPhase3 = true;
            canRockPhase = true;

            swingAnimation = new Texture2D[10];
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
            for (int i = 0; i < swingAnimation.Length; i++)
            {
                swingAnimation[i] = content.Load<Texture2D>($"BringerOfDeath/SwingAttack/Bringer-of-Death_Attack_{i + 1}");
            }
            for (int i = 0; i < magicAnimation.Length; i++)
            {
                magicAnimation[i] = content.Load<Texture2D>($"BringerOfDeath/Bringer-of-Death_Cast_{i + 1}");
            }

            objectSprites = walkAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            playerPosition = GetPlayer().Position;

            Behaviour(gameTime);
            Move(gameTime);
            Animate(gameTime);
            SetOrigin();
            CheckForDeath();

            DamagedFeedBack(gameTime);
            HeavyDamaged(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!attackAnimationActive)
            {
                if (playerPosition.X < position.X)
                {
                    spriteBatch.Draw(objectSprites[(int)animationTime], position, null, color, 0f, origin, scale, SpriteEffects.None, 0.5f);
                    isFacingLeft = true;
                }
                else
                {
                    spriteBatch.Draw(objectSprites[(int)animationTime], position, null, color, 0f, origin, scale, SpriteEffects.FlipHorizontally, 0.5f);
                    isFacingLeft = false;
                }
            }
            else
            {
                if (playerPosTemp.X < position.X)
                {
                    spriteBatch.Draw(objectSprites[(int)animationTime], position, null, color, 0f, origin, scale, SpriteEffects.None, 0.5f);
                    isFacingLeft = true;
                }
                else
                {
                    spriteBatch.Draw(objectSprites[(int)animationTime], position, null, color, 0f, origin, scale, SpriteEffects.FlipHorizontally, 0.5f);
                    isFacingLeft = false;
                }
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

        private void Behaviour(GameTime gameTime)
        {
            CheckForPhaseEnter();

            if (enterPhase)
            {
                PhaseBehaviour(gameTime);
            }
            else
            {
                MovementBehaviour(gameTime);
            }
        }
        private void PhaseBehaviour(GameTime gameTime)
        {

            if (Vector2.Distance(position, new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2)) > 10)
            {
                objectSprites = walkAnimation;
                color = Color.Blue;
                MoveToMiddle();
            }
            if (Vector2.Distance(position, new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2)) <= 10)
            {
                phaseAttackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                velocity = new Vector2(0, 0);


                if (canRockPhase)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Beware rockBeware = new Beware(new Vector2(rndBehaviour.Next(0, (int)GameWorld.ScreenSize.X), rndBehaviour.Next(0, (int)GameWorld.ScreenSize.Y)), true);
                    }
                    canRockPhase = false;
                    animationTime = 0;
                    objectSprites = magicAnimation;
                }

                if (phaseAttackTimer > 2 && CheckForRockPillars())
                {
                    IcicleWall();
                    phaseAttackTimer = 0;
                }
                else if (phaseAttackTimer > 5 && !CheckForRockPillars())
                {
                    enterPhase = false;
                    objectSprites = walkAnimation;
                    color = Color.White;
                    canRockPhase = true;
                }
                
            }
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
                attackAnimationActive = true;

                if (canRollAttack)
                {
                    if (Vector2.Distance(position, playerPosition) <= 300)
                    {
                        randomAttack = rndBehaviour.Next(1, 10);
                    }
                    else
                    {
                        randomAttack = rndBehaviour.Next(1, 5);
                    }
                    canRollAttack = false;
                    playerPosTemp = playerPosition;
                }

                if (animationReset)
                {
                    animationTime = 0;
                    animationReset = false;
                }

                if (randomAttack >= 5)
                {
                    objectSprites = swingAnimation;
                }
                else
                {
                    objectSprites = magicAnimation;
                }

                MeleeAttack();

                velocity = new(0, 0);
            }
            else if (behaviourTimer > randomTimeCount + 1)
            {
                AttackBehaviour();

                randomTimeCount = rndBehaviour.Next(1, 4);
                behaviourTimer = 0;
                animationReset = true;
                canRollAttack = true;
                attackAnimationActive = false;
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
            else if (objectSprites == swingAnimation)
            {
                if (playerPosTemp.X > position.X)
                {
                    origin = new Vector2(swingAnimation[0].Width / 3, swingAnimation[0].Height);
                }
                else
                {
                    origin = new Vector2(swingAnimation[0].Width - swingAnimation[0].Width / 3, swingAnimation[0].Height);
                }
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

        private void MoveToMiddle()
        {
            Vector2 outputVelocity = new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2) - position;
            outputVelocity.Normalize();
            velocity = outputVelocity;
        }

        private void CheckForDeath()
        {
            if (health <= 0)
            {
                GameState.isBossAlive = false;
                toBeRemoved = true;
            }
        }

        private bool CheckForRockPillars()
        {
            foreach (GameObject go in GameState.gameObject)
            {
                if (go is RockPillar)
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckForPhaseEnter()
        {
            if (health <= 2500*0.25 && canEnterPhase3)
            {
                enterPhase = true;
                canEnterPhase3 = false;
                phaseAttackTimer = 0f;
            }
            else if (health <= 2500*0.5 && canEnterPhase2)
            {
                enterPhase = true;
                canEnterPhase2 = false;
                phaseAttackTimer = 0f;
            }
            else if (health <= 2500*0.75 && canEnterPhase1)
            {
                enterPhase = true;
                canEnterPhase1 = false;
                phaseAttackTimer = 0f;
            }
        }

        //-----------------------ATTACKS & SPELLS-----------------------------

        private void AttackBehaviour()
        {

            switch (randomAttack)
            {
                case 1:
                    RainOfFire();
                    break;

                case 2:
                    IcicleWall();
                    break;

                case 3:
                    PillarOfRock();
                    break;

                case 4:
                    ArcaneMissile();
                    break;

                default:
                    break;
            }
        }

        public void RainOfFire()
        {
            Fireball rangedProjektile = new Fireball(playerPosition, new Vector2(playerPosition.X, playerPosition.Y - 1080), new Vector2(0, 1));
        }
        public void IcicleWall()
        {

            int icicleDirection = rndBehaviour.Next(1, 5);

            switch (icicleDirection)
            {
                case 1:
                    //From right of screen

                    int icicleWallHoleRight = rndBehaviour.Next(2, 15);

                    for (int i = 0; i < 16; i++)
                    {
                        if (i != icicleWallHoleRight)
                        {
                            if (i != icicleWallHoleRight + 1)
                            {
                                Icicle icicleWallUnit = new Icicle(new Vector2(-1, 0), new Vector2(1920, i * 68 + 34), 1.55f);
                            }
                        }
                    }
                    break;

                case 2:
                    //From left of screen

                    int icicleWallHoleLeft = rndBehaviour.Next(2, 15);

                    for (int i = 0; i < 16; i++)
                    {
                        if (i != icicleWallHoleLeft)
                        {
                            if (i != icicleWallHoleLeft + 1)
                            {
                                Icicle icicleWallUnit = new Icicle(new Vector2(1, 0), new Vector2(0, i * 68 + 34), -1.55f);
                            }
                        }
                    }
                    break;

                case 3:
                    //From bottom of screen.

                    int icicleWallHoleBottom = rndBehaviour.Next(2, 28);

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

                    int icicleWallHoleTop = rndBehaviour.Next(2, 28);

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
        public void PillarOfRock()
        {
            Beware rockBeware = new Beware(playerPosition, true);
        }
        public void ArcaneMissile()
        {
            RangedAttack attack = new RangedAttack(new Vector2(position.X, position.Y - 100));
            GameState.InstantiateGameObject(attack);
        }
        private void MeleeAttack()
        {
            if (objectSprites == swingAnimation && (int)animationTime == 4)
            {
                SwingProjectile melee = new SwingProjectile(position);
            }
        }
    }
}
