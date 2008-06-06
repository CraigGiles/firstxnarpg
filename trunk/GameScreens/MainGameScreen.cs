using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class MainGameScreen : GameScreen
    {

        Player player = new Player();


        #region Constructor(s)


        public MainGameScreen()
        {
            this.Name = "MainGame";
            this.HasFocus = true;
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


        #region Update

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(bool coveredByOtherScreen)
        {
            if (this.HasFocus)
                UpdateScreen();



            base.Update(coveredByOtherScreen);
        }
        
        
        /// <summary>
        /// Updates the main game screen
        /// </summary>
        private void UpdateScreen()
        {
            //update player
            player.Update();

            foreach (NPC npc in Globals.TileEngine.Map.NPCs)
                npc.Update();


            //update tile engine
            Globals.TileEngine.Update();
            Globals.TileEngine.ClampCameraToBoundries(player.Origin);
        }

        
        #endregion


        #region Draw

        public override void Draw()
        {
            //draw tile engine, regardless if map editor is enabled
            Globals.TileEngine.Draw();

            //if map editor is not active, render the game to screen
            if (!MapEditor.Active)
                RenderGame();


            base.Draw();
        }

        /// <summary>
        /// Renders everything needed to play the game to the screen
        /// </summary>
        private void RenderGame()
        {
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None, Globals.Camera.TransformMatrix);

            player.Draw();

            foreach (NPC npc in Globals.TileEngine.Map.NPCs)
                npc.Draw();

            Globals.Batch.End();
        }

        #endregion
        
    }
}
