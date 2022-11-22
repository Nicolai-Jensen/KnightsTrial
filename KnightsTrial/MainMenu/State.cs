using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    public abstract class State
    {
        //Fields
        protected ContentManager _content;
        protected GameWorld _game;
        protected GraphicsDevice _graphicsDevice;
        protected float animationTime;
        protected float animationSpeed;
        protected Texture2D[] buttonAnimation;

        //Properties

        //Constructors
        public State(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }
        //Methods
        public abstract void Update(GameTime gameTime);
        public abstract void PostUpdate(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        protected void Animate(GameTime gameTime)
        {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds * animationSpeed;

            if (animationTime > buttonAnimation.Length)
            {
                animationTime = 0;
            }
        }
    }
}
