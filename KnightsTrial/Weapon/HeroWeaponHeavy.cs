using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightsTrial
{
    /// <summary>
    /// A class that has the functions of the heavy attack that is initiated from the charge
    /// </summary>
    class HeroWeaponHeavy : Weapon
    {

        //Fields

        //Heavy Attack Texture
        private Texture2D[] sprite;


        //Properties
        //Heavy Attack has no properties

        //Constructors
        /// <summary>
        /// The Method used to Construct the Heavy attack
        /// </summary>
        /// <param name="position">Position is the vector2 that determines where the object is spawned</param>
        public HeroWeaponHeavy(Vector2 position)
        {
            //various variables needed for the object to be drawn properly
            this.position = position;
            rotation = 0f;
            speed = 0f;
            scale = 1.2f;

            //Aims the HeavyAttack in the direction of the mouse & syncs its animationSpeed with the charge feedback
            velocity = Direction(ReturnPlayerPostition());
            animationSpeed = 12f;
        }

        //Methods
        /// <summary>
        /// This Method loads in information and code needed for the game, there is nothing in this one but it is needed anyway as Gameobject has it abstract
        /// </summary>
        /// <param name="content">Parameter given by the monogame framework for our use</param>
        public override void LoadContent(ContentManager content)
        {
            //Instantiates the texture array for the heavy attack
            sprite = new Texture2D[7];

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < sprite.Length; i++)
            {
                sprite[i] = content.Load<Texture2D>($"HeroWeapon/Thrust{i}");
            }

            //objectSprites is the base array Texture used in Draw and it is set during load to give the first animation
            objectSprites = sprite;

            //Sets the origin to the middle verdically but near edge of the object horizontally
            origin = new Vector2(objectSprites[0].Width / 10 , objectSprites[0].Height / 2);
        }
        /// <summary>
        /// The Method for Drawing out a sprite to the screen, this method is an override for the vitual one in GameObject and is called in GameWorld/GameState
        /// </summary>
        /// <param name="spriteBatch">Parameter given by the Monogame framework for the use of sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
        }
        /// <summary>
        /// This Update Method constantly loops throughout the program aslong as it is running, other methods we want to be looped are called inside this one
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
            EndAttack();
        }

        /// <summary>
        /// This Method ends the attack once the animation finishes
        /// </summary>
        public void EndAttack()
        {
            if (animationTime > 6)
            {
                //Removes the heavy attack and sets the player speed to its base
                ToBeRemoved = true;
                SetPlayerSpeed(200f);
            }
        }
    }
}
