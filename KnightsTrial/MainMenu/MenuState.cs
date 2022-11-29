using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace KnightsTrial
{/// <summary>
/// Draws out and Updates the MenuScreen.
/// </summary>
    public class MenuState : State
    {
        //Fields
        public static List<Component> components;

        private Texture2D[] quitButtonAnimation;
        private Texture2D[] playButtonAnimation;
        private Texture2D[] backgroundSprite;
        private Texture2D[] knightsLogo;

        //Properties

        //Constructors
        /// <summary>
        /// When MenuState is instantiated, It Creates the Logo, Buttons, and background.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="content"></param>
        public MenuState(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            #region Instatiated Objects and Components
            knightsLogo = new Texture2D[1];
            knightsLogo[0] = _content.Load<Texture2D>("UI/NewKnightsTrialLogo");

            //trialLogo = new Texture2D[1];
           // trialLogo[0] = _content.Load<Texture2D>("UI/Trial_logo");
            backgroundSprite = new Texture2D[1];

            //swordLogo = new Texture2D[1];
            //swordLogo[0] = _content.Load<Texture2D>("UI/Sword_logo");

            backgroundSprite[0] = _content.Load<Texture2D>("UI/NyBaggrund3");

            playButtonAnimation = new Texture2D[21];
            for (int i = 0; i < playButtonAnimation.Length; i++)
            {
                playButtonAnimation[i] = _content.Load<Texture2D>($"PlayButton/MenuPLAYButtonANI-export{i + 1}");
            }

            quitButtonAnimation = new Texture2D[21];
            for (int i = 0; i < quitButtonAnimation.Length; i++)
            {
                quitButtonAnimation[i] = _content.Load<Texture2D>($"QuitButton/MenuQuitButtonANI{i + 1}");
            }

            UserInterface background = new UserInterface(backgroundSprite, new Vector2(0, 0), 1f, 0f);

            Button startButton = new Button(playButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 175, 700));

            Button quitButton = new Button(quitButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 175, 825));

            startButton.Click += StartButton_Click;
            quitButton.Click += QuitButton_Click;

            components = new List<Component>()
            {
                        background,
                        startButton,
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
        }


        /// <summary>
        /// Runs ChangeState methods and instantiates a new GameState.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, EventArgs e)
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
            _game.Exit();
        }

    }
}
