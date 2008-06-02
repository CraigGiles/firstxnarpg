using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class GameObject
    {

        /// <summary>
        /// Gets or sets the Asset name used to load
        /// item into memory
        /// </summary>
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        string assetName;


        /// <summary>
        /// Sprite sheet of the game object
        /// </summary>
        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }
        Texture2D spriteSheet;


        /// <summary>
        /// Gets the spawn location of the item in Vector form
        /// </summary>
        public Vector2 SpawnLocationVector
        {
            get { return spawnLocationVector; }
            set { spawnLocationVector = value; }
        }
        Vector2 spawnLocationVector;


        /// <summary>
        /// Gets the spawn location tile of the item
        /// </summary>
        public Point SpawnLocationTile
        {
            get { return spawnLocationTile; }
            set { spawnLocationTile = value; }
        }
        Point spawnLocationTile;

    }
}
