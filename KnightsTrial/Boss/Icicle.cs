using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightsTrial
{
    internal class Icicle : GameObject
    {
        //Fields
        private int damageValue;
        private float rotation;

        //Properties
        public override Rectangle CollisionBox
        {
            get
            {
                if (rotation == 1.55f || rotation == -1.55f)
                {
                     return new Rectangle(

                        (int)(position.X - SpriteSize.X),
                        (int)(position.Y - SpriteSize.Y / 4),
                        (int)SpriteSize.Y, (int)SpriteSize.X);

                }
                else
                {
                    return new Rectangle(

                        (int)(position.X - SpriteSize.X / 2),
                        (int)(position.Y - SpriteSize.Y / 2),
                        (int)SpriteSize.X, (int)SpriteSize.Y);
                }
            }
        }

        //Constructors

        public Icicle(Vector2 inputVelocity, Vector2 position, float rotation)
        {
            GameState.InstantiateGameObject(this);
            this.position = position;
            speed = 200f;
            scale = 2f;
            velocity = inputVelocity;
            this.rotation = rotation;
            damageValue = 10;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            objectSprites = new Texture2D[1];

            objectSprites[0] = content.Load<Texture2D>("BringerOfDeath/Icicle");

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);

            blockSound = content.Load<SoundEffect>("SoundEffects/BlockSound");
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[0], position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0.5f);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player && !Player.godMode)
            {
                
                if (Player.Blocking == true && Player.Dodging != true)
                {

                    if (GetPlayer().Stamina < damageValue)
                    {
                        GetPlayer().Health -= damageValue;
                        GetPlayer().HealthModified = true;
                    }
                    GetPlayer().Stamina -= damageValue * 2;
                    SoundEffectInstance blockSoundInstance = blockSound.CreateInstance();
                    blockSoundInstance.Volume = 0.5f;
                    blockSoundInstance.Play();
                    ToBeRemoved = true;
                }

                if (Player.Blocking == false && Player.Dodging == false)
                {
                    GetPlayer().Health -= damageValue;
                    GetPlayer().HealthModified = true;
                    ToBeRemoved = true;
                }
            }
        }

        /// <summary>
        /// Loops through the gameObject list in GameState to find and return the Player object.
        /// If no Player object is found, returns null.
        /// </summary>
        /// <returns></returns>
        protected Player GetPlayer()
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
    }
}
