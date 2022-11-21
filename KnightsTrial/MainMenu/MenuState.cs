using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial
{
    public class MenuState : State
    {

        //Fields
        private List<Component> _components;

        //Properties

        //Constructors
        public MenuState(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Texture2D buttonTexture = _content.Load<Texture2D>("Potion1");
            SpriteFont buttonFont = _content.Load<SpriteFont>("buttonFont");
            Button startButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(GameWorld.ScreenSize.X / 2, 400),
                Text = "Start Game"
            };

            Button quitButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(GameWorld.ScreenSize.X / 2, 600),
                Text = "Quit Game"
            };

            startButton.Click += StartButton_Click;
            quitButton.Click += QuitButton_Click;

            _components = new List<Component>()
            {
                startButton,
                quitButton,

            };
        }



        //Methods
        public override void Update(GameTime gameTime)
        {
            foreach (Component co in _components)
                co.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //remove sprites if they are not needed.
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            foreach (Component co in _components)
                co.Draw(gameTime, spriteBatch);

            //spriteBatch.End();
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            //Starts the game or chooses boss?.
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
        private void QuitButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}
