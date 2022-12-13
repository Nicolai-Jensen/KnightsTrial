using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightsTrial
{
    /// <summary>
    /// This class has the functions of the light attack object
    /// </summary>
    internal class HeroWeapon : Weapon
    {
        //Fields

        //Light Attack Texture
        private Texture2D[] sprite;

        //Properties
        //HeroWeapon has no properties


        //Constructors
        /// <summary>
        /// Constructs a Light attack object for attacking
        /// </summary>
        /// <param name="position">Position is the vector2 that determines where the object is spawned</param>
        public HeroWeapon(Vector2 position)
        {
            //Sets its damage on collision to 30
            if (!Player.godMode)
                damageValue = 30;
            else
                damageValue = 100;
            //Various mandatory variables the object needs to be drawn properly
            this.position = position;
            rotation = -1.5f;
            speed = 0f;
            scale = 2f;
            animationSpeed = 12f;
        }

        //Methods
        /// <summary>
        /// This Method loads in information and code needed for the game, there is nothing in this one but it is needed anyway as Gameobject has it abstract
        /// </summary>
        /// <param name="content">Parameter given by the monogame framework for our use</param>
        public override void LoadContent(ContentManager content)
        {
            //Instantiates the texture array for Light attack
            sprite = new Texture2D[5];

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < sprite.Length; i++)
            {
                sprite[i] = content.Load<Texture2D>($"HeroWeapon/Slash{i}");
            }

            //objectSprites is the base array Texture used in Draw and it is set during load to give the first animation
            objectSprites = sprite;

            //Sets the origin to the middle of the sprite
            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
        }
        /// <summary>
        /// This Update Method constantly loops throughout the program aslong as it is running, other methods we want to be looped are called inside this one
        /// </summary>
        /// <param name="gameTime">Parameter given by the Monogame framework to simulate time</param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
            ChooseDirection();
            EndAttack();
        }

        /// <summary>
        /// The Method for Drawing out a sprite to the screen, this method is an override for the vitual one in GameObject and is called in GameWorld/GameState
        /// </summary>
        /// <param name="spriteBatch">Parameter given by the Monogame framework for the use of sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Flips the sprite if player is attacking left
            if (Player.IsFacingRight == true)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.AntiqueWhite, rotation, origin, scale, SpriteEffects.None, 1f);
            }

            if (Player.IsFacingRight != true)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.AntiqueWhite, rotation, origin, scale, SpriteEffects.FlipVertically, 1f);
            }
            
        }

        /// <summary>
        /// Depending on the players facing direction the attack is moved 50 pixels infront of the player as the attack has to be infront of him and not on him
        /// </summary>
        public void ChooseDirection()
        {
            if (Player.IsFacingRight == true)
            {
                position.X = ReturnPlayerPostition().X + 50;
            }

            if (Player.IsFacingRight != true)
            {
                position.X = ReturnPlayerPostition().X - 50;
            }
        }

        /// <summary>
        /// Ends the attack by removing the light attack object at the right time
        /// </summary>
        public void EndAttack()
        {
            //Once the animation finishes player it activates
            if (animationTime > 4)
            {
                //Removes HeroWeapon object(Light attack), sets players speed to default and deactivates the attacking state
                ToBeRemoved = true;
                SetPlayerSpeed(200f);
                Player.Atacking = false;
            }
        }
    }
}
