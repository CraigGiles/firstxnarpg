using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TileEngine;


namespace ShackRPG.GameScreens
{
    class MapEditor : IGameScreen
    {
        /// <summary>
        /// Holds the current tile location of the mouse
        /// </summary>
        int MouseXtile, MouseYtile;

        public enum DrawLayer
        {
            BaseLayer,
            ObjectLayer,
            CollisionLayer,
            Enemies,
            Bosses,
            Npcs,
        }

        public DrawLayer curDrawLayer = DrawLayer.BaseLayer;

        ContentManager content;
        SpriteBatch spriteBatch;
        BaseGame game;
        SpriteFont font;
        TileEngineV2 tileEngine;

        int TextureIndex = 0;
        int ObjectIndex = 0;

        Texture2D 
            tEmptyTileTexture, 
            tCollisionTile, 
            tTexturePreview,
            tSaveButton,
            tLoadButton,
            tBaseLayer,
            tObjectLayerButton,
            tCollisionLayerButton,
            tNpcLayerButton,
            tEnemyLayerButton,
            tBossesLayerButton;

        Camera camera;

        MouseState curMouseState, prevMouseState;

        /// <summary>
        /// Different UI button locations
        /// </summary>
        Rectangle
            rBaseLayer = new Rectangle(560, 40, 110, 35),
            rObjectLayer = new Rectangle(560, 80, 110, 35),
            rCollisionLayer = new Rectangle(560, 120, 110, 35),

            rBossesLayer = new Rectangle(680, 40, 110, 35),
            rEnemyLayer = new Rectangle(680, 80, 110, 35),
            rNpcLayer = new Rectangle(680, 120, 110, 35),

            rSaveMap = new Rectangle(5, 40, 110, 35),
            rLoadMap = new Rectangle(5, 80, 110, 35);

        Rectangle
            rTexturePreview1 = new Rectangle(550, 210 - TileEngineV2.TileHeight, TileEngineV2.TileWidth, TileEngineV2.TileHeight),
            rUnusedMapPreview = new Rectangle(750, 275 - TileEngineV2.TileHeight, TileEngineV2.TileWidth, TileEngineV2.TileHeight);

        Rectangle CharacterSourceRectangle = new Rectangle(0, 0, 40, 40);

        public MapEditor(ContentManager sContent, 
                            SpriteBatch sSpriteBatch, 
                            BaseGame sGame, 
                            SpriteFont sFont, 
                            TileEngineV2 sTileEngine, 
                            Camera sCamera)
        {
            content = sContent;
            spriteBatch = sSpriteBatch;
            game = sGame;
            font = sFont;
            tileEngine = sTileEngine;
            camera = sCamera;


            Initialize();
        }
        public MapEditor()
        {
        }

        public void Initialize()
        {
            Mouse.SetPosition(game.GraphicsDevice.Viewport.Width / 2, 
                game.GraphicsDevice.Viewport.Height / 2);

            game.IsMouseVisible = true;

            //Loads in the editor textures
            tEmptyTileTexture = content.Load<Texture2D>(@"MapEditor/EmptyTile");
            tCollisionTile = content.Load<Texture2D>(@"MapEditor/unwalkable");
            tTexturePreview = content.Load<Texture2D>(@"MapEditor/TexturePreview");
            tSaveButton = content.Load<Texture2D>(@"MapEditor/SaveMapButton");
            tLoadButton = content.Load<Texture2D>(@"MapEditor/LoadMapButton");
            tObjectLayerButton = content.Load<Texture2D>(@"MapEditor/ObjLayerButton");
            tBaseLayer = content.Load<Texture2D>(@"MapEditor/BaseLayerButton");
            tCollisionLayerButton = content.Load<Texture2D>(@"MapEditor/CollisionLayerButton");
            tNpcLayerButton = content.Load<Texture2D>(@"MapEditor/NpcsLayerButton");
            tBossesLayerButton = content.Load<Texture2D>(@"MapEditor/BossesLayerButton");
            tEnemyLayerButton = content.Load<Texture2D>(@"MapEditor/EnemyLayerButton");
      
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            curMouseState = Mouse.GetState();
            
            #region Reset Camera
            if (Input.Space)
                camera.Position = Vector2.Zero;
            #endregion

            //exits out of editor
            if (Input.Escape)
                ExitMapEditor();

            CheckMouseClicks();
            CheckMapMovementKeys();

            prevMouseState = curMouseState;

        }

