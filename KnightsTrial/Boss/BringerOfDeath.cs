using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    internal class BringerOfDeath : Boss
    {
        //Fields
        private Vector2 playerPosition;
        //Properties

        //Constructors
        public BringerOfDeath()
        {
            GameState.InstantiateGameObject(this);
            health = 2500;
            speed = 150f;
            velocity = new Vector2(1, 0);
            position = new Vector2(200, 200);

            objectSprites = new Texture2D[8];
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            for (int i = 0; i < objectSprites.Length; i++)
            {
                objectSprites[i] = content.Load<Texture2D>($"BringerOfDeath/Bosswalk_{i}");
            }
        }

        public override void Update(GameTime gameTime)
        {
            playerPosition = GetPlayer().Position;

            Move(gameTime);
            Animate(gameTime);
            FollowPlayer();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (playerPosition.X < position.X)
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, 3f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(objectSprites[(int)animationTime], position, null, Color.White, 0f, origin, 3f, SpriteEffects.FlipHorizontally, 1f);
            }
        }

        /// <summary>
        /// Gets the player object from the gameObject list in GameWorld.
        /// If there are no player objects in the list, the methods returns null.
        /// </summary>
        /// <returns>The player object</returns>
        private Player GetPlayer()
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

        private void FollowPlayer()
        {
            Vector2 outputVelocity = playerPosition - position;
            outputVelocity.Normalize();
            velocity = outputVelocity;
        }


        //-----------------------ATTACKS & SPELLS-----------------------------


        public void SwingAttack(GameTime gameTime)
        {
            //Normal attack
        }
        public void MagicAttack(GameTime gameTime)
        {
            // Ranged attack
        }
        public void AOEAttack(GameTime gameTime)
        {
            // Area of effect attack.
        }
    }
}
