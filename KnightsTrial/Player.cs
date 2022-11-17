using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    internal class Player : GameObject
    {
        //Fields
        private int health;
        private int stamina;
        private bool healthModified = false;
        private Color color;
        private Texture2D block;
        private Texture2D heroWeapon;
        private Texture2D idleAnimation;
        private Texture2D runAnimation;
        private Texture2D dodgeAnimation;
        private Texture2D blockAnimation;
        private SoundEffect attackingSound;
        private SoundEffect potionSound;
        private SoundEffect dodgeSound;
        private SoundEffect blockSound;
        private bool isFacingRight = false;
        private bool hitCooldown = false;
        private float hitCooldownTimer;
        private bool attackCooldown = false;
        private float attackCooldownTimer;

        //Properties
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int Stamina
        {
            get { return stamina; }
            set { stamina = value; }
        }

        public bool HealthModified
        {
            get { return healthModified; }
            set { healthModified = value; }
        }
        
        public bool HitCooldown
        {
            get { return hitCooldown; }
        }

        //Constructors
        public Player(Vector2 vector2)
        {
            position = vector2;
            scale = 1f;
            health = 100;
            stamina = 100;
            speed = 200f;
            color = Color.White;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        private void HandleInput(GameTime gameTime)
        {

        }

        public void ScreenWrap()
        {

        }

        public override void OnCollision(GameObject other)
        {

        }

        public void Damaged(GameTime gameTime)
        {

        }

        public void Death()
        {

        }

        public void Attack(GameTime gameTime)
        {

        }

        public void StaminaRegen(GameTime gameTime)
        {

        }

        public void Block(GameTime gameTime)
        {

        }

        public void Dodge(GameTime gameTime)
        {

        }

        public void UsePotion(GameTime gameTime)
        {

        }
    }
}
