using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public class StatisticsValue
    {

        /// <summary>
        /// Health bonus
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = Math.Max(value, 0); }
        }
        int health;


        /// <summary>
        /// Mana bonus
        /// </summary>
        public int Mana
        {
            get { return mana; }
            set { mana = Math.Max(value, 0); }
        }
        int mana;


        /// <summary>
        /// Strength bonus
        /// </summary>
        public int Strength
        {
            get { return strength; }
            set { strength = Math.Max(value, 0); }
        }
        int strength;


        /// <summary>
        /// Stamina bonus
        /// </summary>
        public int Stamina
        {
            get { return stamina; }
            set { stamina = Math.Max(value, 0); }
        }
        int stamina;


        /// <summary>
        /// Agility bonus
        /// </summary>
        public int Agility
        {
            get { return agility; }
            set { agility = Math.Max(value, 0); }
        }
        int agility;


        /// <summary>
        /// Dexterity bonus
        /// </summary>
        public int Dexterity
        {
            get { return dexterity; }
            set { dexterity = Math.Max(value, 0); }
        }
        int dexterity;


        /// <summary>
        /// Wisdom bonus
        /// </summary>
        public int Wisdom
        {
            get { return wisdom; }
            set { wisdom = Math.Max(value, 0); }
        }
        int wisdom;


        /// <summary>
        /// Intelligence bonus
        /// </summary>
        public int Intelligence
        {
            get { return intelligence; }
            set { intelligence = Math.Max(value, 0); }
        }
        int intelligence;


        /// <summary>
        /// Haste bonus
        /// </summary>
        public float Haste
        {
            get { return haste; }
            set { haste = Math.Max(value, 0); }
        }
        float haste;

    }
}
