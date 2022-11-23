using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;

namespace KnightsTrial
{
    class HeroWeaponCharge : Weapon
    {
        //Fields
        private Texture2D[] chargeAnimation;
        //Properties

        //Constructors

        public HeroWeaponCharge(Vector2 position)
        {
          
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

            objectSprites = chargeAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

    }
}
