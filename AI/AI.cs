using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class AI
    {
        public enum Mindset
        {
            Standing,
            Traveling,
            Guarding,
            Roaming,
            Patroling,
            Aggressive,
            Fleeing,
        }


        /// <summary>
        /// Current target for battle
        /// </summary>
        public Character BattleTarget
        {
            get { return battleTarget; }
            set { battleTarget = value; }
        }
        Character battleTarget;


        NPC npc;

        /// <summary>
        /// Current mindset of the character
        /// </summary>
        public Mindset CurrentMindset
        {
            get { return currentMindset; }
            set { currentMindset = value; }
        }
        Mindset currentMindset = Mindset.Standing;
        
        Mindset previousMindset;

        Vector2 targetLocation;

        List<Vector2> patrolLocations = new List<Vector2>();

        #region Constructor(s)


        public AI(NPC npc)
        {
            this.npc = npc;
        }


        #endregion


        #region Update


        public void Update(Player player)
        {
            //if aggresive and current health is < 10%, start fleeing
            if (currentMindset == Mindset.Aggressive && 
                (npc.Health / npc.BaseStats.MaxHealth) < .10)
            {
                ChangeMindset(Mindset.Fleeing);
            }
            //if fleeing and health is above 10%, stop fleeing
            else if (currentMindset == Mindset.Fleeing &&
                (npc.Health / npc.BaseStats.MaxHealth) >= .10)
            {
                ChangeMindset(previousMindset);
            }

        }


        #endregion


        #region Change Mindset


        public void ChangeMindset(Mindset newMindset)
        {
            previousMindset = currentMindset;
            currentMindset = newMindset;
        }


        #endregion

    }
}
