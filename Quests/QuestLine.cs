using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using System.Xml;

namespace ActionRPG
{
    /// <summary>
    /// A line of quests, presented to the player in order.
    /// </summary>
    /// <remarks>
    /// In other words, only one quest is presented at a time and 
    /// must be competed before the line can continue.
    /// </remarks>
    public class QuestLine
    {
        /// <summary>
        /// The name of the quest line.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string name;


        /// <summary>
        /// An ordered list of content names of quests that will be presented in order.
        /// </summary>
        public List<string> QuestContentNames
        {
            get { return questContentNames; }
            set { questContentNames = value; }
        }
        List<string> questContentNames = new List<string>();


        /// <summary>
        /// An ordered list of quests that will be presented in order.
        /// </summary>
        public List<Quest> Quests
        {
            get { return quests; }
        }
        List<Quest> quests = new List<Quest>();


    }
}
