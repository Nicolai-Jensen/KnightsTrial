﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    internal class Icicle : GameObject
    {
        //Fields

        //Properties

        //Constructors

        public Icicle(Vector2 inputVelocity, Vector2 position)
        {
            GameState.InstantiateGameObject(this);
            this.position = position;
            speed = 500f;
            scale = 1f;
            velocity = inputVelocity;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            objectSprites = new Texture2D[1];

            objectSprites[0] = content.Load<Texture2D>("BringerOfDeath/Icicle");

            origin = new Vector2(objectSprites[0].Width / 2, objectSprites[0].Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectSprites[0], position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 1f);
        }

        public override void OnCollision(GameObject other)
        {
            
        }
    }
}