using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace ActionRPG
{
    public class Gear : Item
    {




        /// <summary>
        /// Returns what type of equpiment (Armor, Accessory, Weapon)
        /// </summary>
        public EquipmentType GetEquipmentType
        {
            get
            {
                if (Slot == EquipmentSlot.Accessory)
                    return EquipmentType.Accessory;

                else if (
                     Slot == EquipmentSlot.Arms ||
                     Slot == EquipmentSlot.Belt ||
                     Slot == EquipmentSlot.Boots ||
                     Slot == EquipmentSlot.Chest ||
                     Slot == EquipmentSlot.Gloves ||
                     Slot == EquipmentSlot.Helmet ||
                     Slot == EquipmentSlot.Legs ||
                     Slot == EquipmentSlot.Shield ||
                     Slot == EquipmentSlot.Shoulders)
                {
                    return EquipmentType.Armor;
                }

                else
                    return EquipmentType.Weapon;
            }
        }

        ///// <summary>
        ///// Returns what type of equpiment (Armor, Accessory, Weapon)
        ///// </summary>
        //EquipmentType Type
        //{
        //    get { return type; }
        //    set { type = value; }
        //}
        //EquipmentType type;


        /// <summary>
        /// Stat bonuses available to this item while equipped
        /// </summary>
        public StatisticsValue AddedStats
        {
            get { return addedStats; }
            set { addedStats = value; }
        }
        StatisticsValue addedStats = new StatisticsValue();



        #region Load Gear


        protected void Load(XmlDocument doc)
        {
            foreach (XmlNode root in doc.ChildNodes)
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "Asset")
                        AssetName = node.InnerText;

                    if (node.Name == "Description")
                        Description = node.InnerText;

                    if (node.Name == "GraphicIcon")
                        GraphicIcon = LoadTexture(node.InnerText);

                    if (node.Name == "Slot")
                        Slot = LoadSlot(node.InnerText);

                    if (node.Name == "Stats")
                        LoadStats(node);

                    if (node.Name == "GoldValue")
                        GoldValue = int.Parse(node.InnerText);

                    if (node.Name == "Dropable")
                        Dropable = bool.Parse(node.InnerText);

                    if (node.Name == "ItemType")
                        ItemType = GetItemType(node.InnerText);

                }//end child nodes 
            }

        }

        private ItemType GetItemType(string type)
        {
            switch (type)
            {
                case ("Item"):
                    return ItemType.Item;
                    break;

                case ("Gear"):
                    return ItemType.Gear;
                    break;

                case ("Quest"):
                    return ItemType.Quest;
                    break;
            }

            return ItemType.Item;
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
            EquipmentSlot equipmentSlot = EquipmentSlot.Helmet;

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

                case ("Accessory"):
                    equipmentSlot = EquipmentSlot.Accessory;
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
