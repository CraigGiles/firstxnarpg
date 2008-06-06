using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace ActionRPG
{
    public class Character
    {

        #region Asset Name


        /// <summary>
        /// Asset name of character
        /// </summary>
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        string assetName;


        #endregion


        #region Stats


        /// <summary>
        /// Base statistics that make up the character
        /// </summary>
        public Stats BaseStats
        {
            get { return baseStats; }
            set { baseStats = value; }
        }
        Stats baseStats = new Stats();

        
        /// <summary>
        /// Characters current health
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = (int)MathHelper.Clamp(value, 0, BaseStats.MaxHealth); }
        }
        int health;


        /// <summary>
        /// Movement Speed Modifier
        /// </summary>
        public float MovementSpeedModifier
        {
            get { return movementSpeedModifier; }
            set { movementSpeedModifier = value; }
        }
        float movementSpeedModifier = 1.0f;


        /// <summary>
        /// Returns characters current attack power
        /// </summary>
        public int AttackPower
        {
            get
            {
                return baseStats.Strength + equipment.AddedStrength;
            }
        }


        /// <summary>
        /// Amount of gold character currently has
        /// </summary>
        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }
        int gold;


        #endregion


        #region Equipment / Inventory


        /// <summary>
        /// Characters current equipment
        /// </summary>
        public Equipment Equipment
        {
            get { return equipment; }
        }
        Equipment equipment = new Equipment();


        /// <summary>
        /// Characters current inventory
        /// </summary>
        public Inventory Inventory
        {
            get { return inventory; }
        }
        Inventory inventory = new Inventory();


        #endregion


        #region Position Information

        /// <summary>
        /// Current position of the character
        /// </summary>
        public Vector2 CurrentPosition
        {
            get { return currentPosition; }
            set { currentPosition = value; }
        }
        /// <summary>
        /// Center of sprite
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                return new Vector2(
                    currentPosition.X + (spriteSheet.Width / 2),
                    currentPosition.Y + (spriteSheet.Height / 2));
            }

            set
            {
                currentPosition.X = value.X - (spriteSheet.Width / 2);
                currentPosition.Y = value.Y - (spriteSheet.Height / 2);
            }

        }
        Vector2 currentPosition;


        /// <summary>
        /// Current velocity
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;


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
        /// Radius of the sprite
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        float radius;


        #endregion


        #region Sprite Sheet

        /// <summary>
        /// Sprite sheet of the character
        /// </summary>
        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }
        Texture2D spriteSheet;

        #endregion


        #region Load character


        /// <summary>
        /// Initializes character stats and settings from the
        /// characters xml file
        /// </summary>
        /// <param name="asset">Characters asset name</param>
        public void Initialize(string asset)
        {
            Load(asset);
        }

        private void Load(string asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content\Characters\" + asset + ".xml");

            foreach (XmlNode rootElement in doc.ChildNodes)
            {
                foreach (XmlNode node in rootElement.ChildNodes)
                {
                    if (node.Name == "Asset")
                        SetAsset(node);

                    if (node.Name == "Stats")
                        SetStats(node);

                    if (node.Name == "Inventory")
                        SetInventory(node);

                    if (node.Name == "Equipment")
                        SetEquipment(node);

                    if (node.Name == "Animations")
                        SetAnimations(node);
                }
            }

            LoadCharacterSprites();
        }

        private void LoadCharacterSprites()
        {
            SpriteSheet = Globals.Content.Load<Texture2D>(@"Graphics/Characters/" + AssetName);
        }

        private void SetAsset(XmlNode node)
        {
            AssetName = node.InnerText;
        }

        private void SetStats(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Health")
                    BaseStats.BaseHealth = int.Parse(n.InnerText);

                else if (n.Name == "Mana")
                    BaseStats.BaseMana = int.Parse(n.InnerText);

                else if (n.Name == "Strength")
                    BaseStats.Strength = int.Parse(n.InnerText);

                else if (n.Name == "Stamina")
                    BaseStats.Stamina = int.Parse(n.InnerText);

                else if (n.Name == "Agility")
                    BaseStats.Agility = int.Parse(n.InnerText);

                else if (n.Name == "Dexterity")
                    BaseStats.Dexterity = int.Parse(n.InnerText);

                else if (n.Name == "Intelligence")
                    BaseStats.Intelligence = int.Parse(n.InnerText);

                else if (n.Name == "Wisdom")
                    BaseStats.Wisdom = int.Parse(n.InnerText);
            }
        }

        private void SetInventory(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Gold")
                    Gold = int.Parse(n.InnerText);

                else if (n.Name == "Item")
                    Inventory.AddItem(new Item(n.InnerText));
            }
        }

        private void SetEquipment(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Weapon")
                    Equipment.Equip(new Weapon(n.InnerText));

                if (n.Name == "Armor")
                    Equipment.Equip(new Armor(n.InnerText));

                if (n.Name == "Accessory")
                    Equipment.Equip(new Accessory(n.InnerText));
            }
        }

        private void SetAnimations(XmlNode node)
        {
        }


        #endregion


        #region Update

        public virtual void Update()
        {
        }

        #endregion


        #region Draw


        public virtual void Draw()
        {
            Globals.Batch.Draw(SpriteSheet, CurrentPosition, Color.White);
        }


        #endregion


    }//end class
}//end namespace
