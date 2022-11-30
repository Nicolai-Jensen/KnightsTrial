using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace KnightsTrial
{
    /// <summary>
    /// Updates and Draws out the PauseScreen when you pause the game.
    /// </summary>
    internal class PauseState : State
    {
        //Fields
        public static List<Component> components;
        private Texture2D[] quitButtonAnimation;
        public static Texture2D[] resumeButtonAnimation;
        private Texture2D[] newGameButtonAnimation;
        private Texture2D knightsLogo;
        private Texture2D gameOver;

        //Properties

        //Constructors
        /// <summary>
        /// When PauseState is instantiated, Draws out the Different Buttons.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="content"></param>
        public PauseState(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            #region Instatiated Objects and Components
            knightsLogo = _content.Load<Texture2D>("UI/pause");

            resumeButtonAnimation = new Texture2D[21];
            for (int i = 0; i < resumeButtonAnimation.Length; i++)
            {
                resumeButtonAnimation[i] = _content.Load<Texture2D>($"ResumeButton/ResumeButton{i + 1}");
            }

            quitButtonAnimation = new Texture2D[21];
            for (int i = 0; i < quitButtonAnimation.Length; i++)
            {
                quitButtonAnimation[i] = _content.Load<Texture2D>($"MenuButton/MenuButton{i + 1}");
            }
            newGameButtonAnimation = new Texture2D[21];
            for (int i = 0; i < newGameButtonAnimation.Length; i++)
            {
                newGameButtonAnimation[i] = _content.Load<Texture2D>($"NewGameButton/NewGameButton{i + 1}");
            }

            Button resumeButton = new Button(resumeButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 125, 500));

            Button newGameButton = new Button(newGameButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 125, 625));

            Button quitButton = new Button(quitButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 125, 750));


            resumeButton.Click += ResumeButton_Click;
            newGameButton.Click += NewGameButton_Click;
            quitButton.Click += QuitButton_Click;


            components = new List<Component>()
            {
                        resumeButton,
                        newGameButton,
                        quitButton,

            };
            #endregion
        }


        //Methods
        public override void LoadContent(ContentManager content)
        {
            gameOver = content.Load<Texture2D>("UI/GameOver");
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

            if(GetPlayer().Health > 0)
                spriteBatch.Draw(knightsLogo, new Vector2(450, 50), null, Color.White, 0f, Vector2.Zero, 0.90f, SpriteEffects.None, 1f);
            else
                spriteBatch.Draw(gameOver, new Vector2(450, 25), null, Color.White, 0f, Vector2.Zero, 0.90f, SpriteEffects.None, 1f);

            //Draws out the GameState, but does not Update it, to "pause" the game.
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
            if(GetPlayer().Health > 0)
            _game.ChangeState(GameWorld.gameState);
        }

        /// <summary>
        /// Removes gameObjects, instantiates a new Gamestate and switches to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewGameButton_Click(object sender, EventArgs e)
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
        public void QuitButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(GameWorld.menuState);
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
    }
}
