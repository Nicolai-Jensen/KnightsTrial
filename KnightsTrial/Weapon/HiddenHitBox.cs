using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    class HiddenHitBox : Weapon
    {

        //Fields
        private Texture2D[] sprite;


        //Properties


        //Constructors

        public HiddenHitBox(Vector2 position)
        {
            rotation = 0f;
            speed = 0f;
            scale = 0.3f;
            velocity = Direction(ReturnPlayerPostition());
            this.position = position + velocity * 250;
            this.position.X -= 30;
            damageValue = 100;
            animationSpeed = 12f;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            sprite = new Texture2D[1];


            sprite[0] = content.Load<Texture2D>($"HeroWeapon/Thrust6");


            objectSprites = sprite;


            origin = new Vector2(objectSprites[0].Width / 10, objectSprites[0].Height / 2);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
        }
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
            EndAttack();
        }


        public void EndAttack()
        {
            if (Player.HeavyAtkAnim == false)
            {
                ToBeRemoved = true;
            }
        }
    }
}