        /// <summary>
        /// Checks to see if the arrow keys have been pressed, and move
        /// the camera accordingly
        /// </summary>
        private void CheckMapMovementKeys()
        {
            if (Input.Up)
                camera.Position.Y += TileEngineV2.TileHeight;
            if (Input.Down)
                 camera.Position.Y -= TileEngineV2.TileHeight;
            if (Input.Left)
                 camera.Position.X += TileEngineV2.TileWidth;
            if (Input.Right)
                camera.Position.X -= TileEngineV2.TileWidth;
        }

        private void CheckMouseClicks()
        {
            int textureNumber = 0;
            int objectNumber = 0;
            int enemyNumber = 0;
            int npcNumber = 0;
            int bossNumber = 0;
            bool drawTexture = true;

            Point editorMouse = new Point(curMouseState.X, curMouseState.Y);

            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            MouseXtile = mouseX / TileEngineV2.TileWidth;
            MouseYtile = mouseY / TileEngineV2.TileHeight;

            MouseXtile = (int)MathHelper.Clamp(MouseXtile, 0, tileEngine.GetWidthInPixels);
            MouseYtile = (int)MathHelper.Clamp(MouseYtile, 0, tileEngine.GetHeightInPixels);

            //ajusts the point by where the camera is. If the camera is at 0,0 it worked
            // fine, but if you scrolled even 1 tile over, it would go out of bounds.
            // The following code prevents this.
            int camXoffset = (int)camera.Position.X / TileEngineV2.TileWidth;
            int camYoffset = (int)camera.Position.Y / TileEngineV2.TileHeight;
            Point mouse = new Point(MouseXtile - camXoffset, MouseYtile - camYoffset);

            #region Left Mouse Button Pressed
            if (curMouseState.LeftButton == ButtonState.Pressed)
            {                
                CheckForButtonPress(ref textureNumber, ref objectNumber, ref npcNumber, ref enemyNumber, ref bossNumber, ref drawTexture, ref editorMouse);

                //Sets Base Layer
                if (drawTexture && curDrawLayer == DrawLayer.BaseLayer)
                    tileEngine.SetBaseLayerCellIndex(mouse, TextureIndex);

                //Sets Object Layer - can not click + drag
                else if (drawTexture && curDrawLayer == DrawLayer.ObjectLayer && prevMouseState.LeftButton == ButtonState.Released)
                    tileEngine.SetObjectLayerCellIndex(mouse, TextureIndex);

                //Sets Collision - can click and drag
                else if (drawTexture && curDrawLayer == DrawLayer.CollisionLayer)
                    tileEngine.AddCollisionCell(mouse);

                //Sets NPCs -- Can not click and drag

                //Sets enemies -- Can not click and drag
                else if (drawTexture && curDrawLayer == DrawLayer.Enemies && prevMouseState.LeftButton == ButtonState.Released)
                    tileEngine.AddEnemyToMap(mouse, TextureIndex);
                //Sets Bosses -- Can not click and drag
                             
            }
            #endregion

            #region Right Mouse Button Pressed
            else if (curMouseState.RightButton == ButtonState.Pressed)
            {
                if (curDrawLayer == DrawLayer.ObjectLayer)
                    tileEngine.DeleteObjectLayerCellIndex(mouse);

                if (curDrawLayer == DrawLayer.CollisionLayer)
                    tileEngine.RemoveCollisionCell(mouse);

                if (curDrawLayer == DrawLayer.Npcs)
                {}  //tileEngine.RemoveNpc(mouse);

                if (curDrawLayer == DrawLayer.Bosses)
                {}   //tileEngine.RemoveBoss(mouse);

                if (curDrawLayer == DrawLayer.Enemies)
                    tileEngine.RemoveEnemyFromMap(mouse);
            }
            #endregion
        }

