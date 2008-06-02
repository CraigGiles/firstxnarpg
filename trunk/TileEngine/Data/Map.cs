using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml;

using Helper;

namespace ActionRPG
{
    public class Map
    {

        #region Map Data

        
        /// <summary>
        /// Gets or sets map Name
        /// </summary>
        public string Name
        {
            get { return mapName; }
            set { mapName = value; }
        }
        string mapName;


        /// <summary>
        /// Gets or sets map Height
        /// </summary>
        public int Height
        {
            get { return mapDimentions.Y; }
            set { mapDimentions.Y = value; }
        }
        /// <summary>
        /// Gets or sets map width
        /// </summary>
        public int Width
        {
            get { return mapDimentions.X; }
            set { mapDimentions.X = value; }
        }
        Point mapDimentions;


        /// <summary>
        /// Gets or sets tile height
        /// </summary>
        public int TileHeight
        {
            get { return tileSize.Y; }
            set { tileSize = new Point(value, value); }
        }
        /// <summary>
        /// Gets or sets tile width
        /// </summary>
        public int TileWidth
        {
            get { return tileSize.X; }
            set { tileSize = new Point(value, value); }
        }
        Point tileSize;


        /// <summary>
        /// Gets or sets current TileSet
        /// </summary>
        public string Tileset
        {
            get { return tileSet; }
            set { tileSet = value; }
        }
        string tileSet;


        /// <summary>
        /// List of currently loaded tiles
        /// </summary>
        public List<Texture2D> Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }
        List<Texture2D> tiles = new List<Texture2D>();


        /// <summary>
        /// Map tile location of player respawn
        /// </summary>
        public Point PlayerRespawnTile
        {
            get { return playerRespawn; }
            set { playerRespawn = value; }
        }
        /// <summary>
        /// Vector location of player respawn
        /// </summary>
        public Vector2 PlayerRespawnVector
        {
            get 
            { 
                return new Vector2(playerRespawn.X * TileWidth, playerRespawn.Y * TileHeight); 
            }
        }
        Point playerRespawn;



        /// <summary>
        /// List of NPCs on current map
        /// </summary>
        public List<NPC> NPCs
        {
            get { return npcs; }
            set { npcs = value; }
        }
        List<NPC> npcs = new List<NPC>();


        /// <summary>
        /// List of items in the current maps world
        /// </summary>
        public List<Item> Items
        {
            get { return items; }
            set { items = value; }
        }
        List<Item> items = new List<Item>();


        /// <summary>
        /// List of portals on the current map
        /// </summary>
        public List<Portal> Portals
        {
            get { return portals; }
            set { portals = value; }
        }
        List<Portal> portals = new List<Portal>();


        /// <summary>
        /// Music for the current map
        /// </summary>
        public string MusicCue
        {
            get { return musicCue; }
            set { musicCue = value; }
        }
        string musicCue;

        
        /// <summary>
        /// Current Map BaseLayer
        /// </summary>
        public int[,] BaseLayer
        {
            get { return baseLayer; }
            set { baseLayer = (int[,])value.Clone(); }
        }
        /// <summary>
        /// Current Map FringeLayer
        /// </summary>
        public int[,] FringeLayer
        {
            get { return fringeLayer; }
            set { fringeLayer = (int[,])value.Clone(); }
        }
        /// <summary>
        /// Current Map ObjectLayer
        /// </summary>
        public int[,] ObjectLayer
        {
            get { return objectLayer; }
            set { objectLayer = (int[,])value.Clone(); }
        }
        /// <summary>
        /// Current Map CollisionLayer
        /// </summary>
        public int[,] CollisionLayer
        {
            get { return collisionLayer; }
            set { collisionLayer = (int[,])value.Clone(); }
        }
        int[,] baseLayer,
            fringeLayer,
            objectLayer,
            collisionLayer;


        #endregion


        #region MapEditor Delete Lists
        List<NPC> npcsToRemove = new List<NPC>();
        List<Item> itemsToRemove = new List<Item>();
        List<Portal> portalsToRemove = new List<Portal>();
        #endregion


        #region Save Current Map

