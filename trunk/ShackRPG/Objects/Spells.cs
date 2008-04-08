using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ShackRPG
{
    public class Spells : BaseObjects
    {
        int _ManaCost;
        int _SpellPower;

        SpellType _SpellType;

        public enum SpellType
        {
            Healing,
            Protective,
            Damage,
            Debuff,
        }

        public int ManaCost
        {
            get { return _ManaCost; }
            //set { _ManaCost = Math.Max(value, 0); }
        }

        public SpellType Type
        {
            get { return _SpellType; }
            set { _SpellType = value; }
        }

        public Spells(
            int id, 
            Texture2D texture, 
            string name,
            string description, 
            int manaCost, 
            SpellType spellType,
            int spellPower)
        {
            _Id = id;
            _Texture = texture;

            _Name = name;

            _ManaCost = manaCost;
            _SpellType = spellType;
            _SpellPower = spellPower;
        }
       
    }
}
