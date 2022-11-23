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
    class HeroWeaponCharge : Weapon
    {
        //Fields
        private Texture2D[] chargeAnimation;
        //Properties

        //Constructors

        public HeroWeaponCharge(Texture2D sprite, Vector2 position)
        {
            objectSprites = new Texture2D[1];
            objectSprites[0] = sprite;
            this.position = position;
            rotation = 0f;
            speed = 0f;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            chargeAnimation = new Texture2D[4];

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < chargeAnimation.Length; i++)
            {
                chargeAnimation[i] = content.Load<Texture2D>($"HeroWeapoon/Shine{i}");
            }
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

    }
}
