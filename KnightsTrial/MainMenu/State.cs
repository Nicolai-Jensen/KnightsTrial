﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace KnightsTrial
{
    /// <summary>
    /// A Abstract superclass, made so other states can inherit from it.
    /// </summary>
    public abstract class State
    {
        //Fields
        protected ContentManager _content;
        protected GameWorld _game;
        protected GraphicsDevice _graphicsDevice;
        protected Song backgroundMusic;

        //Properties

        //Constructors
        public State(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }
        //Methods
        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
