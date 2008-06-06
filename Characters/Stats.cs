using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class Stats
    {

        #region Stats Cap

        /// <summary>
        /// The available cap for all stats
        /// </summary>
        public int StatCap
        {
            get { return cap; }
            set { cap = value; }
        }
        int cap = Globals.StatCap;

        #endregion


        #region Strength Stats

        /// <summary>
        /// Characters strength and power
        /// </summary>
        public int Strength
        {
            get { return strength; }
            set { strength = (int)MathHelper.Clamp(value, 0, cap); }
        }
        int strength;

        #endregion


        #region Stamina Stats

        /// <summary>
        /// Characters stamina and physical health
        /// </summary>
        public int Stamina
        {
            get { return stamina; }
            set { stamina = (int)MathHelper.Clamp(value, 0, cap); }
        }
        int stamina;

        #endregion


        #region Agility Stats

        /// <summary>
        /// Characters agility
        /// </summary>
        public int Agility
        {
            get { return agility; }
            set { agility = (int)MathHelper.Clamp(value, 0, cap); }
        }
        int agility;

        #endregion


        #region Dexterity Stats

        /// <summary>
        /// Characters dexterity and accuracy 
        /// </summary>
        public int Dexterity
        {
            get { return dexterity; }
            set { dexterity = (int)MathHelper.Clamp(value, 0, cap); }
        }
        int dexterity;

        #endregion


        #region Wisdom Stats

        /// <summary>
        /// Characters wisdom and magical accuracy
        /// </summary>
        public int Wisdom
        {
            get { return wisdom; }
            set { wisdom = (int)MathHelper.Clamp(value, 0, cap); }
        }
        int wisdom;

        #endregion


        #region Intelligence Stats
        
        /// <summary>
        /// Characters intelligence and magical power
        /// </summary>
        public int Intelligence
        {
            get { return intelligence; }
            set { intelligence = (int)MathHelper.Clamp(value, 0, cap); }
        }
        int intelligence;


        #endregion

        
        #region Health Stats

        /// <summary>
        /// Characters base health
        /// </summary>
        public int BaseHealth
        {
            get { return baseHealth; }
            set { baseHealth = value; } //change this formula
        }
        int baseHealth;
        
        /// <summary>
        /// Characters max health
        /// </summary>
        public int MaxHealth
        {
            get { return baseHealth + (stamina * 3); }
        }

        /// <summary>
        /// Characters health regen per tick
        /// </summary>
        public int HealthRegenPerTick
        {
            get { return stamina / 10; }
        }
        
        #endregion 


        #region Mana Stats
        

        /// <summary>
        /// Characters base mana
        /// </summary>
        public int BaseMana
        {
            get { return baseMana; }
            set { baseMana = value; } //change this formula
        }
        int baseMana;

        /// <summary>
        /// Characters max mana
        /// </summary>
        public int MaxMana
        {
            get { return baseMana + (intelligence * 2); }
        }

        /// <summary>
        /// Characters mana regen per tick
        /// </summary>
        public int ManaRegenPerTick
        {
            get { return wisdom / 10; }
        }

        #endregion
        
    }//end Stats
}//end Namespace
