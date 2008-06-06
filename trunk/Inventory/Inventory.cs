using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public class Inventory
    {


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
        /// Removes an item from characters inventory
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }
    }
}
