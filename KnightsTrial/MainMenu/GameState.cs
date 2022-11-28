using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace KnightsTrial
{/// <summary>
/// A sub-class from State, Is being used when you start the game to Load, Update and Draw everything that has something to do with the GameState.
/// </summary>
    internal class GameState : State
    {

        //Fields

        //A list used for UI and buttons.
        private List<Component> gameComponents;

        //Lists for gameObjects such as the Player, and boss.
        public static List<GameObject> gameObject = new List<GameObject>();
        private static List<GameObject> gameObjectsToAdd = new List<GameObject>();
        private static List<GameObject> gameObjectsToRemove = new List<GameObject>();

        //Different Texture arrays.
        private Texture2D[] menuButtonAnimation;
        private Texture2D[] gameBackground;
        private Texture2D[] playerHealth;
        private Texture2D[] playerHealthUI;
        private Texture2D[] playerStamina;
        private Texture2D[] playerStaminaUI;
        private Texture2D[] bossHealthUI;
        private Texture2D[] bossHealth;

        public static bool isBossAlive = true;
        
        //Rectangles for the players Health and Stamina, and the boss health.
        private Rectangle hpRectangle;
        private Rectangle staminaRectangle;
        private Rectangle bossHPRectangle;

        //Properties

        //Constructors
        /// <summary>
        /// When GameState is instantiated, It Creates the GameObjects, Buttons, and background.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="content"></param>
        public GameState(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            #region Instantiate Objects and Components
            Player Knight = new Player(new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 8 * 6));
            gameObject.Add(Knight);
            BringerOfDeath BoD = new BringerOfDeath();
            gameObject.Add(BoD);

            gameBackground = new Texture2D[1];
            gameBackground[0] = _content.Load<Texture2D>("UI/NewKnightsTrialBackground");
            UserInterface Background = new UserInterface(gameBackground, new Vector2(0, 0), 1f, 0f);

            hpRectangle = new Rectangle(95, 40, GetPlayer().Health * 2 + 30, 26);
            playerHealth = new Texture2D[1];
            playerHealth[0] = _content.Load<Texture2D>("UI/RedHealth");

            staminaRectangle = new Rectangle(135, 112, GetPlayer().Stamina * 2 + 30, 26);
            playerStamina = new Texture2D[1];
            playerStamina[0] = _content.Load<Texture2D>("UI/YellowStamina");

            bossHPRectangle = new Rectangle(710, 946, GetBoss().Health / 5 + 60, 52);
            bossHealth = new Texture2D[1];
            bossHealth[0] = _content.Load<Texture2D>("UI/RedHealth");

            playerHealthUI = new Texture2D[1];
            playerHealthUI[0] = _content.Load<Texture2D>("UI/Knight_Trial_HPBar");
            UserInterface hpUI = new UserInterface(playerHealthUI, new Vector2(20, 10), 1.5f, 0.8f);

            playerStaminaUI = new Texture2D[1];
            playerStaminaUI[0] = _content.Load<Texture2D>("UI/Knight_Trial_StaminaBar");
            UserInterface staminaUI = new UserInterface(playerStaminaUI, new Vector2(60, 80), 1.5f, 0.8f);

            bossHealthUI = new Texture2D[1];
            bossHealthUI[0] = _content.Load<Texture2D>("UI/Knights_Trial_BossHPBar");
            UserInterface bossHP = new UserInterface(bossHealthUI, new Vector2(550, 890), 2f, 0.8f);

            menuButtonAnimation = new Texture2D[21];
            for (int i = 0; i < menuButtonAnimation.Length; i++)
            {
                menuButtonAnimation[i] = _content.Load<Texture2D>($"MenuButton/MenuButton{i + 1}");
            }
            Button pauseButton = new Button(menuButtonAnimation, new Vector2(1600, 30));

            #endregion

            gameComponents = new List<Component>()
            {
                Background,
                pauseButton,
                staminaUI,
                hpUI,
                bossHP,
            };

            pauseButton.Click += PauseButton_Click;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            foreach (GameObject go in gameObject)
            {
                go.LoadContent(content);
            }
        }


        public override void Update(GameTime gameTime)
        {
            RemoveGameObjects();

            foreach (GameObject go in gameObject)
            {
                go.Update(gameTime);

                foreach (GameObject other in gameObject)
                {
                    if (go.IsColliding(other))
                    {
                        go.OnCollision(other);
                        other.OnCollision(go);
                    }
                }
            }

            hpRectangle.Width = GetPlayer().Health * 2 + 30;
            staminaRectangle.Width = GetPlayer().Stamina * 2 + 30;
            
            if (isBossAlive == true)
            {
                bossHPRectangle.Width = GetBoss().Health / 5 + 60;
            }
            else
            {
                bossHPRectangle.Width = 0;
            }

            foreach (Component co in gameComponents)
                co.Update(gameTime);

            foreach (GameObject gameObjectsToSpawn in gameObjectsToAdd)
            {
                gameObjectsToSpawn.LoadContent(_content);
                gameObject.Add(gameObjectsToSpawn);
            }

            gameObjectsToAdd.Clear();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject go in gameObject)
            {
                go.Draw(spriteBatch);
                _game.DrawCollisionBox(go);
            }

            spriteBatch.Draw(playerStamina[0], staminaRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);
            spriteBatch.Draw(playerHealth[0], hpRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);
            spriteBatch.Draw(bossHealth[0], bossHPRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);

            foreach (Component co in gameComponents)
            {
                co.Draw(gameTime, spriteBatch);
            }

        }
        /// <summary>
        /// Checks if something is out of bounds, or if the gameObject should be removed. and removes if true.
        /// </summary>
        public static void RemoveGameObjects()
        {
            foreach (GameObject go in gameObject)
            {
                bool shouldRemoveGameObject = go.IsOutOfBounds();
                if (shouldRemoveGameObject || go.ToBeRemoved)
                {
                    gameObjectsToRemove.Add(go);
                }
            }

            foreach (GameObject goToRemove in gameObjectsToRemove)
            {
                gameObject.Remove(goToRemove);
            }
        }


        /// <summary>
        /// Adds a Gameobject to the gameObjectsToAdd list.
        /// </summary>
        /// <param name="gObject"></param>
        public static void InstantiateGameObject(GameObject gObject)
        {
            gameObjectsToAdd.Add(gObject);

        }

        /// <summary>
        /// Changes State to PauseState when Clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(GameWorld.pauseState);
        }
        /// <summary>
        /// Checks the GameObject list for the Player, and returns the player.
        /// </summary>
        /// <returns></returns>
        private Player GetPlayer()
        {
            //loops through the gameObject list untill it finds the player, then returns it. 
            foreach (GameObject go in gameObject)
            {
                if (go is Player)
                {
                    return (Player)go;
                }
            }
            //if no player object is found, returns null.
            return null;
        }
        /// <summary>
        /// Checks the gameobject list for the BringerOfDeath, and returns it, if its on the lists.
        /// </summary>
        /// <returns></returns>
        private BringerOfDeath GetBoss()
        {
            //loops through the gameObject list untill it finds the BringerOfDeath, then returns it. 
            foreach (GameObject go in gameObject)
            {
                if (go is BringerOfDeath)
                {
                    return (BringerOfDeath)go;
                }
            }
            //if no BringerOfDeath object is found, returns null.
            return null;
        }
    }

}
