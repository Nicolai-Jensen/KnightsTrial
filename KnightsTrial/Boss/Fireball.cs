using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial.Boss
{
    internal class Fireball : GameObject
    {
        //Fields
        private int damageValue;
        //Properties

        //Constructors

        public Fireball(Vector2 spawnPosition, Vector2 projectileDirection)
        {
            GameState.InstantiateGameObject(this);
            position = spawnPosition;
            speed = 800f;
            velocity = projectileDirection;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            objectSprites = new Texture2D[4];

            for(int i = 0; i < objectSprites.Length; i++)
            {
                objectSprites[i] = content.Load<Texture2D>($"BringerOfDeath/Fireball/Fireball{i+1}");
            }

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, 2f, SpriteEffects.None, 1f);
        }
    }
}