        private void CheckForButtonPress(ref int textureNumber, ref int objectNumber, ref int npcNumber, ref int enemyNumber, ref int bossNumber, ref bool drawTexture, ref Point editorMouse)
        {
            #region Texture Buttons
            foreach (Texture2D t in TileEngineV2.curMapTiles)
            {
                Rectangle TextureRect = new Rectangle(0,0,0,0);

                if (textureNumber < 5)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + (textureNumber * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y,
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 10)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 5) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 1),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 15)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 10) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 2),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 20)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 15) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 3),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 25)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 20) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 4),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 30)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 25) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 5),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 35)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 30) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 6),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 40)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 35) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 7),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 45)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 40) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 8),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }

                if (TextureRect.Contains(editorMouse))
                {
                    TextureIndex = textureNumber;
                    drawTexture = false;
                    ObjectIndex = 0;
                    continue;
                }
                               
                textureNumber++;
            }
            #endregion

            #region Object Buttons
            foreach (Texture2D t in TileEngineV2.curObjTiles)
            {
                Rectangle TextureRect = new Rectangle(0,0,0,0);

                if (objectNumber < 5)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + (objectNumber * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y,
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (objectNumber < 10)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((objectNumber - 5) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 1),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (objectNumber < 15)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((objectNumber - 10) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 2),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (objectNumber < 20)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((objectNumber - 15) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 3),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (objectNumber < 25)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((objectNumber - 20) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 4),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (objectNumber < 30)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((objectNumber - 25) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 5),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (objectNumber < 35)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((objectNumber - 30) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 6),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (objectNumber < 40)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((objectNumber - 35) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 7),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (objectNumber < 45)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((objectNumber - 40) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 8),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }

                if (TextureRect.Contains(editorMouse))
                {
                    TextureIndex = objectNumber;
                    drawTexture = false;
                    continue;
                }

                objectNumber++;
            }
            #endregion

            #region Enemy Buttons
            foreach (Texture2D t in TileEngineV2.curEnemyTiles)
            {
                Rectangle TextureRect = new Rectangle(0, 0, 0, 0);

                if (enemyNumber < 5)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + (enemyNumber * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y,
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (enemyNumber < 10)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((enemyNumber - 5) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 1),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (enemyNumber < 15)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((enemyNumber - 10) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 2),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (enemyNumber < 20)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((enemyNumber - 15) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 3),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (enemyNumber < 25)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((enemyNumber - 20) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 4),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (enemyNumber < 30)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((enemyNumber - 25) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 5),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (enemyNumber < 35)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((enemyNumber - 30) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 6),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (enemyNumber < 40)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((enemyNumber - 35) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 7),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (enemyNumber < 45)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((enemyNumber - 40) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 8),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }

                if (TextureRect.Contains(editorMouse))
                {
                    TextureIndex = enemyNumber;
                    drawTexture = false;
                    continue;
                }

                enemyNumber++;
            }
            #endregion

            #region Layer Buttons
            if (rBaseLayer.Contains(editorMouse) && Input.Shift)
            {
                drawTexture = false;
                TextureIndex = 0;
                curDrawLayer = DrawLayer.BaseLayer;
            }
            if (rObjectLayer.Contains(editorMouse) && Input.Shift)
            {
                drawTexture = false;
                TextureIndex = 0;
                curDrawLayer = DrawLayer.ObjectLayer;

            }
            if (rCollisionLayer.Contains(editorMouse) && Input.Shift)
            {
                drawTexture = false;
                TextureIndex = 0;
                curDrawLayer = DrawLayer.CollisionLayer;
            }
            if (rNpcLayer.Contains(editorMouse) && Input.Shift)
            {
                drawTexture = false;
                TextureIndex = 0;
                curDrawLayer = DrawLayer.Npcs;
            }

            if (rEnemyLayer.Contains(editorMouse) && Input.Shift)
            {
                drawTexture = false;
                TextureIndex = 0;
                curDrawLayer = DrawLayer.Enemies;
            }

            if (rBossesLayer.Contains(editorMouse) && Input.Shift)
            {
                drawTexture = false;
                TextureIndex = 0;
                curDrawLayer = DrawLayer.Bosses;
            }

            #endregion
            
            #region Save / Load Button
            if (rSaveMap.Contains(editorMouse) && Input.Shift && prevMouseState.LeftButton == ButtonState.Released)
            {
                drawTexture = false;
                tileEngine.SaveFile();
                game.AddNewGameScreen(new Saved(content, spriteBatch, game, font));
            }
            if (rLoadMap.Contains(editorMouse) && Input.Shift && prevMouseState.LeftButton == ButtonState.Released)
            {
                drawTexture = false;
                tileEngine.UnloadCurrentMap();
                tileEngine.LoadMap(content, GameArea.curGameArea + ".map");
                game.AddNewGameScreen(new Loaded(content, spriteBatch, game, font));
                
            }
            #endregion
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.CornflowerBlue);

            

            int width = tileEngine.GetWidthInPixels / TileEngineV2.TileWidth;
            int height = tileEngine.GetHeightInPixels / TileEngineV2.TileHeight;

            tileEngine.DrawInMapEditor(spriteBatch, camera);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, 
                            SpriteSortMode.Texture, 
                            SaveStateMode.None, 
                            camera.MapEditorMatrix);

            spriteBatch.Draw(tEmptyTileTexture, new Rectangle(
                            MouseXtile * TileEngineV2.TileWidth - (int)camera.Position.X,
                            MouseYtile * TileEngineV2.TileHeight - (int)camera.Position.Y,
                            TileEngineV2.TileWidth,
                            TileEngineV2.TileHeight),
                            Color.Red);

            

            #region Draws red tile if tile is unwalkable
            for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                            if (TileEngineV2.CollisionLayer[y , x ] != 0)
                                    spriteBatch.Draw(tCollisionTile,
                                            new Rectangle(x * TileEngineV2.TileWidth,
                                                          y * TileEngineV2.TileHeight,
                                                          TileEngineV2.TileWidth,
                                                          TileEngineV2.TileHeight),
                                            Color.White);

            #endregion


            spriteBatch.End();

            DrawEditorInterface();
        }

        void DrawEditorInterface()
        {
            #region button Colors
            Color baseLayerColor = Color.White;
            Color objectLayerColor = Color.White;
            Color collisionLayerColor = Color.White;
            Color npcLayerColor = Color.White;
            Color bossesLayerColor = Color.White;
            Color enemyLayerColor = Color.White;

            switch (curDrawLayer)
            {
                case DrawLayer.BaseLayer:
                     baseLayerColor = Color.Cyan;
                     objectLayerColor = Color.White;
                     collisionLayerColor = Color.White;
                     npcLayerColor = Color.White;
                     bossesLayerColor = Color.White;
                     enemyLayerColor = Color.White;

                    break;

                case DrawLayer.ObjectLayer:
                     baseLayerColor = Color.White;
                     objectLayerColor = Color.Cyan;
                     collisionLayerColor = Color.White;
                     npcLayerColor = Color.White;
                     bossesLayerColor = Color.White;
                     enemyLayerColor = Color.White;
                    break;

                case DrawLayer.CollisionLayer:
                     baseLayerColor = Color.White;
                     objectLayerColor = Color.White;
                     collisionLayerColor = Color.Cyan;
                     npcLayerColor = Color.White;
                     bossesLayerColor = Color.White;
                     enemyLayerColor = Color.White;
                    break;

                case DrawLayer.Npcs:
                     baseLayerColor = Color.White;
                     objectLayerColor = Color.White;
                     collisionLayerColor = Color.White;
                     npcLayerColor = Color.Cyan;
                     bossesLayerColor = Color.White;
                     enemyLayerColor = Color.White;
                    break;

                case DrawLayer.Enemies:
                     baseLayerColor = Color.White;
                     objectLayerColor = Color.White;
                     collisionLayerColor = Color.White;
                     npcLayerColor = Color.White;
                     bossesLayerColor = Color.White;
                     enemyLayerColor = Color.Cyan;
                    break;

                case DrawLayer.Bosses:
                     baseLayerColor = Color.White;
                     objectLayerColor = Color.White;
                     collisionLayerColor = Color.White;
                     npcLayerColor = Color.White;
                     bossesLayerColor = Color.Cyan;
                     enemyLayerColor = Color.White;
                    break;
            }


            

            #endregion
            
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            DrawBaseLayerPreviewTextures();
   
            #region Draw Buttons
            spriteBatch.Draw(tBaseLayer, rBaseLayer, baseLayerColor);
            spriteBatch.Draw(tObjectLayerButton, rObjectLayer, objectLayerColor);
            spriteBatch.Draw(tCollisionLayerButton, rCollisionLayer, collisionLayerColor);

            spriteBatch.Draw(tNpcLayerButton, rNpcLayer, npcLayerColor);
            spriteBatch.Draw(tBossesLayerButton, rBossesLayer, bossesLayerColor);
            spriteBatch.Draw(tEnemyLayerButton, rEnemyLayer, enemyLayerColor);

            spriteBatch.Draw(tSaveButton, rSaveMap, Color.White);
            spriteBatch.Draw(tLoadButton, rLoadMap, Color.White);
            #endregion

            #region Draw Instructions Text
            switch (curDrawLayer)
            {
                case DrawLayer.BaseLayer:
                    spriteBatch.DrawString(font, "L.Mouse: Draw Texture", new Vector2(0, 490), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
                    break;

                case DrawLayer.ObjectLayer:
                    spriteBatch.DrawString(font, "L.Mouse: Draws Object", new Vector2(0, 490), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font, "R.Mouse: Delete Object", new Vector2(0, 510), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
                    break;

                case DrawLayer.CollisionLayer:
                    spriteBatch.DrawString(font, "L.Mouse: Removes Walkability from cell", new Vector2(0, 490), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font, "R.Mouse: Adds Walkability to cell", new Vector2(0, 510), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
                    break;
            }

            spriteBatch.DrawString(font, "W,A,S,D:        Move camera", new Vector2(0, 540), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Space:         Reset Camera to 0,0", new Vector2(0, 560), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Esc:            exit editor", new Vector2(0, 580), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Shift + Click", new Vector2(8, 5), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "to save / load", new Vector2(8, 20), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);



            spriteBatch.DrawString(font, "Current Tileset: " + TileEngineV2.curTileSet.ToString(), new Vector2(300, 2), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Shift + Click", new Vector2(665, 5), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "to change layer", new Vector2(650, 20), Color.Red, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);




            #endregion

            spriteBatch.End();
        }

        public void DrawBaseLayerPreviewTextures()
        {
            int textureNumber = 0;
            List<Texture2D> tList = new List<Texture2D>();

            if (curDrawLayer == DrawLayer.BaseLayer)
                tList = TileEngineV2.curMapTiles;
            if (curDrawLayer == DrawLayer.ObjectLayer)
                tList = TileEngineV2.curObjTiles;
            if (curDrawLayer == DrawLayer.CollisionLayer)
                tList = TileEngineV2.curMapTiles;
            if (curDrawLayer == DrawLayer.Npcs)
                tList = TileEngineV2.curNpcTiles;
            if (curDrawLayer == DrawLayer.Enemies)
                tList = TileEngineV2.curEnemyTiles;
            if (curDrawLayer == DrawLayer.Bosses)
                tList = TileEngineV2.curBossTiles;

            //Draws the map textures
            foreach (Texture2D t in tList)
            {
                Rectangle TextureRect = new Rectangle(0,0,0,0);

                if (textureNumber < 5)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + (textureNumber * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y,
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 10)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 5) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 1),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 15)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 10) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 2),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 20)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 15) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 3),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 25)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 20) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 4),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 30)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 25) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 5),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 35)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 30) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 6),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 40)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 35) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 7),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }
                else if (textureNumber < 45)
                {
                    TextureRect = new Rectangle(rTexturePreview1.X + ((textureNumber - 40) * TileEngineV2.TileWidth),
                                        rTexturePreview1.Y + (TileEngineV2.TileHeight * 8),
                                        rTexturePreview1.Width,
                                        rTexturePreview1.Height);
                }

                Color drawColor = Color.White;

                if (curDrawLayer == DrawLayer.CollisionLayer)
                    drawColor = Color.DimGray;

                
                if (curDrawLayer == DrawLayer.Npcs || curDrawLayer == DrawLayer.Enemies || curDrawLayer == DrawLayer.Bosses)
                    spriteBatch.Draw(t, TextureRect, CharacterSourceRectangle, drawColor);
                else
                    spriteBatch.Draw(t, TextureRect, drawColor);  //Draws the texture under its "frame"

                spriteBatch.Draw(tTexturePreview, TextureRect, drawColor);    //draws the 'frame'

                textureNumber++;
            }
        }

        private void ExitMapEditor()
        {
            game.RemoveCurrentGameScreen();
        }
    }//end MapEditor
}


