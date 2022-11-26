using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTrial.MainMenu
{
    internal class PauseState : State
    {
        //Fields
        public static List<Component> components;
        private Texture2D[] quitButtonAnimation;
        private Texture2D[] resumeButtonAnimation;
        private Texture2D[] newGameButtonAnimation;
        private Texture2D[] backgroundSprite;
        private Texture2D[] knightsLogo;

        //Properties

        //Constructors
        public PauseState(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            #region Instatiated Objects and Components
            knightsLogo = new Texture2D[1];
            knightsLogo[0] = _content.Load<Texture2D>("UI/pause");

            backgroundSprite = new Texture2D[1];

            backgroundSprite[0] = _content.Load<Texture2D>("UI/KnightsTrialBackground");

            resumeButtonAnimation = new Texture2D[21];
            for (int i = 0; i < resumeButtonAnimation.Length; i++)
            {
                resumeButtonAnimation[i] = _content.Load<Texture2D>($"ResumeButton/ResumeButton{i + 1}");
            }

            quitButtonAnimation = new Texture2D[21];
            for (int i = 0; i < quitButtonAnimation.Length; i++)
            {
                quitButtonAnimation[i] = _content.Load<Texture2D>($"QuitButton/MenuQuitButtonANI{i + 1}");
            }
            newGameButtonAnimation = new Texture2D[21];
            for (int i = 0; i < newGameButtonAnimation.Length; i++)
            {
                newGameButtonAnimation[i] = _content.Load<Texture2D>($"NewGameButton/NewGameButton{i + 1}");
            }

            UserInterface background = new UserInterface(backgroundSprite, new Vector2(0, 0), 1f, 0f);

            Button resumeButton = new Button(resumeButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 175, 500));

            Button newGameButton = new Button(newGameButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 175, 625));

            Button quitButton = new Button(quitButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 175, 750));


            resumeButton.Click += ResumeButton_Click;
            newGameButton.Click += NewGameButton_Click;
            quitButton.Click += QuitButton_Click;


            components = new List<Component>()
            {
                        background,
                        resumeButton,
                        newGameButton,
                        quitButton,

            };
            #endregion
        }


        //Methods
        public override void LoadContent(ContentManager content)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component co in components)
                co.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component co in components)
                co.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(knightsLogo[0], new Vector2(400, 50), null, Color.White, 0f, Vector2.Zero, 0.90f, SpriteEffects.None, 1f);

            GameWorld.gameState.Draw(gameTime, spriteBatch);
        }


        /// <summary>
        /// Runs ChangeState methods and switches to the GameState.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResumeButton_Click(object sender, EventArgs e)
        {
            //Starts the game or chooses boss?.
            _game.ChangeState(GameWorld.gameState);
        }
        /// <summary>
        /// Removes gameObjects, instantiates a new Gamestate and switches to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            foreach (GameObject go in GameState.gameObject)
            {
                go.ToBeRemoved = true;
            }
            GameState.RemoveGameObjects();
            GameState newGame = new GameState(_game, _graphicsDevice, _content);
            GameWorld.gameState = newGame;
            _game.ChangeState(GameWorld.gameState);
        }
        /// <summary>
        /// Quits the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(GameWorld.menuState);
        }
    }
}
