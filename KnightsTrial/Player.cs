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
        private SoundEffect attackingSound;
        private SoundEffect potionSound;
        private SoundEffect dodgeSound;
        private SoundEffect blockSound;
        private bool isFacingRight = false;
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

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < idleAnimation.Length; i++)
            {
                idleAnimation[i] = content.Load<Texture2D>($"PIdle{i}");
            }

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

            objectSprites = idleAnimation;

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
            Dodge(gameTime);
            SetDodgeVelocity();
            Move(gameTime);
            Animate(gameTime);
            ScreenWrap();
            Block(gameTime);
            StaminaRegen(gameTime);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //A Draw Method with different overloads, this particular one has 10 variables which can be defined

            //If the player has last pressed "D" to move right, it calls the first draw method, which doesn't flip the sprites
            if (isFacingRight)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, color, 0, origin, scale, SpriteEffects.None, 0);
            }
            //If the player has last pressed "A" to move left the draw method with the sprites flipped horizontally will be called
            else if (!isFacingRight)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, color, 0, origin, scale, SpriteEffects.FlipHorizontally, 0);
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
            if (dodging == false & blocking == false)
            {
                if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
                {
                    HeroWeaponCharge chargeSprite = new HeroWeaponCharge(new Vector2(position.X, position.Y));
                    GameState.InstantiateGameObject(chargeSprite);
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

            if (dodging == false)
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
            if (blocking != true && currentKey.IsKeyDown(Keys.Space) && previousKey.IsKeyUp(Keys.Space) && dodging == false && dodgeCooldown == false)
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
    }
}
