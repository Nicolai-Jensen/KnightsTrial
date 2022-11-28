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
        private int damageValue = 25;
        private int health;
        //Properties

        public override Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(

                    (int)(position.X - SpriteSize.X / 4),
                    (int)(position.Y - SpriteSize.Y / 4 + 40),
                    (int)(SpriteSize.X - SpriteSize.X / 2), (int)(SpriteSize.Y - SpriteSize.Y / 3 - 40));
            }
        }

        //Constructors

        public RockPillar(Vector2 spawnPosition)
        {
            GameState.InstantiateGameObject(this);
            position = spawnPosition;
            scale = 1f;
            health = 5;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            rockPillarAnimation = new Texture2D[10];
            rockPillarStatic = new Texture2D[1];
            rockPillarStatic[0] = content.Load<Texture2D>("BringerOfDeath/RockPillarHoleAni10");

            for (int i = 0; i < rockPillarAnimation.Length; i++)
            {
                rockPillarAnimation[i] = content.Load<Texture2D>($"BringerOfDeath/RockPillarHoleAni{i+1}");
            }

            objectSprites = rockPillarAnimation;

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            AnimationSwap();
            Animate(gameTime);
            CheckForRemove();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0.5f);

        }
        private Player GetPlayer()
        {
            //loops through the gameObject list untill it finds the player, then returns it. 
            foreach (GameObject go in GameState.gameObject)
            {
                if (go is Player)
                {
                    return (Player)go;
                }
            }
            //if no player object is found, returns null.
            return null;
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player && objectSprites == rockPillarAnimation && GetPlayer().HealthModified == false)
            {
                if (Player.Blocking == true && Player.Dodging != true)
                {
                    if (GetPlayer().Stamina < damageValue)
                    {
                        GetPlayer().Health -= damageValue;
                        GetPlayer().HealthModified = true;
                    }
                    GetPlayer().Stamina -= damageValue * 2;
                }

                if (Player.Blocking == false && Player.Dodging == false)
                {
                    GetPlayer().Health -= damageValue;
                    GetPlayer().HealthModified = true;
                }
            }
            else if (other is Icicle)
            {
                health -= 1;
                other.ToBeRemoved = true;
            }
            else if (other is Beware)
            {
                other.ToBeRemoved = true;
            }
        }

        private void AnimationSwap()
        {
            if ((int)animationTime == 9)
            {
                animationTime = 0;
                objectSprites = rockPillarStatic;
            }
        }

        private void CheckForRemove()
        {
            if (health <= 0)
            {
                toBeRemoved = true;
            }
        }
    }
}
