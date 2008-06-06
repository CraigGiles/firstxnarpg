using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;
using Microsoft.Xna.Framework.Input;

namespace ActionRPG
{
    public class MapEditor : GameScreen
    {
        #region Data


        public static bool Active = false;

        int mouseTileX;
        int mouseTileY;
        Point mouse;

        Texture2D emptyTile,
            textureBorder,
            unwalkableTile,
            playerRespawn,
            monsterSpawn,
            portal,
            WorldItem;
        
        Texture2D[] previewTextures;
        int previewTextureSize = 48;


        int newCellIndex = 1;

        public MapLayer editLayer = MapLayer.Base;
        #endregion


        #region Constructor(s)


        public MapEditor()
        {
            this.HasFocus = true;
            this.IsPopup = true;
            Globals.Game.IsMouseVisible = true;
            this.Name = "MapEditor";
            
            LoadContent();
        }


        ~MapEditor()
        {
            UnloadContent();
        }


        public override void LoadContent()
        {
            emptyTile = Globals.Content.Load<Texture2D>(@"Graphics/MapEditor/EmptyTile");
            textureBorder = Globals.Content.Load<Texture2D>(@"Graphics/MapEditor/texturepreview");
            unwalkableTile = Globals.Content.Load<Texture2D>(@"Graphics/MapEditor/unwalkable");
            playerRespawn = Globals.Content.Load<Texture2D>(@"Graphics/MapEditor/PlayerSpawn");
            monsterSpawn = Globals.Content.Load<Texture2D>(@"Graphics/MapEditor/MonsterSpawn");
            portal = Globals.Content.Load<Texture2D>(@"Graphics/MapEditor/portal");
            WorldItem = Globals.Content.Load<Texture2D>(@"Graphics/MapEditor/WorldItem");

            previewTextures = Globals.TileEngine.Map.Tiles.ToArray();

            base.LoadContent();
        }


        public override void UnloadContent()
        {
            Globals.Game.IsMouseVisible = false;

            emptyTile = null;
            textureBorder = null;
            unwalkableTile = null;
            previewTextures = null;
            playerRespawn = null;

            base.UnloadContent();
        }


        #endregion


        #region Update

        public override void Update(bool coveredByOtherScreen)
        {
            mouseTileX = Globals.TileEngine.MouseLocation.X - (int)Globals.Camera.Position.X / Globals.TileEngine.Map.TileWidth;
            mouseTileY = Globals.TileEngine.MouseLocation.Y - (int)Globals.Camera.Position.Y / Globals.TileEngine.Map.TileHeight;

            if (this.HasFocus)
            {
                CheckKeyboardInput();
                CheckMouseInput(); 
            }

            base.Update(coveredByOtherScreen);
        }

        #endregion


        #region Keyboard Input


