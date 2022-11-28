using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace KnightsTrial
{
    class RangedAttack : GameObject
    {

        //Fields
        private Texture2D[] sprite;
        private float rotation;
        private int damageValue = 20;
        private bool homing = false;
        private float homingTimer;
        private bool hasDamaged;

        //Properties

        protected override Vector2 SpriteSize
        {
            get { return new Vector2(sprite[0].Width * scale / 4, sprite[0].Height * scale / 4); }
        }

        public override Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(

                    (int)(position.X - SpriteSize.X / 5 * 2),
                    (int)(position.Y - SpriteSize.Y / 4),
                    (int)SpriteSize.X, (int)SpriteSize.Y);
            }
        }
        //Constructors

        public RangedAttack(Vector2 position)
        {
            this.position = position;
            rotation = 0f;
            speed = 180f;
            scale = 2f;
            homing = true;
            animationSpeed = 60f;
            hasDamaged = false;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            sprite = new Texture2D[60];

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < sprite.Length; i++)
            {
                sprite[i] = content.Load<Texture2D>($"BringerOfDeath/RangedAttack/1_{i}");
            }

            objectSprites = sprite;

            origin = new Vector2(objectSprites[0].Width / 5 * 2, objectSprites[0].Height / 2);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
        }
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
            TravelTime(gameTime);
        }

        public void TravelTime(GameTime gameTime)
        {
            if (homing == true)
            {
                velocity = Direction(ReturnPlayerPostition());
            }
            homingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (homingTimer > 2f)
            {
                homing = false;
                homingTimer = 0;
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player && hasDamaged == false)
            {
                hasDamaged = true;

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

        protected Vector2 Direction(Vector2 playerPosition)
        {
            Vector2 direction;

            direction = playerPosition - position;
            direction.Normalize();
            rotation = (float)Math.Atan2(position.Y - playerPosition.Y, position.X - playerPosition.X) - 0.1f;

            return direction;
        }

        protected Vector2 ReturnPlayerPostition()
        {
            foreach (GameObject go in GameState.gameObject)
            {

                if (go is Player)
                {

                    return go.Position;
                }
            }

            return new Vector2(position.X, -100);
        }

    }
}
