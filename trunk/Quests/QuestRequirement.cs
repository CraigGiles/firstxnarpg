using System;
using Microsoft.Xna.Framework.Content;
using System.Xml;


namespace ActionRPG
{
    /// <summary>
    /// A requirement for a particular number of a piece of content.
    /// </summary>
    /// <remarks>Used to track gear acquired and monsters killed.</remarks>
    public class QuestRequirement<T>
    {
        /// <summary>
        /// The quantity of the content entry that has been acquired.
        /// </summary>
        public int CompletedCount
        {
            get { return completedCount; }
            set { completedCount = value; }
        }
        int completedCount;

    }
}
