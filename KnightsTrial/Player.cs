using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    internal class Player : GameObject
    {
        //Fields
        private int health;
        private int stamina;
        private bool healthModified = false;
        private bool regenStamina = true;
        private static bool blocking = false;
        private static bool dodging = false;
        private static bool attacking = false;
        private static bool heavyAtkAnim = false;
        private static bool lightAtkAnim = false;
        private static bool chargeAtkAnim = false;
        private bool dodgingAnim = false;
        private Color color;
        private Texture2D[] block;
        private Texture2D[] heroWeaponPrep;
        private Texture2D[] heroWeapon;
        private Texture2D[] heroWeapon2;
        private Texture2D[] idleAnimation;
        private Texture2D[] runAnimation;
        private Texture2D[] dodgeAnimation;
        private Texture2D[] blockAnimation;
        private Texture2D[] collisionSprite;
        private SoundEffect attackingSound;
        private SoundEffect potionSound;
        private SoundEffect dodgeSound;
        private SoundEffect blockSound;
        private  static bool isFacingRight = false;
        private bool hitCooldown = false;
        private float hitCooldownTimer;
        private float dodgeTimer;
        private bool dodgeCooldown = false;
        private float dodgeCooldownTimer;
        private bool attackCooldown = false;
        private float attackCooldownTimer;
        private float staminaRegenerating;

        private Vector2 dodgeVelocity;
        private MouseState currentMouse;
        private MouseState previousMouse;
        private KeyboardState currentKey;
        private KeyboardState previousKey;

        //Properties
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

        protected override Vector2 SpriteSize
        {
            get { return new Vector2(collisionSprite[0].Width * scale / 2, collisionSprite[0].Height * scale / 2); }
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
        public Player(Vector2 vector2)
        {
            position = vector2;
            scale = 2f;
            health = 100;
            stamina = 100;
            speed = 200f;
            color = Color.White;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            idleAnimation = new Texture2D[8];
            runAnimation = new Texture2D[8];
            blockAnimation = new Texture2D[3];
            dodgeAnimation = new Texture2D[10];
            heroWeapon = new Texture2D[5];
            heroWeapon2 = new Texture2D[5];
            heroWeaponPrep = new Texture2D[1];

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

            objectSprites = idleAnimation;
            collisionSprite = idleAnimation;


            //This line of code places the objects origin within the middle of the sprite assuming all sprites in the array share the same size
            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);

            //Loads the textures for the player



            block = new Texture2D[1];
            block[0] = content.Load<Texture2D>("Block");

        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            previousKey = currentKey;
            currentKey = Keyboard.GetState();

            HandleInput(gameTime);
            FacingAttack();
            Dodge(gameTime);
            SetDodgeVelocity();
            AttackingAnimations();
            Setorigin();
            Move(gameTime);
            Animate(gameTime);
            ScreenWrap();
            Block(gameTime);
            Attack(gameTime);
            StaminaRegen(gameTime);
            
        }

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

        private void HandleInput(GameTime gameTime)
        {
            if (dodging == false)
            {
                //velocity determines the direction the object is moving, this code sets the vector values to 0
                velocity = Vector2.Zero;

                //Keystate reads which key is being used
                KeyboardState keyState = Keyboard.GetState();


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

        public void Setorigin()
        {
            if (chargeAtkAnim == true)
            {

                if (isFacingRight != true)
                {
                    origin = new Vector2(objectSprites[0].Width - 16, objectSprites[0].Height - 16);
                }
                if (isFacingRight == true)
                {
                    origin = new Vector2(objectSprites[0].Width - 27, objectSprites[0].Height - 16);
                }               
            }

            if (heavyAtkAnim == true)
            {
                if (animationTime == 0 && isFacingRight != true)
                {
                    origin = new Vector2(objectSprites[0].Width - 12, objectSprites[0].Height - 16);
                }
                if (animationTime == 0 && isFacingRight == true)
                {
                    origin = new Vector2(objectSprites[0].Width - 31, objectSprites[0].Height - 16);
                }
                if (animationTime == 1 && isFacingRight != true)
                {
                    origin = new Vector2(objectSprites[0].Width - 13, objectSprites[0].Height - 16);
                }
                if (animationTime == 1 && isFacingRight == true)
                {
                    origin = new Vector2(objectSprites[0].Width - 30, objectSprites[0].Height - 16);
                }
                if (animationTime == 2 && isFacingRight != true)
                {
                    origin = new Vector2(objectSprites[0].Width - 14, objectSprites[0].Height - 16);
                }
                if (animationTime == 2 && isFacingRight == true)
                {
                    origin = new Vector2(objectSprites[0].Width - 29, objectSprites[0].Height - 16);
                }
                if (animationTime == 3 && isFacingRight != true)
                {
                    origin = new Vector2(objectSprites[0].Width - 15, objectSprites[0].Height - 16);
                }
                if (animationTime == 3 && isFacingRight == true)
                {
                    origin = new Vector2(objectSprites[0].Width - 28, objectSprites[0].Height - 16);
                }
            }

            if (lightAtkAnim == true)
            {

                if (isFacingRight != true)
                {
                    if (animationTime > 0)
                    {
                        origin = new Vector2(objectSprites[0].Width - 29, objectSprites[0].Height - 16);
                    }
                    if (animationTime > 1)
                    {
                        origin = new Vector2(objectSprites[1].Width - 30, objectSprites[0].Height - 12);
                    }
                    if (animationTime > 2)
                    {
                        origin = new Vector2(objectSprites[2].Width - 18, objectSprites[0].Height - 16);
                    }
                    if (animationTime > 3)
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

            if (chargeAtkAnim == false && heavyAtkAnim == false && lightAtkAnim == false)
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

        public override void OnCollision(GameObject other)
        {

        }

        public void Damaged(GameTime gameTime)
        {

        }

        public void Death()
        {

        }

        public void Attack(GameTime gameTime)
        {
            if (dodging == false && blocking == false && attacking == false && dodgingAnim == false)
            {
                if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
                {
                    HeroWeaponCharge chargeSprite = new HeroWeaponCharge(new Vector2(position.X, position.Y));
                    GameState.InstantiateGameObject(chargeSprite);
                    attacking = true;
                    chargeAtkAnim = true;
                }
            }
        }

        public void AttackingAnimations()
        {

            if (chargeAtkAnim == true)
            {
                objectSprites = heroWeaponPrep;
            }

            if (heavyAtkAnim == true)
            {
                animationSpeed = 8f;
                objectSprites = heroWeapon2;

                if(animationTime > 4)
                {
                    attacking = false;
                    heavyAtkAnim = false;
                }
            }

            if (lightAtkAnim == true)
            {
                animationSpeed = 8f;
                objectSprites = heroWeapon;

                if (animationTime > 4)
                {
                    attacking = false;
                    lightAtkAnim = false;
                }
            }
        }

        public void StaminaRegen(GameTime gameTime)
        {
            int regenTick;

            if (regenStamina == true && stamina < 100)
            {
                staminaRegenerating += (float)gameTime.ElapsedGameTime.TotalSeconds;

                regenTick = (int)staminaRegenerating % 2;
                if (regenTick == 1)
                {
                    stamina += 5;
                }
            }

        }

        public void Block(GameTime gameTime)
        {

            if (dodging == false && attacking == false && dodgingAnim == false)
            {


                if (currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released)
                {
                    Shield blockingSprite = new Shield(block[0], new Vector2(position.X, position.Y));
                    GameState.InstantiateGameObject(blockingSprite);
                }

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

        public void Dodge(GameTime gameTime)
        {
            if (blocking != true && attacking != true && currentKey.IsKeyDown(Keys.Space) && previousKey.IsKeyUp(Keys.Space) && dodging == false && dodgeCooldown == false)
            {
                dodging = true;
                dodgingAnim = true;
                dodgeCooldown = true;
                animationTime = 0;
                dodgeVelocity = velocity;
            }

            if (dodging == true)
            {
                dodgeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (dodgeTimer > 0.3f)
                {
                    dodging = false;
                    dodgeTimer = 0;                   
                }
            }

            if (dodgingAnim == true)
            {
                animationSpeed = 16f;
                objectSprites = dodgeAnimation;
                if (animationTime > 9)
                {
                    dodgingAnim = false;
                    animationSpeed = 8f;
                }
            }   
            
            if (dodgeCooldown == true)
            {
                dodgeCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (dodgeCooldownTimer > 1f)
            {
                dodgeCooldown = false;
                dodgeCooldownTimer = 0;
            }
        }

        public void SetDodgeVelocity()
        {
            if (dodging == true)
            {
                velocity = dodgeVelocity * 4f;
            }
        }

        public void UsePotion(GameTime gameTime)
        {

        }

        public void FacingAttack()
        {
            if (attacking == true)
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 mousePosition = mouseState.Position.ToVector2();

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
