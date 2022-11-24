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
    class HeroWeaponHeavy : Weapon
    {

        //Fields
        private Texture2D[] sprite;
        private int damage;


        //Properties

        //Constructors

        public HeroWeaponHeavy(Vector2 position)
        {
            this.position = position;
            rotation = 0f;
            speed = 0f;
            scale = 1.2f;
            velocity = Direction(ReturnPlayerPostition());
            damage = 5;
            animationSpeed = 12f;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            sprite = new Texture2D[7];

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < sprite.Length; i++)
            {
                sprite[i] = content.Load<Texture2D>($"HeroWeapon/Thrust{i}");
            }

            objectSprites = sprite;

            origin = new Vector2(objectSprites[0].Width / 10 , objectSprites[0].Height / 2);
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
            if (animationTime > 6)
            {
                ToBeRemoved = true;
                SetPlayerSpeed(200f);
                //Player.Atacking = false;
                //Player.HeavyAtkAnim = false;
            }
        }
    }
}
