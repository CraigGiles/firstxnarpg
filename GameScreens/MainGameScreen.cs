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
        DialogManager dialog = new DialogManager();

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
            {
                CheckButtonPress();                
            }

            //if map editor is NOT active, update game
            if (!MapEditor.Active)
            {
                UpdateScreen();
                UpdateDialogs();
            }

            base.Update(coveredByOtherScreen);
        }
        
        
        /// <summary>
        /// Updates the main game screen
        /// </summary>
        private void UpdateScreen()
        {
            //Update player
            player.Update();

            //Updates all NPCs on screen
            UpdateNpcCharacters();

            //Checks for collisions
            CheckCollisions();

            //update Text Animations
            TextManager.Update();

            //update tile engine
            Globals.TileEngine.Update();
            Globals.TileEngine.ClampCameraToBoundries(player.Origin);

            ClearUnusedResources();
        }


        private void UpdateDialogs()
        {

        }


        private void CheckButtonPress()
        {
            if (Globals.Input.IsKeyPressed(Globals.Settings.Inventory) ||
                Globals.Input.IsButtonPressed(Globals.Settings.GamePadInventory))
            {
                ScreenManager.AddScreen(new GameMenu(player));
            }

            else if (Globals.Input.IsKeyPressed(Globals.Settings.Okay) ||
                Globals.Input.IsButtonPressed(Buttons.A))
            {
                foreach (NPC npc in Globals.TileEngine.Map.NPCs)
                {
                    if (AreWithinRange(player, npc))
                    {
                        //start new dialog
                    }
                }

                foreach (Item item in Globals.TileEngine.Map.WorldItems)
                {
                }


                //check distance between player and objects
                //if object is within X distance
                    //execute objects "okay" setting (merchant, talk, pickup, etc)
            }
        }


        /// <summary>
        /// Updates all NPC character logic
        /// </summary>
        private void UpdateNpcCharacters()
        {
            foreach (NPC npc in Globals.TileEngine.Map.NPCs)
            {
                npc.Update(player);

            } 


        }


        /// <summary>
        /// Checks for collisions between characters and objects
        /// </summary>
        private void CheckCollisions()
        {
            CheckNpcCollision();
            CheckItemCollision();
        }


        /// <summary>
        /// Checks for a collision between characters on screen
        /// </summary>
        private void CheckNpcCollision()
        {
            foreach (NPC npc in Globals.TileEngine.Map.NPCs)
            {
                if (AreColliding(player, npc))
                {
                    Vector2 d = Vector2.Normalize(npc.FootLocation - player.FootLocation);

                    player.Origin =
                        npc.Origin - (d * (player.CollisionRadius + npc.CollisionRadius));
                }
            }
        }
        

        /// <summary>
        /// Returns true if two characters are colliding
        /// </summary>
        /// <param name="a">Character A</param>
        /// <param name="b">Character B</param>
        /// <returns>bool</returns>
        private bool AreColliding(Character a, Character b)
        {
            Vector2 d = b.FootLocation - a.FootLocation;

            return (d.Length() < b.CollisionRadius + a.CollisionRadius);
        }


        private bool AreWithinRange(Character a, Character b)
        {
            Vector2 d = b.FootLocation - a.FootLocation;

            return (d.Length() < b.InteractionRadius + a.InteractionRadius);
        }

        /// <summary>
        /// Checks collision between player and world items.
        /// </summary>
        private void CheckItemCollision()
        {
            foreach (Item i in Globals.TileEngine.Map.WorldItems)
            {
                Vector2 d = player.FootLocation - i.Origin;

                if (d.Length() < player.CollisionRadius * 1f)
                {
                    //TODO; pop up msgbox stating what you just picked up                    
                    //ScreenManager.AddScreen(new MessageBox("Gained: " + i.AssetName, 3f));

                    Vector2 textLocation = player.Origin - new Vector2(0, player.SpriteManager.SpriteBox.Height / 2);

                    Globals.TileEngine.Map.RemoveWorldItem(i);
                    player.Inventory.AddItem(i, textLocation);
                }
            }
        }


        private void ClearUnusedResources()
        {

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
            Globals.Batch.Begin(
                SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, 
                SaveStateMode.None, Globals.Camera.TransformMatrix);

            
            foreach (NPC npc in Globals.TileEngine.Map.NPCs)
                npc.Draw();

            player.Draw();

            //draws text animations
            TextManager.Draw();

            Globals.Batch.End();
        }

        #endregion
        
    }
}
