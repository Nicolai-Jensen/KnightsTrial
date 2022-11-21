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

        private int damage;
        private bool attacked;
        private float attackedTimer;
        private float travelDistance;

        //Properties

        //Constructors

        public HeroWeapon(Texture2D sprite, Vector2 position)
        {
            objectSprites = new Texture2D[1];
            objectSprites[0] = sprite;
            this.position = position;
            rotation = 0f;
            speed = 0f;
            velocity = Direction(ReturnPlayerPostition());
            damage = 1;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

    }
}
