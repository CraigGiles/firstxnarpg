using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{

    /// <summary>
    /// All possible equippable equipment slots
    /// </summary>
    public enum EquipmentSlot
    {
        Helmet,
        Shoulders,
        Arms,
        Gloves,
        Chest,
        Belt,
        Legs,
        Boots,
        Weapon,
        Shield,
        Inventory,
    }


    /// <summary>
    /// Travel directions
    /// </summary>
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


    /// <summary>
    /// Game Tilesets
    /// </summary>
    public enum Tileset
    {
        Field,
        Town,
    }


    /// <summary>
    /// Layers of each map
    /// </summary>
    public enum MapLayer
    {
        Base,
        Fringe,
        Object,
        Collision,
    }

}
