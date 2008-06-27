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
        /// Calculates the added sta from all equipped items
        /// </summary>
        public int AddedStamina
        {
            get
            {
                int added = 0;
                foreach (Gear gear in equippedItems)
                {
                    added += gear.AddedStats.Stamina;
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


        /// <summary>
        /// Calculates the added defense from all equipped items
        /// </summary>
        public int Defense
        {
            get
            {
                int defense = 0;

                foreach (Gear gear in equippedItems)
                {
                    if (gear.Slot != EquipmentSlot.Weapon &&
                        gear.Slot != EquipmentSlot.Accessory)
                    {
                        Armor temp = gear as Armor;
                        defense += temp.Defense;
                    }
       
                }
                
                return defense;
            }
        }


        #endregion

        
        #region Equipped Items


        /// <summary>
        /// List of equipped items for use when gathering stats
        /// </summary>
        public List<Gear> EquippedItems
        {
            get { return equippedItems; }            
        }
        List<Gear> equippedItems = new List<Gear>();

                
        /// <summary>
        /// Characters helmet equipment
        /// </summary>
        public Armor Helmet
        {
            get { return helmet; }
            set 
            {
                if (value == null)
                    equippedItems.Remove(helmet);
                else
                    equippedItems.Add(value);

                helmet = value; 
            }
        }
        Armor helmet;


        /// <summary>
        /// Characters shoulder equipment
        /// </summary>
        public Armor Shoulders
        {
            get { return shoulders; }
            set
            {
                if (value == null)
                    equippedItems.Remove(shoulders);
                else
                    equippedItems.Add(value);

                shoulders = value;
            }
        }
        Armor shoulders;


        /// <summary>
        /// Characters armplates
        /// </summary>
        public Armor Arms
        {
            get { return arms; }
            set
            {
                if (value == null)
                    equippedItems.Remove(arms);
                else
                    equippedItems.Add(value);

                arms = value;
            }
        }
        Armor arms;


        /// <summary>
        /// Characters gloves
        /// </summary>
        public Armor Gloves
        {
            get { return gloves; }
            set
            {
                if (value == null)
                    equippedItems.Remove(gloves);
                else
                    equippedItems.Add(value);

                gloves = value;
            }
        }
        Armor gloves;


        /// <summary>
        /// Characters breastplate
        /// </summary>
        public Armor Chest
        {
            get { return chest; }
            set
            {
                if (value == null)
                    equippedItems.Remove(chest);
                else
                    equippedItems.Add(value);

                chest = value;
            }
        }
        Armor chest;


        /// <summary>
        /// Characters belt
        /// </summary>
        public Armor Belt
        {
            get { return belt; }
            set
            {
                if (value == null)
                    equippedItems.Remove(belt);
                else
                    equippedItems.Add(value);

                belt = value;
            }
        }
        Armor belt;


        /// <summary>
        /// Characters legplates
        /// </summary>
        public Armor Legs
        {
            get { return legs; }
            set
            {
                if (value == null)
                    equippedItems.Remove(legs);
                else
                    equippedItems.Add(value);

                legs = value;
            }
        }
        Armor legs;


        /// <summary>
        /// Characters boots
        /// </summary>
        public Armor Boots
        {
            get { return boots; }
            set
            {
                if (value == null)
                    equippedItems.Remove(boots);
                else
                    equippedItems.Add(value);

                boots = value;
            }
        }
        Armor boots;


        /// <summary>
        /// Characters shield
        /// </summary>
        public Armor Shield
        {
            get { return shield; }
            set
            {
                if (value == null)
                    equippedItems.Remove(shield);
                else
                    equippedItems.Add(value);

                shield = value;
            }
        }
        Armor shield;


        /// <summary>
        /// Characters weapon
        /// </summary>
        public Weapon Weapon
        {
            get { return weapon; }
            set
            {
                if (value == null)
                    equippedItems.Remove(weapon);
                else
                    equippedItems.Add(value);

                weapon = value;
            }
        }
        Weapon weapon;


        /// <summary>
        /// Characters accessory slot
        /// </summary>
        public Accessory Accessory
        {
            get { return accessory; }
            set 
            {
                if (value == null)
                    equippedItems.Remove(accessory);
                else
                    equippedItems.Add(value);

                accessory = value; 
            }
        }
        Accessory accessory;
        

        /// <summary>
        /// Returns true if an item is currently equipped
        /// in the equipment slot
        /// </summary>
        /// <param name="slot">Slot to check for equipment</param>
        /// <returns>bool</returns>
        public bool IsSlotEquipped(EquipmentSlot slot)
        {
            foreach (Gear item in equippedItems)
            {
                if (item.Slot == slot)
                    return true;
            }

            return false;
        }


        public Gear GetEquipmentBySlot(EquipmentSlot slot)
        {
            foreach (Gear item in equippedItems)
            {
                if (item.Slot == slot)
                    return item;
            }

            return null;
        }

        #endregion


    }//end class
}//end namespace
