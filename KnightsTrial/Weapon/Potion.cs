using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightsTrial
{
    /// <summary>
    /// A class for the potion object. Can be used three times by pressing "Q" when playing.
    /// Has a short cooldown on use.
    /// </summary>
    internal class Potion : Weapon
    {
        //Fields
        private int healAmount;
        private float potionTimer;
        private SpriteFont cooldownFont;
        private SoundEffect potionSound;

        //Controlls the sprite chosen for the draw method as well as the amount of uses/charges left in the potion.
        private int potionStatus;

        //Counter to show the cooldown on the potion.
        private int cooldownCounter;

        //Properties

        //Constructors

        public Potion()
        {
            objectSprites = new Texture2D[4];

            potionStatus = 0;
            healAmount = 70;
            cooldownCounter = 0;
        }
        //Methods

        public override void LoadContent(ContentManager content)
        {
            objectSprites[0] = content.Load<Texture2D>("Potion/HealthPotion1");
            objectSprites[1] = content.Load<Texture2D>("Potion/HealthPotion2");
            objectSprites[2] = content.Load<Texture2D>("Potion/HealthPotion3");
            objectSprites[3] = content.Load<Texture2D>("Potion/HealthPotion4");

            cooldownFont = content.Load<SpriteFont>("CooldownFont");
            potionSound = content.Load<SoundEffect>("SoundEffects/PotionSound");
        }

        public override void Update(GameTime gameTime)
        {
            UsePotion(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[potionStatus], new(108, 225), null, Color.White, 0f, new(objectSprites[0].Width / 2, objectSprites[0].Height / 2), 1f, SpriteEffects.None, 0.8f);

            if (cooldownCounter > 0)
            {
                spriteBatch.DrawString(cooldownFont, $": {cooldownCounter}", new(135, 205), Color.Red, 0f, new(0, 0), 1f, SpriteEffects.None, 0.8f);
            }
        }

        /// <summary>
        /// Uses the potion if it is off cooldown and the player presses "Q".
        /// It then removes "use" charge from the potion.
        /// </summary>
        /// <param name="gameTime"></param>
        private void UsePotion(GameTime gameTime)
        {
            //Gets the keyboardState and saves it in the keyState variable.
            KeyboardState keyState = Keyboard.GetState();

            //begins the timer.
            potionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //If the timer has passed 1 second and the cooldownCounter is not 0 it will decrease the counter by 1 and reset the timer.
            if (potionTimer >= 1 && cooldownCounter != 0)
            {
                cooldownCounter--;
                potionTimer = 0;
            }

            //If the counter is equal to 0 and The "Q" key is pressed as well as the potionStatus(Charges) being less than or equal to 3
            //it will reset the cooldownCounter, add 1 to the potionStatus and increase player health with the specified amount.
            if (cooldownCounter == 0 && keyState.IsKeyDown(Keys.Q) && potionStatus <= 2)
            {
                cooldownCounter = 10;
                potionStatus++;
                GetPlayer().Health += healAmount;

                SoundEffectInstance potionInstance = potionSound.CreateInstance();
                potionInstance.Play();
            }
        }

    }
}