        private void CheckKeyboardInput()
        {
            /* Left Alt: This key adjusts the height of the map
             * Left Control: This key adjusts the width of the map */
            #region Add / Remove from width
            // ctrl + add = increase width by 1
            if (Globals.Input.IsKeyHeld(Keys.LeftControl) && Globals.Input.IsKeyPressed(Keys.Add) ||
                Globals.Input.IsKeyHeld(Keys.LeftControl) && Globals.Input.IsKeyPressed(Keys.OemPlus))
            {
                Globals.TileEngine.Map.AdjustMapSize(0, +1);
            }

            // ctrl + subtract = decrease width by 1
            else if (Globals.Input.IsKeyHeld(Keys.LeftControl) && Globals.Input.IsKeyPressed(Keys.Subtract) ||
                Globals.Input.IsKeyHeld(Keys.LeftControl) && Globals.Input.IsKeyPressed(Keys.OemMinus))
            {
                Globals.TileEngine.Map.AdjustMapSize(0, -1);
            }
            #endregion

            #region Add / Remove from Height
            // alt + Add = increase height by 1
            if (Globals.Input.IsKeyHeld(Keys.LeftAlt) && Globals.Input.IsKeyPressed(Keys.Add) ||
                Globals.Input.IsKeyHeld(Keys.LeftAlt) && Globals.Input.IsKeyPressed(Keys.OemPlus))
            {
                Globals.TileEngine.Map.AdjustMapSize(1, 0);
            }

            // alt + subtract = decrease height by 1
            else if (Globals.Input.IsKeyHeld(Keys.LeftAlt) && Globals.Input.IsKeyPressed(Keys.Subtract) ||
                Globals.Input.IsKeyHeld(Keys.LeftAlt) && Globals.Input.IsKeyPressed(Keys.OemMinus))
            {
                Globals.TileEngine.Map.AdjustMapSize(-1, 0);
            }
            #endregion

            /* Scroll and Zoom Camera*/
            #region Camera

            /* Arrow keys: Scroll Camera or
             * W-A-S-D: Scroll Camera */
            if (Globals.Input.IsKeyHeld(Keys.Up) || Globals.Input.IsKeyHeld(Keys.W))
                Globals.Camera.Position.Y += Globals.TileEngine.Map.TileHeight;
            if (Globals.Input.IsKeyHeld(Keys.Down) || Globals.Input.IsKeyHeld(Keys.S))
                Globals.Camera.Position.Y -= Globals.TileEngine.Map.TileHeight;
            if (Globals.Input.IsKeyHeld(Keys.Left) || Globals.Input.IsKeyHeld(Keys.A))
                Globals.Camera.Position.X += Globals.TileEngine.Map.TileWidth;
            if (Globals.Input.IsKeyHeld(Keys.Right) || Globals.Input.IsKeyHeld(Keys.D))
                Globals.Camera.Position.X -= Globals.TileEngine.Map.TileWidth;

            /* Home / End : Zoom Camera */
            if (Globals.Input.IsKeyHeld(Keys.Home))
                Globals.TileEngine.Map.Scale += Globals.DeltaTime;

            if (Globals.Input.IsKeyHeld(Keys.End))
                Globals.TileEngine.Map.Scale -= Globals.DeltaTime;

            /* Reset Camera */
            if (Globals.Input.IsKeyPressed(Keys.Space))
            {
                Globals.Camera.Position = new Vector2(0, 0);
                Globals.TileEngine.Map.Scale = 1.0f;
            }

            #endregion           

            /* F1 - F4 selects which layer is being edited */
            #region Select layer to edit

            if (Globals.Input.IsKeyPressed(Keys.F1))
                editLayer = MapLayer.Base;
            if (Globals.Input.IsKeyPressed(Keys.F2))
                editLayer = MapLayer.Fringe;
            if (Globals.Input.IsKeyPressed(Keys.F3))
                editLayer = MapLayer.Object;
            if (Globals.Input.IsKeyPressed(Keys.F4))
                editLayer = MapLayer.Collision;

            #endregion

            /* PgUp, PgDwn, and Mouse Wheel cycles through preview textures */
            #region Select Texture


            if (Globals.Input.IsKeyPressed(Keys.PageUp))
                newCellIndex = (int)MathHelper.Clamp(newCellIndex - 1, 0, previewTextures.Length - 1);
            
            if (Globals.Input.IsKeyPressed(Keys.PageDown))
                newCellIndex = (int)MathHelper.Clamp(newCellIndex + 1, 0, previewTextures.Length - 1);

            //mouse wheel
            newCellIndex = (int)MathHelper.Clamp(newCellIndex -= Globals.Input.MouseScrollWheel , 0, previewTextures.Length - 1);
            

            #endregion

            /* Add player spawn */
            if (Globals.Input.IsKeyPressed(Keys.P))
                Globals.TileEngine.Map.SetPlayerRespawn(new Point(mouseTileX, mouseTileY));

            /* Add a monster spawn */
            if (Globals.Input.IsKeyPressed(Keys.M))
                Globals.TileEngine.Map.AddNpc(new Point(mouseTileX, mouseTileY));

            /* Add an WorldItem spawn */
            if (Globals.Input.IsKeyPressed(Keys.I))
                Globals.TileEngine.Map.AddWorldItem(new Point(mouseTileX, mouseTileY));

            /* Add Portal */
            if (Globals.Input.IsKeyPressed(Keys.O))
                Globals.TileEngine.Map.AddPortal(new Point(mouseTileX, mouseTileY));

            /* Delete any object on current tile */
            if (Globals.Input.IsKeyPressed(Keys.Delete))
                DeleteObjects();

        }

        /// <summary>
        /// Deletes any portals, monsters, WorldItems, etc... from tile
        /// </summary>
        private void DeleteObjects()
        {
            //Removes any NPCs on current tile
            foreach (NPC n in Globals.TileEngine.Map.NPCs)
            {
                if (n.SpawnLocationTile.X == mouseTileX && n.SpawnLocationTile.Y == mouseTileY)
                    Globals.TileEngine.Map.RemoveNpc(n);
            }


            //removes any portals on current tile
            foreach (Portal p in Globals.TileEngine.Map.Portals)
            {
                if (p.PortalEnteranceTile.X == mouseTileX && p.PortalEnteranceTile.Y == mouseTileY)
                    Globals.TileEngine.Map.RemovePortal(p);
            }


            //removes any WorldItems on current tile
            foreach (WorldItem i in Globals.TileEngine.Map.WorldItems)
            {
                if (i.SpawnLocationTile.X == mouseTileX && i.SpawnLocationTile.Y == mouseTileY)
                    Globals.TileEngine.Map.RemoveWorldItem(i);
            }


            //Trashes all removed objects.
            Globals.TileEngine.Map.TrashDeletedWorldItems();

        }


