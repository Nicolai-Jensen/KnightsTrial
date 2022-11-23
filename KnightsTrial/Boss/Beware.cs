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
    internal class Beware : GameObject
    {
        //Fields

        //Properties

        //Constructors

        public Beware(Vector2 inputPosition)
        {
            GameState.InstantiateGameObject(this);
            position = inputPosition;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            objectSprites = new Texture2D[4];

            for (int i = 0; i < objectSprites.Length; i++)
            {
                objectSprites[i] = content.Load<Texture2D>($"Beware/Beware{i+1}");
            }

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);

        }

        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 1f);

        }
    }
}
