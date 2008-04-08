using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace TileEngine
{
    public class TileEngineV2
    {
        #region Variables
        ContentManager content;

        Texture2D tDefault;

        /// <summary>
        /// Current tileset being used by the tile engine
        /// </summary>
        public static TileSet curTileSet;

        /// <summary>
        /// A list of all the tilesets available within the game
        /// </summary>
        public enum TileSet
        {
            Field,
        }

        /// <summary>
        /// List of textures used for the current tileset
        /// </summary>
        public static List<Texture2D> curMapTiles = new List<Texture2D>();

        /// <summary>
        /// List of names of textures for the current tileset
        /// </summary>
        public static List<string> curMapTileNames = new List<string>();

        /// <summary>
        /// List of all object textures used for the tileset
        /// </summary>
        public static List<Texture2D> curObjTiles = new List<Texture2D>();

        /// <summary>
        /// List of object textures names for the current tileset
        /// </summary>
        public static List<string> curObjTileNames = new List<string>();

        /// <summary>
        /// List of all NPC textures used for the tileset
        /// </summary>
        public static List<Texture2D> curNpcTiles = new List<Texture2D>();

        /// <summary>
        /// List of NPC textures names for the current tileset
        /// </summary>
        public static List<string> curNpcTileNames = new List<string>();
        

        /// <summary>
        /// List of all enemy textures used for the tileset
        /// </summary>
        public static List<Texture2D> curEnemyTiles = new List<Texture2D>();

        /// <summary>
        /// List of enemy textures names for the current tileset
        /// </summary>
        public static List<string> curEnemyTileNames = new List<string>();

        /// <summary>
        /// List of all Bosses textures used for the tileset
        /// </summary>
        public static List<Texture2D> curBossTiles = new List<Texture2D>();

        /// <summary>
        /// List of Bosses textures names for the current tileset
        /// </summary>
        public static List<string> curBossTileNames = new List<string>();

        /// <summary>
        /// The base layer of our map
        /// </summary>
        public static int[,] baseLayer = new int[32,20];
        
        /// <summary>
        /// Collision Detection layer of our map (Walkable or Unwalkable)
        /// </summary>
        public static int[,] CollisionLayer;

        /// <summary>
        /// Object layer for drawing objects such as treasure chests to screen
        /// </summary>
        public static int[,] objectLayer;

        public static int[,] enemyLayer;

        /// <summary>
        /// If a tile is unwalkable, a rectangle will be placed over that tile
        /// preventing the player from walking onto it. That rectangle is held
        /// in this array
        /// </summary>
        public static Rectangle[,] rUnwalkable;

        /// <summary>
        /// Tile Height and Width in Pixels
        /// </summary>
        public const int TileHeight = 48;
        public const int TileWidth = 48;
        #endregion

        #region properties
        /// <summary>
        /// Gets the width of the map in pixels
        /// </summary>
        public int GetWidthInPixels
        {
            get
            {
                int width = TileWidth * baseLayer.GetLength(1);
                return width;
            }
                
        }

        /// <summary>
        /// Gets the number of tiles going across the map
        /// </summary>
        public int GetWidthInTiles
        {
            get
            {
                return baseLayer.GetLength(1);
            }
        }

        /// <summary>
        /// Gets the number of tiles going down the map
        /// </summary>
        public int GetHeightInTiles
        {
            get
            {
                return baseLayer.GetLength(0);
            }
        }

        /// <summary>
        /// Gets the height of the map in pixels
        /// </summary>
        public int GetHeightInPixels
        {
            get
            {
                int height = TileHeight * baseLayer.GetLength(0);
                return height;
            }
        }
        #endregion

        #region Constructor
        public TileEngineV2(ContentManager content)
        {
            this.content = content;                        
            LoadMap(content, "NewGame.map");
            tDefault = content.Load<Texture2D>(@"TileSets/Field/se_free_grass_texture");
        }
        public TileEngineV2()
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Checks to see if a new map needs to be loaded, moves the player around
        /// the current map and checks to see if the player is moving onto a blocked
        /// tile
        /// </summary>
        public void Update()
        {
            if (GameArea.curGameArea != GameArea.prevGameArea)
                LoadMap(content,GameArea.curGameArea.ToString()+".map");   // Checks to see if a new map needs to be loaded

            
        }//end Update()

        /// <summary>
        /// Loads an area map from a file "FileName.layer"
        /// </summary>
        /// <param name="content">Content manager</param>
        /// <param name="fileName">File name to be loaded</param>
        public void LoadMap(ContentManager content, string fileName)
        {
            //used to determine which area of the data file we are currently reading
            bool readingTileset = false;
            bool readingObjectTextures = false;
            bool readingBaseLayer = false;
            bool readingCollisionLayer = false;
            bool readingObjectLayer = false;
            bool readingNpcLayer = false;
            bool readingEnemyLayer = false;
            bool readingBossLayer = false;

            //Temp lists to house the data until they are loaded
            List<string> textureNames = new List<string>();
            List<string> objectTextures = new List<string>();
            List<List<int>> tempBaseLayer = new List<List<int>>();
            List<List<int>> tempCollisionLayer = new List<List<int>>();
            List<List<int>> tempObjectLayer = new List<List<int>>();
            List<List<int>> tempEnemyLayer = new List<List<int>>();

            //Read the data from file
            using (StreamReader reader = new StreamReader(MapFilePath + fileName))
            {
                while (!reader.EndOfStream)                     //while still reading
                {
                    #region Reading the stream and loading into temp lists
                    string line = reader.ReadLine().Trim();     //reads the current line

                    if (string.IsNullOrEmpty(line))             //if the line is null or empty
                        continue;                                   //kick out of the loop

                    #region Reading Layer Tags
                    if (line.Contains("[Tileset]"))             //if line is the textures tag
                    {
                        readingTileset = true;                 //execute textures portion of code
                        readingBaseLayer = false;
                        readingCollisionLayer = false;
                        readingObjectLayer = false;
                        readingObjectTextures = false;
                        readingEnemyLayer = false;
                    }
                    else if (line.Contains("[Objects]")) //if line is the textures tag
                    {
                        readingTileset = false;                //execute textures portion of code
                        readingBaseLayer = false;
                        readingCollisionLayer = false;
                        readingObjectLayer = false;
                        readingObjectTextures = true;
                        readingEnemyLayer = false;
                    }
                    else if (line.Contains("[BaseLayer]"))      //if line is BaseLayer tag
                    {
                        readingBaseLayer = true;                //execute base layer portion of code
                        readingTileset = false;
                        readingCollisionLayer = false;
                        readingObjectLayer = false;
                        readingObjectTextures = false;
                        readingEnemyLayer = false;
                    }
                    else if (line.Contains("[CollisionLayer]"))  //if line is walkable tag
                    {
                        readingCollisionLayer = true;            //execute walkable layer portion of code
                        readingBaseLayer = false;
                        readingTileset = false;
                        readingObjectLayer = false;
                        readingObjectTextures = false;
                        readingEnemyLayer = false;
                    }
                    else if (line.Contains("[ObjectLayer]"))  //if line is ObjectLayer tag
                    {
                        readingCollisionLayer = false;            //execute walkable layer portion of code
                        readingBaseLayer = false;
                        readingTileset = false;
                        readingObjectLayer = true;
                        readingObjectTextures = false;
                        readingEnemyLayer = false;
                    }
                    else if (line.Contains("[EnemyLayer]"))  //if line is ObjectLayer tag
                    {
                        readingCollisionLayer = false;            //execute walkable layer portion of code
                        readingBaseLayer = false;
                        readingTileset = false;
                        readingObjectLayer = false;
                        readingObjectTextures = false;
                        readingEnemyLayer = true;
                    }
                    #endregion

                    #region Reading Layer Data
                    //While reading the textures portion of the file,
                    //input all texture names into the 'textureNames' list
                    else if (readingTileset == true)
                    {
                        if (line == TileSet.Field.ToString())
                            LoadFieldTileset();

                        /* add any new tilesets to here, such as Dungeon, Farm, Town, etc
                         * new load methods will also need to be created. IE: LoadDungeonTileset(); */
                    }

                    //while reading the object textures, input
                    // all texture names into the 'object textures' list
                    else if (readingObjectTextures == true)
                    {
                        objectTextures.Add(line);
                    }

                    //While reading the base layer portion of the file,
                    //read each line and input each row into a temp int[]
                    else if (readingBaseLayer == true)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        tempBaseLayer.Add(row);
                    }

                    //While reading the walkable layer portion of the file,
                    //read each line and input each row into a temp int[]
                    else if (readingCollisionLayer == true)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }
                        tempCollisionLayer.Add(row);
                    }

                    //While reading the object layer portion of the file,
                    //read each line and input each row into a temp int[]
                    else if (readingObjectLayer == true)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        tempObjectLayer.Add(row);
                    }

                    else if (readingEnemyLayer == true)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        tempEnemyLayer.Add(row);
                    }
                    #endregion

                    #endregion
                }
            }//end using steamreader

            //This section will initialize the map files and load them
            //into the tilemap 
            int width = tempBaseLayer[0].Count;
            int height = tempBaseLayer.Count;

            CollisionLayer = new int[height, width];
            objectLayer = new int[height, width];
            rUnwalkable = new Rectangle[height, width];
            baseLayer = new int[height, width];
            enemyLayer = new int[height, width];


            #region Unload old and load in new
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //unloads any old rectangles used for collsion detection
                    rUnwalkable[y, x] = new Rectangle(0, 0, 0, 0);

                    //Loads in the layer data
                    baseLayer[y, x] = tempBaseLayer[y][x];
                    CollisionLayer[y, x] = tempCollisionLayer[y][x];
                    objectLayer[y, x] = tempObjectLayer[y][x];
                    enemyLayer[y, x] = tempEnemyLayer[y][x];
 
                    //if the walkable map's current location is anything but 0, mark the tile as
                    // unwalkable by placing a rectangle over the tile. 
                    if (CollisionLayer[y, x] != 0)
                        rUnwalkable[y, x] = new Rectangle(x * TileWidth + 15, y * TileHeight, TileWidth - 10, TileHeight - 10);

                }
            }//end for loops
            #endregion

            GameArea.prevGameArea = GameArea.curGameArea;
            //GameArea.NewAreaLoaded(fileName.Trim("*.map"));
        }

        /// <summary>
        /// Loads the tileset for TestField into memory
        /// </summary>
        private void LoadFieldTileset()
        {
            #region Load Names List
            curMapTileNames.Add("Tilesets/Field/se_free_dirt_texture");
            curMapTileNames.Add("Tilesets/Field/se_free_grass_texture");
            curMapTileNames.Add("Tilesets/Field/se_free_ground_texture");
            curMapTileNames.Add("Tilesets/Field/se_free_mud_texture");
            curMapTileNames.Add("Tilesets/Field/se_free_road_texture");
            curMapTileNames.Add("Tilesets/Field/se_free_rock_texture");
            curMapTileNames.Add("Tilesets/Field/se_free_wood_texture");

            curMapTileNames.Add("Tilesets/Field/BrickGray01");
            curMapTileNames.Add("Tilesets/Field/BrickGray02");
            curMapTileNames.Add("Tilesets/Field/BrickGray03");
            curMapTileNames.Add("Tilesets/Field/BrickGray04");
            curMapTileNames.Add("Tilesets/Field/BrickGray05");
            curMapTileNames.Add("Tilesets/Field/BrickGray06");
            curMapTileNames.Add("Tilesets/Field/BrickGray08");

            curObjTileNames.Add("Tilesets/Field/Objects/WallCenter1");
            curObjTileNames.Add("Tilesets/Field/Objects/WallCenter2");
            curObjTileNames.Add("Tilesets/Field/Objects/WallCorner02");
            curObjTileNames.Add("Tilesets/Field/Objects/WallCorner03");
            curObjTileNames.Add("Tilesets/Field/Objects/WallLeftCorner");
            curObjTileNames.Add("Tilesets/Field/Objects/WallRightCorner1");
            curObjTileNames.Add("Tilesets/Field/Objects/WallSide01");
            curObjTileNames.Add("Tilesets/Field/Objects/WallSide02");
            curObjTileNames.Add("Tilesets/Field/Objects/WallSide03");
            curObjTileNames.Add("Tilesets/Field/Objects/WallSide04");

            curObjTileNames.Add("Tilesets/Field/Objects/rocks");
            curObjTileNames.Add("Tilesets/Field/Objects/StoneChair");
            curObjTileNames.Add("Tilesets/Field/Objects/StoneTable");
            curObjTileNames.Add("Tilesets/Field/Objects/TreasureChest");
            curObjTileNames.Add("Tilesets/Field/Objects/WoodTableLeft");
            curObjTileNames.Add("Tilesets/Field/Objects/WoodTableRight");
            
            curEnemyTileNames.Add("Characters/Enemies/Flower");
            curEnemyTileNames.Add("Characters/Enemies/Rabite");

            #endregion

            #region Load Textures Lists
            foreach (string s in curMapTileNames)
                curMapTiles.Add(content.Load<Texture2D>(s));

            foreach (string s in curObjTileNames)
                curObjTiles.Add(content.Load<Texture2D>(s));

            foreach (string s in curNpcTileNames)
                curNpcTiles.Add(content.Load<Texture2D>(s));

            foreach (string s in curEnemyTileNames)
                curEnemyTiles.Add(content.Load<Texture2D>(s));

            foreach (string s in curBossTileNames)
                curBossTiles.Add(content.Load<Texture2D>(s));
            #endregion
        }

        /// <summary>
        /// Sets a sell index on the baseLayer map to index specified
        /// </summary>
        /// <param name="point">Point on map to set</param>
        /// <param name="newIndex">new index to be set</param>
        public void SetBaseLayerCellIndex(Point point, int newIndex)
        {
            bool setIndex = true;

            //Creates a few variables used to check for any errors
            int maxIndex = 0;                       //max index available for the tileset
            int height = baseLayer.GetLength(0);    //current height of the map
            int width = baseLayer.GetLength(1);     //current width of the map

            foreach (Texture2D t in curMapTiles)    //foreach texture in the current tileset
                maxIndex++;                             //increase the maxIndex by 1

            if (point.X < 0 || point.X > width-1)     //if user clicked outside the map boundries
                setIndex = false;                       //set the "setIndex" bool to false
            if (point.Y < 0 || point.Y > height-1)
                setIndex = false;

            if (newIndex < 0)                       //if the index is somehow less than 0, it doesn't exist
                setIndex = false;                       //set the "setIndex" bool to false
            else if (newIndex > maxIndex)           //if the index is somehow over the max number of tiles in the tileset
                setIndex = false;                       //set the "setIndex" bool to false

            try
            {
                if (setIndex)                       //if the set index bool is 'true'
                    baseLayer[point.Y, point.X] = newIndex; //set the index at the point provided
            }
            catch
            {
                throw new Exception("Base Layer Index is Out of Range");
            }
        }

        /// <summary>
        /// Adds an object to the tile specified
        /// </summary>
        /// <param name="point">Location to add object</param>
        /// <param name="newIndex">Object to be added</param>
        public void SetObjectLayerCellIndex(Point point, int newIndex)
        {
            bool setIndex = CheckForValidObjectLayerIndex(ref point, newIndex);

            try
            {
                if (setIndex)
                    objectLayer[point.Y, point.X] = newIndex;
            }
            catch
            {
                throw new Exception("Object Layer Index is out of range");
            }
        }

        /// <summary>
        /// Deletes an object on the specified tile
        /// </summary>
        /// <param name="point">Tile to delete object from</param>
        public void DeleteObjectLayerCellIndex(Point point)
        {
            int newIndex = -1;
            int height = objectLayer.GetLength(0);    //current height of the map
            int width = objectLayer.GetLength(1);     //current width of the map
            bool setIndex = true;

            if (point.X < 0 || point.X > width - 1)     //if user clicked outside the map boundries
                setIndex = false;                       //set the "setIndex" bool to false
            if (point.Y < 0 || point.Y > height - 1)
                setIndex = false;
            
            try
            {
                if (setIndex)
                    objectLayer[point.Y, point.X] = newIndex;
            }
            catch
            {
                throw new Exception("Object Layer Index is out of range");
            }
        }

        /// <summary>
        /// Checks to see if the new index is a valid index
        /// </summary>
        /// <param name="point">Point of the map being checked</param>
        /// <param name="newIndex">Index being checked</param>
        /// <returns>True: It is ok to set the index. False: Index is out of current boundires</returns>
        private static bool CheckForValidObjectLayerIndex(ref Point point, int newIndex)
        {
            bool setIndex = true;

            //Creates a few variables used to check for any errors
            int maxIndex = 0;                       //max index available for the tileset
            int height = objectLayer.GetLength(0);    //current height of the map
            int width = objectLayer.GetLength(1);     //current width of the map

            foreach (Texture2D t in curObjTiles)    //foreach texture in the current tileset
                maxIndex++;                             //increase the maxIndex by 1

            if (point.X < 0 || point.X > width-1)     //if user clicked outside the map boundries
                setIndex = false;                       //set the "setIndex" bool to false
            if (point.Y < 0 || point.Y > height-1)
                setIndex = false;

            if (newIndex < 0)                       //if the index is somehow less than 0, it doesn't exist
                setIndex = false;                       //set the "setIndex" bool to false
            else if (newIndex > maxIndex)           //if the index is somehow over the max number of tiles in the tileset
                setIndex = false;                       //set the "setIndex" bool to false
            return setIndex;
        }

        /// <summary>
        /// Toggles the tile as walkable or unwalkable. 
        /// </summary>
        /// <param name="point">Tile location</param>
        public void ToggleCollisionLayerCellIndex(Point point)
        {
            //If the walkable layer is 0, the tile is currently walkable
            if (CollisionLayer[point.Y, point.X] == 0)
            {
                CollisionLayer[point.Y, point.X] = 1;                //change to unwalkable
                rUnwalkable[point.Y, point.X] = new Rectangle(      //create the rectangle for collision detection
                    point.X * TileWidth + 15,
                    point.Y * TileHeight,
                    TileWidth - 10,
                    TileHeight - 10);
            }
            //if the tile is un-walkable, make it walkable
            else
            {
                CollisionLayer[point.Y, point.X] = 0;                        //change to walkable
                rUnwalkable[point.Y, point.X] = new Rectangle(0, 0, 0, 0);  //delete the rectangle for collision detection
            }
        }//end ToggleCollisionLayerCellIndex

        /// <summary>
        /// Sets a collision tile on the specified cell
        /// </summary>
        /// <param name="point">Tile location</param>
        public void AddCollisionCell(Point point)
        {
            bool setIndex = true;

            //Creates a few variables used to check for any errors
            int height = CollisionLayer.GetLength(0);    //current height of the map
            int width = CollisionLayer.GetLength(1);     //current width of the map

            if (point.X < 0 || point.X > width - 1)     //if user clicked outside the map boundries
                setIndex = false;                       //set the "setIndex" bool to false
            if (point.Y < 0 || point.Y > height - 1)
                setIndex = false;

            try
            {
                if (setIndex)
                {
                    CollisionLayer[point.Y, point.X] = 1;               //change to unwalkable
                    rUnwalkable[point.Y, point.X] = new Rectangle(      //create the rectangle for collision detection
                        point.X * TileWidth + 15,
                        point.Y * TileHeight,
                        TileWidth - 10,
                        TileHeight - 10);
                }
            }
            catch
            {
                throw new Exception("Collision Layer is out of bounds");
            }
        }//end AddCollisionCell

        /// <summary>
        /// Removes the collision tile from the specified cell
        /// </summary>
        /// <param name="point">Tile Location</param>
        public void RemoveCollisionCell(Point point)
        {
            bool setIndex = true;

            //Creates a few variables used to check for any errors
            int height = CollisionLayer.GetLength(0);    //current height of the map
            int width = CollisionLayer.GetLength(1);     //current width of the map

            if (point.X < 0 || point.X > width-1)     //if user clicked outside the map boundries
                setIndex = false;                       //set the "setIndex" bool to false
            if (point.Y < 0 || point.Y > height-1)
                setIndex = false;

            try
            {
                if (setIndex)
                {
                    CollisionLayer[point.Y, point.X] = 0;                        //change to walkable
                    rUnwalkable[point.Y, point.X] = new Rectangle(0, 0, 0, 0);  //delete the rectangle for collision detection
                }
            }
            catch
            {
                throw new Exception("Collision Layer is out of bounds");
            }
        }

        public void AddEnemyToMap(Point mouse, int TextureIndex)
        {
            try
            {
                enemyLayer[mouse.Y, mouse.X] = TextureIndex;
            }
            catch
            {
                throw new Exception("Object index was out of range");
            }
        }

        public void RemoveEnemyFromMap(Point mouse)
        {
            try
            {
                enemyLayer[mouse.Y, mouse.X] = -1;
            }
            catch
            {
                throw new Exception("Object index was out of range");
            }
        }

        /// <summary>
        /// Gets the index for the specific tile on the base layer
        /// </summary>
        /// <param name="point">point on map to get</param>
        /// <returns>Current index</returns>
        public int GetBaseLayerCellIndex(Point point)
        {
            return baseLayer[point.Y, point.X];
        }

        /// <summary>
        /// Gets the index for the specific tile on the object layer
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int GetObjectLayerCellIndex(Point point)
        {
            return objectLayer[point.Y, point.X];
        }

        /// <summary>
        /// Gets the index for the specific tile on the collision layer
        /// </summary>
        public int GetCollisionLayerCellIndex(Point point)
        {
            return CollisionLayer[point.Y, point.X];
        }

        #endregion


        /// <summary>
        /// Converts the current player position into a cell location 
        /// </summary>
        /// <param name="position">Players Vector2 position</param>
        /// <returns>Returns the cell location on the map</returns>
        public static Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)TileWidth),
                (int)(position.Y / (float)TileHeight));
        }

        /// <summary>
        /// Loads the texture data from the map tiles in the .map file
        /// </summary>
        /// <param name="content">Content Manager</param>
        /// <param name="textureNames">Texture names to be loaded</param>
        public void LoadTileTextures(ContentManager content, params string[] textureNames)
        {
            Texture2D texture;

            foreach (string s in textureNames)
            {
                texture = content.Load<Texture2D>(s);
                curMapTiles.Add(texture);
            }
        }

        #region Draw Methods
        /// <summary>
        /// Draws the curret map data and player location onto the screen
        /// </summary>
        /// <param name="setGameTime">Game Timer</param>
        public void Draw(SpriteBatch batch, Camera camera)
        {
            int height = baseLayer.GetLength(0);
            int width = baseLayer.GetLength(1);

            batch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.BackToFront,
                SaveStateMode.None,
                camera.TransformMatrix);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    #region Draw Each Layer to Screen

                    DrawBaseLayer(batch, x, y);
                    DrawObjectLayer(batch, x, y);

                    // Should be drawn only in map editor
                    //DrawEnemyLayer(batch, x, y);

                    #endregion
                }
            }

            batch.End();
        }

        /// <summary>
        /// Renders the base layer to screen
        /// </summary>
        private static void DrawBaseLayer(SpriteBatch batch, int x, int y)
        {
            int maxIndex = 0;
         
            foreach (Texture2D t in curMapTiles)
                maxIndex++;

            int baseLayerIndex = baseLayer[y, x];
            bool draw = true;

            if (baseLayerIndex < 0)
                draw = false;

            if (baseLayerIndex >= maxIndex)
                draw = false;

            if (draw)
            {
                Texture2D texture = curMapTiles[baseLayerIndex];
                batch.Draw(texture,
                    new Rectangle(
                        x * TileWidth,
                        y * TileHeight,
                        TileWidth,
                        TileHeight),
                        null,
                        new Color(new Vector4(1f, 1f, 1f, 1f)),
                        0.0f,
                        Vector2.Zero, SpriteEffects.None, 1);
            }

        }

        /// <summary>
        /// Draws the object layer
        /// </summary>
        private static void DrawObjectLayer(SpriteBatch batch, int x, int y)
        {
            int objectLayerIndex = objectLayer[y, x];


            if (objectLayerIndex != -1)
            {

                Texture2D objTexture = curObjTiles[objectLayerIndex];
                batch.Draw(objTexture,
                            new Rectangle(
                                x * TileWidth,
                                y * TileHeight,
                                TileWidth,
                                TileHeight),
                            null,
                            new Color(new Vector4(1f, 1f, 1f, 1f)),
                            0.0f,
                            Vector2.Zero, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Draws the Enemy layer
        /// </summary>
        private static void DrawEnemyLayer(SpriteBatch batch, int x, int y)
        {
            int enemyLayerIndex = enemyLayer[y, x];


            if (enemyLayerIndex != -1)
            {

                Texture2D enemyTexture = curEnemyTiles[enemyLayerIndex];
                batch.Draw(enemyTexture,
                            new Rectangle(
                                x * TileWidth,
                                y * TileHeight,
                                TileWidth,
                                TileHeight),
                                new Rectangle(0,0,40,40),
                            new Color(new Vector4(1f, 1f, 1f, 1f)),
                            0.0f,
                            Vector2.Zero, SpriteEffects.None, 0);
            }
        }
        #endregion

        #region MapEditor Methods
        /// <summary>
        /// Path list of all object textures within the Tileset path
        /// </summary>
        public const string ObjectsSubDir = "/Objects/";

        /// <summary>
        /// Path to all of the different tilesets used in the engine
        /// </summary>
        public const string TilesetDirPath = "Tilesets/";

        /// <summary>
        /// Path list of all map files
        /// </summary>
        public const string MapFilePath = "Content/Maps/";

        /// <summary>
        /// Gets the path to the current tileset being used
        /// </summary>
        public string GetCurrentTilesetPath
        {
            get 
            {
                string path = TilesetDirPath + GameArea.curGameArea.ToString();
                return path;
            }
        }

        /// <summary>
        /// Saves the current Tile Map to a file
        /// </summary>
        /// <param name="filename">File name to be saved to</param>
        /// <param name="textureNames">List of texture names</param>
        public void SaveFile()
        {
            string curArea = GameArea.curGameArea.ToString();
            string path = TileEngineV2.MapFilePath;
            int Height = GetHeightInPixels / TileEngineV2.TileHeight;
            int Width = GetWidthInPixels / TileEngineV2.TileWidth;

            using (StreamWriter writer = new StreamWriter(path + curArea+".map"))
            {
                //writes the map textures to file
                writer.WriteLine("[Tileset]");
                writer.WriteLine(curTileSet);

                writer.WriteLine();

                //writes the Object textures to file
                writer.WriteLine("[Objects]");
                writer.WriteLine(curTileSet);

                writer.WriteLine();

                writer.WriteLine("[BaseLayer]");
                for (int y = 0; y < Height; y++)
                {
                    string line = string.Empty;

                    for (int x = 0; x < Width; x++)
                    {
                        line += baseLayer[y, x].ToString() + " ";
                    }

                    writer.WriteLine(line);
                }

                writer.WriteLine();

                writer.WriteLine("[ObjectLayer]");
                for (int y = 0; y < Height; y++)
                {
                    string line = string.Empty;

                    for (int x = 0; x < Width; x++)
                    {
                        line += objectLayer[y, x].ToString() + " ";
                    }

                    writer.WriteLine(line);
                }


                writer.WriteLine();

                writer.WriteLine("[CollisionLayer]");

                for (int y = 0; y < Height; y++)
                {
                    string line = string.Empty;

                    for (int x = 0; x < Width; x++)
                    {
                        line += CollisionLayer[y, x].ToString() + " ";
                    }

                    writer.WriteLine(line);
                }

                writer.WriteLine("[EnemyLayer]");

                for (int y = 0; y < Height; y++)
                {
                    string line = string.Empty;

                    for (int x = 0; x < Width; x++)
                    {
                        line += enemyLayer[y, x].ToString() + " ";
                    }

                    writer.WriteLine(line);
                }
                
            }
        }

        /// <summary>
        /// Unloads all the assets of the current map before a new
        /// one is loaded in
        /// </summary>
        public void UnloadCurrentMap()
        {
            curMapTiles.Clear();            
            curMapTileNames.Clear();
            curObjTiles.Clear();
            curObjTileNames.Clear();
            curEnemyTiles.Clear();
            curEnemyTileNames.Clear();
        }

        /// <summary>
        /// Renders the tile engine while using the map editor. Use this draw method
        /// ONLY when inside the map editor (or any instance when you're controlling
        /// the camera and not the player)
        /// </summary>
        public void DrawInMapEditor(SpriteBatch batch, Camera camera)
        {
            int height = baseLayer.GetLength(0);
            int width = baseLayer.GetLength(1);
                        
            batch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.BackToFront,
                SaveStateMode.None,
                camera.MapEditorMatrix);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    #region Draws each layer

                    DrawBaseLayer(batch, x, y);
                    DrawObjectLayer(batch, x, y);
                    DrawEnemyLayer(batch, x, y);

                    #endregion
                }
            }
            batch.End();
        }

        #endregion






    }//end Class

}
