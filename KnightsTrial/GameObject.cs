using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightsTrial
{
    /// <summary>
    /// A abstract superclass used to put most objects in a gameobjectlist.
    /// </summary>
    public abstract class GameObject
    {
        //Fields
        //Texture array used for sprites, and animation.
        protected Texture2D[] objectSprites;

        //Different fields used for the drawing method, and constructors.
        protected Vector2 position;
        protected Vector2 origin;
        protected Vector2 velocity;
        protected float scale;
        protected float speed;
        protected Color color;
        protected int layerDepth;

        //Timer used for the animate and draw method.
        protected float animationTime;

        //animationSpeed used for how fast the animation should run.
        protected float animationSpeed = 8f;

        //bool if the object should be removed.
        protected bool toBeRemoved;

        //Soundeffect for the block.
        protected SoundEffect blockSound;
        //Properties

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float AnimationTime
        {
            get { return animationTime; }
            set { animationTime = value; }
        }
        private Texture2D CurrentSprite
        {
            get { return objectSprites[(int)animationTime]; }
        }
        protected virtual Vector2 SpriteSize
        {
            get { return new Vector2(CurrentSprite.Width * scale, CurrentSprite.Height * scale); }
        }
        public virtual Rectangle CollisionBox
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
        
        //---------Abstract methods our subclasses ís going to have.
        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);
        //----------------------------------------------------------

        /// <summary>
        /// Virtual draw method used for overriding in the subclasses.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// If a gameobject collides with a other gameobject, return true.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsColliding(GameObject other)
        {
            if (this == other)
            {
                return false;
            }
            return CollisionBox.Intersects(other.CollisionBox);
        }

        /// <summary>
        /// Virtual onCollision method used for overriding in the subclasses.
        /// </summary>
        /// <param name="other"></param>
        public virtual void OnCollision(GameObject other)
        {

        }
        /// <summary>
        /// Uses a timer, speed and velocity to calculate the new position.
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += ((velocity * speed) * deltaTime);
        }
        /// <summary>
        /// Uses the animationTime to animate throu over texture loops, so simulate a animation.
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Animate(GameTime gameTime)
        {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds * animationSpeed;

            if (animationTime > objectSprites.Length)
            {
                animationTime = 0;
            }
        }
        /// <summary>
        /// if and object goes outside the screen, return true.
        /// </summary>
        /// <returns></returns>
        public bool IsOutOfBounds()
        {
            if (this is not Fireball)
            {
                return (position.Y > GameWorld.ScreenSize.Y || position.X > GameWorld.ScreenSize.X || position.Y < -50 || position.X < -50);
            }
            else
            {
                return false;
            }
        }


    }
}

