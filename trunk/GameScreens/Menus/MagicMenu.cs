using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public class MagicMenu : GameScreen
    {
        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(bool coveredByOtherScreen)
        {
            if (this.HasFocus)
                UpdateScreen();

            base.Update(coveredByOtherScreen);
        }

        private void UpdateScreen()
        {
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
