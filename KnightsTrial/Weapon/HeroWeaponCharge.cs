using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Input;

namespace KnightsTrial
{
    class HeroWeaponCharge : Weapon
    {
        //Fields
        private Texture2D[] chargeAnimation;
        private Texture2D[] chargeAnimation2;
        private Texture2D[] releaseAnimation;
        private float chargeTimer;
        private MouseState currentMouse;
        private MouseState previousMouse;
        //Properties

        //Constructors

        public HeroWeaponCharge(Vector2 position)
        {
            damageValue = 0;
            this.position = position;
            rotation = 0f;
            speed = 0f;
            scale = 1f;
            animationSpeed = 4f;
        }

        //Methods

        public override void LoadContent(ContentManager content)
        {
            chargeAnimation = new Texture2D[2];
            chargeAnimation2 = new Texture2D[1];
            releaseAnimation = new Texture2D[5];


            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < releaseAnimation.Length; i++)
            {
                releaseAnimation[i] = content.Load<Texture2D>($"HeroWeapon/Shine{i + 1}");
            }

            //The Array is then looped with this for loop where it cycles through a list of sprites with the array numbers
            for (int i = 0; i < chargeAnimation.Length; i++)
            {
                chargeAnimation[i] = content.Load<Texture2D>($"HeroWeapon/Shine{i + 1}");
            }


            chargeAnimation2[0] = content.Load<Texture2D>($"HeroWeapon/Shine3");

            objectSprites = chargeAnimation;

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Move(gameTime);
            Animate(gameTime);
            Charge(gameTime);
            Release(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
        }

        public void Charge(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                position = ReturnPlayerPostition();
                SetPlayerSpeed(20f);
                chargeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            }

            if (chargeTimer > 0.7f)
            {
                objectSprites = chargeAnimation2;
                scale = 0.8f;
                rotation += 0.08f;
            }
        }

        public void Release(GameTime gameTime)
        {
            if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed && chargeTimer < 0.7f && GetPlayer().Stamina > 20)
            {

                HeroWeapon slashSprite = new HeroWeapon(new Vector2(position.X, position.Y));
                GameState.InstantiateGameObject(slashSprite);
                chargeTimer = 0;
                Player.ChargeAtkAnim = false;
                SetPlayerStamina(20);
                Player.LightAtkAnim = true;
                SetPlayerAnimationTime(0);
                ToBeRemoved = true;
                SetPlayerSpeed(150f);      
            }

            if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed && chargeTimer > 0.7f && GetPlayer().Stamina > 40)
            {
                HeroWeaponHeavy thrust = new HeroWeaponHeavy(new Vector2(position.X, position.Y));
                GameState.InstantiateGameObject(thrust);
                Player.ChargeAtkAnim = false;
                SetPlayerStamina(40);
                Player.HeavyAtkAnim = true;
                HiddenHitBox hitBox = new HiddenHitBox(new Vector2(position.X, position.Y));
                HiddenHitBox2 hitBox2 = new HiddenHitBox2(new Vector2(position.X, position.Y));
                GameState.InstantiateGameObject(hitBox2);
                GameState.InstantiateGameObject(hitBox);
                SetPlayerAnimationTime(0);
                chargeTimer = 0;
                animationTime = 0;
                animationSpeed = 15f;
                Direction(ReturnPlayerPostition());
                rotation += 1.55f;
                objectSprites = releaseAnimation;
                SetPlayerSpeed(0f);
            }

            if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed && chargeTimer < 0.7f && GetPlayer().Stamina < 20)
            {
                Player.Atacking = false;
                Player.ChargeAtkAnim = false;                
                Player.RegenStamina = true;
                chargeTimer = 0;
                ToBeRemoved = true;
                SetPlayerSpeed(200f);
            }

            if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed && chargeTimer > 0.7f && GetPlayer().Stamina < 40)
            {
                Player.Atacking = false;
                Player.ChargeAtkAnim = false;
                Player.RegenStamina = true;
                chargeTimer = 0;
                ToBeRemoved = true;
                SetPlayerSpeed(200f);
            }


            if (animationTime > 4)
            {
                ToBeRemoved = true;
            }

        }

    }
}
