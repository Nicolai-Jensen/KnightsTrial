using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightsTrial
{
    internal class Shield : Weapon
    {
        //Fields

        private int blockValue;

        private MouseState currentMouse;
        private MouseState previousMouse;

        //Properties

        //Constructors

        public Shield(Texture2D sprite, Vector2 position)
        {
            objectSprites = new Texture2D[1];
            objectSprites[0] = sprite;
            scale = 0.1f;

            this.position = position;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            HoldBlock(gameTime);
            RemoveShield();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.Aqua * 0.4f, 0, origin, scale, SpriteEffects.None, 1f);
        }

        public void HoldBlock(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                position = ReturnPlayerPostition();

                SetPlayerSpeed(0f);
                Player.Blocking = true;
            }
        }

        public void RemoveShield()
        {
            if (currentMouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
            {
                ToBeRemoved = true;
                Player.Blocking = false;
                SetPlayerSpeed(200f);
            }
        }

    }
}
