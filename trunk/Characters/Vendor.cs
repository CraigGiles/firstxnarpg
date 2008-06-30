using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public class Vendor
    {

        #region Constructor(s)


        /// <summary>
        /// Determines if the NPC has items to sell
        /// </summary>
        public static bool Active
        {
            get { return active; }
            set { active = value; }
        }
        static bool active = false;


        public Vendor()
        {
            active = true;
        }


        #endregion


        /// <summary>
        /// Percent of items value paid to character
        /// selling item.
        /// </summary>
        public float BuyBackPercent
        {
            get { return buyBackPercent; }
            set { buyBackPercent = value; }
        }
        float buyBackPercent;
        

        #region Buy / Sell Gear

        /// <summary>
        /// All available items for sale by merchant
        /// </summary>
        public List<Gear> GearForSale
        {
            get { return gearForSale; }
        }
        List<Gear> gearForSale = new List<Gear>();


        /// <summary>
        /// Buys equipment from vendor
        /// </summary>
        /// <param name="character"></param>
        /// <param name="index"></param>
        public void BuyGear(Character character, int index)
        {
            switch (gearForSale[index].GetEquipmentType)
            {

                case EquipmentType.Accessory:
                    character.Inventory.AddItem(new Accessory(gearForSale[index].AssetName));
                    break;

                case EquipmentType.Armor:
                    character.Inventory.AddItem(new Armor(gearForSale[index].AssetName));
                    break;

                case EquipmentType.Weapon:
                    character.Inventory.AddItem(new Weapon(gearForSale[index].AssetName));
                    break;

            }

            character.Inventory.Gold -= gearForSale[index].GoldValue;
        }


        /// <summary>
        /// sell gear to vendor, return gold value
        /// </summary>
        /// <param name="character">Character selling gear</param>
        /// <param name="gear">Gear to sell</param>
        /// <returns>int</returns>
        public int SellGear(Character character, Gear gear)
        {
            int gold = 0;

            foreach (Item item in character.Inventory.Items)
            {
                if (item.AssetName == gear.AssetName)
                    character.Inventory.RemoveItem(item);

                gold = (int)(item.GoldValue * buyBackPercent);
            }

            return gold;
        }

        #endregion


        #region Buy / Sell Items

        /// <summary>
        /// All available items for sale by merchant
        /// </summary>
        public List<Item> ItemsForSale
        {
            get { return itemsForSale; }
        }
        List<Item> itemsForSale = new List<Item>();


        /// <summary>
        /// Purchases an item from vendor
        /// </summary>
        /// <param name="character"></param>
        /// <param name="index"></param>
        public void BuyItem(Character character, int index)
        {
            character.Inventory.AddItem(new Item(itemsForSale[index].AssetName));
            character.Inventory.Gold -= itemsForSale[index].GoldValue;
        }


        /// <summary>
        /// Sells an item to vendor, returns gold value
        /// </summary>
        /// <param name="character">Character selling item</param>
        /// <param name="itemToSell">Item being sold</param>
        /// <returns>int</returns>
        public int SellItem(Character character, Item itemToSell)
        {
            int gold = 0;

            foreach (Item item in character.Inventory.Items)
            {
                if (item.AssetName == itemToSell.AssetName)
                    character.Inventory.RemoveItem(item);

                gold = (int)(item.GoldValue * buyBackPercent);
            }

            return gold;
            
        }

        #endregion


    }
}
