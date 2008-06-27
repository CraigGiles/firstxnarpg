using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class Timers
    {

        #region Attack Timer


        /// <summary>
        /// Attack Delay Timer
        /// </summary>
        float attackTimer;

        /// <summary>
        /// Can the character attack
        /// </summary>
        public bool CanAttack
        {
            get { return canAttack; }
            set { canAttack = value; }
        }
        bool canAttack = true;


        /// <summary>
        /// Sets all timers associated with attacking
        /// </summary>
        /// <param name="attackDelay"></param>
        public void Attack(float attackDelay)
        {
            canAttack = false;
            attackTimer = MathHelper.Clamp(attackDelay, 0, 50f);
        }


        /// <summary>
        /// Updates attack timers
        /// </summary>
        private void UpdateAttack()
        {
            attackTimer -= Globals.DeltaTime;

            if (attackTimer <= 0)
                canAttack = true;
        }


        #endregion


        #region Spell Timers

        /// <summary>
        /// Spell Delay Timer
        /// </summary>
        float spellTimer;

        /// <summary>
        /// Can the character cast a spell
        /// </summary>
        public bool CanCastSpell
        {
            get { return canCastSpell; }
            set { canCastSpell = value; }
        }
        bool canCastSpell = true;


        /// <summary>
        /// Sets all timers associated with attacking
        /// </summary>
        /// <param name="attackDelay"></param>
        public void SpellCastSpell(float spellCastDelay)
        {
            canCastSpell = false;
            spellCastDelay = MathHelper.Clamp(spellCastDelay, 0, 50f);
        }


        /// <summary>
        /// Updates attack timers
        /// </summary>
        private void UpdateSpell()
        {
            spellTimer -= Globals.DeltaTime;

            if (spellTimer <= 0)
                canCastSpell = true;
        }

        #endregion


        #region Respawn Timer


        /// <summary>
        /// Respawn timer for NPC characters. 
        /// </summary>
        /// <remarks>How long the NPC remains dead before
        /// respawning into the world</remarks>
        public float Respawn
        {
            get { return respawn; }
            set { respawn = value; }
        }
        float respawn;


        private void UpdateRespawn()
        {
            respawn -= (float)Globals.GameTime.ElapsedRealTime.TotalMinutes;
        }

        #endregion


        /// <summary>
        /// Updates timers associated with combat
        /// </summary>
        public void Update()
        {
            if (attackTimer > 0)
                UpdateAttack();

            if (spellTimer > 0)
                UpdateSpell();

            if (respawn > 0)
                UpdateRespawn();
        }

    }//end class
}//end namespace
