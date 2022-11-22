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
        public static List<Component> _components;
        private Texture2D[] quitButtonAnimation;
        private Texture2D[] playButtonAnimation;
        private Texture2D[] backgroundSprite;

        //Properties

        //Constructors
        public MenuState(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        { }
        //Methods
        public override void LoadContent(ContentManager content)
        {
    
            backgroundSprite = new Texture2D[1];

            backgroundSprite[0] = _content.Load<Texture2D>("UI/NyBaggrund3");

            playButtonAnimation = new Texture2D[20];
            for (int i = 0; i < playButtonAnimation.Length; i++)
            {
                playButtonAnimation[i] = _content.Load<Texture2D>($"PlayButton/Play button ani-{i}");
            }

            quitButtonAnimation = new Texture2D[20];
            for (int i = 0; i < quitButtonAnimation.Length; i++)
            {
                quitButtonAnimation[i] = _content.Load<Texture2D>($"PlayButton/Play button ani-{i}");
            }

            UserInterface background = new UserInterface(backgroundSprite);

            Button startButton = new Button(playButtonAnimation)
            {
                Position = new Vector2(GameWorld.ScreenSize.X / 2 - 75, 400),
            };

            Button quitButton = new Button(quitButtonAnimation)
            {
                Position = new Vector2(GameWorld.ScreenSize.X / 2 - 75, 600),
            };
            startButton.Click += StartButton_Click;
            quitButton.Click += QuitButton_Click;

            _components = new List<Component>()
            {
                        background,
                        startButton,
                        quitButton,

            };
}

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
