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
        private int charges;
        private int healAmount;
        private float potionTimer;
        private bool usePotionAnimation;
        private int potionStatus;
        private int cooldownCounter;

        private SpriteFont cooldownFont;

        //Properties

        //Constructors

        public Potion()
        {
            objectSprites = new Texture2D[3];

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

            cooldownFont = content.Load<SpriteFont>("CooldownFont");
        }

        public override void Update(GameTime gameTime)
        {
            UsePotion(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[potionStatus], new(500, 500), null, Color.White, 0f, new(objectSprites[0].Width / 2, objectSprites[0].Height / 2), 1f, SpriteEffects.None, 1f);
            
            if (cooldownCounter > 0)
            {
                spriteBatch.DrawString(cooldownFont, $"{cooldownCounter}", new(500, 500), Color.White, 0f, new(0, 0), 0f, SpriteEffects.None, 2f);
            }
        }

        public void UsePotion(GameTime gameTime)
        {
            KeyboardState keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            potionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (potionTimer >= 1 && cooldownCounter != 0)
            {
                cooldownCounter--;
                potionTimer = 0;
            }

            if (cooldownCounter == 0 && keyState.IsKeyDown(Keys.Q))
            {
                cooldownCounter = 11;

                GetPlayer().Health += 70;

            }
        }

        private Player GetPlayer()
        {
            foreach (GameObject go in GameWorld.gameObject)
            {
                if (go is Player)
                {
                    return (Player)go;
                }
            }
            return null;
        }
    }
}
