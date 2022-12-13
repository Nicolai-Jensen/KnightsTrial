using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KnightsTrial
{
    /// <summary>
    /// A class for an invisible box object that is used as a hitbox
    /// </summary>
    class HiddenHitBox : Weapon
    {

        //Fields
        //The Texture for the sprite
        private Texture2D[] sprite;


        //Properties


        //Constructors
        /// <summary>
        /// Constructor for the hidden box that will act as a hitbox for the heavy attack
        /// </summary>
        /// <param name="position">A Vector2 for the object to use</param>
        public HiddenHitBox(Vector2 position)
        {
            //Various variables needed to draw the sprite
            rotation = 0f;
            speed = 0f;
            scale = 0.3f;

            //Aims in the direction of the mouse
            velocity = Direction(ReturnPlayerPostition());

            //Puts the hitbox out to the edge of the heavy thrust by adding the velocity timed by the length of the heavy attack
            this.position = position + velocity * 250;

            //Slightly adjusts the X position so that it lines up more with the attack
            this.position.X -= 30;

            //has a DamageValue of that meant for the heavy attack as this object is the heavy attacks collision indicator
            if (!Player.godMode)
                damageValue = 100;
            else
                damageValue = 250;

            //Syncs the speed with the heavy attack
            animationSpeed = 12f;
        }

        //Methods
        /// <summary>
        /// This Method loads in information and code needed for the game, there is nothing in this one but it is needed anyway as Gameobject has it abstract
        /// </summary>
        /// <param name="content">Parameter given by the monogame framework for our use</param>
        public override void LoadContent(ContentManager content)
        {
            //Instantiates the texture array for the hiddenhitbox
            sprite = new Texture2D[1];

            //Sets the sprite to be a png made completely of transparency
            sprite[0] = content.Load<Texture2D>($"HeroWeapon/Thrust6");

            //objectSprites is the base array Texture used in Draw and it is set during load to give the first animation
            objectSprites = sprite;

            //Uses the same origin value as the heavy attack as it is the same shape
            origin = new Vector2(objectSprites[0].Width / 10, objectSprites[0].Height / 2);
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
        /// Removes the hiddenHitBox when the heavyAtk ends
        /// </summary>
        public void EndAttack()
        {
            if (Player.HeavyAtkAnim == false)
            {
                ToBeRemoved = true;
            }
        }
    }
}
