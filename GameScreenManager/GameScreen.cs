#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace ActionRPG
{
    public abstract class GameScreen
    {
        #region Screen Settings


        /// <summary>
        /// Gets or sets the screens name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string name;

        /// <summary>
        /// Gets or sets if the screen is a popup screen
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }
        bool isPopup = false;

        
        /// <summary>
        /// Gets or sets if this screen has focus
        /// </summary>
        public bool HasFocus
        {
            get { return hasFocus; }
            set { hasFocus = value; }
        }
        bool hasFocus = false;

               
        /// <summary>
        /// Gets or sets if this screen is hidden
        /// </summary>
        public bool IsScreenHidden
        {
            get { return isScreenHidden; }
            set { isScreenHidden = value; }
        }
        bool isScreenHidden = false;
        
                
        /// <summary>
        /// Gets or sets if this screen is to be removed
        /// </summary>
        public bool RemoveScreen
        {
            get { return removeScreen; }
            set { removeScreen = value; }
        }
        bool removeScreen = false;


        /// <summary>
        /// Screen manager managing all game screens
        /// </summary>
        public static ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }
        static ScreenManager screenManager;

        #endregion       

        #region Constructor(s)
        public virtual void LoadContent()
        { }

        public virtual void UnloadContent()
        { }
        #endregion

        #region Public Methods
        public virtual void Update(bool coveredByOtherScreen)
        { }

        public virtual void Draw()
        { }
        #endregion
        
    }
}
