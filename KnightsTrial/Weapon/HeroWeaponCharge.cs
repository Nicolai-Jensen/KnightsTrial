using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace KnightsTrial
{
    /// <summary>
    /// The Charge class determines how the players attacks work, it instantiates the other forms of attack and is controlled by left click
    /// </summary>
    class HeroWeaponCharge : Weapon
    {
        //Fields

        //Different textures used for the different states of the charge animation
        private Texture2D[] chargeAnimation;
        private Texture2D[] chargeAnimation2;
        private Texture2D[] releaseAnimation;

        //The attacks Sound effects
        private SoundEffect lightAttack;
        private SoundEffect heavyAttack;

        //A Timer to determine which attack is instantiated 
        private float chargeTimer;

        //Variables used for the mouse to later avtivate single presses/releases
        private MouseState currentMouse;
        private MouseState previousMouse;
        //Properties

        //Constructors
        /// <summary>
        /// The constructor for the charge up object
        /// </summary>
        /// <param name="position">Position is the vector2 that determines where the object is spawned</param>
        public HeroWeaponCharge(Vector2 position)
        {
            damageValue = 0;
            this.position = position;
            rotation = 0f;
            speed = 0f;
            scale = 1f;
            animationSpeed = 4f;
        }

        //Methods
        /// <summary>
        /// This Method loads in information and code needed for the game, there is nothing in this one but it is needed anyway as Gameobject has it abstract
        /// </summary>
        /// <param name="content">Parameter given by the monogame framework for our use</param>
        public override void LoadContent(ContentManager content)
        {
            //Instantiates the arrays for the different sprite animations
            chargeAnimation = new Texture2D[2];
            chargeAnimation2 = new Texture2D[1];
            releaseAnimation = new Texture2D[5];


            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < releaseAnimation.Length; i++)
            {
                releaseAnimation[i] = content.Load<Texture2D>($"HeroWeapon/Shine{i + 1}");
            }

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < chargeAnimation.Length; i++)
            {
                chargeAnimation[i] = content.Load<Texture2D>($"HeroWeapon/Shine{i + 1}");
            }

            //ChargeAnimation2 is just one sprite that rotates
            chargeAnimation2[0] = content.Load<Texture2D>($"HeroWeapon/Shine3");

            //objectSprites is the base array Texture used in Draw and it is set during load to give the first animation
            objectSprites = chargeAnimation;

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);

            //Loads the Sounds for the object
            heavyAttack = content.Load<SoundEffect>("SoundEffects/HeavyAttackSound");
            lightAttack = content.Load<SoundEffect>("SoundEffects/LightAttackSound");
        }
        /// <summary>
        /// This Update Method constantly loops throughout the program aslong as it is running, other methods we want to be looped are called inside this one
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Move(gameTime);
            Animate(gameTime);
            Charge(gameTime);
            Release(gameTime);
        }
        /// <summary>
        /// The Method for Drawing out a sprite to the screen, this method is an override for the vitual one in GameObject and is called in GameWorld/GameState
        /// </summary>
        /// <param name="spriteBatch">Parameter given by the Monogame framework for the use of sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// This Method starts a ChargeTimer and puts the Players speed to a very slow amount, it also makes sure the charge object follows the player
        /// Since this Object is only initialized when leftmouse is pressed, this method is always activated the moment the object is made.
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public void Charge(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            //Updates playerSpeed and HeroWeaponCharge's position, while starting a timer
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                position = ReturnPlayerPostition();
                SetPlayerSpeed(20f);
                chargeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            }

            //At a certain point in the timer the sprite is changed to a different bigger animation to indicate they are at the heavy attack threshhold and can let go of left mouse
            if (chargeTimer > 0.7f)
            {
                objectSprites = chargeAnimation2;
                scale = 0.8f;
                rotation += 0.08f;
            }
        }

        /// <summary>
        /// This Method determines what happens when the Left mouse button is released
        /// The different outcomes include: A Light attack, A heavy Attack, or Nothing
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public void Release(GameTime gameTime)
        {
            //This if statement activates the Light attack, it needs the charge timer to be low and for the player to have above 0 stamina
            if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed && chargeTimer < 0.7f && GetPlayer().Stamina > 0)
            {
                //Instantiates a Light attack
                HeroWeapon slashSprite = new HeroWeapon(new Vector2(position.X, position.Y));
                GameState.InstantiateGameObject(slashSprite);
                SoundEffectInstance lightAttackInstance = lightAttack.CreateInstance();
                lightAttackInstance.Volume = 0.3f;
                lightAttackInstance.Play();

                //Resets the Timer
                chargeTimer = 0;

                //deactivates the players chargestate and activates his light attack state, while taking 20 stamina as that is the cost of a Light attack
                Player.ChargeAtkAnim = false;
                SetPlayerStamina(20);
                Player.LightAtkAnim = true;

                //Player animation time is set to 0 so that the light attack plays from its first sprite and so that animationTime can not result in Null
                SetPlayerAnimationTime(0);

                //Removes the Charge Object and sets player speed to a slightly lower than base speed for his Light attack
                ToBeRemoved = true;
                SetPlayerSpeed(150f);      
            }

            //This if statement activates the heavy attack, it needs the charge timer to be high and for the player to hae above 0 stamina
            if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed && chargeTimer > 0.7f && GetPlayer().Stamina > 0)
            {
                //Instantiates a Heavy Attack
                HeroWeaponHeavy thrust = new HeroWeaponHeavy(new Vector2(position.X, position.Y));
                GameState.InstantiateGameObject(thrust);
                SoundEffectInstance heavyAttackInstance = heavyAttack.CreateInstance();
                heavyAttackInstance.Volume = 0.2f;
                heavyAttackInstance.Play();

                //Deactivates the players chargestate and activates his heavy attack state, while taking 40 stamina for the cost of the attack
                Player.ChargeAtkAnim = false;
                SetPlayerStamina(40);
                Player.HeavyAtkAnim = true;

                //Instantiates 2 invisible objects that will act as the heavy attacks hitbox, they are hard coded to spawn along the thrust
                HiddenHitBox hitBox = new HiddenHitBox(new Vector2(position.X, position.Y));
                HiddenHitBox2 hitBox2 = new HiddenHitBox2(new Vector2(position.X, position.Y));
                GameState.InstantiateGameObject(hitBox2);
                GameState.InstantiateGameObject(hitBox);

                //Resets the Charge timer, sets player animation to 0 to avoid null and gives the HeroWeaponCharge a new animationspeed as its still needed
                SetPlayerAnimationTime(0);
                chargeTimer = 0;
                animationTime = 0;
                animationSpeed = 15f;

                //Uses the Direction method to face the position of the mouse along with slight tweaks with rotatoin
                Direction(ReturnPlayerPostition());
                rotation += 1.55f;

                //Gives the HeroWeaponCharge a different animation as visual feedback for the heavy attack, this helps give the attack ooomph
                objectSprites = releaseAnimation;

                //Sets the players speed to 0 so he can't move during the heavy attack
                SetPlayerSpeed(0f);
            }

            //If the player lacks the stamina to cast an attack nothing happens when releasing left mouse
            if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed && GetPlayer().Stamina <= 0)
            {
                //Disables players attacking states and resets the player to base state
                Player.Atacking = false;
                Player.ChargeAtkAnim = false;                
                Player.RegenStamina = true;

                //Removes the charge object and restarts the charge timer
                chargeTimer = 0;
                ToBeRemoved = true;
                SetPlayerSpeed(200f);
            }

            //Since the only animation in HeroWeaponCharge that goes over 4 is the feedback one on heavy attack, we can have this if statement here which will automatically remove it once its finished its job
            if (animationTime > 4)
            {
                ToBeRemoved = true;
            }

        }

    }
}
