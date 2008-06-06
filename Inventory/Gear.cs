using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace ActionRPG
{
    public class Gear
    {

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
        /// Get or set the objects slot used to equip
        /// this item.
        /// </summary>
        public EquipmentSlot Slot
        {
            get { return slot; }
            set { slot = value; }
        }
        EquipmentSlot slot;


        /// <summary>
        /// Stat bonuses available to this item while equipped
        /// </summary>
        public StatisticsValue AddedStats
        {
            get { return addedStats; }
            set { addedStats = value; }
        }
        StatisticsValue addedStats = new StatisticsValue();


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


        #region Load Gear


        protected void Load(XmlDocument doc)
        {
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

                    if (node.Name == "Slot")
                        slot = LoadSlot(node.InnerText);

                    if (node.Name == "Stats")
                        LoadStats(node);

                    if (node.Name == "GoldValue")
                        goldValue = int.Parse(node.InnerText);

                    if (node.Name == "Dropable")
                        dropable = bool.Parse(node.InnerText);
                }//end child nodes 
            }

        }


        private void LoadStats(XmlNode node)
        {
            int hp= 0;
            int mp= 0;
            int str= 0;
            int sta= 0;
            int agi= 0;
            int dex= 0;
            int wis= 0;
            int intel= 0;
            float haste= 0;

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Health")
                    int.TryParse(n.InnerText, out hp);

                if (n.Name == "Mana")
                    int.TryParse(n.InnerText, out mp);

                if (n.Name == "Strength")
                    int.TryParse(n.InnerText, out str);

                if (n.Name == "Stamina")
                    int.TryParse(n.InnerText, out sta);

                if (n.Name == "Agility")
                    int.TryParse(n.InnerText, out agi);

                if (n.Name == "Dexterity")
                    int.TryParse(n.InnerText, out dex);

                if (n.Name == "Wisdom")
                    int.TryParse(n.InnerText, out wis);

                if (n.Name == "Intelligence")
                    int.TryParse(n.InnerText, out intel);

                if (n.Name == "Haste")
                    float.TryParse(n.InnerText, out haste);
            }

            addedStats.Health = hp;
            addedStats.Mana = mp;
            addedStats.Strength = str;
            addedStats.Stamina = sta;
            addedStats.Agility = agi;
            addedStats.Dexterity = dex;
            addedStats.Wisdom = wis;
            addedStats.Intelligence = intel;
            addedStats.Haste = haste;
        }


        private EquipmentSlot LoadSlot(string slot)
        {
            EquipmentSlot equipmentSlot = EquipmentSlot.Inventory;

            switch (slot)
            {
                case ("Helmet"):
                    equipmentSlot = EquipmentSlot.Helmet;
                    break;

                case ("Shoulders"):
                    equipmentSlot = EquipmentSlot.Shoulders;
                    break;

                case ("Arms"):
                    equipmentSlot = EquipmentSlot.Arms;
                    break;

                case ("Gloves"):
                    equipmentSlot = EquipmentSlot.Gloves;
                    break;

                case ("Chest"):
                    equipmentSlot = EquipmentSlot.Chest;
                    break;

                case ("Belt"):
                    equipmentSlot = EquipmentSlot.Belt;
                    break;

                case ("Legs"):
                    equipmentSlot = EquipmentSlot.Legs;
                    break;

                case ("Boots"):
                    equipmentSlot = EquipmentSlot.Boots;
                    break;

                case ("Weapon"):
                    equipmentSlot = EquipmentSlot.Weapon;
                    break;

                case ("Shield"):
                    equipmentSlot = EquipmentSlot.Shield;
                    break;

                case ("Inventory"):
                    equipmentSlot = EquipmentSlot.Inventory;
                    break;

            }

            return equipmentSlot;
        }


        private Texture2D LoadTexture(string asset)
        {
            return Globals.Content.Load<Texture2D>("Graphics/Gear/"+ asset);
        }

        #endregion


    }//end class
}//end namespace
