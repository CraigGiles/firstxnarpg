using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;

namespace ActionRPG
{
    public class TileEngine
    {
        
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
        }

        public void Draw()
        {
            map.Draw();
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
        public void CreateNewMap(Enum.Tileset tileSet)
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



        public bool IsTileWalkable(int tileX, int tileY)
        {
            if (map.CollisionLayer[tileY, tileX] == 0)
                return true;
            else
                return false;
        }


        public Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)map.TileWidth),
                (int)(position.Y / (float)map.TileHeight));
        }

    }//end TileEngine
}//end ShackRPG
