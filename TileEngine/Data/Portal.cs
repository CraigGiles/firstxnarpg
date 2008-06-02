using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class Portal
    {
        /// <summary>
        /// Portal Enterance Location Point
        /// </summary>
        public Point PortalEnteranceTile
        {
            get { return portalEnteranceTile; }
            set { portalEnteranceTile = value; }
        }
        Point portalEnteranceTile;


        /// <summary>
        /// Portal enterance Vector2 location
        /// </summary>
        public Vector2 PortalEnteranceVector
        {
            get { return portalEnteranceVector; }
            set { portalEnteranceVector = value; }
        }
        Vector2 portalEnteranceVector;



        /// <summary>
        /// Destinations Map Name
        /// </summary>
        public string DestinationMap
        {
            get { return destinationMap; }
            set { destinationMap = value; }
        }
        string destinationMap;

        /// <summary>
        /// Spawn location on the destination map
        /// </summary>
        public Point DestinationSpawn
        {
            get { return destinationSpawn; }
            set { destinationSpawn = value; }
        }
        Point destinationSpawn;
    }
}
