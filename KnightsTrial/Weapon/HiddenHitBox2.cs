using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KnightsTrial
{
    /// <summary>
    /// A class for an invisible box object that is used as a hitbox
    /// </summary>
    class HiddenHitBox2 : Weapon
    {
        //Fields
        //The Texture for the sprite
        private Texture2D[] sprite;

        //Properties

        //Constructors
        /// <summary>
        /// Constructor for the hidden box 2 that will act as a hitbox for the heavy attack
        /// </summary>
        /// <param name="position">A Vector2 for the object to use</param>
        public HiddenHitBox2(Vector2 position)
        {
            //Various variables needed to draw the sprite
            rotation = 0f;
            speed = 0f;
            scale = 0.3f;

            //Aims in the direction of the mouse
            velocity = Direction(ReturnPlayerPostition());

            //Puts the hitbox out to the middle of the heavy thrust by adding the velocity timed by half the length of the heavy attack
            this.position = position + velocity * 125;

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

        public override void LoadContent(ContentManager content)
        {
            sprite = new Texture2D[1];


            sprite[0] = content.Load<Texture2D>($"HeroWeapon/Thrust6");


            objectSprites = sprite;


            origin = new Vector2(objectSprites[0].Width / 10, objectSprites[0].Height / 2);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
        }
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
            EndAttack();
        }


        public void EndAttack()
        {
            if (Player.HeavyAtkAnim == false)
            {
                ToBeRemoved = true;
            }
        }
    }
}
