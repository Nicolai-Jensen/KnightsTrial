using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial.Boss
{
    internal class BringerOfDeath : Boss
    {
        //Fields

        //Properties

        //Constructors
        public BringerOfDeath()
        {
            health = 2500;
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
        public void SwingAttack(GameTime gameTime)
        {
            //Normal attack
        }
        public void MagicAttack(GameTime gameTime)
        {
            // Ranged attack
        }
        public void AOEAttack(GameTime gameTime)
        {
            // Area of effect attack.
        }
    }
}
