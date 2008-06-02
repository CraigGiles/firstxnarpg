using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ActionRPG
{
    public class ScreenManager
    {
        #region Data


        //Is the screen manager initialized
        bool initialized = false;

        /// <summary>
        /// List of all screens loaded into memory
        /// </summary>
        List<GameScreen> screens = new List<GameScreen>();

        /// <summary>
        /// List of screens needing to be updated
        /// </summary>
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        /// <summary>
        /// List of screens ready to be removed from memory
        /// </summary>
        List<GameScreen> screensToRemove = new List<GameScreen>();

        /// <summary>
        /// Input helper
        /// </summary>
        Helper.Input input = new Helper.Input();


        #endregion


        #region Constructor(s)

        public ScreenManager()
        {
            //Lets the gamescreens know where to find the screen manager
            GameScreen.ScreenManager = this;
        }

        ~ScreenManager()
        {
            //unloads all content associated with the game
            UnloadContent();
        }

        /// <summary>
        /// Initializes the Screen Manager and sets an entry screen
        /// </summary>
        /// <param name="entryScreen">First screen shown to player</param>
        public void Initialize(GameScreen entryScreen)
        {
            //adds the entry screen to the gamescreens
            screens.Add(entryScreen);

            //marks ScreenManager as initialized
            initialized = true;
        }

        /// <summary>
        /// Unloads all game screens from memory
        /// </summary>
        public void UnloadContent()
        {
            //cycles through all game screens currently loaded, and
            //tells them to unload all their content
            foreach (GameScreen s in screens)
                s.UnloadContent();

            //removes all entries from the screens list
            screens.Clear();
            screensToRemove.Clear();
            screensToUpdate.Clear();
        }


        #endregion


        #region Public Methods

        /// <summary>
        /// Adds a new game screen to the game
        /// </summary>
        /// <param name="screen">Game Screen to be loaded</param>
        public void AddScreen(GameScreen screen)
        {
            //load the new screens content
            screen.LoadContent();

            //new screen has focus
            screen.HasFocus = true;

            //all old screens do not have focus
            foreach (GameScreen s in screens)
                s.HasFocus = false;

            //add screen to list
            screens.Add(screen);

            if (screen.Name == "MapEditor")
                MapEditor.Active = true;
        }


        /// <summary>
        /// Removes a specific game screen from memory
        /// </summary>
        /// <param name="screen"></param>
        public void RemoveScreen(GameScreen screen)
        {
            //adds the screen to the ScreensToRemove list
            screensToRemove.Add(screen);

            //sets the 2nd to last screen to "Screen Has Focus"
            screensToUpdate[screensToUpdate.Count - 2].HasFocus = true;
        }


        /// <summary>
        /// Removes a screen with the asset name specified
        /// </summary>
        /// <param name="asset"></param>
        public void RemoveScreen(string asset)
        {
            //Cycles through all game screens to search for asset name
            foreach (GameScreen screen in screens)
            {
                //if screens name is the asset to be removed, remove it
                if (screen.Name == asset)
                {
                    screensToRemove.Add(screen);
                    screensToUpdate[screensToUpdate.Count - 2].HasFocus = true;
                                        
                    break;
                }
            }
        }
        

        /// <summary>
        /// Updates all screens needing to be updated
        /// </summary>
        public void Update()
        {
            //remove any screens that requested to be removed
            ClearRemovedScreens();

            //Get users input and put it into Globals
            input.GetUserInput(Globals.GameTime);
            Globals.Input = input;


            

            //clears the update list
            screensToUpdate.Clear();

            //populates the update list
            foreach (GameScreen s in screens)
                screensToUpdate.Add(s);

            //set popscreen active to false
            bool isPopupScreenActive = false;

            //update all screens
            foreach (GameScreen s in screensToUpdate)
            {

                s.Update(isPopupScreenActive); 


                if (s.IsPopup)
                    isPopupScreenActive = true;                
            }

            //Check for console
            if (Globals.Input.IsKeyPressed(Keys.OemTilde) && screens[screens.Count - 1].Name != "Console")
                this.AddScreen(new Console());

        }//end Update()


        /// <summary>
        /// Renders all screens to the screen
        /// </summary>
        public void Draw()
        {

            foreach (GameScreen s in screens)
            {
                //if screen is hidden, do not draw
                if (s.IsScreenHidden)
                    continue;

                //if screen is not hidden, draw the screen
                s.Draw();
            }

        }//end Draw()

        #endregion


        #region Private methods

        /// <summary>
        /// Removes and unloads all game screens content in the ScreensToRemove list
        /// </summary>
        private void ClearRemovedScreens()
        {
            //cycle through all game screens to be removed
            foreach (GameScreen screen in screensToRemove)
            {
                //unload the screens content
                screen.UnloadContent();

                //remove the screen from all lists
                screens.Remove(screen);
                screensToUpdate.Remove(screen);

                if (screen.Name == "MapEditor")
                    MapEditor.Active = false;
            }

            //once all screens have been removed, clear the list
            screensToRemove.Clear();
        }

        #endregion
        
    }//end Class
}//end Namespace