        #endregion


        #region Mouse Input


        private void CheckMouseInput()
        {
            if (Globals.Input.IsMouseButtonHeldLeft())
                LeftMouseButton();

            if (Globals.Input.IsMouseButtonHeldRight())
                RightMouseButton();
        }


        private void LeftMouseButton()
        {
            if (editLayer != MapLayer.Collision)
            {
                if (CheckForValidClick())
                {
                    Globals.TileEngine.Map.SetCellIndex(editLayer, mouseTileX, mouseTileY, newCellIndex);
                }
            }
            else
            {
                if (CheckForValidClick())
                    Globals.TileEngine.Map.SetCellIndex(MapLayer.Collision, mouseTileX, mouseTileY, 1);
            }
        }


        private void RightMouseButton()
        {
            if (editLayer == MapLayer.Collision)
            {
                if (CheckForValidClick())
                    Globals.TileEngine.Map.SetCellIndex(MapLayer.Collision, mouseTileX, mouseTileY, 0);
            }
        }


        private bool CheckForValidClick()
        {
            return mouseTileX >= 0 && mouseTileX < Globals.TileEngine.Map.Width &&
                                mouseTileY >= 0 && mouseTileY < Globals.TileEngine.Map.Height;
        }


        #endregion


        #region Draw


        public override void Draw()
        {
            //Draws the player respawn icon 
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, Globals.Camera.MapEditorTransformMatrix);
            
            Globals.Batch.Draw(
                playerRespawn,
                new Rectangle(
                    (int)(Globals.TileEngine.Map.PlayerRespawnVector.X * Globals.TileEngine.Map.Scale),
                    (int)(Globals.TileEngine.Map.PlayerRespawnVector.Y * Globals.TileEngine.Map.Scale),
                        (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale),
                        (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale)),
                Color.White);

            foreach (NPC n in Globals.TileEngine.Map.NPCs)
            {
                Globals.Batch.Draw(
                    monsterSpawn,
                    new Rectangle(
                        (int)(n.SpawnLocationVector.X * Globals.TileEngine.Map.Scale),
                        (int)(n.SpawnLocationVector.Y * Globals.TileEngine.Map.Scale),
                        (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale),
                        (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale)),
                    Color.White);
            }

            foreach (Portal p in Globals.TileEngine.Map.Portals)
            {
                Globals.Batch.Draw(
                    portal,
                    new Rectangle(
                        (int)(p.PortalEnteranceVector.X * Globals.TileEngine.Map.Scale),
                        (int)(p.PortalEnteranceVector.Y * Globals.TileEngine.Map.Scale),
                        (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale),
                        (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale)),
                    Color.White);
            }

            foreach (WorldItem i in Globals.TileEngine.Map.WorldItems)
            {
                Globals.Batch.Draw(
                    WorldItem,
                    new Rectangle(
                        (int)(i.SpawnLocationVector.X * Globals.TileEngine.Map.Scale),
                        (int)(i.SpawnLocationVector.Y * Globals.TileEngine.Map.Scale),
                        (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale),
                        (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale)),
                    Color.White);
            }

            Globals.Batch.End();


            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            
            DrawOverlay();
            DrawPreviewTextures();

            Globals.Batch.End();



            /* Draw Collisions if editing collision layer */
            #region Draw Collisions
            
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, Globals.Camera.MapEditorTransformMatrix);
            
            if (editLayer == MapLayer.Collision)
            {
                DrawCollisions();
            }

            Globals.Batch.End();
            
            #endregion

