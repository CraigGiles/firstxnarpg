using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class NPC : Character
    {     
        /// <summary>
        /// Will the NPC attack if you if in range
        /// </summary>
        public bool Hostile
        {
            get { return hostile; }
            set { hostile = value; }
        }
        bool hostile;


        /// <summary>
        /// List of items that can be dropped when killed
        /// </summary>
        public List<Item> DropableItems
        {
            get { return dropableItems; }
            set { dropableItems = value; }
        }
        List<Item> dropableItems = new List<Item>();

        public NPC()
        {
        }

        public void Initialize()
        {
        }

    }
}
