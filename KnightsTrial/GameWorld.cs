using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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

        private Texture2D pixel;

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
            Player Knight = new Player(new Vector2(0, 0));
            GameState.gameObject.Add(Knight);
            BringerOfDeath BoD = new BringerOfDeath();
            GameState.gameObject.Add(BoD);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content);

            foreach (GameObject go in GameState.gameObject)
            {
                go.LoadContent(Content);
            }

            //pixel = Content.Load<Texture2D>("pixel");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }

            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);

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
        public void ChangeState(State state)
        {
            _nextState = state;
        }

        private void DrawCollisionBox(GameObject go)

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