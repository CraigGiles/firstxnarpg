using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;
using Microsoft.Xna.Framework.Input;

namespace ActionRPG
{
    public class TileEngine
    {

        #region TileEngine Data
        public static Vector2 Velocity = Vector2.Zero;

        /// <summary>
        /// Returns if a new map is to be loaded
        /// via a player stepping onto a portal or 
        /// other means
        /// </summary>
        public bool LoadNewMap
        {
            get { return loadNewMap; }
            set { loadNewMap = value; }
        }
        bool loadNewMap = false;
               

        /// <summary>
        /// Map file read into the tile engine
        /// </summary>
        public Map Map
        {
            get { return map; }
            set { map = value; }
        }
        Map map;


        /// <summary>
        /// Returns the current tile underneath the mouse cursor
        /// </summary>
        public Point MouseLocation
        {
            get 
            {
                Vector2 location = Globals.Input.MouseLocation;

                return new Point(
                    (int)location.X / map.TileWidth,
                    (int)location.Y / map.TileHeight);

            }
        }

        #endregion


        #region Constructor(s)

        public TileEngine()
        {
            Initialize();
        }

        private void Initialize()
        {
            map = new Map();
            map.Load("Map001");
        }

        #endregion


        #region Update / Draw


        public void Update()
        {
            map.TrashDeletedWorldItems();
        }

        public void Draw()
        {
            map.Draw();
        }


        #endregion


        #region Clamp Camera


        /// <summary>
        /// Clamps the camera from going off the edge of the map, and keeps
        /// the main character centered on the screen
        /// </summary>
        /// <param name="playerPosition">Players position vector</param>
        public void ClampCameraToBoundries(Vector2 position)
        {
            //clamps the camera to the players position
            Globals.Camera.Position.X = position.X - (Globals.Graphics.GraphicsDevice.Viewport.Width / 2);
            Globals.Camera.Position.Y = position.Y - (Globals.Graphics.GraphicsDevice.Viewport.Height / 2);

            //Clamps the camera within the game screen
            Globals.Camera.Position.X = MathHelper.Clamp(Globals.Camera.Position.X, 0, (map.Width * map.TileWidth) - Globals.Graphics.GraphicsDevice.Viewport.Width);
            Globals.Camera.Position.Y = MathHelper.Clamp(Globals.Camera.Position.Y, 0, (map.Height * map.TileHeight) - Globals.Graphics.GraphicsDevice.Viewport.Height);
        }

        #endregion


        #region Player Movement


        /// <summary>
        /// Calculates the characters movement and returns new position vector
        /// </summary>
        /// <param name="down">Is character moving down</param>
        /// <param name="left">Is character moving left</param>
        /// <param name="up">Is character moving up</param>
        /// <param name="right">Is character moving right</param>
        /// <param name="currentPosition">Characters current position vector</param>
        /// <param name="speedMod">Characters speed modifier</param>
        /// <returns>Rectangle: Characters new spritebox</returns>
        public Rectangle CalculateCharacterMovement(bool down, bool left, bool up, bool right, 
            Rectangle spriteBox, float speedMod)
        {
            Vector2 velocity = Vector2.Zero;

            Vector2 currentPosition = new Vector2(spriteBox.X, spriteBox.Y);
            Vector2 newPosition = new Vector2(spriteBox.X, spriteBox.Y);



            if (right)
                velocity += new Vector2(3 * speedMod, 0 * speedMod);
            if (left)
                velocity -= new Vector2(3 * speedMod, 0 * speedMod);

            if (up)
                velocity -= new Vector2(0 * speedMod, 3 * speedMod);
            if (down)
                velocity += new Vector2(0 * speedMod, 3 * speedMod);

            velocity = CheckTileCollision(spriteBox, velocity);
            newPosition += velocity;
            Velocity = velocity;

            //adjusts the sprite box position based on the new position
            spriteBox.X = (int)newPosition.X;
            spriteBox.Y = (int)newPosition.Y;
            
            return spriteBox;
        }


        /// <summary>
        /// Checks collisions between character and map tiles
        /// </summary>
        /// <param name="spriteBox">Sprite Bounding Box of the character</param>
        /// <param name="velocity">Characters current Velocity</param>
        /// <returns>Vector2 position</returns>
        private Vector2 CheckTileCollision(Rectangle spriteBox, Vector2 velocity)
        {            

            //Determine where the foot location of the sprite is
            Vector2 footLocation = new Vector2(
                spriteBox.X + (spriteBox.Width / 2),
                spriteBox.Y + (spriteBox.Height - 5));

            //Convert the characters new position to a map tile
            Point currentTile = ConvertPositionToTile(footLocation += velocity);
                        
            //if new tile is walkable, let character move. If not, change velocity to 0
            if (!IsTileWalkable(currentTile.X, currentTile.Y))
                velocity = Vector2.Zero;

            return velocity;
        }


        /// <summary>
        /// Checks to see if player has entered a portal to another map,
        /// and if so, return the players new position vector on new map
        /// </summary>
        /// <param name="position">Players current position vector</param>
        /// <returns>Vector2</returns>
        public bool CheckForPortalEntry(Vector2 footLocation, out Vector2 newPosition)
        {

            //converts players new position to a map tile
            Point playerTile = ConvertPositionToTile(footLocation);

            //checks to see if player is standing on a portal
            Portal portal = null;

            foreach (Portal p in map.Portals)
            {
                if (p.PortalEnteranceTile == playerTile)
                {
                    portal = p;
                    LoadNewMap = true;
                    break;
                }
            }

            if (LoadNewMap)
            {
                //footLocation = map.PlayerSteppedInPortal(portal);
                LoadNewMap = false;
                newPosition = map.PlayerSteppedInPortal(portal);
                return true; 
            }

            newPosition = Vector2.Zero;
            return false;
        }


        /// <summary>
        /// Checks if a specific tile is able to be walked on
        /// </summary>
        /// <param name="tileX">X location of tile</param>
        /// <param name="tileY">Y Location of tile</param>
        /// <returns>bool</returns>
        public bool IsTileWalkable(int tileX, int tileY)
        {
            if (map.CollisionLayer[tileY, tileX] == 0)
                return true;
            else
                return false;
        }
        

        /// <summary>
        /// Converts Vector2 position into a Point tile
        /// </summary>
        /// <param name="position">Vector2 position to convert</param>
        /// <returns>Point</returns>
        public Point ConvertPositionToTile(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)map.TileWidth),
                (int)(position.Y / (float)map.TileHeight));
        }

        #endregion


        #region Save / Load / Create new Map


        /// <summary>
        /// Loads a new map
        /// </summary>
        /// <param name="asset">Asset name of map to load</param>
        public void LoadMap(string asset)
        {
            map.Load(asset);
            map.Populate();
        }


        /// <summary>
        /// Saves current map to file
        /// </summary>
        public void SaveMap()
        {
            map.SaveMap();
        }


        /// <summary>
        /// Creates a new map
        /// </summary>
        /// <param name="tileSet">Tileset for map to use</param>
        public void CreateNewMap(Tileset tileSet)
        {
            //Unload the current map 
            map.UnloadMap();

            //sets the map name and tileset
            map.Name = "NewMap001";
            map.Tileset = tileSet.ToString();

            //resets the map tile size
            map.TileWidth = 32;
            map.TileHeight = 32;

            //sets the player respawn to the middle of the map
            map.PlayerRespawnTile = new Point(10, 10);

            //Load tiles from tileset into memory
            map.LoadTileset();

        }


        #endregion

    }//end TileEngine
}//end ShackRPG
