using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public class Enum
    {
        public enum Direction
        {
            South,
            SouthWest,
            West,
            NorthWest,
            North,
            NorthEast,
            East,
            SouthEast,
        }

        public enum Tileset
        {
            Field,
        }

        public enum MapLayer
        {
            Base,
            Fringe,
            Object,
            Collision,
        }
    }
}
