using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    internal class RockPillar : GameObject
    {
        //Fields
        private Texture2D[] rockPillarAnimation;
        private Texture2D[] rockPillarStatic;
        //Properties

        //Constructors

        public RockPillar(Vector2 spawnPosition)
        {
            GameState.InstantiateGameObject(this);
            position = spawnPosition;
            scale = 1f;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            rockPillarAnimation = new Texture2D[10];
            rockPillarStatic[0] = content.Load<Texture2D>("BringerOfDeath/RockPillarHoleAni10");

            for (int i = 0; i < rockPillarAnimation.Length; i++)
            {
                content.Load<Texture2D>($"BringerOfDeath/RockPillarHoleAni{i+1}");
            }

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);

            objectSprites = rockPillarAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            AnimationSwap();
            Animate(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 1f);

        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player && objectSprites == rockPillarAnimation)
            {
                (other as Player).Health -= 25;
            }
        }

        private void AnimationSwap()
        {
            if ((int)animationTime >= rockPillarAnimation.Length)
            {
                animationTime = 0;
                objectSprites = rockPillarStatic;
            }
        }
    }
}
