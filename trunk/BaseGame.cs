using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using Helper;

namespace ActionRPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BaseGame : Microsoft.Xna.Framework.Game
    {
        FPSCounter fps = new FPSCounter();
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        SpriteBatch spriteBatch;
        TileEngine tileEngine;

        Camera2D camera;

        #region Constructor(s)
        public BaseGame()
        {
            graphics = new GraphicsDeviceManager(this);
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
            Settings.LoadSettings();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
                        
            PopulateGlobals();
            InitiateScreenManager();
            
        }

        /// <summary>
        /// Populates all static global settings
        /// </summary>
        private void PopulateGlobals()
        {
            Globals.Batch = spriteBatch;
            Globals.Font = Content.Load<SpriteFont>(@"Courier New");
            Globals.Content = Content;
            Globals.Graphics = graphics;
            Globals.Game = this;

            tileEngine = new TileEngine();
            Globals.TileEngine = tileEngine;

            camera = new Camera2D();
            Globals.Camera = camera;
        }
        
        /// <summary>
        /// Starts the Screen Manager and loads the default screen
        /// </summary>
        private void InitiateScreenManager()
        {
            screenManager = new ScreenManager();
            screenManager.Initialize(new MainMenuScreen());
            screenManager.AddScreen(new MapEditor());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            screenManager.UnloadContent();
        }
        #endregion

        #region Update
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

            UpdateDeltaTime(gameTime);
            fps.Update();
            screenManager.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the delta time Global settings
        /// </summary>
        /// <param name="gameTime"></param>
        private static void UpdateDeltaTime(GameTime gameTime)
        {
            Globals.GameTime = gameTime;
            Globals.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draws all screens
            screenManager.Draw();

            //Draws components (such as FPS counter)
            DrawGameComponents();



            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws all game components such as FPS counter
        /// </summary>
        private void DrawGameComponents()
        {
            Globals.Batch.Begin();

            fps.Draw();

            Globals.Batch.End();
        }
        #endregion
    }
}
