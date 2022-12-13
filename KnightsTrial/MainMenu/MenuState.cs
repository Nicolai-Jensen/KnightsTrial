using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;


namespace KnightsTrial
{/// <summary>
/// Draws out and Updates the MenuScreen.
/// </summary>
    public class MenuState : State
    {
        //Fields
        //A list of components which is used for the Buttons and UserInterfaces.
        public static List<Component> components;

        //Texture arrays for the various Buttons and Backgrounds.
        private Texture2D[] quitButtonAnimation;
        private Texture2D[] playButtonAnimation;
        private Texture2D[] backgroundSprite;
        private Texture2D[] knightsLogo;
        public static Texture2D[] godModeButton;
        public static Texture2D[] currentGodMode;
        private Texture2D[] godModeBox;

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
            backgroundSprite = new Texture2D[1];
            
            playButtonAnimation = new Texture2D[21];
            quitButtonAnimation = new Texture2D[21];
            
            godModeButton = new Texture2D[2];
            currentGodMode = new Texture2D[1];
            godModeBox = new Texture2D[1];
           
            knightsLogo[0] = _content.Load<Texture2D>("UI/NewKnightsTrialLogo");
            backgroundSprite[0] = _content.Load<Texture2D>("UI/Knights_Trial_MenuScreen");
            godModeButton[0] = _content.Load<Texture2D>("GodModeButton/GodmodeTickBox1");
            godModeButton[1] = _content.Load<Texture2D>("GodModeButton/GodmodeTickBox2");
            godModeBox[0] = _content.Load<Texture2D>("GodModeButton/GodmodeBox");

            for (int i = 0; i < playButtonAnimation.Length; i++)
            {
                playButtonAnimation[i] = _content.Load<Texture2D>($"PlayButton/MenuPLAYButtonANI-export{i + 1}");
            }

            for (int i = 0; i < quitButtonAnimation.Length; i++)
            {
                quitButtonAnimation[i] = _content.Load<Texture2D>($"QuitButton/MenuQuitButtonANI{i + 1}");
            }

            currentGodMode[0] = godModeButton[0];

            UserInterface background = new UserInterface(backgroundSprite, new Vector2(0, 0), 1f, 0f);

            UserInterface godModeUI = new UserInterface(godModeBox, new Vector2(1625, 850), 1.1f, 0.7f);

            Button startButton = new Button(playButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 125, 700));

            Button quitButton = new Button(quitButtonAnimation, new Vector2(GameWorld.ScreenSize.X / 2 - 125, 825));

            Button godButton = new Button(currentGodMode, new Vector2(1715, 903));

            startButton.Click += StartButton_Click;
            quitButton.Click += QuitButton_Click;
            godButton.Click += GodButton_Click;

            components = new List<Component>()
            {
                        background,
                        godModeUI,
                        startButton,
                        quitButton,
                        godButton,

            };
            #endregion
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            backgroundMusic = _content.Load<Song>("BackgroundMusic/MainMenuMusic");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;
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

            spriteBatch.Draw(knightsLogo[0], new Vector2(475, 50), null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 1f);
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

        /// <summary>
        /// When clicked godMode activate, Player cannot take damage and his damage is increased.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GodButton_Click(object sender, EventArgs e)
        {
            if (!Player.godMode)
            {
                Player.godMode = true;
                currentGodMode[0] = godModeButton[1];
            }
            else
            {
                Player.godMode = false;
                currentGodMode[0] = godModeButton[0];
            }
        }

    }
}
