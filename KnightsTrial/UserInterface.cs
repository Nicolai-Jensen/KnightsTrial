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
    internal class UserInterface : Component
    {
        //Fields
        private Texture2D[] UIcomponents;
        private SpriteFont gameFont;


        //Properties

        //Constructors
        public UserInterface(Texture2D[] texture)
        {
            UIcomponents = texture;
        }
        //Methods
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UIcomponents[0], Vector2.Zero, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
