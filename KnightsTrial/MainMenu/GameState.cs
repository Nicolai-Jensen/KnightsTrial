using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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

        //Different Texture arrays, for the various UserInterfaces and Buttons.
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
            playerHealth = new Texture2D[1];
            playerStamina = new Texture2D[1];
            bossHealth = new Texture2D[1];
            playerHealthUI = new Texture2D[1];
            playerStaminaUI = new Texture2D[1];
            bossHealthUI = new Texture2D[1];
            menuButtonAnimation = new Texture2D[21];
            
            
            gameBackground[0] = _content.Load<Texture2D>("UI/NewKnightsTrialBackground");
            playerHealth[0] = _content.Load<Texture2D>("UI/RedHealth");
            bossHealth[0] = _content.Load<Texture2D>("UI/RedHealth");
            playerStamina[0] = _content.Load<Texture2D>("UI/YellowStamina"); 
            playerHealthUI[0] = _content.Load<Texture2D>("UI/Knight_Trial_HPBar");
            playerStaminaUI[0] = _content.Load<Texture2D>("UI/Knight_Trial_StaminaBar");
            bossHealthUI[0] = _content.Load<Texture2D>("UI/Knights_Trial_BossHPBar");

            hpRectangle = new Rectangle(95, 40, GetPlayer().Health * 2 + 30, 26);
            staminaRectangle = new Rectangle(135, 112, GetPlayer().Stamina * 2 + 30, 26);          
            bossHPRectangle = new Rectangle(710, 946, GetBoss().Health / 5 + 60, 52);

            UserInterface Background = new UserInterface(gameBackground, new Vector2(0, 0), 1f, 0f);
            UserInterface hpUI = new UserInterface(playerHealthUI, new Vector2(20, 10), 1.5f, 0.8f);
            UserInterface staminaUI = new UserInterface(playerStaminaUI, new Vector2(60, 80), 1.5f, 0.8f);
            UserInterface bossHP = new UserInterface(bossHealthUI, new Vector2(550, 890), 2f, 0.8f);

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
            //If the player isnt dead, it updates the game.
            if (!Player.dead && isBossAlive)
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
                //Updates the players Health and Stamina bar.
                hpRectangle.Width = (int)(GetPlayer().Health / 0.434f);
                staminaRectangle.Width = (int)(GetPlayer().Stamina / 0.434f);

                //if the boss is alive, it should update the Health bar, until the boss is dead.
                if (isBossAlive)
                {
                    bossHPRectangle.Width = (int)(GetBoss().Health / 4.46f);
                }

                //Loops throu the gameComponents list to find Components and Update those Components.
                foreach (Component co in gameComponents)
                    co.Update(gameTime);

                //Loops throu the gameObjectsToAdd list to find gameObjects and add those to the gameObject list.
                foreach (GameObject gameObjectsToSpawn in gameObjectsToAdd)
                {
                    gameObjectsToSpawn.LoadContent(_content);
                    gameObject.Add(gameObjectsToSpawn);
                }

                gameObjectsToAdd.Clear();
            }
            //If the player dies, it changes the State to pauseState.
            else
             _game.ChangeState(GameWorld.pauseState);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject go in gameObject)
            {
                go.Draw(spriteBatch);
                //_game.DrawCollisionBox(go);
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
