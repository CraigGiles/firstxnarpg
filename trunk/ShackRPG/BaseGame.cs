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
using ShackRPG.GameScreens;
using ShackRPG.GameComponents;

namespace ShackRPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BaseGame : Microsoft.Xna.Framework.Game
    {
        

        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Stack<IGameScreen> screen = new Stack<IGameScreen>();
        #endregion

        #region Constructor and Initializing / unloading methods
        public BaseGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            base.Components.Add(new FPSCounter(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            screen.Push(new MainMenu(Content, spriteBatch, this, font));

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>(@"Courier New");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }
        #endregion

        #region Methods
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
            Input.GetUserInput();

            screen.Peek().Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Adds a new game screen on top of the stack
        /// </summary>
        /// <param name="newScreen"></param>
        public void AddNewGameScreen(IGameScreen newScreen)
        {
            screen.Push(newScreen);
        }

        /// <summary>
        /// Removes the current game screen from the stack
        /// </summary>
        public void RemoveCurrentGameScreen()
        {
            screen.Pop();

            if (screen.Count <= 0)
                screen.Push(new MainMenu(Content, spriteBatch, this, font));
        }

        #endregion

        #region Draw and Animation methods
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            screen.Peek().Draw(gameTime);

            base.Draw(gameTime);
        }
        #endregion

    }//end class
}//end namespace
