using System;
using System.Collections.Generic;
using System.Text;
using Helper;

namespace ActionRPG
{
    public class ScreenManager
    {
        bool initialized = false;

        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();
        List<GameScreen> screensToRemove = new List<GameScreen>();

        Helper.Input input = new Helper.Input();

        #region Constructor(s)
        public ScreenManager()
        {
            GameScreen.ScreenManager = this;
        }

        ~ScreenManager()
        {
            UnloadContent();
        }

        public void Initialize(GameScreen entryScreen)
        {
            screens.Add(entryScreen);
            initialized = true;
        }

        public void UnloadContent()
        {
            foreach (GameScreen s in screens)
                s.UnloadContent();

            screens.Clear();
            screensToRemove.Clear();
            screensToUpdate.Clear();
        }
        #endregion

        #region Public Methods

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
        }

        public void RemoveScreen(GameScreen screen)
        {
            screensToRemove.Add(screen);
            screensToUpdate[screensToUpdate.Count - 2].HasFocus = true;
        }

        public void RemoveScreen(string asset)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.Name == asset)
                {
                    screensToRemove.Add(screen);
                    screensToUpdate[screensToUpdate.Count - 2].HasFocus = true;

                    break;
                }
            }
        }
        
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

        }//end Update()

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

        private void ClearRemovedScreens()
        {
            foreach (GameScreen screen in screensToRemove)
            {
                screen.UnloadContent();

                screens.Remove(screen);
                screensToUpdate.Remove(screen);
            }

            screensToRemove.Clear();
        }

        #endregion

    }//end Class
}//end Namespace
