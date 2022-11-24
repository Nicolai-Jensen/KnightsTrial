using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace KnightsTrial
{
    internal class GameState : State
    {

        //Fields

        private List<Component> gameComponents;

        public static List<GameObject> gameObject = new List<GameObject>();
        private static List<GameObject> gameObjectsToAdd = new List<GameObject>();
        private static List<GameObject> gameObjectsToRemove = new List<GameObject>();

        private Texture2D[] menuButtonAnimation;
        private Texture2D[] gameBackground;
        private Texture2D[] playerHealth;
        private Texture2D[] playerHealthUI;
        private Texture2D[] playerStamina;
        private Texture2D[] playerStaminaUI;
        private Texture2D[] bossHealthUI;
        private Texture2D[] bossHealth;

        private Rectangle hpRectangle;
        private Rectangle staminaRectangle;
        private Rectangle bossHPRectangle;

        //Properties

        //Constructors
        public GameState(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            #region Instantiate Objects and Components
            Player Knight = new Player(new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 8 * 6));
            gameObject.Add(Knight);
            BringerOfDeath BoD = new BringerOfDeath();
            gameObject.Add(BoD);

            gameBackground = new Texture2D[1];
            gameBackground[0] = _content.Load<Texture2D>("UI/KnightsTrialBackground");
            UserInterface Background = new UserInterface(gameBackground, new Vector2(0, 0), 1f, 0f);

            hpRectangle = new Rectangle(95, 40, GetPlayer().Health * 2 + 30, 25);
            playerHealth = new Texture2D[1];
            playerHealth[0] = _content.Load<Texture2D>("UI/RedHealth");

            staminaRectangle = new Rectangle(135, 112, GetPlayer().Stamina * 2 + 30, 25);
            playerStamina = new Texture2D[1];
            playerStamina[0] = _content.Load<Texture2D>("UI/YellowStamina");

            bossHPRectangle = new Rectangle(735, 946, GetBoss().Health / 5 + 60, 52);
            bossHealth = new Texture2D[1];
            bossHealth[0] = _content.Load<Texture2D>("UI/RedHealth");

            playerHealthUI = new Texture2D[1];
            playerHealthUI[0] = _content.Load<Texture2D>("UI/Knight_Trial_HPBar");
            UserInterface hpUI = new UserInterface(playerHealthUI, new Vector2(20, 10), 1.5f, 1f);

            playerStaminaUI = new Texture2D[1];
            playerStaminaUI[0] = _content.Load<Texture2D>("UI/Knight_Trial_StaminaBar");
            UserInterface staminaUI = new UserInterface(playerStaminaUI, new Vector2(60, 80), 1.5f, 1f);

            bossHealthUI = new Texture2D[1];
            bossHealthUI[0] = _content.Load<Texture2D>("UI/Knights_Trial_BossHPBar");
            UserInterface bossHP = new UserInterface(bossHealthUI, new Vector2(575, 890), 2f, 1f);

            menuButtonAnimation = new Texture2D[21];
            for (int i = 0; i < menuButtonAnimation.Length; i++)
            {
                menuButtonAnimation[i] = _content.Load<Texture2D>($"MenuButton/MenuButton{i + 1}");
            }
            Button menuButton = new Button(menuButtonAnimation, new Vector2(1600, 50));

            #endregion

            gameComponents = new List<Component>()
            {
                Background,
                menuButton,
                staminaUI,
                hpUI,
                bossHP,
            };

            menuButton.Click += MenuButton_Click;
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

            spriteBatch.Draw(playerStamina[0], staminaRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(playerHealth[0], hpRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(bossHealth[0], bossHPRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

            foreach (Component co in gameComponents)
            {
                co.Draw(gameTime, spriteBatch);
            }

        }
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
        private void MenuButton_Click(object sender, EventArgs e)
        {
            //foreach (GameObject go in gameObject)
            //{
            //    go.ToBeRemoved = true;
            //}

            //RemoveGameObjects();

            _game.ChangeState(GameWorld.menuState);
        }
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
        private BringerOfDeath GetBoss()
        {
            //loops through the gameObject list untill it finds the player, then returns it. 
            foreach (GameObject go in gameObject)
            {
                if (go is BringerOfDeath)
                {
                    return (BringerOfDeath)go;
                }
            }
            //if no player object is found, returns null.
            return null;
        }



    }

}
