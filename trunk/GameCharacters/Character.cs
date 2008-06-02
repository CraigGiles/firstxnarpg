using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ActionRPG
{
    public class Character : Stats
    {

        /// <summary>
        /// Asset name of character
        /// </summary>
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        string assetName;
        

        /// <summary>
        /// Spawn location Vector2
        /// </summary>
        public Vector2 SpawnLocationVector
        {
            get { return spawnLocationVector; }
            set { spawnLocationVector = value; }
        }
        Vector2 spawnLocationVector;


        /// <summary>
        /// Spawn tile located on map
        /// </summary>
        public Point SpawnLocationTile
        {
            get { return spawnLocationTile; }
            set { spawnLocationTile = value; }
        }
        Point spawnLocationTile;


        /// <summary>
        /// Sprite sheet of the character
        /// </summary>
        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }
        Texture2D spriteSheet;

        //----------------------------------------------------------------------------
        // eventually move this to a more suitable location 
        //----------------------------------------------------------------------------

        /// <summary>
        /// Amount of gold character currently has
        /// </summary>
        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }
        int gold;


    }//end class
}//end namespace
