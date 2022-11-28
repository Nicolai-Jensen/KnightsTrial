using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial.Boss
{
    internal class SwingProjectile : GameObject
    {
        //Fields

        private int damageValue;
        private Vector2 position;

        private float timer;


        //Properties
        public override Rectangle CollisionBox
        {
            get
            {
                if (GetPlayer().Position.X > position.X)
                {
                    return new Rectangle((int)(position.X), (int)(position.Y - 20), 200, 60);
                }
                else
                {
                    return new Rectangle((int)(position.X - 200), (int)(position.Y - 20), 200, 60);
                }
                    
            }
        }

        //Constructor
        public SwingProjectile(Vector2 posValue)
        {
            GameState.InstantiateGameObject(this);

            this.position = posValue;
            scale = 1f;
            damageValue = 25;
        }
        //Methods
        public override void LoadContent(ContentManager content)
        {

        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > 0.5f)
                toBeRemoved = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

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
        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                (other as Player).Health -= damageValue;

                toBeRemoved = true;
            }
        }
    }
}
