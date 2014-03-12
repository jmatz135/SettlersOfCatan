/* Called from program
 * Initializes all of the components and sets up the gamestates for the program to run.
 * Calls StartMenuState to move the game to the start menu
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using SOCProjectLibrary;
using System.IO;

namespace SettlersOfCatan
{
    public class SettlersOfCatan : Microsoft.Xna.Framework.Game
    {
        public GameBoard gameBoard;
        public Dice gameDice;
        public string gameType;

        private GraphicsDeviceManager graphics;
        public  SpriteBatch SpriteBatch;

        public  Camera camera;
        public InputHandler input;
        private GameStateManager gameManager;

        public IStartMenuState StartMenuState;
        public IPlayingState PlayingState;
        public IFinishedState FinishedState;

        public SpriteFont Font;

        public Boolean fullscreen = false;
        public int height = 768;
        public int width = 1024;

        public int mouseX;
        public int mouseY;

        //Players
        public int numPlayers;
        public List<Player> players;
        public Player humanPlayer;
        public Player aiPlayer1;
        public Player aiPlayer2;

        private FrameRate fps;

        public SettlersOfCatan()
        {
            gameType = "BaseGame";
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = fullscreen;
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;

            input = new InputHandler(this);
            Components.Add(input);

            camera = new Camera(this);
            Components.Add(camera);
            camera.UpdateInput = false;

            gameManager = new GameStateManager(this);
            Components.Add(gameManager);

            //Components.Add(new GamerServicesComponent(this));

            //TitleIntroState = new TitleIntroState(this);
            StartMenuState = new StartMenuState(this);
            PlayingState = new PlayingState(this);
            FinishedState = new FinishedState(this);

            gameManager.ChangeState(StartMenuState.Value);

#if DEBUG
            fps = new FrameRate(this);
#else
            fps = new FrameRate(this, true, false);
#endif
            Components.Add(fps);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>(@"Fonts\game");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Gray);

            base.Draw(gameTime);
        }
    }
}
