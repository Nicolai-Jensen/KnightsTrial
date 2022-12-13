using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KnightsTrial
{/// <summary>
/// A class for the beware object. Used as a warning to the player for some of the boss' attacks.
/// Has two constructors, one for the Fireball and one for the RockPillar.
/// </summary>
    internal class Beware : GameObject
    {
        //Fields

        private bool isRockPillar;
        private float rockPillarTimer;

        //Properties

        public override Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(

                    (int)(position.X - SpriteSize.X / 2),
                    (int)(position.Y),
                    (int)SpriteSize.X, 10);
            }
        }

        //Constructors

        public Beware(Vector2 inputPosition)
        {
            GameState.InstantiateGameObject(this);
            position = inputPosition;
            scale = 1.5f;
            isRockPillar = false;
        }

        /// <summary>
        /// Overloaded constructor for when the Beware object should instantiate a RockPillar object.
        /// </summary>
        /// <param name="inputPosition"></param>
        /// <param name="isRockPillar"></param>
        public Beware(Vector2 inputPosition, bool isRockPillar)
        {
            GameState.InstantiateGameObject(this);
            position = inputPosition;
            scale = 1.5f;
            this.isRockPillar = isRockPillar;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            objectSprites = new Texture2D[4];

            for (int i = 0; i < objectSprites.Length; i++)
            {
                objectSprites[i] = content.Load<Texture2D>($"Beware/Beware{i + 1}");
            }

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);

        }

        public override void Update(GameTime gameTime)
        {
            if (isRockPillar)
            {
                SpawnRockPillar(gameTime);
            }

            Animate(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 1f);

        }

        /// <summary>
        /// Starts the rockPillarTimer and instantiates a RockPillar after 0.5 seconds.
        /// </summary>
        /// <param name="gameTime"></param>
        private void SpawnRockPillar(GameTime gameTime)
        {
            rockPillarTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (rockPillarTimer >= 0.5f)
            {
                rockPillarTimer = 0;
                RockPillar rock = new RockPillar(new Vector2(position.X, position.Y - 40));
            }

        }
    }
}
