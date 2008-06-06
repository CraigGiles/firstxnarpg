using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class Timers
    {

        /// <summary>
        /// Attack timer
        /// </summary>
        public float Attack
        {
            get { return attack; }
            set { attack = MathHelper.Clamp(value, 0, 10f); }
        }
        float attack;


        /// <summary>
        /// Spell Cast Timer
        /// </summary>
        public float Spell
        {
            get { return spell; }
            set { spell = MathHelper.Clamp(value, 0, 10f); }
        }
        float spell;


        public void Update()
        {
            if (attack > 0)
                attack -= Globals.DeltaTime;

            if (spell > 0)
                spell -= Globals.DeltaTime;
        }

    }//end class
}//end namespace
