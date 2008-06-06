using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public class Equipment
    {

        #region Constructor(s)

        public Equipment()
        {
        }

        #endregion
        

        #region Unequipped items


        /// <summary>
        /// List of all unequipped armor in inventory
        /// </summary>
        public List<Armor> UnEquippedArmor
        {
            get { return unEquippedArmor; }
            set { unEquippedArmor = value; }
        }
        List<Armor> unEquippedArmor = new List<Armor>();


        /// <summary>
        /// List of all unequipped weapons in inventory
        /// </summary>
        public List<Weapon> UnEquippedWeapons
        {
            get { return unEquippedWeapons; }
            set { unEquippedWeapons = value; }
        }
        List<Weapon> unEquippedWeapons = new List<Weapon>();


        /// <summary>
        /// List of all unequipped accessories in inventory
        /// </summary>
        public List<Accessory> UnEquippedAccessories
        {
            get { return unEquippedAccessories; }
            set { unEquippedAccessories = value; }
        }
        List<Accessory> unEquippedAccessories = new List<Accessory>();


        #endregion


        #region Added stats from equipment


        /// <summary>
        /// Calculates the added health from all equipped items
        /// </summary>
        public int AddedHealth
        {
            get
            {
                int added = 0;
                foreach (Gear gear in equippedItems)
                {
                    added += gear.AddedStats.Health;
                }

                return added;
            }
        }


        /// <summary>
        /// Calculates the added mana from all equipped items
        /// </summary>
        public int AddedMana
        {
            get
            {
                int added = 0;
                foreach (Gear gear in equippedItems)
                {
                    added += gear.AddedStats.Mana;
                }
                return added;
            }
        }


        /// <summary>
        /// Calculates the added str from all equipped items
        /// </summary>
        public int AddedStrength
        {
            get
            {
                int added = 0;
                foreach (Gear gear in equippedItems)
                {
                    added += gear.AddedStats.Strength;
                }
                return added;
            }
        }


        /// <summary>
        /// Calculates the added agi from all equipped items
        /// </summary>
        public int AddedAgility
        {
            get
            {
                int added = 0;
                foreach (Gear gear in equippedItems)
                {
                    added += gear.AddedStats.Agility;
                }
                return added;
            }
        }


        /// <summary>
        /// Calculates the added dex from all equipped items
        /// </summary>
        public int AddedDexterity
        {
            get
            {
                int added = 0;
                foreach (Gear gear in equippedItems)
                {
                    added += gear.AddedStats.Dexterity;
                }
                return added;
            }
        }


        /// <summary>
        /// Calculates the added wis from all equipped items
        /// </summary>
        public int AddedWisdom
        {
            get
            {
                int added = 0;
                foreach (Gear gear in equippedItems)
                {
                    added += gear.AddedStats.Wisdom;
                }
                return added;
            }
        }


        /// <summary>
        /// Calculates the added int from all equipped items
        /// </summary>
        public int AddedIntelligence
        {
            get
            {
                int added = 0;
                foreach (Gear gear in equippedItems)
                {
                    added += gear.AddedStats.Intelligence;
                }
                return added;
            }
        }


        /// <summary>
        /// Calculates the added haste effects from all equipped items
        /// </summary>
        public float AddedHaste
        {
            get
            {
                float added = 0;
                foreach (Gear gear in equippedItems)
                {
                    added += gear.AddedStats.Haste;
                }
                return added;
            }
        }



        #endregion


        #region Equipped Items


        /// <summary>
        /// Characters helmet equipment
        /// </summary>
        public Armor Helmet
        {
            get { return helmet; }
            //set { helmet = value; }
        }
        Armor helmet;


        /// <summary>
        /// Characters shoulder equipment
        /// </summary>
        public Armor Shoulder
        {
            get { return shoulder; }
            //set { shoulder = value; }
        }
        Armor shoulder;


        /// <summary>
        /// Characters armplates
        /// </summary>
        public Armor Arm
        {
            get { return arm; }
            //set { arm = value; }
        }
        Armor arm;


        /// <summary>
        /// Characters gloves
        /// </summary>
        public Armor Gloves
        {
            get { return gloves; }
            //set { gloves = value; }
        }
        Armor gloves;


        /// <summary>
        /// Characters breastplate
        /// </summary>
        public Armor Chest
        {
            get { return chest; }
            //set { chest = value; }
        }
        Armor chest;


        /// <summary>
        /// Characters belt
        /// </summary>
        public Armor Belt
        {
            get { return belt; }
            //set { belt = value; }
        }
        Armor belt;


        /// <summary>
        /// Characters legplates
        /// </summary>
        public Armor Legs
        {
            get { return legs; }
            //set { legs = value; }
        }
        Armor legs;


        /// <summary>
        /// Characters boots
        /// </summary>
        public Armor Boots
        {
            get { return boots; }
            //set { boots = value; }
        }
        Armor boots;


        /// <summary>
        /// Characters shield
        /// </summary>
        public Armor Shield
        {
            get { return shield; }
            //set { shield = value; }
        }
        Armor shield;


        /// <summary>
        /// Characters weapon
        /// </summary>
        public Weapon Weapon
        {
            get { return weapon; }
            //set { weapon = value; }
        }
        Weapon weapon;


        /// <summary>
        /// Characters accessory slot
        /// </summary>
        public Accessory Accessory
        {
            get { return accessory; }
            //set { accessory = value; }
        }
        Accessory accessory;


        #endregion


        #region Equip / Unequip methods



        /// <summary>
        /// List of equipped items for use when gathering stats
        /// </summary>
        List<Gear> equippedItems = new List<Gear>();


        /// <summary>
        /// Unequips an item in the selected slot, and places
        /// that item in the appropriate unequipped list
        /// </summary>
        /// <param name="slot">Slot to unequip</param>
        public void Unequip(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Helmet:
                    if (Helmet != null)
                        unEquippedArmor.Add(Helmet);
                    helmet = null;
                    break;

                case EquipmentSlot.Shoulders:
                    if (Shoulder != null)
                        unEquippedArmor.Add(Shoulder);
                    shoulder = null;
                    break;

                case EquipmentSlot.Arms:
                    if (Arm != null)
                        unEquippedArmor.Add(Arm);
                    arm = null;
                    break;

                case EquipmentSlot.Gloves:
                    if (Gloves != null)
                        unEquippedArmor.Add(Gloves);
                    gloves = null;
                    break;

                case EquipmentSlot.Chest:
                    if (Chest != null)
                        unEquippedArmor.Add(Chest);
                    chest = null;
                    break;

                case EquipmentSlot.Belt:
                    if (Belt != null)
                        unEquippedArmor.Add(Belt);
                    belt = null;
                    break;

                case EquipmentSlot.Legs:
                    if (Legs != null)
                        unEquippedArmor.Add(Legs);
                    legs = null;
                    break;

                case EquipmentSlot.Boots:
                    if (Boots != null)
                        unEquippedArmor.Add(Boots);
                    boots = null;
                    break;

                case EquipmentSlot.Weapon:
                    if (Weapon != null)
                        unEquippedWeapons.Add(Weapon);
                    weapon = null;
                    break;

                case EquipmentSlot.Shield:
                    if (Shield != null)
                        unEquippedArmor.Add(Shield);
                    shield = null;
                    break;
            }
        }


        /// <summary>
        /// Equips armor in selected slot
        /// </summary>
        /// <param name="armor">Armor to equip</param>
        public void Equip(Armor armor)
        {
            Unequip(armor.Slot);

            switch (armor.Slot)
            {
                case EquipmentSlot.Helmet:
                    helmet = armor;
                    break;

                case EquipmentSlot.Shoulders:
                    shoulder = armor;
                    break;

                case EquipmentSlot.Arms:
                    arm = armor;
                    break;

                case EquipmentSlot.Gloves:
                    gloves = armor;
                    break;

                case EquipmentSlot.Chest:
                    chest = armor;
                    break;

                case EquipmentSlot.Belt:
                    belt = armor;
                    break;

                case EquipmentSlot.Legs:
                    legs = armor;
                    break;

                case EquipmentSlot.Boots:
                    boots = armor;
                    break;

                case EquipmentSlot.Shield:
                    shield = armor;
                    break;
            }

            equippedItems.Add(armor);
        }


        /// <summary>
        /// Equips a weapon
        /// </summary>
        /// <param name="weapon">Weapon to equip</param>
        public void Equip(Weapon weapon)
        {
            Unequip(weapon.Slot);

            this.weapon = weapon;

            equippedItems.Add(weapon);
        }


        /// <summary>
        /// Equips an accessory
        /// </summary>
        /// <param name="accessory">Accessory to equip</param>
        public void Equip(Accessory accessory)
        {
            Unequip(accessory.Slot);

            this.accessory = accessory;

            equippedItems.Add(accessory);
        }


        #endregion


    }//end class
}//end namespace