        /// <summary>
        /// Saves the current map
        /// </summary>
        public void SaveMap()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement Map = doc.CreateElement("Map");
            doc.AppendChild(Map);

            //Write map information
            WriteMapName(doc, Map);
            WriteMapDimensions(doc, Map);
            WriteTileSize(doc, Map);
            WriteTileSet(doc, Map);
            WritePlayerRespawn(doc, Map);
            WriteNpcInfo(doc, Map);
            WriteItemInfo(doc, Map);
            WritePortalInfo(doc, Map);
            WriteMusicCue(doc, Map);
            WriteLayerInfo(doc, Map);

            //save map
            //doc.Save(@"Content/Maps" + Name + ".xml");
            doc.Save(@"D:/" + Name + ".xml");
        }

        private void WriteLayerInfo(XmlDocument doc, XmlElement Map)
        {
            WriteBaseLayerInfo(doc, Map);
            WriteFringeLayerInfo(doc, Map);
            WriteObjectLayerInfo(doc, Map);
            WriteCollisionLayerInfo(doc, Map);
        }

        private void WriteCollisionLayerInfo(XmlDocument doc, XmlElement Map)
        {
            XmlElement CollisionLayerElement = doc.CreateElement("CollisionLayer");

            for (int y = 0; y < Height; y++)
            {
                XmlNode RowNode = doc.CreateElement("Row");

                string rowInformation = string.Empty;

                for (int x = 0; x < Width; x++)
                {
                    rowInformation += collisionLayer[y, x].ToString() + " ";
                }

                RowNode.InnerText = rowInformation;
                CollisionLayerElement.AppendChild(RowNode);
            }

            Map.AppendChild(CollisionLayerElement);
        }

        private void WriteObjectLayerInfo(XmlDocument doc, XmlElement Map)
        {
            XmlElement ObjectLayerElement = doc.CreateElement("ObjectLayer");

            for (int y = 0; y < Height; y++)
            {
                XmlNode RowNode = doc.CreateElement("Row");

                string rowInformation = string.Empty;

                for (int x = 0; x < Width; x++)
                {
                    rowInformation += objectLayer[y, x].ToString() + " ";
                }

                RowNode.InnerText = rowInformation;
                ObjectLayerElement.AppendChild(RowNode);
            }

            Map.AppendChild(ObjectLayerElement);
        }

        private void WriteFringeLayerInfo(XmlDocument doc, XmlElement Map)
        {
            XmlElement FringeLayerElement = doc.CreateElement("FringeLayer");

            for (int y = 0; y < Height; y++)
            {
                XmlNode RowNode = doc.CreateElement("Row");

                string rowInformation = string.Empty;

                for (int x = 0; x < Width; x++)
                {
                    rowInformation += fringeLayer[y, x].ToString() + " ";
                }

                RowNode.InnerText = rowInformation;
                FringeLayerElement.AppendChild(RowNode);
            }

            Map.AppendChild(FringeLayerElement);
        }

        private void WriteBaseLayerInfo(XmlDocument doc, XmlElement Map)
        {
            XmlElement BaseLayerElement = doc.CreateElement("BaseLayer");

            for (int y = 0; y < Height; y++)
            {
                XmlNode RowNode = doc.CreateElement("Row");

                string rowInformation = string.Empty;

                for (int x = 0; x < Width; x++)
                {
                    rowInformation += baseLayer[y, x].ToString() + " ";
                }

                RowNode.InnerText = rowInformation;
                BaseLayerElement.AppendChild(RowNode);
            }

            Map.AppendChild(BaseLayerElement);
        }

        private void WriteMusicCue(XmlDocument doc, XmlElement Map)
        {
            XmlElement MusicCueElement = doc.CreateElement("MusicCueName");
            MusicCueElement.InnerText = MusicCue;

            Map.AppendChild(MusicCueElement);
        }

        private void WritePortalInfo(XmlDocument doc, XmlElement Map)
        {
            foreach (Portal portal in portals)
            {
                XmlElement PortalElement = doc.CreateElement("Portal");

                XmlElement EnteranceElement = doc.CreateElement("Enterance");

                    XmlNode EnteranceX = doc.CreateElement("X");
                    EnteranceX.InnerText = portal.PortalEnteranceTile.X.ToString();
                    XmlNode EnteranceY = doc.CreateElement("Y");
                    EnteranceY.InnerText = portal.PortalEnteranceTile.Y.ToString();

                    EnteranceElement.AppendChild(EnteranceX);
                    EnteranceElement.AppendChild(EnteranceY);

                PortalElement.AppendChild(EnteranceElement);

                XmlElement DestinationElement = doc.CreateElement("Destination");

                    XmlNode DestinationMap = doc.CreateElement("Map");
                    DestinationMap.InnerText = portal.DestinationMap;

                    XmlNode DestinationSpawnX = doc.CreateElement("X");
                    DestinationSpawnX.InnerText = portal.DestinationSpawn.X.ToString();
                    XmlNode DestinationSpawnY = doc.CreateElement("Y");
                    DestinationSpawnY.InnerText = portal.DestinationSpawn.Y.ToString();

                    DestinationElement.AppendChild(DestinationMap);
                    DestinationElement.AppendChild(DestinationSpawnX);
                    DestinationElement.AppendChild(DestinationSpawnY);

                PortalElement.AppendChild(DestinationElement);

                Map.AppendChild(PortalElement);
            }
        }

        private void WriteItemInfo(XmlDocument doc, XmlElement Map)
        {
            foreach (Item item in items)
            {
                XmlElement ItemElement = doc.CreateElement("Item");

                XmlNode itemAsset = doc.CreateElement("Asset");
                itemAsset.InnerText = item.AssetName;

                XmlNode itemX = doc.CreateElement("X");
                itemX.InnerText = item.SpawnLocationTile.X.ToString();

                XmlNode itemY = doc.CreateElement("Y");
                itemY.InnerText = item.SpawnLocationTile.Y.ToString();

                ItemElement.AppendChild(itemAsset);
                ItemElement.AppendChild(itemX);
                ItemElement.AppendChild(itemY);

                Map.AppendChild(ItemElement);
            }
        }

        private void WriteNpcInfo(XmlDocument doc, XmlElement Map)
        {
            foreach (NPC npc in npcs)
            {
                XmlElement NpcElement = doc.CreateElement("NPC");

                XmlNode npcAsset = doc.CreateElement("Asset");
                npcAsset.InnerText = npc.AssetName;

                XmlNode npcX = doc.CreateElement("X");
                npcX.InnerText = npc.SpawnLocationTile.X.ToString();

                XmlNode npcY = doc.CreateElement("Y");
                npcY.InnerText = npc.SpawnLocationTile.Y.ToString();

                NpcElement.AppendChild(npcAsset);
                NpcElement.AppendChild(npcX);
                NpcElement.AppendChild(npcY);

                Map.AppendChild(NpcElement);
            }
        }

        private void WritePlayerRespawn(XmlDocument doc, XmlElement Map)
        {
            XmlElement PlayerRespawnElement = doc.CreateElement("PlayerRespawn");
            XmlNode pRespawnX = doc.CreateElement("X");
            pRespawnX.InnerText = playerRespawn.X.ToString();

            XmlNode pRespawnY = doc.CreateElement("Y");
            pRespawnY.InnerText = playerRespawn.Y.ToString();

            PlayerRespawnElement.AppendChild(pRespawnX);
            PlayerRespawnElement.AppendChild(pRespawnY);

            Map.AppendChild(PlayerRespawnElement);
        }

        private void WriteTileSet(XmlDocument doc, XmlElement Map)
        {
            XmlElement TileSetElement = doc.CreateElement("TileSet");
            TileSetElement.InnerText = tileSet.ToString();
            Map.AppendChild(TileSetElement);
        }

        private void WriteTileSize(XmlDocument doc, XmlElement Map)
        {
            XmlElement TileSizeElement = doc.CreateElement("TileSize");
            XmlNode tileSizeWidth = doc.CreateElement("Width");
            tileSizeWidth.InnerText = TileWidth.ToString();

            XmlNode tileSizeHeight = doc.CreateElement("Height");
            tileSizeHeight.InnerText = TileHeight.ToString();

            TileSizeElement.AppendChild(tileSizeWidth);
            TileSizeElement.AppendChild(tileSizeHeight);

            Map.AppendChild(TileSizeElement);
        }

        private void WriteMapName(XmlDocument doc, XmlElement Map)
        {
            XmlElement Name = doc.CreateElement("Name");
            Name.InnerText = mapName;
            Map.AppendChild(Name);
        }

        private void WriteMapDimensions(XmlDocument doc, XmlElement Map)
        {
            XmlElement MapDimensions = doc.CreateElement("MapDimensions");
            XmlNode mapDimensionWidth = doc.CreateElement("Width");
            mapDimensionWidth.InnerText = Width.ToString();

            XmlNode mapDimensionHeight = doc.CreateElement("Height");
            mapDimensionHeight.InnerText = Height.ToString();

            MapDimensions.AppendChild(mapDimensionWidth);
            MapDimensions.AppendChild(mapDimensionHeight);

            Map.AppendChild(MapDimensions);
        }



        #endregion


        #region Load Map

        //Temp Lists of each layer
        List<List<int>> tempBaseLayer = new List<List<int>>();
        List<List<int>> tempFringeLayer = new List<List<int>>();
        List<List<int>> tempObjectLayer = new List<List<int>>();
        List<List<int>> tempCollisionLayer = new List<List<int>>();

        /// <summary>
        /// Loads a map from the content pipeline
        /// </summary>
        /// <param name="name">Asset name of map to load</param>
        public void Load(string name)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content/Maps/" + name + ".xml");
            //doc.Load(@"D:/" + name + ".xml");

            foreach (XmlNode nodes in doc)
            {
                foreach (XmlNode node in nodes.ChildNodes)
                {
                    if (node.Name == "Name")
                        mapName = node.InnerText;

                    else if (node.Name == "MapDimensions")
                        SetMapDimensions(node);

                    else if (node.Name == "TileSize")
                        SetTileSize(node);

                    else if (node.Name == "TileSet")
                        SetTileSet(node);

                    else if (node.Name == "PlayerRespawn")
                        SetPlayerRespawn(node);

                    else if (node.Name == "NPC")
                        SetNpc(node);

                    else if (node.Name == "Item")
                        SetItem(node);

                    else if (node.Name == "Portal")
                        SetPortal(node);

                    else if (node.Name == "MusicCueName")
                        SetMusicCue(node);

                    else if (node.Name == "BaseLayer")
                        LoadMapLayer(Enum.MapLayer.Base, node);

                    else if (node.Name == "FringeLayer")
                        LoadMapLayer(Enum.MapLayer.Fringe, node);

                    else if (node.Name == "ObjectLayer")
                        LoadMapLayer(Enum.MapLayer.Object, node);

                    else if (node.Name == "CollisionLayer")
                        LoadMapLayer(Enum.MapLayer.Collision, node);
                }
            }

            LoadTileset();
            InitializeMapLayout();
        }

        private void LoadMapLayer(Enum.MapLayer layer, XmlNode nodes)
        {
            foreach (XmlNode node in nodes.ChildNodes)
            {
                //initiate a temp row to house data in stream
                List<int> row = new List<int>();


                //house each cell in the cells array
                string[] cells = node.InnerText.Trim().Split(' ');

                //convert each string to an int, and add it to the row list
                foreach (string c in cells)
                {
                    int cell = 0;

                    if (!string.IsNullOrEmpty(c))
                    {
                        int.TryParse(c, out cell);
                    }

                    row.Add(cell);
                }

                //depending what layer we're inputing, add the row to that layer
                switch (layer)
                {
                    case Enum.MapLayer.Base:
                        tempBaseLayer.Add(row);
                        break;

                    case Enum.MapLayer.Fringe:
                        tempFringeLayer.Add(row);
                        break;

                    case Enum.MapLayer.Object:
                        tempObjectLayer.Add(row);
                        break;

                    case Enum.MapLayer.Collision:
                        tempCollisionLayer.Add(row);
                        break;
                }//end switch
            }
        }

        private void SetMusicCue(XmlNode node)
        {
        }

        private void SetPortal(XmlNode node)
        {
            portals.Add(new Portal());
            Point sLocation = Point.Zero;
            Point pLocation = Point.Zero;

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Destination")
                {
                    foreach (XmlNode dNode in n.ChildNodes)
                    {
                        if (dNode.Name == "Map")
                            portals[portals.Count - 1].DestinationMap = dNode.InnerText;

                        if (dNode.Name == "X")
                            sLocation.X = int.Parse(dNode.InnerText);

                        if (dNode.Name == "Y")
                            sLocation.Y = int.Parse(dNode.InnerText);
                    }
                }

                if (n.Name == "Enterance")
                {
                    foreach (XmlNode eNode in n.ChildNodes)
                    {
                        if (eNode.Name == "X")
                            pLocation.X = int.Parse(eNode.InnerText);

                        if (eNode.Name == "Y")
                            pLocation.Y = int.Parse(eNode.InnerText);
                    }
                }
            }

            //set the spawn location to sLocation (spawn location)
            portals[portals.Count - 1].DestinationSpawn = sLocation;

            //set the enterance to pLocation (portal location)
            portals[portals.Count - 1].PortalEnteranceTile = pLocation;
            portals[portals.Count - 1].PortalEnteranceVector = new Vector2(
                pLocation.X * TileWidth,
                pLocation.Y * TileHeight);
        }

        private void SetItem(XmlNode node)
        {
            items.Add(new Item());
            Point sLocation = Point.Zero;


            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Asset")
                    items[npcs.Count - 1].AssetName = n.InnerText;

                if (n.Name == "X")
                    sLocation.X = int.Parse(n.InnerText);

                if (n.Name == "Y")
                    sLocation.Y = int.Parse(n.InnerText);
            }

            //sets the spawn location vectors and points
            items[items.Count - 1].SpawnLocationTile = sLocation;
            items[items.Count - 1].SpawnLocationVector = new Vector2(
                sLocation.X * TileWidth,
                sLocation.Y * TileHeight);
        }

        private void SetNpc(XmlNode node)
        {
            npcs.Add(new NPC());
            Point sLocation = Point.Zero;

            
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Asset")
                    npcs[npcs.Count - 1].AssetName = n.InnerText;

                if (n.Name == "X")
                    sLocation.X = int.Parse(n.InnerText);

                if (n.Name == "Y")
                    sLocation.Y = int.Parse(n.InnerText);
            }

            //converts the map point location to position vector2 location
            npcs[npcs.Count - 1].SpawnLocationTile = sLocation;
            npcs[npcs.Count - 1].SpawnLocationVector = new Vector2(
                sLocation.X * TileWidth,
                sLocation.Y * TileHeight);
        }

        private void SetPlayerRespawn(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "X")
                    playerRespawn.X = int.Parse(n.InnerText);

                else if (n.Name == "Y")
                    playerRespawn.Y = int.Parse(n.InnerText);
            }
        }

        private void SetTileSet(XmlNode node)
        {
            tileSet = node.InnerText;
        }

        private void SetTileSize(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Width")
                    TileWidth = int.Parse(n.InnerText);
                else if (n.Name == "Height")
                    TileHeight = int.Parse(n.InnerText);
            }
        }

        private void SetMapDimensions(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Width")
                    Width = int.Parse(n.InnerText);
                else if (n.Name == "Height")
                    Height = int.Parse(n.InnerText);
            }
        }



        /// <summary>
        /// Initializes map once all data has been loaded
        /// </summary>
        private void InitializeMapLayout()
        {
            //create new arrays to house each layer
            baseLayer = new int[mapDimentions.Y, mapDimentions.X];
            fringeLayer = new int[mapDimentions.Y, mapDimentions.X];
            objectLayer = new int[mapDimentions.Y, mapDimentions.X];
            collisionLayer = new int[mapDimentions.Y, mapDimentions.X];

            for (int y = 0; y < mapDimentions.Y; y++)
            {
                for (int x = 0; x < mapDimentions.X; x++)
                {
                    //Loads in the layer data from temp list
                    baseLayer[y, x] = tempBaseLayer[y][x];
                    fringeLayer[y, x] = tempFringeLayer[y][x];
                    objectLayer[y, x] = tempObjectLayer[y][x];
                    CollisionLayer[y, x] = tempCollisionLayer[y][x];

                }
            }//end for loops

        }

        #endregion


        #region Load New Tileset

        /// <summary>
        /// Loads all textures associated with the current tileset
        /// </summary>
        public void LoadTileset()
        {
            //Xml Document
            XmlDocument doc = new XmlDocument();

            //Load XML document
            doc.Load(@"Content/Graphics/Tilesets/Tilesets.xml");

            //cycle through all children nodes
            foreach (XmlNode node in doc.ChildNodes)
            {
                //initialize loaded bool
                bool loaded = false;
                
                foreach (XmlNode n in node.ChildNodes)
                {
                    //if node's name is the tileset we're looking for
                    if (n.Name == Tileset)
                    {
                        //create an array to house all texture assets
                        string[] t = new string[n.ChildNodes.Count];

                        //cycle through all the child nodes within the tileset node
                        for (int i = 0; i < n.ChildNodes.Count; i++)
                        {
                            //store the texture asset into the array using the textures ID attribute
                            t[int.Parse(n.ChildNodes[i].Attributes["ID"].Value)] = n.ChildNodes[i].Attributes["Asset"].Value.ToString();
                        }

                        //load all textures in the tileset
                        LoadTextures(t);

                        //set bool to true, so we don't cycle through the rest 
                        //of our tilesets
                        loaded = true;
                    }
                }

                //if we already loaded a tileset, break out of loop
                if (loaded)
                    break;
            }

            

        }

        /// <summary>
        /// Loads a texture from the tileset
        /// </summary>
        /// <param name="asset"></param>
        private void LoadTextures(string[] assets)
        {
            for (int i = 0; i < assets.Length; i++)
            {
                tiles.Add(Globals.Content.Load<Texture2D>(@"Graphics/Tilesets/" + Tileset + "/" + assets[i]));
            }
        }

        #endregion


        #region Populate Map

        public void Populate()
        {
            //populate NPCs
            PopulateNpcs();

            //populate World Items
            PopulateItems();
        }

        private void PopulateNpcs()
        {
            foreach (NPC npc in npcs)
            {
                npc.Initialize();
            }
        }

        private void PopulateItems()
        {
            foreach (Item item in items)
            {
                item.Initialize();
            }
        }

        #endregion


        #region Unload Map

        public void UnloadMap()
        {
            baseLayer = new int[20, 20];
            objectLayer = new int[20, 20];
            fringeLayer = new int[20, 20];
            collisionLayer = new int[20, 20];

            tiles.Clear();
            npcs.Clear();
            items.Clear();
            portals.Clear();
        }
        
        #endregion

        
        #region Player Stepped Into Portal

        
        /// <summary>
        /// When a player steps into a portal, loads the new map and 
        /// returns the portals destination tile on new map
        /// </summary>
        /// <param name="portal">Portal stepped onto by player</param>
        /// <returns>Point</returns>
        public Point PlayerSteppedInPortal(Portal portal)
        {            
            Load(portal.DestinationMap);

            return portal.DestinationSpawn;
        }


        #endregion


        #region Draw Methods

        /// <summary>
        /// Render the loaded map to screen
        /// </summary>
        /// <param name="camera">Game Camera</param>
        public void Draw()
        {
            if (!MapEditor.Active)
                Globals.Batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None, Globals.Camera.TransformMatrix);
            else
                Globals.Batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None, Globals.Camera.MapEditorTransformMatrix);


            for (int y = 0; y < mapDimentions.Y; y++)
            {
                for (int x = 0; x < mapDimentions.X; x++)
                {
                    DrawBaseLayer(y, x);
                    DrawFringeLayer(y, x);
                    DrawObjectLayer(y, x);
                }
            }

            Globals.Batch.End();
        }

        /// <summary>
        /// Renders the base layer tile located at the given cell
        /// </summary>
        /// <param name="y">Y Tile</param>
        /// <param name="x">X Tile</param>
        private void DrawBaseLayer(int y, int x)
        {
            Globals.Batch.Draw(
                tiles[baseLayer[y, x]],
                new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight),
                Color.White);
        }

        /// <summary>
        /// Renders the Fringe Layer tile located at the given cell
        /// </summary>
        /// <param name="y">Y Tile</param>
        /// <param name="x">X Tile</param>
        private void DrawFringeLayer(int y, int x)
        {
            if (fringeLayer[y, x] != 0)
                Globals.Batch.Draw(
                tiles[fringeLayer[y, x]],
                new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight),
                Color.White);
        }


        /// <summary>
        /// Renders the Object Layer tile located at the given cell
        /// </summary>
        /// <param name="y">Y Tile</param>
        /// <param name="x">X Tile</param>
        private void DrawObjectLayer(int y, int x)
        {
            if (objectLayer[y, x] != 0)
                Globals.Batch.Draw(
                tiles[objectLayer[y, x]],
                new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight),
                Color.White);
        }


        #endregion


        #region Editor Methods


        /// <summary>
        /// Sets the new cell index for current map
        /// </summary>
        /// <param name="layer">Layer to modify</param>
        /// <param name="x">Tile located at X position</param>
        /// <param name="y">Tile located at Y position</param>
        /// <param name="index">new cell index</param>
        public void SetCellIndex(Enum.MapLayer layer, int x, int y, int index)
        {
            bool setIndex = false;

            //if the layer being modified is collision
            if (layer == Enum.MapLayer.Collision)
            {
                //if the index is 1 or 0, set bool to true
                if (index == 1 || index == 0)
                    setIndex = true;
            }

            //else if layer being modified is NOT collision
            else
            {
                //ensure the layer is in the maps dimentions
                if (x >= 0 && x <= mapDimentions.X &&
                   y >= 0 && y <= mapDimentions.Y)
                    setIndex = true;
            }
            
            //if it is safe to set the new index
            if (setIndex)
            {
                //set the appropriate layer's index
                switch (layer)
                {
                    case Enum.MapLayer.Base:
                        baseLayer[y, x] = index;
                        break;

                    case Enum.MapLayer.Fringe:
                        fringeLayer[y, x] = index;
                        break;

                    case Enum.MapLayer.Object:
                        objectLayer[y, x] = index;
                        break;

                    case Enum.MapLayer.Collision:
                        collisionLayer[y, x] = index;
                        break;

                }//end switch
            }
                
        }
        

        /// <summary>
        /// Adjusts the size of the map
        /// </summary>
        /// <param name="height">Amount of Cols to adjust the map</param>
        /// <param name="width">Amount of Rows to adjust the map</param>
        internal void AdjustMapSize(int height, int width)
        {
            //clamps the height and width between 1 and 99
            int newHeight = (int)MathHelper.Clamp(Height + height, 1, 99);
            int newWidth = (int)MathHelper.Clamp(Width + width, 1, 99);


            //creates temp arrays 
            int[,] tempBase = new int[99, 99];
            int[,] tempFringe = new int[99, 99];
            int[,] tempObject = new int[99, 99];
            int[,] tempCollision = new int[99, 99];

            
            //copy old map to temp array
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    tempBase[y, x] = baseLayer[y, x];
                    tempFringe[y, x] = fringeLayer[y, x];
                    tempObject[y, x] = objectLayer[y, x];
                    tempCollision[y, x] = collisionLayer[y, x];
                }
            }

            
            //adjust the size of the map
            baseLayer = new int[newHeight, newWidth];
            fringeLayer = new int[newHeight, newWidth];
            objectLayer = new int[newHeight, newWidth];
            collisionLayer = new int[newHeight, newWidth];


            //copy old map to new arrays
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    baseLayer[y, x] = tempBase[y, x];
                    fringeLayer[y, x] = tempFringe[y, x];
                    objectLayer[y, x] = tempObject[y, x];
                    collisionLayer[y, x] = tempCollision[y, x];
                }
            }

            //adjust the size variables
            mapDimentions = new Point(newWidth, newHeight);

        }


        /// <summary>
        /// Adjusts the size of each tile used to draw the map
        /// </summary>
        /// <param name="newValue">New size of tile</param>
        internal void AdjustTileSize(int newValue)
        {
            TileHeight = (int)MathHelper.Clamp(newValue, 2, 128);
            TileWidth = (int)MathHelper.Clamp(newValue, 2, 128);
        }


        /// <summary>
        /// Sets the player respawn location tile
        /// </summary>
        /// <param name="point">point: map tile to place spawn</param>
        internal void SetPlayerRespawn(Point point)
        {
            playerRespawn.X = point.X;
            playerRespawn.Y = point.Y;
        }


        #region Add / Remove Npc

        /// <summary>
        /// Adds a monster to the map. 
        /// Edit monster asset via XML
        /// </summary>
        /// <param name="point">tile to add monster</param>
        internal void AddNpc(Point point)
        {
            bool add = true;
            foreach (NPC n in npcs)
            {
                if (n.SpawnLocationTile == point)
                    add = false;
            }

            if (add)
            {
                //adds monster to the NPCs list
                npcs.Add(new NPC());

                //converts the monsters spawn tile to its Vector2 location
                npcs[npcs.Count - 1].SpawnLocationVector = new Vector2(point.X * TileWidth, point.Y * TileHeight); 
            }
        }


        /// <summary>
        /// Adds npc to the 'npcsToRemove' list
        /// </summary>
        /// <param name="npc">npc to delete</param>
        internal void RemoveNpc(NPC npc)
        {
            npcsToRemove.Add(npc);
        }

        #endregion


        #region Add / Remove Items

        /// <summary>
        /// Adds an item to the map
        /// edit item asset via XML
        /// </summary>
        /// <param name="point">tile to add item</param>
        internal void AddItem(Point point)
        {
            bool add = true;

            foreach (Item i in items)
            {
                if (i.SpawnLocationTile == point)
                    add = false;
            }

            if (add)
            {
                //adds item to items list
                items.Add(new Item());

                //converts the items spawn tile to its Vector2 location
                items[items.Count - 1].SpawnLocationTile = point;
                items[items.Count - 1].SpawnLocationVector = new Vector2(
                    point.X * TileWidth,
                    point.Y * TileHeight); 
            }
        }


        /// <summary>
        /// Adds item to the 'itemsToRemove' list
        /// </summary>
        /// <param name="item">item to be deleted</param>
        internal void RemoveItem(Item item)
        {
            itemsToRemove.Add(item);
        }

        #endregion


        #region Add / Remove Portals


        /// <summary>
        /// Adds a portal to the map
        /// edit portal asset via XML
        /// </summary>
        /// <param name="point">tile to add portal</param>
        internal void AddPortal(Point point)
        {
            bool add = true;

            foreach (Portal p in portals)
            {
                if (p.PortalEnteranceTile == point)
                    add = false;
            }

            if (add)
            {
                //adds a new portal to the portals list
                portals.Add(new Portal());

                //converts the portals spawn tile to its Vector2 location
                portals[portals.Count - 1].PortalEnteranceTile = point;
            }
        }


        /// <summary>
        /// Adds portal to the 'portalsToRemove' list
        /// </summary>
        /// <param name="portal">portal to be deleted</param>
        internal void RemovePortal(Portal portal)
        {
            portalsToRemove.Add(portal);
        }

        #endregion


        /// <summary>
        /// Trashes any objects removed by the user
        /// </summary>
        internal void TrashDeletedItems()
        {
            foreach (NPC n in npcsToRemove)
                npcs.Remove(n);

            foreach (Item i in itemsToRemove)
                items.Remove(i);

            foreach (Portal p in portalsToRemove)
                portals.Remove(p);

            npcsToRemove.Clear();
            itemsToRemove.Clear();
            portalsToRemove.Clear();
        }

        #endregion

    }//end class
}//end namespace
