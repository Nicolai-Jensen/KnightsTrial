using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    public abstract class GameObject
    {
        //Fields
        protected Texture2D[] objectSprites;
        protected Vector2 position;
        protected Vector2 origin;
        protected Vector2 velocity;
        protected float animationTime;
        protected float animationSpeed = 8f;
        protected float scale;
        protected float speed;
        protected int layerDepth;
        protected bool toBeRemoved;
        //Properties

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        private Texture2D CurrentSprite
        {
            get { return objectSprites[(int)animationTime]; }
        }
        protected Vector2 SpriteSize
        {
            get { return new Vector2(CurrentSprite.Width * scale, CurrentSprite.Height * scale); }
        }
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(

                    (int)(position.X - SpriteSize.X / 2),
                    (int)(position.Y - SpriteSize.Y / 2),
                    (int)SpriteSize.X, (int)SpriteSize.Y);
            }
        }
        public Vector2 Position { get { return position; } set { position = value; } }
        public bool ToBeRemoved
        {
            get
            {
                return toBeRemoved;
            }
            set { toBeRemoved = value; }
        }
        //Constructors

        //Methods
        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
        public bool IsColliding(GameObject other)
        {
            if (this == other)
            {
                return false;
            }
            return CollisionBox.Intersects(other.CollisionBox);
        }
        public virtual void OnCollision(GameObject other)
        {

        }
        protected void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += ((velocity * speed) * deltaTime);
        }
        protected void Animate(GameTime gameTime)
        {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds * animationSpeed;

            if (animationTime > objectSprites.Length)
            {
                animationTime = 0;
            }
        }
        public bool IsOutOfBounds()
        {
            return (position.Y > GameWorld.ScreenSize.Y || position.X > GameWorld.ScreenSize.X || position.Y < -50 || position.X < -50);
        }
    }
}

