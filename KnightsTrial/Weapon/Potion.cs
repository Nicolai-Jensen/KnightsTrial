using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace KnightsTrial
{
    internal class Potion : Weapon
    {
        //Fields
        private int healAmount;
        private float potionTimer;
        private SpriteFont cooldownFont;

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
            objectSprites[0] = content.Load<Texture2D>("Potion1");
            objectSprites[1] = content.Load<Texture2D>("Potion2");
            objectSprites[2] = content.Load<Texture2D>("Potion3");
            objectSprites[3] = content.Load<Texture2D>("Potion4");

            cooldownFont = content.Load<SpriteFont>("CooldownFont");
        }

        public override void Update(GameTime gameTime)
        {
            UsePotion(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[potionStatus], new(100, 200), null, Color.White, 0f, new(objectSprites[0].Width / 2, objectSprites[0].Height / 2), 1f, SpriteEffects.None, 1f);

            if (cooldownCounter > 0)
            {
                spriteBatch.DrawString(cooldownFont, $"{cooldownCounter}", new(500, 500), Color.White, 0f, new(0, 0), 0f, SpriteEffects.None, 2f);
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
            KeyboardState keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

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
                cooldownCounter = 11;
                potionStatus++;
                GetPlayer().Health += healAmount;

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
    }
}
