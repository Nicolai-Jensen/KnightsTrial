using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

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

        //Properties

        //Constructors
        public GameState(GameWorld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Player Knight = new Player(new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 8 * 7));
            gameObject.Add(Knight);
            BringerOfDeath BoD = new BringerOfDeath();

            gameBackground = new Texture2D[1];
            gameBackground[0] = _content.Load<Texture2D>("UI/KnightsTrialBackground");

            UserInterface Background = new UserInterface(gameBackground, new Vector2(0, 0));

            menuButtonAnimation = new Texture2D[21];
            for (int i = 0; i < menuButtonAnimation.Length; i++)
            {
                menuButtonAnimation[i] = _content.Load<Texture2D>($"MenuButton/MenuButton{i + 1}");
            }
            Button menuButton = new Button(menuButtonAnimation, new Vector2(1600, 50));

            gameComponents = new List<Component>()
            {
                Background,
                menuButton,
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



    }

}