            base.Draw();
        }


        /// <summary>
        /// Draws the red collision tiles when collision layer
        /// is being modified
        /// </summary>
        private void DrawCollisions()
        {
            for (int x = 0; x < Globals.TileEngine.Map.Width; x++)
            {
                for (int y = 0; y < Globals.TileEngine.Map.Height; y++)
                {
                    if (Globals.TileEngine.Map.CollisionLayer[y, x] != 0)
                        Globals.Batch.Draw(
                        unwalkableTile,
                        new Rectangle(
                            x * (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale), 
                            y * (int)(Globals.TileEngine.Map.TileHeight * Globals.TileEngine.Map.Scale), 
                            (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale), 
                            (int)(Globals.TileEngine.Map.TileHeight * Globals.TileEngine.Map.Scale)),
                        Color.Red);
                }
            }
        }


        /// <summary>
        /// Draws the overlay information over the rendered map
        /// </summary>
        private void DrawOverlay()
        {

            //ajusts the point by where the camera is. If the camera is at 0,0 it worked
            // fine, but if you scrolled even 1 tile over, it would go out of bounds.
            // The following code prevents this.
            int camXoffset = (int)Globals.Camera.Position.X / Globals.TileEngine.Map.TileWidth;
            int camYoffset = (int)Globals.Camera.Position.Y / Globals.TileEngine.Map.TileHeight;
            mouse = new Point(mouseTileX - camXoffset, mouseTileY - camYoffset);


            #region Mouse Information
            
            
            /* Vector2 location of mouse cursor in gameworld */
            float xLoc = Globals.Input.MouseLocation.X - Globals.Camera.Position.X;
            float yLoc = Globals.Input.MouseLocation.Y - Globals.Camera.Position.Y;


            /* Draw the red empty tile texture below mouse cursor */
            Globals.Batch.Draw(emptyTile, new Rectangle(
                Globals.TileEngine.MouseLocation.X * (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale),
                Globals.TileEngine.MouseLocation.Y * (int)(Globals.TileEngine.Map.TileHeight * Globals.TileEngine.Map.Scale),
                (int)(Globals.TileEngine.Map.TileWidth * Globals.TileEngine.Map.Scale),
                (int)(Globals.TileEngine.Map.TileHeight * Globals.TileEngine.Map.Scale)),
                Color.Red);
            

            #endregion


            #region Text Overlay

            
            /* Draws text information overlay */
            Globals.Batch.DrawString(Globals.Font, "Map Stats: ", new Vector2(5, 2), Color.LightCoral);
            Globals.Batch.DrawString(Globals.Font, "Name: " + Globals.TileEngine.Map.Name, new Vector2(5, 13), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "Tileset: " + Globals.TileEngine.Map.Tileset, new Vector2(5, 26), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "Map Width: " + Globals.TileEngine.Map.Width, new Vector2(5, 39), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "Map Height: " + Globals.TileEngine.Map.Height, new Vector2(5, 52), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "Tile Size: (" + Globals.TileEngine.Map.TileWidth + "x" + Globals.TileEngine.Map.TileHeight + ")", new Vector2(5, 65), Color.LightGreen);            
            Globals.Batch.DrawString(Globals.Font, "Camera: " + Globals.Camera.Position.ToString(), new Vector2(5, 78), Color.LightGreen);

            Globals.Batch.DrawString(Globals.Font, "Mouse Stats: ", new Vector2(5, 91 + 10), Color.LightCoral);
            Globals.Batch.DrawString(Globals.Font, "Loc: {X:" + xLoc.ToString() + " Y:" + yLoc.ToString() + "}", new Vector2(5, 104 + 10), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "Tile: {X:" + mouseTileX.ToString() + " Y:" + mouseTileY.ToString() + "}", new Vector2(5, 117 + 10), Color.LightGreen);

            Globals.Batch.DrawString(Globals.Font, "Move Camera Keys:", new Vector2(5, 130+20), Color.LightCoral);
            Globals.Batch.DrawString(Globals.Font, "- Arrow Keys", new Vector2(5, 143+20), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "- W. A. S. D.", new Vector2(5, 156+20), Color.LightGreen);

            Globals.Batch.DrawString(Globals.Font, "Change Texture:", new Vector2(5, 169+30), Color.LightCoral);
            Globals.Batch.DrawString(Globals.Font, "PageUp - Previous", new Vector2(5, 182+30), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "PageDown - Next", new Vector2(5, 195+30), Color.LightGreen);

            Globals.Batch.DrawString(Globals.Font, "Change Layer:", new Vector2(5, 208+40), Color.LightCoral);
            Globals.Batch.DrawString(Globals.Font, "F1 - Base", new Vector2(5, 221+40), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "F2 - Fringe", new Vector2(5, 234+40), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "F3 - Object", new Vector2(5, 247+40), Color.LightGreen);
            Globals.Batch.DrawString(Globals.Font, "F4 - Collision", new Vector2(5, 260+40), Color.LightGreen);

            Globals.Batch.DrawString(Globals.Font, "Current Layer:", new Vector2(5, 273+50), Color.LightCoral);
            Globals.Batch.DrawString(Globals.Font, editLayer.ToString(), new Vector2(5, 286+50), Color.LightGreen);
            #endregion

        }


        /// <summary>
        /// Draws the scrollable preview texture
        /// </summary>
        private void DrawPreviewTextures()
        {

            /* *  *  *  *  *  *  *  *  *  *  *  * 
             * Previous Texture
             * * * * * * * * * * * * * * * * * */
            if (newCellIndex != 0)
                Globals.Batch.Draw(previewTextures[newCellIndex - 1],
                    new Rectangle(
                        Globals.Graphics.GraphicsDevice.Viewport.Width - 50,
                        (Globals.Graphics.GraphicsDevice.Viewport.Height - 150) + (0 * previewTextureSize),
                        previewTextureSize, previewTextureSize),
                    Color.White);
            else
                if (newCellIndex == 0)
                    Globals.Batch.Draw(previewTextures[newCellIndex],
                        new Rectangle(
                            Globals.Graphics.GraphicsDevice.Viewport.Width - 50,
                            (Globals.Graphics.GraphicsDevice.Viewport.Height - 150) + (0 * previewTextureSize),
                            previewTextureSize, previewTextureSize),
                        Color.Black);

            Globals.Batch.Draw(textureBorder,
                    new Rectangle(
                        Globals.Graphics.GraphicsDevice.Viewport.Width - 50,
                        (Globals.Graphics.GraphicsDevice.Viewport.Height - 150) + (0 * previewTextureSize),
                        previewTextureSize, previewTextureSize),
                    Color.White);

            Globals.Batch.DrawString(Globals.Font,
                "Previous -> ",
                new Vector2(
                        Globals.Graphics.GraphicsDevice.Viewport.Width - 143,
                        (Globals.Graphics.GraphicsDevice.Viewport.Height - 135) + (0 * previewTextureSize)),
                Color.LightGreen);

            /* *  *  *  *  *  *  *  *  *  *  *  * 
             * Currently Selected Texture
             * * * * * * * * * * * * * * * * * */
            Globals.Batch.Draw(previewTextures[newCellIndex],
                    new Rectangle(
                        Globals.Graphics.GraphicsDevice.Viewport.Width - 50,
                        (Globals.Graphics.GraphicsDevice.Viewport.Height - 150) + (1 * previewTextureSize),
                        previewTextureSize, previewTextureSize),
                    Color.White);

            Globals.Batch.Draw(textureBorder,
                    new Rectangle(
                        Globals.Graphics.GraphicsDevice.Viewport.Width - 50,
                        (Globals.Graphics.GraphicsDevice.Viewport.Height - 150) + (1 * previewTextureSize),
                        previewTextureSize, previewTextureSize),
                    Color.Red);

            Globals.Batch.DrawString(Globals.Font,
                    "Current -> ",
                    new Vector2(
                            Globals.Graphics.GraphicsDevice.Viewport.Width - 135,
                            (Globals.Graphics.GraphicsDevice.Viewport.Height - 135) + (1 * previewTextureSize)),
                    Color.LightGreen);


            /* *  *  *  *  *  *  *  *  *  *  *  * 
             * Next Texture
             * * * * * * * * * * * * * * * * * */
            if (newCellIndex != previewTextures.Length - 1)
                Globals.Batch.Draw(previewTextures[newCellIndex + 1],
                    new Rectangle(
                        Globals.Graphics.GraphicsDevice.Viewport.Width - 50,
                        (Globals.Graphics.GraphicsDevice.Viewport.Height - 150) + (2 * previewTextureSize),
                        previewTextureSize, previewTextureSize),
                        Color.White);
            else
                Globals.Batch.Draw(previewTextures[newCellIndex],
                    new Rectangle(
                        Globals.Graphics.GraphicsDevice.Viewport.Width - 50,
                        (Globals.Graphics.GraphicsDevice.Viewport.Height - 150) + (2 * previewTextureSize),
                        previewTextureSize, previewTextureSize),
                        Color.Black);

            Globals.Batch.Draw(textureBorder,
                    new Rectangle(
                        Globals.Graphics.GraphicsDevice.Viewport.Width - 50,
                        (Globals.Graphics.GraphicsDevice.Viewport.Height - 150) + (2 * previewTextureSize),
                        previewTextureSize, previewTextureSize),
                     Color.White);

            Globals.Batch.DrawString(Globals.Font,
                "Next -> ",
                new Vector2(
                        Globals.Graphics.GraphicsDevice.Viewport.Width - 110,
                        (Globals.Graphics.GraphicsDevice.Viewport.Height - 135) + (2 * previewTextureSize)),
                Color.LightGreen);

        }


        #endregion
    }
}
