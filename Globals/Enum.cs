using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{

    public enum CharacterState
    {
        Alive,
        Dying,
        Dead,
    }
        

    public enum Actions
    {
        Attack,
        CastSpell,
        Defend,
        UseItem,
        Traveling, 
    }


    /// <summary>
    /// All possible equippable equipment slots
    /// </summary>
    public enum EquipmentSlot
    {
        Weapon = 1,
        Helmet = 2,
        Shoulders = 3,
        Chest = 4,
        Gloves = 5,
        Belt = 6,
        Arms = 7,
        Accessory = 8,
        Shield = 9,
        Legs = 10,
        Boots = 11, 
        None = 12,
    }

    public enum ItemType
    {
        Gear,
        Item,
        Quest,
    }

    public enum EquipmentType
    {
        Armor,
        Weapon,
        Accessory,
    }


    /// <summary>
    /// Damage types used when damaging in game characters
    /// </summary>
    public enum DamageType
    {
        Physical,
        Magical,
        Environmental,
    }





    /// <summary>
    /// Travel directions
    /// </summary>
    public enum Direction : int
    {
        Down = 180,
        DownLeft = 225,
        Left = 270,
        UpLeft = 315,
        Up = 0,
        UpRight = 45,
        Right = 90,
        DownRight = 135,
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
