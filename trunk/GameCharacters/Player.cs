using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public class Player : Character
    {

        /// <summary>
        /// Gets or sets the name of the player
        /// </summary>
        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
        string playerName;


    }
}
