using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightsTrial
{/// <summary>
/// a Sub-class from Component, Used to draw out, UI in the game.
/// </summary>
    internal class UserInterface : Component
    {
        //Fields
        //Texture2D array for the Userinterface.
        private Texture2D[] uiComponents;

        private Vector2 position;
        private float scale;
        private float layerDepth;
        private Color color;


        //Properties

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, uiComponents[0].Width, uiComponents[0].Height);
            }
        }

        //Constructors
        public UserInterface(Texture2D[] texture, Vector2 posValue, float howbig, float depth)
        {
            uiComponents = texture;

            color = Color.White;

            position = posValue;

            scale = howbig;

            layerDepth = depth;
        }
        //Methods
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(uiComponents[0], position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }
    }
}
