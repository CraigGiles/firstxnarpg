using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class Item 
    {
        #region Data
        /// <summary>
        /// Get or set the objects slot used to equip
        /// this item.
        /// </summary>
        public EquipmentSlot Slot
        {
            get { return slot; }
            set { slot = value; }
        }
        EquipmentSlot slot = EquipmentSlot.None;


        /// <summary>
        /// Objects item type
        /// </summary>
        public ItemType ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }
        ItemType itemType = ItemType.Item;

        /// <summary>
        /// Objects Asset name
        /// </summary>
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        string assetName;


        /// <summary>
        /// Objects description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        string description;


        /// <summary>
        /// Objects icon graphic
        /// </summary>
        public Texture2D GraphicIcon
        {
            get { return graphicIcon; }
            set { graphicIcon = value; }
        }
        Texture2D graphicIcon;


        /// <summary>
        /// Objects value in gold
        /// </summary>
        public int GoldValue
        {
            get { return goldValue; }
            set { goldValue = value; }
        }
        int goldValue;


        /// <summary>
        /// Can the object be dropped
        /// </summary>
        public bool Dropable
        {
            get { return dropable; }
            set { dropable = value; }
        }
        bool dropable;


        /// <summary>
        /// Chance of item dropping from current
        /// NPCs loot table
        /// </summary>
        public int Percent
        {
            get { return percent; }
            set { percent = value; }
        }
        int percent;


        /// <summary>
        /// Gets the spawn location of the WorldItem in Vector form
        /// </summary>
        public Vector2 SpawnLocationVector
        {
            get { return spawnLocationVector; }
            set { spawnLocationVector = value; }
        }
        Vector2 spawnLocationVector;


        /// <summary>
        /// Gets the spawn location tile of the WorldItem
        /// </summary>
        public Point SpawnLocationTile
        {
            get { return spawnLocationTile; }
            set { spawnLocationTile = value; }
        }
        Point spawnLocationTile;


        /// <summary>
        /// Items draw spritebox
        /// </summary>
        public Rectangle SpriteBox
        {
            get { return spriteBox; }
            set { spriteBox = value; }
        }
        Rectangle spriteBox;


        public Vector2 Origin
        {
            get
            {
                return new Vector2(
                    SpawnLocationVector.X + SpriteBox.Width / 2,
                    SpawnLocationVector.Y + SpriteBox.Height / 2);
            }
        }

        #endregion


        #region Constructor(s)

        public Item()
        {
        }

        public Item(string asset)
        {
            Load(asset);
        }

        #endregion


        #region Load Item


        protected void Load(string asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content/Items/" + asset + ".xml");

            foreach (XmlNode root in doc.ChildNodes)
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "Asset")
                        assetName = node.InnerText;

                    if (node.Name == "Description")
                        description = node.InnerText;

                    if (node.Name == "GraphicIcon")
                        graphicIcon = LoadTexture(node.InnerText);

                    if (node.Name == "GoldValue")
                        goldValue = int.Parse(node.InnerText);

                    if (node.Name == "Dropable")
                        dropable = bool.Parse(node.InnerText);

                    if (node.Name == "ItemType")
                        itemType = GetItemType(node.InnerText);

                }//end child nodes 
            }

        }

        private ItemType GetItemType(string type)
        {
            switch (type)
            {
                case ("Item"):
                    return ItemType.Item;

                case ("Gear"):
                    return ItemType.Gear;

                case ("Quest"):
                    return ItemType.Quest;

            }

            return ItemType.Item;
        }




        private Texture2D LoadTexture(string asset)
        {
            return Globals.Content.Load<Texture2D>("Graphics/Items/" + asset);
        }


        #endregion


        #region ICloneable Members


        #endregion
    }
}
