using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
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
        /// List of Gear items that can be dropped when killed
        /// </summary>
        public List<Gear> LootTable
        {
            get { return lootTable; }
            set { lootTable = value; }
        }
        List<Gear> lootTable = new List<Gear>();


        public NPC()
        {
        }


    }
}
