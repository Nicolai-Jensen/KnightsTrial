using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightsTrial
{
    /// <summary>
    /// A class that determines how the controllable object "player" functions
    /// </summary>
    internal class Player : GameObject
    {
        //Fields

        //Player stats
        private int health;
        private int stamina;
        private bool dying = false;
        public static bool dead = false;

        //All of the bools that are used to activate processes in the Player class
        private bool healthModified = false;
        private bool hitCooldown = false;
        private bool dodgeCooldown = false;
        private bool attackCooldown = false;
        private bool dodgingAnim = false;
        private bool initiateDeath = false;
        private static bool regenStamina = true;
        private static bool blocking = false;
        private static bool dodging = false;
        private static bool attacking = false;
        private static bool heavyAtkAnim = false;
        private static bool lightAtkAnim = false;
        private static bool chargeAtkAnim = false;
        private static bool isFacingRight = false;
        public static bool godMode = false;
        
        //All of the different sprites that are cycled through to play different animations
        private Texture2D[] block;
        private Texture2D[] heroWeaponPrep;
        private Texture2D[] heroWeapon;
        private Texture2D[] heroWeapon2;
        private Texture2D[] idleAnimation;
        private Texture2D[] runAnimation;
        private Texture2D[] dodgeAnimation;
        private Texture2D[] blockAnimation;
        private Texture2D[] collisionSprite;
        private Texture2D[] deathAnimation;

        //All the SoundEffects that play through the player
        private SoundEffect dodgeSound;
        private SoundEffect runSound;
        private SoundEffect hurtSound;
        SoundEffectInstance runSoundInstance;

        //The Timers used for gameTime methods
        private float hitCooldownTimer;
        private float dodgeTimer;
        private float dodgeCooldownTimer;
        private float attackCooldownTimer;
        private float staminaRegenerating;

        //A Potion object that gets initiated in the players constructor
        private Potion potions;

        //Another direction for the dodge funktion and the State variables needed for single press If statements
        private Vector2 dodgeVelocity;
        private MouseState currentMouse;
        private MouseState previousMouse;
        private KeyboardState currentKey;
        private KeyboardState previousKey;
        private Texture2D[] currentSprite;
        private Texture2D[] previousSprite;

        //Properties

        //These are all used as a way for other classes to reference some of the field values of Player without making the fields themselves public
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int Stamina
        {
            get { return stamina; }
            set { stamina = value; }
        }

        public bool HealthModified
        {
            get { return healthModified; }
            set { healthModified = value; }
        }
        
        public bool HitCooldown
        {
            get { return hitCooldown; }
        }

        //An override to SpriteSize which is used for making the collisionBox, this override makes it so the players HitBox doesn't change when it plays animations, its also a bit smaller to give leeway
        protected override Vector2 SpriteSize
        {
            get { return new Vector2(collisionSprite[0].Width * scale / 2, collisionSprite[0].Height * scale / 2); }
        }

        public static bool RegenStamina
        {
            get { return regenStamina; }
            set { regenStamina = value; }
        }

        public static bool Blocking
        {
            get { return blocking; }
            set { blocking = value; }
        }

        public static bool Dodging
        {
            get { return dodging; }
            set { dodging = value; }
        }

        public static bool Atacking
        {
            get { return attacking; }
            set { attacking = value; }
        }
        
        public static bool HeavyAtkAnim
        {
            get { return heavyAtkAnim; }
            set { heavyAtkAnim = value; }
        }

        public static bool LightAtkAnim
        {
            get { return lightAtkAnim; }
            set { lightAtkAnim = value; }
        }

        public static bool ChargeAtkAnim
        {
            get { return chargeAtkAnim; }
            set { chargeAtkAnim = value; }
        }

        public static bool IsFacingRight
        {
            get { return isFacingRight; }
        }


        //Constructors
        /// <summary>
        /// The Player constructor, it sets up the spawn position(which you choose when initiating a player object) 
        /// then gives the player stats and variables a value for use, We instantiate a potion object here on the player
        /// </summary>
        /// <param name="vector2">This Vector2 parameterd determines where the player is spawned upon being initialized </param>
        public Player(Vector2 vector2)
        {
            position = vector2;
            scale = 2f;
            health = 100;
            stamina = 100;
            speed = 200f;
            color = Color.White;
            potions = new Potion();
            GameState.InstantiateGameObject(potions);
            chargeAtkAnim = false;
            attacking = false;
            blocking = false;
            dead = false;
        }

        //Methods

        /// <summary>
        /// This Method loads in information and code needed for the game
        /// </summary>
        /// <param name="content">Parameter given by the monogame framework for our use</param>
        public override void LoadContent(ContentManager content)
        {
            //All of the Sprite Arrays used for animations throughout Player
            idleAnimation = new Texture2D[8];
            runAnimation = new Texture2D[8];
            blockAnimation = new Texture2D[3];
            dodgeAnimation = new Texture2D[10];
            heroWeapon = new Texture2D[5];
            heroWeapon2 = new Texture2D[5];
            heroWeaponPrep = new Texture2D[1];
            deathAnimation = new Texture2D[9];

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < idleAnimation.Length; i++)
            {
                idleAnimation[i] = content.Load<Texture2D>($"PIdle{i}");
            }

            heroWeaponPrep[0] = content.Load<Texture2D>("PlayerAttackAnimations/Pthrust1");

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < runAnimation.Length; i++)
            {
                runAnimation[i] = content.Load<Texture2D>($"PRun{i}");
            }

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < blockAnimation.Length; i++)
            {
                blockAnimation[i] = content.Load<Texture2D>($"PShield{i}");
            }

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < dodgeAnimation.Length; i++)
            {
                dodgeAnimation[i] = content.Load<Texture2D>($"Dodge/Roll{i}");
            }

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < heroWeapon2.Length; i++)
            {
                heroWeapon2[i] = content.Load<Texture2D>($"PlayerAttackAnimations/Pthrust{i + 2}");
            }

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < heroWeapon.Length; i++)
            {
                heroWeapon[i] = content.Load<Texture2D>($"PlayerAttackAnimations/Pslash{i}");
            }

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < deathAnimation.Length; i++)
            {
                deathAnimation[i] = content.Load<Texture2D>($"PlayerDeathAnim/Pdeath{i}");
            }

            //objectSprites is the base array Texture used in Draw and it is set during load to give the first animation, CollisionSprite sets the collisionBox's base size by basing it on a sprite
            objectSprites = idleAnimation;
            collisionSprite = idleAnimation;


            //This line of code places the objects origin within the middle of the sprite assuming all sprites in the array share the same size, some don't and we have a way around that later on in the code.
            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);

            //Loads in the block Texture
            block = new Texture2D[1];
            block[0] = content.Load<Texture2D>("Block");

            //Loads all the players sound effects
            dodgeSound = content.Load<SoundEffect>("SoundEffects/RollSound");
            runSound = content.Load<SoundEffect>("SoundEffects/RunSound");
            hurtSound = content.Load<SoundEffect>("SoundEffects/HurtSound");
            runSoundInstance = runSound.CreateInstance();

        }

        /// <summary>
        /// This Update Method constantly loops throughout the program aslong as it is running, other methods we want to be looped are called inside this one
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public override void Update(GameTime gameTime)
        {
            //constantly updates which keys/mousebuttons are being pressed and released
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            previousKey = currentKey;
            currentKey = Keyboard.GetState();

            previousSprite = currentSprite;
            currentSprite = objectSprites;

            //All of the Players methods that need to be looped constantly are called here
            HandleInput(gameTime);
            FacingAttack();
            Dodge(gameTime);
            SetDodgeVelocity();
            AttackingAnimations();
            Death();
            RunningSound();
            Move(gameTime);
            Animate(gameTime);
            ScreenWrap();
            Block(gameTime);
            Attack(gameTime);
            StaminaRegen(gameTime);
            HealthCheck();
            Setorigin();
            Damaged(gameTime);
            UsePotion(gameTime);

        }


        /// <summary>
        /// The Method for Drawing out a sprite to the screen, this method is an override for the vitual one in GameObject and is called in GameWorld/GameState
        /// </summary>
        /// <param name="spriteBatch">Parameter given by the Monogame framework for the use of sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //A Draw Method with different overloads, this particular one has 10 variables which can be defined

            //If the player has last pressed "D" to move right, it calls the first draw method, which doesn't flip the sprites
            if (isFacingRight)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, color, 0, origin, scale, SpriteEffects.None, 0.5f);
            }
            //If the player has last pressed "A" to move left the draw method with the sprites flipped horizontally will be called
            else if (!isFacingRight)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, color, 0, origin, scale, SpriteEffects.FlipHorizontally, 0.5f);
            }
        }

        /// <summary>
        /// The Player movement Input is controlled in this Method
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        private void HandleInput(GameTime gameTime)
        {
            //Checks that the player is not currently executing a dodge, as they use the same variable for their funktions
            if (dodging == false && dying == false)
            {
                //velocity determines the direction the object is moving, this code sets the vector values to 0
                velocity = Vector2.Zero;

                //Keystate reads which key is being used
                KeyboardState keyState = Keyboard.GetState();

                //Using any of these movement keys will change the sprite texture into the running one
                //Moves the player up when pressing W by removing Y position value 
                if (keyState.IsKeyDown(Keys.W) && blocking != true)
                {
                    velocity += new Vector2(0, -1);
                    objectSprites = runAnimation;
                }

                //Moves the player left when pressing A by removing X position value, and sets the the bool to false to determine which draw method to use
                if (keyState.IsKeyDown(Keys.A) && blocking != true)
                {
                    velocity += new Vector2(-1, 0);
                    objectSprites = runAnimation;
                    isFacingRight = false;
                }
                //Moves the player right when pressing D by adding X position value, and sets the bool to true to determine which draw method to use
                if (keyState.IsKeyDown(Keys.D) && blocking != true)
                {
                    velocity += new Vector2(+1, 0);
                    objectSprites = runAnimation;
                    isFacingRight = true;
                }
                //Moves the player down when pressing S by adding Y position value 
                if (keyState.IsKeyDown(Keys.S) && blocking != true)
                {
                    velocity += new Vector2(0, +1);
                    objectSprites = runAnimation;
                }

                //Changes the Animation state back to idle if no keys are pressed
                if (!keyState.IsKeyDown(Keys.S) && !keyState.IsKeyDown(Keys.W) && !keyState.IsKeyDown(Keys.A) && !keyState.IsKeyDown(Keys.D) && blocking != true)
                {
                    objectSprites = idleAnimation;
                }

                //Code needed so that the objects speed isn't increased when moving diagonally
                if (velocity != Vector2.Zero)
                {
                    velocity.Normalize();
                }     
            }
        }

        /// <summary>
        /// This method is used for playing and deactivating the running sound effect
        /// </summary>
        public void RunningSound()
        {
            runSoundInstance.Volume = 0.3f;
            //currentKey.IsKeyDown(Keys.W) && previousKey.IsKeyUp(Keys.W) || currentKey.IsKeyDown(Keys.A) && previousKey.IsKeyUp(Keys.A) || currentKey.IsKeyDown(Keys.S) && previousKey.IsKeyUp(Keys.S) || currentKey.IsKeyDown(Keys.D) && previousKey.IsKeyUp(Keys.D)
            if (currentSprite == runAnimation && previousSprite != runAnimation)
            {
                runSoundInstance.Play();
            }

            if (objectSprites != runAnimation)
            {
                runSoundInstance.Stop();
            }
        }

        /// <summary>
        /// This method checks different bools to see which animations are playing and changes the origin point of the sprite to the correct one that lines up with the other sprites as these textures are different sizes
        /// </summary>
        public void Setorigin()
        {
            //The Chargeup animation is slightly wider than idle and there if its true needs its origin changed
            if (chargeAtkAnim == true)
            {
                //depending on the facing direction of the player the origin point is set to the correct position for this sprite
                if (isFacingRight != true)
                {
                    origin = new Vector2(objectSprites[0].Width - 16, objectSprites[0].Height - 16);
                }
                if (isFacingRight == true)
                {
                    origin = new Vector2(objectSprites[0].Width - 27, objectSprites[0].Height - 16);
                }               
            }

            //The heavy attack like the chargeup animation is also wider and needs its origin changed
            if (heavyAtkAnim == true)
            {
                //Depending on the direction of the player the origin point is changed to the correct position
                if (animationTime == 0 && isFacingRight != true)
                {
                    origin = new Vector2(objectSprites[0].Width - 12, objectSprites[0].Height - 16);
                }
                if (animationTime == 0 && isFacingRight == true)
                {
                    origin = new Vector2(objectSprites[0].Width - 31, objectSprites[0].Height - 16);
                }

            }

            //The Light attack animation is both wider and taller than all other player animations and need different origin points for each frame of the animation as we didn't have a pivot point for the Knight SpriteSheet
            if (lightAtkAnim == true)
            {
                //Depending on the Direction of the player it will set the originpoint for each frame, animationTime has to be converted to Int for the origin point changes to swap at the correct tiem
                if (isFacingRight != true)
                {
                    if ((int)animationTime == 0)
                    {
                        origin = new Vector2(objectSprites[0].Width - 29, objectSprites[0].Height - 16);
                    }
                    if ((int)animationTime == 1)
                    {
                        origin = new Vector2(objectSprites[1].Width - 30, objectSprites[0].Height - 12);
                    }
                    if ((int)animationTime == 2)
                    {
                        origin = new Vector2(objectSprites[2].Width - 18, objectSprites[0].Height - 16);
                    }
                    if ((int)animationTime == 3)
                    {
                        origin = new Vector2(objectSprites[3].Width - 18, objectSprites[0].Height - 16);
                    }
                }

                if (isFacingRight == true)
                {
                    if (animationTime > 0)
                    {
                        origin = new Vector2(objectSprites[0].Width - 13, objectSprites[0].Height - 16);
                    }
                    if (animationTime > 1)
                    {
                        origin = new Vector2(objectSprites[1].Width - 36, objectSprites[0].Height - 12);
                    }
                    if (animationTime > 2)
                    {
                        origin = new Vector2(objectSprites[2].Width - 36, objectSprites[0].Height - 16);
                    }
                    if (animationTime > 3)
                    {
                        origin = new Vector2(objectSprites[3].Width - 36, objectSprites[0].Height - 16);
                    }
                }
            }

            //If none of the above animations are in play it resets the origin Point to the base one
            if (chargeAtkAnim == false && heavyAtkAnim == false && lightAtkAnim == false && dying == false)
            {
                origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
            }

            //If the player is dying this origin is played
            if (dying == true)
            {
                origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
            }
        }

        /// <summary>
        /// This Method is used to make sure the object is not moving outside the screenbounds
        /// It does this with if statements checking if the objects position values are outside the bounders of the screen
        /// </summary>
        public void ScreenWrap()
        {
            //Checks if the sprite is moving outside the bottom of the screen and blocks it
            if (position.Y + objectSprites[0].Height / 2 * scale >= GameWorld.ScreenSize.Y)
            {
                position.Y = GameWorld.ScreenSize.Y - objectSprites[0].Height / 2 * scale;
            }
            //Checks if the sprite is moving outside the top of the screen and blocks it
            if (position.Y - objectSprites[0].Height / 2 * scale < 0)
            {
                position.Y = 0 + objectSprites[0].Height / 2 * scale;
            }
            //Checks if the sprite is moving outside the right of the screen and blocks it
            if (position.X + objectSprites[0].Width / 2 * scale >= GameWorld.ScreenSize.X)
            {
                position.X = GameWorld.ScreenSize.X - objectSprites[0].Width / 2 * scale;
            }
            //Checks if the sprite is moving outside the left of the screen and blocks it
            if (position.X - objectSprites[0].Width / 2 * scale < 0)
            {
                position.X = 0 + objectSprites[0].Width / 2 * scale;
            }
        }

        /// <summary>
        /// The OnCollision method is used for collision if the player has to react in anyway, however most of our collision is done in the bosses collision
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {
            if (other is BringerOfDeath || other is RockPillar)
            {
                Vector2 d = position - other.Position;
                position += 150 * d / (d.LengthSquared() + 1);
            }
        }

        /// <summary>
        /// A Method that waits for an enemy to damage the player
        /// when the player is damaged it makes the enemy unable to hit the player and marks the player as red
        /// a timer starts and when it is over the player is tangible again for enemies to hit reseting the timer and color.
        /// </summary>
        /// <param name="gameTime">A parameter from the Framework that acts as a timer</param>
        public void Damaged(GameTime gameTime)
        {
            //When Damaged by an enemy they set this value to true on their collision method
                if (healthModified == true)
                {
                    //when hit cooldown is true it turns the player red and prevents enemies from doing damage to the player. It also starts the a timer
                    hitCooldown = true;
                    if (hitCooldown == true)
                    {
                        color = Color.Red;
                    }
                    hitCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    //this timer if statement is hit almost immediately, making the code inside only able to play effectly once
                    if (hitCooldownTimer <= 0.05f)
                {
                    //plays a Sound effect that indicates the player has been hit
                    SoundEffectInstance hurtSoundInstance = hurtSound.CreateInstance();
                    hurtSoundInstance.Volume = 0.5f;
                    hurtSoundInstance.Play();
                }

                    //When the timer hits over this value it makes the player normal colored and enables them to be hit again
                    if (hitCooldownTimer >= 0.5f)
                    {
                        hitCooldown = false;
                        color = Color.White;
                        hitCooldownTimer = 0;
                        healthModified = false;
                    }
                }
        }

        /// <summary>
        /// This Method players the death animation when the players HP reaches 0
        /// </summary>
        public void Death()
        {
            if (initiateDeath == true)
            {
                AnimationTime = 0;
                objectSprites = deathAnimation;
                speed = 0f;
                initiateDeath = false;
            }

            if (dying == true)
            {
                if (animationTime > 8)
                {
                    dead = true;
                }
            }
        }

        /// <summary>
        /// This method determines the players attack and the cooldown between attacks
        /// </summary>
        /// <param name="gameTime">A parameter from the Framework that acts as a timer</param>
        public void Attack(GameTime gameTime)
        {
            //Checks that the player is not already doing an action
            if (dodging == false && blocking == false && attacking == false && dodgingAnim == false && attackCooldown == false && dying == false)
            {
                //Instantiates the WeaponCharge Object(which spawns the other attacks) and sets a new animation while enabling the attacking action. Also Disables stamina Regen while charging
                if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
                {
                    HeroWeaponCharge chargeSprite = new HeroWeaponCharge(new Vector2(position.X, position.Y));
                    GameState.InstantiateGameObject(chargeSprite);
                    attacking = true;
                    chargeAtkAnim = true;
                    regenStamina = false;
                }
            }

            //When an attack finishes a cooldown bool is activated which starts this timer that determines when you can attack again as it acts as an action.
            if (attackCooldown == true)
            {
                attackCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (attackCooldownTimer > 0.5f)
                {
                    attackCooldownTimer = 0;
                    attackCooldown = false;
                }
            }
        }

        /// <summary>
        /// When an attack is used/instantiated the player needs to change animation and play it, this method starts these animations and determines when the attack is finished
        /// </summary>
        public void AttackingAnimations()
        {

            if (health >= 0)
            {
                //The Charge animation when holding left click
                if (chargeAtkAnim == true)
                {
                    objectSprites = heroWeaponPrep;
                }

                //The heavy stab animation of the player when initiating a heavy attack
                if (heavyAtkAnim == true)
                {
                    animationSpeed = 8f;
                    objectSprites = heroWeapon2;

                    //When the animation is finished it marks the attack as finished and starts the cooldowntimer & stamina regen
                    if (animationTime > 4)
                    {
                        attacking = false;
                        heavyAtkAnim = false;
                        attackCooldown = true;
                        regenStamina = true;
                    }
                }

                //The Light attack swing animation of the player when initiating a light attack
                if (lightAtkAnim == true)
                {
                    animationSpeed = 8f;
                    objectSprites = heroWeapon;

                    //When the animation is finished it marks the attack as finished and starts the cooldowntimer & stamina regen
                    if (animationTime > 4)
                    {
                        attacking = false;
                        lightAtkAnim = false;
                        attackCooldown = true;
                        regenStamina = true;
                    }
                }
            }
        }

        /// <summary>
        /// The Potion heals a flat 70 hp when used, however the player shouldn't be able to go over 100 hp so this method stops that and sets any value over 100 to 100
        /// </summary>
        public void HealthCheck()
        {
            if (health > 100)
            {
                health = 100;
            }

            if (health <= 0 && !dying)
            {
                initiateDeath = true;
                dying = true;
                velocity = new Vector2(0,0);
            }
        }

        /// <summary>
        /// The Players Stamina is used by every action and this method regenerates stamina at a consistent pace, this method is sometimes disabled so you don't regen during certain actions
        /// </summary>
        /// <param name="gameTime">A parameter from the Framework that acts as a timer</param>
        public void StaminaRegen(GameTime gameTime)
        {
            //int regenTick;

            if (regenStamina == true && stamina < 100)
            {
                staminaRegenerating += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //regenTick = (int)staminaRegenerating % 3;
                //if (regenTick == 1)
                if (staminaRegenerating > 0.03f)
                {
                    stamina += 1;
                    staminaRegenerating = 0;
                }
            }

        }

        /// <summary>
        /// This Method is used for the block action, which stops your movement but prevents your hp from being damaged in exchange for stamina
        /// </summary>
        /// <param name="gameTime">A parameter from the Framework that acts as a timer</param>
        public void Block(GameTime gameTime)
        {

            //Checks that another action isn't currently playing
            if (dodging == false && attacking == false && dodgingAnim == false && dying == false)
            {
                //Instantiates a block object and sets stamina to not regen
                if (currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released)
                {
                    Shield blockingSprite = new Shield(block[0], new Vector2(position.X, position.Y));
                    GameState.InstantiateGameObject(blockingSprite);
                    regenStamina = false;
                }

                //Sets the texture to the blocking sprite
                if (blocking == true)
                {
                    if (animationTime > 3)
                    {
                        animationTime = 0;
                    }
                    objectSprites = blockAnimation;
                }
            }
        }

        /// <summary>
        /// This Method is used for the dodge action, locks your movement in one direction for a set time where you move faster and are unable to be hit
        /// </summary>
        /// <param name="gameTime">A parameter from the Framework that acts as a timer</param>
        public void Dodge(GameTime gameTime)
        {
            //checks that other actions are not currently in effect and then activates the dodge funktion
            if (blocking != true && attacking != true && currentKey.IsKeyDown(Keys.Space) && previousKey.IsKeyUp(Keys.Space) && dodging == false && dodgeCooldown == false && stamina > 0 && dying == false)
            {
                dodging = true;
                dodgingAnim = true;
                dodgeCooldown = true;
                regenStamina = false;
                stamina -= 20;
                animationTime = 0;
                dodgeVelocity = velocity;

                SoundEffectInstance newSoundInstance = dodgeSound.CreateInstance();
                newSoundInstance.Volume = 0.3f;
                newSoundInstance.Play();
            }

            //starts a timer that determines when the dodge is finished
            if (dodging == true)
            {
                dodgeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (dodgeTimer > 0.3f)
                {
                    dodging = false;
                    dodgeTimer = 0;                   
                }
            }

            //Changes the player animation to the dodge animation and plays it, once played stamina is reactivated and the base animations are played again
            if (dodgingAnim == true)
            {
                animationSpeed = 16f;
                objectSprites = dodgeAnimation;
                if (animationTime > 9)
                {
                    dodgingAnim = false;
                    regenStamina = true;
                    animationSpeed = 8f;
                }
            }   
            
            //A cooldown timer which is activated to make sure the player can't spam their dodge 24/7
            if (dodgeCooldown == true)
            {
                dodgeCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            //The Timer for the cooldown
            if (dodgeCooldownTimer > 1f)
            {
                dodgeCooldown = false;
                dodgeCooldownTimer = 0;
            }
        }

        /// <summary>
        /// Sets the speed and direction of the dodge
        /// </summary>
        public void SetDodgeVelocity()
        {
            if (dodging == true)
            {
                velocity = dodgeVelocity * 4f;
            }
        }

        /// <summary>
        /// A Method that was going to be used for using a potion however that action is done inside the potion class, its here incase we want to add visuelle feedback from the player like a drinking animation or particle effects
        /// </summary>
        /// <param name="gameTime">A parameter from the Framework that acts as a timer</param>
        public void UsePotion(GameTime gameTime)
        {

        }

        /// <summary>
        /// This Method makes the player face the direction of his attack, this is done because otherwise he would only face the direction he moves
        /// </summary>
        public void FacingAttack()
        {
            //Checks if the Player is attacking
            if (attacking == true)
            {
                //Uses a static class extension which we made to convert a point to a vector2 value.
                MouseState mouseState = Mouse.GetState();
                Vector2 mousePosition = mouseState.Position.ToVector2();

                //Checks the position of the mouse and turns the player in the direction of it, since attacks also track mouse this faces the player in the attacking direction
                if (mousePosition.X > position.X)
                {
                    isFacingRight = true;
                }

                if (mousePosition.X < position.X)
                {
                    isFacingRight = false;
                }
            }
        }
    }
}
