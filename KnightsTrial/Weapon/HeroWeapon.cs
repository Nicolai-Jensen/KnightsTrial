using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace KnightsTrial
{
    internal class HeroWeapon : Weapon
    {
        //Fields
        private Texture2D[] sprite;



        //Properties

        //Constructors

        public HeroWeapon(Vector2 position)
        {
            damageValue = 30;
            this.position = position;
            rotation = -1.5f;
            speed = 0f;
            scale = 2f;
            animationSpeed = 12f;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            sprite = new Texture2D[5];

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < sprite.Length; i++)
            {
                sprite[i] = content.Load<Texture2D>($"HeroWeapon/Slash{i}");
            }

            objectSprites = sprite;

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
            ChooseDirection();
            EndAttack();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Player.IsFacingRight == true)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.AntiqueWhite, rotation, origin, scale, SpriteEffects.None, 1f);
            }

            if (Player.IsFacingRight != true)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.AntiqueWhite, rotation, origin, scale, SpriteEffects.FlipVertically, 1f);
            }
            
        }


        public void ChooseDirection()
        {
            if (Player.IsFacingRight == true)
            {
                position.X = ReturnPlayerPostition().X + 50;
            }

            if (Player.IsFacingRight != true)
            {
                position.X = ReturnPlayerPostition().X - 50;
            }
        }

        public void EndAttack()
        {
            if (animationTime > 4)
            {
                ToBeRemoved = true;
                SetPlayerSpeed(200f);
                Player.Atacking = false;
            }
        }
    }
}
