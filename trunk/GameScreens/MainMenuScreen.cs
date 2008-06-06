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
        

        #region Constructor(s)

        public MainMenuScreen()
        {
            this.Name = "MainMenu";
            LoadContent();
        }

        public override void LoadContent()
        {
            this.HasFocus = true;

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
            if (this.HasFocus)
                UpdateScreen();

            base.Update(coveredByOtherScreen);
        }


        private void UpdateScreen()
        {
            if (Globals.Input.IsKeyPressed(Keys.Enter))
                ScreenManager.AddScreen(new MainGameScreen());
        }


        public override void Draw()
        {
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None);

            DrawTextInstructions();

            Globals.Batch.End();

            base.Draw();
        }

        private void DrawTextInstructions()
        {
            Globals.Batch.DrawString(Globals.Font, "Enter: Begin Game",
                new Vector2(10, 10), Color.Yellow, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

            Globals.Batch.DrawString(Globals.Font, "Tilde: Enter Console",
                new Vector2(10, 50), Color.Yellow, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

            Globals.Batch.DrawString(Globals.Font, "Console: Map_Editor 1 \n Launch Editor",
                new Vector2(10, 100), Color.Yellow, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

            Globals.Batch.DrawString(Globals.Font, "Game: W.A.S.D. Move",
                new Vector2(10, 250), Color.Yellow, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
        }


        #endregion

    }
}
