using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public enum GameState
{
    Menu,
    lvl1,
    lvl2,
    lvl3,
    End,
}

namespace GameBehaviour
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game // this is your game manager class you dork. Fix!
    {
        GameState _state = GameState.Menu;
        private AIManager manager;
        //use underscores so we can differentiate between global and member variables
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private Texture2D playerTexture;
        private Player player;

        private Texture2D keyTexture;
        private Key key;

        private Menu menu;
        private Texture2D menuTexture;

        private Texture2D groundTexture;
        private Texture2D bounceTexture;
        private Board board;
        private Astar pathfinding;

        private Drone drone;
        private Texture2D droneTexture;

        private Selector selector;
        private Texture2D selectorTexture;

        //private Crate testCrate;
        private CrateSpawn crateSpawn;
        private Texture2D crateTexture;

        private World _physicsWorld;
        private Camera camera;

        List<GameObject> activeObjects = new List<GameObject>();
        List<Tile> activeTiles = new List<Tile>();

        private bool paused = false;
        private bool pauseKeyDown = false;
        private bool pausedForGuide = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1330;  //set window size to 720(ish)p
            _graphics.PreferredBackBufferHeight = 770;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Create the physics world object
           

            playerTexture = Content.Load<Texture2D>("PlayerSprite");
            groundTexture = Content.Load<Texture2D>("GroundBlock");
            bounceTexture = Content.Load<Texture2D>("BouncyBlock");
            crateTexture = Content.Load<Texture2D>("Crate");
            selectorTexture = Content.Load<Texture2D>("Selector");
            droneTexture = Content.Load<Texture2D>("pointmarker");
            keyTexture = Content.Load<Texture2D>("Key");
            menuTexture = Content.Load<Texture2D>("MenuBG");

            font = Content.Load<SpriteFont>("Fuel");

            menu = new Menu(_spriteBatch, menuTexture, new Vector2(0, 0), new Vector2(0, 0), 1, "mainMenu");
            board = new Board(_spriteBatch, groundTexture, bounceTexture, 57, 11);
            pathfinding = new Astar();
            pathfinding.board = board;
            manager = new AIManager(pathfinding);
            pathfinding.Manager = manager;

            player = new Player(new RigidBody2D(new Vector2(100, 560), new Vector2(0, 0), 1, "player", false, 5, 4)
                , playerTexture, _spriteBatch, 5);

            selector = new Selector(player.Position, new Vector2(0, 0), 1, "Selector", board, _spriteBatch, selectorTexture);

            key = new Key(new RigidBody2D(new Vector2(1000, 300), new Vector2(0, 0), 1, "key", false, 0, 1),
                keyTexture, _spriteBatch);

            crateSpawn = new CrateSpawn(_spriteBatch, crateTexture);

            //testCrate = new Crate(new RigidBody2D(new Vector2(2000, 100), new Vector2(0, 0), 1, "crate", false, 1, 4),
                //_spriteBatch, crateTexture);
            
            _physicsWorld = new World();

            activeObjects.Add(player);//add the player to the list of active objects
            activeObjects.Add(key);
            //activeObjects.Add(testCrate);
            _physicsWorld.PhysObjects.Add(player.ObjRB);//add the player's rigidbody to the world object
            _physicsWorld.PhysObjects.Add(key.ObjRB);
            //_physicsWorld.PhysObjects.Add(testCrate.ObjRB);

            drone = new Drone(manager, player, selector, pathfinding, new RigidBody2D(player.ObjRB.Position, new Vector2(0, 0), 1, "drone", false, 1, 3)
                , _spriteBatch, droneTexture);

            player.drone = drone;
            
            activeObjects.Add(drone);
            _physicsWorld.PhysObjects.Add(drone.ObjRB);
           
            foreach (Tile tile in board.tiles)
            {
                activeTiles.Add(tile);//add each active tile to the list of active tiles

                if (tile.IsRendered)
                {
                    //_physicsWorld.PhysObjects.Add(tile.ObjRB);
                }
            }

            foreach (Crate crate in crateSpawn.crates)
            {
                activeObjects.Add(crate);
                _physicsWorld.PhysObjects.Add(crate.ObjRB);
            }

            foreach (Platform platform in board.platforms)
            {
                _physicsWorld.PhysObjects.Add(platform.ObjRB);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        //public List<Node> path;
        protected override void Update(GameTime gameTime)
        {
            switch (_state)
            {
                case GameState.Menu:
                    UpdateMenu(gameTime);
                    break;
                case GameState.lvl1:
                    UpdateLvl1(gameTime);
                    break;
                case GameState.End:
                    UpdateEnd(gameTime);
                    break;
            }
            
        }

         void UpdateLvl1(GameTime gameTime)
        {
            if (board.generationFinished)
            {

                CheckPauseKey(Keyboard.GetState());

                if (!paused)
                {
                    camera.Update(gameTime, player);
                    base.Update(gameTime);
                    selector.isVisible = false;
                    selector.Position = drone.Position;
                    _physicsWorld.Step(gameTime); //update the physics world once per frame

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();

                    if (Keyboard.GetState().IsKeyDown(Keys.E))
                    {
                        selector.nodeSet = false;
                    }

                    foreach (GameObject actObj in activeObjects)
                        actObj.Update(gameTime);

                    foreach (Tile tile in activeTiles)
                        tile.Update(gameTime);

                    player.PlayerNode = board.NodeFromWorldPoint(player.Center);

                    if (!selector.nodeSet)
                    {
                        drone.target = player.Position;
                        player.PlayerNode.isTraversible = true;
                    }
                    else
                    {
                        player.PlayerNode.isTraversible = false;
                    }

                    if (player.Position.X > 1300)
                        crateSpawn.timeToSpawn = true;
                    
                }
                else
                {
                    base.Update(gameTime);
                    //Open Selector Screen
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();
                    selector.isVisible = true;
                    selector.Update(gameTime);
                    camera.Update(gameTime, selector);
                    //Console.WriteLine(player.Position.X);
                    if (selector.nodeSet)
                        drone.target = selector.targetNode.worldPosition;
                }
            }
           
        }

        void UpdateMenu(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                _state = GameState.lvl1;
        }

        void UpdateEnd(GameTime gameTime)
        {

        }

        private void BeginPause(bool userInitiated)
        {
            paused = true;
            pausedForGuide = !userInitiated;
        }

        private void EndPause()
        {
            pausedForGuide = false;
            paused = false;
        }

        private void CheckPauseKey(KeyboardState keyboardState)
        {
            bool pauseKeyDownThisFrame = (keyboardState.IsKeyDown(Keys.Tab));

            if (!pauseKeyDown && pauseKeyDownThisFrame)
            {
                if (!paused)
                    BeginPause(true);
                else
                    EndPause();
            }
            pauseKeyDown = pauseKeyDownThisFrame;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            switch (_state)
            {
                case GameState.Menu:
                    DrawMenu();
                    break;
                case GameState.lvl1:
                    DrawLvl1();
                    break;
            }
        }

        void DrawMenu()
        {
            GraphicsDevice.Clear(Color.PaleVioletRed);
            _spriteBatch.Begin();
            menu.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        void DrawLvl1()
        {
            GraphicsDevice.Clear(Color.PaleVioletRed);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                camera.transform);
            // TODO: Add your drawing code here
            
            board.Draw(_spriteBatch);
            player.Draw(_spriteBatch);
            selector.Draw(_spriteBatch);
            drone.Draw(_spriteBatch);
            key.Draw(_spriteBatch);
            crateSpawn.Draw(_spriteBatch);
            //testCrate.Draw(_spriteBatch);
            foreach (RigidBody2D rb in _physicsWorld.PhysObjects)//draw bounding boxes
            {
                rb.Draw(_spriteBatch);
            }
            _spriteBatch.DrawString(font, "Fuel: " + player.currentJetPackFuel, new Vector2(camera.centre.X + 50, camera.centre.Y + 700), Color.Black);
            _spriteBatch.End();
        }
    }
}
