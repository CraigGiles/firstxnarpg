using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;

namespace ActionRPG
{
    public class Inventory
    {
        #region Eqiupment

        /// <summary>
        /// Characters currently equipped items
        /// </summary>
        public Equipment Equipment
        {
            get { return equipment; }
        }
        Equipment equipment = new Equipment();


        /// <summary>
        /// Unequip an item from the specified slot
        /// </summary>
        /// <param name="slot">Slot to unequip item</param>
        public void Unequip(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Helmet:
                    AddItem(equipment.Helmet);
                    equipment.Helmet = null;
                    break;

                case EquipmentSlot.Shoulders:
                    AddItem(equipment.Shoulders);
                    equipment.Shoulders = null;
                    break;

                case EquipmentSlot.Arms:
                    AddItem(equipment.Arms);
                    equipment.Arms = null;
                    break;

                case EquipmentSlot.Gloves:
                    AddItem(equipment.Gloves);
                    equipment.Gloves = null;
                    break;

                case EquipmentSlot.Chest:
                    AddItem(equipment.Chest);
                    equipment.Chest = null;
                    break;

                case EquipmentSlot.Belt:
                    AddItem(equipment.Belt);
                    equipment.Belt = null;
                    break;

                case EquipmentSlot.Legs:
                    AddItem(equipment.Legs);
                    equipment.Legs = null;
                    break;

                case EquipmentSlot.Boots:
                    AddItem(equipment.Boots);
                    equipment.Boots = null;
                    break;

                case EquipmentSlot.Weapon:
                    AddItem(equipment.Weapon);
                    equipment.Weapon = null;
                    break;

                case EquipmentSlot.Shield:
                    AddItem(equipment.Shield);
                    equipment.Shield = null;
                    break;

                case EquipmentSlot.Accessory:
                    AddItem(equipment.Accessory);
                    equipment.Accessory = null;
                    break;
            }
        }


        /// <summary>
        /// Equips a weapon
        /// </summary>
        /// <param name="weapon">Weapon to equip</param>
        public void Equip(Weapon weapon)
        {
            if (equipment.IsSlotEquipped(weapon.Slot))
                Unequip(weapon.Slot);

            equipment.Weapon = weapon;
            items.Remove(weapon);

        }


        /// <summary>
        /// Equips a peice of armor
        /// </summary>
        /// <param name="armor">Armor to equip</param>
        public void Equip(Armor armor)
        {
            if (equipment.IsSlotEquipped(armor.Slot))
                Unequip(armor.Slot);

            switch (armor.Slot)
            {
                case EquipmentSlot.Helmet:
                    Equipment.Helmet = armor;
                    items.Remove(armor);
                    break;

                case EquipmentSlot.Shoulders:
                    Equipment.Shoulders = armor;
                    items.Remove(armor);
                    break;

                case EquipmentSlot.Arms:
                    Equipment.Arms = armor;
                    items.Remove(armor);
                    break;

                case EquipmentSlot.Gloves:
                    Equipment.Gloves = armor;
                    items.Remove(armor);
                    break;

                case EquipmentSlot.Chest:
                    Equipment.Chest = armor;
                    items.Remove(armor);
                    break;

                case EquipmentSlot.Belt:
                    Equipment.Belt = armor;
                    items.Remove(armor);
                    break;

                case EquipmentSlot.Legs:
                    Equipment.Legs = armor;
                    items.Remove(armor);
                    break;

                case EquipmentSlot.Boots:
                    Equipment.Boots = armor;
                    items.Remove(armor);
                    break;

                case EquipmentSlot.Shield:
                    Equipment.Shield = armor;
                    items.Remove(armor);
                    break;

            }
        }


        /// <summary>
        /// Equips an accessory
        /// </summary>
        /// <param name="accessory">Accessory to equip</param>
        public void Equip(Accessory accessory)
        {
            if (equipment.IsSlotEquipped(accessory.Slot))
                Unequip(accessory.Slot);

            equipment.Accessory = accessory;
            items.Remove(accessory);
        }


        #endregion


        #region Inventory

        
        
        /// <summary>
        /// Items in characters inventory
        /// </summary>
        public List<Item> Items
        {
            get { return items; }
        }
        List<Item> items = new List<Item>();
                      

        /// <summary>
        /// Adds an item to characters inventory
        /// </summary>
        /// <param name="item">Item to add</param>
        public void AddItem(Item item)
        {
            items.Add(item);
        }

        /// <summary>
        /// Adds an item to characters inventory while launching a text animation
        /// </summary>
        /// <param name="item"></param>
        /// <param name="textAnimationLocation"></param>
        public void AddItem(Item item, Vector2 textAnimationLocation)
        {
            TextManager.LaunchAnimation("+" + item.AssetName,
                textAnimationLocation,
                RNG.GetRandomFloat(0.4f, 1.0f),
                Color.FloralWhite,
                1.5f);

            AddItem(item);
        }


        /// <summary>
        /// Removes an item from characters inventory
        /// </summary>
        /// <param name="item">item to remove</param>
        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }


        #endregion


        #region Gold
        /// <summary>
        /// Amount of gold character currently has
        /// </summary>
        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }
        int gold;

        /// <summary>
        /// Adds gold to the characters inventory
        /// while launching a text animation
        /// </summary>
        /// <param name="gold">Gold added</param>
        /// <param name="textAnimationLocation">Characters Origin - height</param>
        public void AddGold(int gold, Vector2 textAnimationLocation)
        {
            Gold += gold;
            TextManager.LaunchAnimation(
                "+" + gold.ToString() + " Gold!",
                textAnimationLocation + (new Vector2(0, 10f)),
                RNG.GetRandomFloat(0.4f, 1.0f),
                Color.Yellow,
                1f);
        }

        #endregion
    }
}
