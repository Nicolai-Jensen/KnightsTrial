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
        private Texture2D[] uiComponents;
        private SpriteFont gameFont;


        //Properties
        public Vector2 Position { get; set; }

        public Color PenColor { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, uiComponents[0].Width, uiComponents[0].Height);
            }
        }
        public string Text;



        //Constructors
        public UserInterface(Texture2D[] texture)
        {
            uiComponents = texture;

            PenColor = Color.White;
        }
        //Methods

        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(uiComponents[0], Vector2.Zero, Color.White);

            if (!string.IsNullOrEmpty(Text))
            {
                float x = (Rectangle.X + (Rectangle.Width / 2)) - (gameFont.MeasureString(Text).X / 2);
                float y = (Rectangle.Y + (Rectangle.Height / 2)) - (gameFont.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(gameFont, Text, new Vector2(x, y), PenColor);
            }
        }
    }
}
