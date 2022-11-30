using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightsTrial
{
    public class GameWorld : Game
    {
        //Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private static Vector2 screenSize;

        private State _currentState;
        private State _nextState;

        private KeyboardState currentKey;
        private KeyboardState previousKey;

        public static State gameState;
        public static State menuState;
        public static State pauseState;

        public Texture2D pixel;

        //Properties
        public static Vector2 ScreenSize
        {
            get
            {
                return screenSize;
            }
        }

        //Constructors
        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }
        //Methods
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            MenuState menu = new MenuState(this, _graphics.GraphicsDevice, Content);
            menuState = menu;

            PauseState paused = new PauseState(this, _graphics.GraphicsDevice, Content);
            pauseState = paused;

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = menuState;

            _currentState.LoadContent(Content);

            pixel = Content.Load<Texture2D>("pixel");
        }

        protected override void Update(GameTime gameTime)
        {
            previousKey = currentKey;
            currentKey = Keyboard.GetState();

            if (currentKey.IsKeyDown(Keys.Escape) && previousKey.IsKeyUp(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if(_currentState != pauseState && _currentState != menuState)
                    ChangeState(pauseState);

                if (_currentState == pauseState)
                    ChangeState(gameState);
            }

                    
            _currentState.Update(gameTime);

            if (_nextState != null)
            {
                _currentState = _nextState;

                _currentState.LoadContent(Content);

                _nextState = null;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            _currentState.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        /// <summary>
        /// Sets _nextState to the new State.
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public void DrawCollisionBox(GameObject go)

        {
            Rectangle top = new Rectangle(go.CollisionBox.X, go.CollisionBox.Y, go.CollisionBox.Width, 1);
            Rectangle bottom = new Rectangle(go.CollisionBox.X, go.CollisionBox.Y + go.CollisionBox.Height, go.CollisionBox.Width, 1);
            Rectangle left = new Rectangle(go.CollisionBox.X, go.CollisionBox.Y, 1, go.CollisionBox.Height);
            Rectangle right = new Rectangle(go.CollisionBox.X + go.CollisionBox.Width, go.CollisionBox.Y, 1, go.CollisionBox.Height);

            _spriteBatch.Draw(pixel, top, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(pixel, bottom, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(pixel, left, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(pixel, right, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);

        }

    }
}