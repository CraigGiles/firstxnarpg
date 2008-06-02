using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    class MainMenuScreen : GameScreen
    {
        Camera2D camera;

        #region Constructor(s)

        public MainMenuScreen()
        {
            this.Name = "MainMenu";
            LoadContent();
        }

        public override void LoadContent()
        {
            this.HasFocus = true;
            //camera = new Camera2D();
            //Globals.Camera = camera;

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        #region Public Methods

        public override void Update(bool coveredByOtherScreen)
        {
            //if this screen doesn't have focus
            if (!this.HasFocus)
                UpdateIfNotFocused();

            //if this screen does have focus
            else
            {




                Globals.TileEngine.Update();

            }

            base.Update(coveredByOtherScreen);
        }

        public override void Draw()
        {
            Globals.TileEngine.Draw();
            base.Draw();
        }

        #endregion

        #region Private Methods

        private void UpdateIfNotFocused()
        {
        }

        #endregion

    }
}
