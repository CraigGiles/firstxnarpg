using System;
using System.Collections.Generic;
using System.Text;

using TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ShackRPG.GameScreens;

namespace ShackRPG
{
    public class BattleManager
    {
        public enum BattleCommands
        {
            Fight,
            Run,
            Defend,
            None,
        }

        public BattleManager()
        {
        }

        /// <summary>
        /// Issues the Attack command in battle
        /// </summary>
        /// <param name="attack">Attack power of the attacker</param>
        /// <returns>Damage Delt</returns>
        public int Attack(int attackPower)
        {
            int damage = 0;

            RandomHelper.GenerateNewRandomGenerator();
            damage = attackPower + RandomHelper.GetRandomInt(10);

            return damage;
        }

        /// <summary>
        /// Issues the Run Away command in battle
        /// </summary>
        /// <returns>True: Ran away - False; Failed attempt</returns>
        public bool RunAway()
        {
            RandomHelper.GenerateNewRandomGenerator();

            if (RandomHelper.GetRandomInt(1000) <= 350)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Damages the character
        /// </summary>
        /// <param name="damage">Pure Damage delt to the character</param>
        /// <param name="armor">Current armor value of the character</param>
        /// <returns>Damage to be taken after armor checks</returns>
        public int TakePhysicalDamage(int damage, int armor)
        {
            damage -= armor;

            return damage;
        }

    }
}
