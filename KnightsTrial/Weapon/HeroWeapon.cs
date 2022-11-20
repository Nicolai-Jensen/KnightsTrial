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
