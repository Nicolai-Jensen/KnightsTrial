using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace KnightsTrial
{
    /// <summary>
    /// This is the class for the Bringer of Death boss.
    /// </summary>
    internal class BringerOfDeath : GameBoss
    {
        //Fields
        private Vector2 playerPosition;
        private float behaviourTimer;

        private Random rndBehaviour = new Random();

        //Random value to check timers up against.
        private int randomTimeCount;

        //Bool to see if animationTime should be reset to 0.
        private bool animationReset;

        private bool canRollAttack;
        private bool canRockPhase;
        private bool attackAnimationActive;
        public static bool isFacingLeft;
        private float phaseAttackTimer;

        //Bools to check if boss should enter a new phase and start PhaseBehaviour.
        private bool enterPhase;
        private bool canEnterPhase1;
        private bool canEnterPhase2;
        private bool canEnterPhase3;

        private bool dead = false;
        private bool canMelee = true;

        private SoundEffect fallingFire;
        private SoundEffect impactStone;
        private SoundEffect formingStone;
        private SoundEffect iceSound;
        private SoundEffect swingSound;
        private SoundEffect arcaneSound;


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
            position = new Vector2(GameWorld.ScreenSize.X / 2, 100);
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
            dead = false;

            //Animation arrays.
            swingAnimation = new Texture2D[10];
            walkAnimation = new Texture2D[8];
            magicAnimation = new Texture2D[9];
            deathAnimation = new Texture2D[10];
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
            for (int i = 0; i < deathAnimation.Length; i++)
            {
                deathAnimation[i] = content.Load<Texture2D>($"BringerOfDeath/BoDDeathAnimation/Bringer-of-Death_Death_{i + 1}");
            }

            objectSprites = walkAnimation;

            fallingFire = content.Load<SoundEffect>("SoundEffects/FireFallingSound");
            impactStone = content.Load<SoundEffect>("SoundEffects/StoneImpactSound");
            formingStone = content.Load<SoundEffect>("SoundEffects/StoneFormingSound");
            iceSound = content.Load<SoundEffect>("SoundEffects/IceSpawnSound");
            swingSound = content.Load<SoundEffect>("SoundEffects/SwordSwingBossSound");
            arcaneSound = content.Load<SoundEffect>("SoundEffects/BossProjectileSound");
        }

        public override void Update(GameTime gameTime)
        {
            //Updates the playerPosition variable by getting the players position.
            playerPosition = GetPlayer().Position;

            if (!dead)
            {
                Behaviour(gameTime);
                Move(gameTime);
            }

            Animate(gameTime);
            DamagedFeedBack(gameTime);
            HeavyDamaged(gameTime);

            CheckForDeath();
            BossDeath();

            SetOrigin();
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
        /// Gets the player object from the gameObject list in GameState.
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
        /// <summary>
        /// Checks if the boss should enter a new phase and controls which behaviour the boss
        /// should use depending on the status of the enterPhase bool.
        /// </summary>
        /// <param name="gameTime"></param>
        private void Behaviour(GameTime gameTime)
        {
            CheckForPhaseEnter();

            //If the enterPhase bool is true, the boss will use the PhaseBehaviour, otherwise it will use its standard MovementBehaviour.
            if (enterPhase)
            {
                PhaseBehaviour(gameTime);
            }
            else
            {
                MovementBehaviour(gameTime);
            }
        }
        /// <summary>
        /// Has the boss become invulnerable and move to the middle of the screen, after which it will stand still
        /// and begin to cast 5 rock pillars on the screen. Afterwards it will then spawn icicle walls untill
        /// no more rock pillars are left, and then exit the PhaseBehaviour.
        /// </summary>
        /// <param name="gameTime"></param>
        private void PhaseBehaviour(GameTime gameTime)
        {
            //If the boss is more than 10 pixels away from the middle of the screen it will move to the middle, set the color of the boss to blue
            // and set the currently used animation to the walk animation.
            if (Vector2.Distance(position, new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2)) > 10)
            {
                objectSprites = walkAnimation;
                color = Color.Blue;
                MoveToMiddle();
            }
            //if the distance between the boss and the middle of the screen is equal to or less than 10 pixels,
            //the boss will stop moving and the phaseAttackTimer will count up.
            if (Vector2.Distance(position, new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2)) <= 10)
            {
                phaseAttackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                velocity = new Vector2(0, 0);

                //If the canRockPhase bool is true, the boss instantiates 5 Beware objects on random locations that will then instantiate 5 rock pillars.
                //It also changes the current animation to magicAnimation, resets animationTime and sets the canRockPhase to false, so it wont run this code over and over again.
                if (canRockPhase)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Beware rockBeware = new Beware(new Vector2(rndBehaviour.Next(0, (int)GameWorld.ScreenSize.X), rndBehaviour.Next(0, (int)GameWorld.ScreenSize.Y)), true);
                        SoundEffectInstance stone = impactStone.CreateInstance();
                        stone.Volume = 0.2f;
                        stone.Play();
                        SoundEffectInstance stoneInstance = formingStone.CreateInstance();
                        stoneInstance.Volume = 0.3f;
                        stoneInstance.Play();
                    }
                    canRockPhase = false;
                    animationTime = 0;
                    objectSprites = magicAnimation;
                }

                //Checks to see if there are still rock pillars on the screen and more than 2 seconds has passed.
                //If both are true, the boss will spawn an icicle wall and reset the timer.
                if (phaseAttackTimer > 2 && CheckForRockPillars())
                {
                    IcicleWall();
                    phaseAttackTimer = 0;
                }

                //If the more than 5 seconds has passed since the last icicleWall and there are no more rock pillars on the screen,
                //The enterPhase bool will be set to false, to allow the boss to go back to MovementBehaviour, the current animation is set to walkAnimation,
                //the color is set back to white and the canRockPhase set to true to prepare for the next time a new phase happens.
                else if (phaseAttackTimer > 5 && !CheckForRockPillars())
                {
                    enterPhase = false;
                    objectSprites = walkAnimation;
                    color = Color.White;
                    canRockPhase = true;
                }
                
            }
        }
        /// <summary>
        /// Causes the boss to move towards the player for 3 seconds the first time (random between 1-3 seconds afterwards),
        /// after which it will roll a random attack based on the players proximity to the boss. The attack will be executed,
        /// and the method will prepare itself to be run again.
        /// </summary>
        /// <param name="gameTime"></param>
        private void MovementBehaviour(GameTime gameTime)
        {
            //Starts the behaviour timer.
            behaviourTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //If the behaviourTimer is less than the randomTimeCount(3 the first time) the current animation is set to walkAnimation,
            //and the boss will move towards the player.
            if (behaviourTimer < randomTimeCount)
            {
                objectSprites = walkAnimation;
                FollowPlayer();
            }
            //Else if the behaviourTimer is greater than the randomTimeCount with less than 1 the attackAnimationActive bool is set to true,
            //and a random attack is chosen based on the players proximity to the boss. The boss velocity is also set to 0,0 so that the boss wont move while attacking.
            else if (behaviourTimer > randomTimeCount && behaviourTimer < randomTimeCount + 1)
            {
                attackAnimationActive = true;

                //If the canRollAttack bool is true, an attack is rolled, and the canRollAttack bool is set to false,
                //the players current position is also saved in the cariable playerPositionTemp.
                if (canRollAttack)
                {
                    //If the players distance to the boss is less than or equal to 300 pixels, the randomAttack will get a value of 1-9.
                    if (Vector2.Distance(position, playerPosition) <= 300)
                    {
                        randomAttack = rndBehaviour.Next(1, 10);
                    }
                    //Else the randomAttack will get a value of 1-4.
                    else
                    {
                        randomAttack = rndBehaviour.Next(1, 5);
                    }
                    canRollAttack = false;
                    playerPosTemp = playerPosition;
                }

                //If the animationReset bool is true, the animationTime will be reset and the bool will be set to false.
                if (animationReset)
                {
                    animationTime = 0;
                    animationReset = false;
                }

                //If randomAttack is greater or equal to 5 the current animation will be set to swingAnimation (Melee attack animation).
                if (randomAttack >= 5)
                {
                    objectSprites = swingAnimation;
                }
                //Else the current animation will be set to magicAnimation (Ranged cast animation).
                else
                {
                    objectSprites = magicAnimation;
                }

                MeleeAttack();

                velocity = new(0, 0);
            }
            //If the behaviourTimer is greater than the randomTimeCount + 1 the AttackBehaviour method is called and a new randomTimeCount value is set.
            //The behaviourTimer is reset, as well as the bools animationReset and canRollAttack. The attackAnimationActive is also set to false.
            else if (behaviourTimer > randomTimeCount + 1)
            {
                AttackBehaviour();

                randomTimeCount = rndBehaviour.Next(1, 4);
                behaviourTimer = 0;
                animationReset = true;
                canRollAttack = true;
                attackAnimationActive = false;
                canMelee = true;
            }
        }

        /// <summary>
        /// Sets the origin of the boss based on the current animation as the boss is not centered in all of the sprites.
        /// If the animation used is not centered, the if-statements check to see if the player is to the left or right of the boss,
        /// after which it sets the origin accordingly.
        /// </summary>
        private void SetOrigin()
        {
            //Walk animation.
            if (objectSprites == walkAnimation)
            {
                origin = new Vector2(walkAnimation[0].Width / 2, walkAnimation[0].Height);
            }

            //Magic animation.
            else if (objectSprites == magicAnimation)
            {
                origin = new Vector2(magicAnimation[0].Width / 2, magicAnimation[0].Height);
            }

            //Swing animation.
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

            //Death animation.
            else if (objectSprites == deathAnimation)
            {
                if (playerPosition.X > position.X)
                {
                    origin = new Vector2(deathAnimation[0].Width / 4, deathAnimation[0].Height);
                }
                else
                {
                    origin = new Vector2(deathAnimation[0].Width - deathAnimation[0].Width / 4, deathAnimation[0].Height);
                }
            }

            //If none of the above are true.
            else
            {
                origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height);
            }
        }

        /// <summary>
        /// Sets the boss velocity to the normalized "playerPosition - position" vector2
        /// to have the boss move in the direction of the player.
        /// </summary>
        private void FollowPlayer()
        {
            Vector2 outputVelocity = playerPosition - position;
            outputVelocity.Normalize();
            velocity = outputVelocity;
        }

        /// <summary>
        /// Sets the boss velocity to the normalized "middle of the screen - position" vector2
        /// to have the boss move towards the middle of the screen.
        /// </summary>
        private void MoveToMiddle()
        {
            Vector2 outputVelocity = new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2) - position;
            outputVelocity.Normalize();
            velocity = outputVelocity;
        }

        /// <summary>
        /// Checks if the boss is dead by checking if health is less than or equal to 0 and the GameState bool "isBossAlive" is currently true.
        /// If it is, the current animation is set to deathAnimation, the GameState bool "isBossAlive" is set to false, and the animationTime is reset.
        /// </summary>
        private void CheckForDeath()
        {
            if (health <= 0 && dead == false)
            {
                animationTime = 0f;
                objectSprites = deathAnimation;
                dead = true;
            }
        }
        /// <summary>
        /// Checks to see if the current sprite in the animation is equal to th sprite in deathAnimation on index 9.
        /// if it is, the bool "toBeRemoved" is set to true, and the boss object is removed.
        /// </summary>
        private void BossDeath()
        {
            if (objectSprites[(int)animationTime] == deathAnimation[9])
            {
                toBeRemoved = true;
                GameState.isBossAlive = false;
            }

        }
        /// <summary>
        /// Loops through the gameObject list in GameState to see if any of the objects are rock pillars.
        /// if it finds one, the method will return true, else it will return false.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks to see if the boss should enter a new phase.
        /// </summary>
        private void CheckForPhaseEnter()
        {
            //if the boss' health is less than or equal to 25% of max and "canEnterPhase3" is true,
            //sets enterPhase to true, canEnterPhase3 to false and resets the phaseAttackTimer.
            if (health <= 2500*0.25 && canEnterPhase3)
            {
                enterPhase = true;
                canEnterPhase3 = false;
                phaseAttackTimer = 0f;
            }
            //if the boss' health is less than or equal to 50% of max and "canEnterPhase2" is true,
            //sets enterPhase to true, canEnterPhase2 to false and resets the phaseAttackTimer.
            else if (health <= 2500*0.5 && canEnterPhase2)
            {
                enterPhase = true;
                canEnterPhase2 = false;
                phaseAttackTimer = 0f;
            }
            //if the boss' health is less than or equal to 75% of max and "canEnterPhase1" is true,
            //sets enterPhase to true, canEnterPhase1 to false and resets the phaseAttackTimer.
            else if (health <= 2500*0.75 && canEnterPhase1)
            {
                enterPhase = true;
                canEnterPhase1 = false;
                phaseAttackTimer = 0f;
            }
        }

        //-----------------------ATTACKS & SPELLS-----------------------------

        /// <summary>
        /// Uses a random ranged attack based on the randomAttack variable.
        /// </summary>
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

        /// <summary>
        /// Instantiates an object of the Fireball class.
        /// </summary>
        public void RainOfFire()
        {
            Fireball rangedProjektile = new Fireball(playerPosition, new Vector2(playerPosition.X, playerPosition.Y - 1080), new Vector2(0, 1));
            SoundEffectInstance fallingFireInstance = fallingFire.CreateInstance();
            fallingFireInstance.Volume = 0.1f;
            fallingFireInstance.Play();
        }

        /// <summary>
        /// Instantiates several objects of the Icicle class in a line to to form a wall that moves across the screen from a random direction.
        /// </summary>
        public void IcicleWall()
        {
            //Sets icicleDirection to a random value between 1 and 4.
            int icicleDirection = rndBehaviour.Next(1, 5);

            SoundEffectInstance ice = iceSound.CreateInstance();
            ice.Volume = 0.1f;
            ice.Play();

            //Takes in the icicleDirection to choose a direction.
            switch (icicleDirection)
            {
                //From right.
                case 1:

                    //Sets icicleWallHoleRight to a random value between 2 and 14.
                    int icicleWallHoleRight = rndBehaviour.Next(2, 15);

                    //loops through 16 times and instantiates the next Icicle in the wall if "i" is not equal to the value of icicleWallHoleRight or icicleWallHoleRight +1,
                    //to ensure a hole of 2 objects in the wall, so that the player can safely get through. It then breaks out of the switch case.
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

                //From left.
                case 2:

                    //Sets icicleWallHoleLeft to a random value between 2 and 14.
                    int icicleWallHoleLeft = rndBehaviour.Next(2, 15);

                    //loops through 16 times and instantiates the next Icicle in the wall if "i" is not equal to the value of icicleWallHoleLeft or icicleWallHoleLeft +1,
                    //to ensure a hole of 2 objects in the wall, so that the player can safely get through. It then breaks out of the switch case.
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

                //From bottom.
                case 3:

                    //Sets icicleWallHoleBottom to a random value between 2 and 28.
                    int icicleWallHoleBottom = rndBehaviour.Next(2, 28);

                    //loops through 29 times and instantiates the next Icicle in the wall if "i" is not equal to the value of icicleWallHoleBottom or icicleWallHoleBottom +1,
                    //to ensure a hole of 2 objects in the wall, so that the player can safely get through. It then breaks out of the switch case.
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

                //From top.
                case 4:

                    //Sets icicleWallHoleTop to a random value between 2 and 28.
                    int icicleWallHoleTop = rndBehaviour.Next(2, 28);

                    //loops through 29 times and instantiates the next Icicle in the wall if "i" is not equal to the value of icicleWallHoleTop or icicleWallHoleTop +1,
                    //to ensure a hole of 2 objects in the wall, so that the player can safely get through. It then breaks out of the switch case.
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

        /// <summary>
        /// Instantiates an object of the class Beware with the "isRockPillar" bool set to true.
        /// </summary>
        public void PillarOfRock()
        {
            Beware rockBeware = new Beware(playerPosition, true);
            SoundEffectInstance stone = impactStone.CreateInstance();
            stone.Volume = 0.2f;
            stone.Play();
            SoundEffectInstance stoneInstance = formingStone.CreateInstance();
            stoneInstance.Volume = 0.3f;
            stoneInstance.Play();
        }

        /// <summary>
        /// Instantiates an obejct of the class RangedAttack, and adds it to the gameObjectsToAdd list in GameState.
        /// </summary>
        public void ArcaneMissile()
        {
            RangedAttack attack = new RangedAttack(new Vector2(position.X, position.Y - 100));
            GameState.InstantiateGameObject(attack);
            SoundEffectInstance arcane = arcaneSound.CreateInstance();
            arcane.Volume = 0.3f;
            arcane.Play();
        }

        /// <summary>
        /// If the animation is swingAnimation, the method instantiates an object of the class SwingProjectile when the animationtime reaches a value of 4.
        /// </summary>
        private void MeleeAttack()
        {
            if (objectSprites == swingAnimation && (int)animationTime == 4 && canMelee)
            {
                SwingProjectile melee = new SwingProjectile(position);
                SoundEffectInstance swing = swingSound.CreateInstance();
                swing.Volume = 0.3f;
                swing.Play();
                canMelee = false;
            }

        }
    }
}
