using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Helper;

namespace ActionRPG
{
    public class Battle
    {
        public void Attack(Character attacker, Character attacked, int comboSwing)
        {
            Vector2 distance = attacked.FootLocation - attacker.FootLocation;

            if (distance.Length() < attacker.AttackRange)
            {
                if (IsAttackerFacingTarget(attacker, attacked))
                    DamageTarget(attacker, attacked, comboSwing);
            }

        }


        /// <summary>
        /// Is the attacker facing the target of their attack
        /// </summary>
        /// <param name="attacker">The attacking character</param>
        /// <param name="attacked">Character being attacked</param>
        /// <returns>bool</returns>
        private bool IsAttackerFacingTarget(Character attacker, Character attacked)
        {
            Point attackerTile = Globals.TileEngine.ConvertPositionToTile(attacker.Origin);
            Point attackedTile = Globals.TileEngine.ConvertPositionToTile(attacked.Origin);


            switch (attacker.LastDirection)
            {
                case Direction.Down:
                    if (attackedTile.Y >= attackerTile.Y &&
                        attackedTile.X <= attackerTile.X + 3 &&
                        attackedTile.X >= attackerTile.X - 3)
                    {
                        return true;
                    }
                    break;

                case Direction.Left:
                    if (attackedTile.X <= attackerTile.X &&
                        attackedTile.Y <= attackerTile.Y + 3 &&
                        attackedTile.Y >= attackerTile.Y - 3)
                    {
                        return true;
                    }
                    break;

                case Direction.Up:
                    if (attackedTile.Y <= attackerTile.Y &&
                        attackedTile.X <= attackerTile.X + 3 &&
                        attackedTile.X >= attackerTile.X - 3)
                    {
                        return true;
                    }
                    break;

                case Direction.Right:
                    if (attackedTile.X >= attackerTile.X &&
                        attackedTile.Y <= attackerTile.Y + 3 &&
                        attackedTile.Y >= attackerTile.Y - 3)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }


        /// <summary>
        /// If attack was successful, damage the target of attack
        /// </summary>
        /// <param name="attacker">The attacking character</param>
        /// <param name="attacked">Character being attacked</param>
        /// <param name="comboSwing">the current combo swing</param>
        private void DamageTarget(Character attacker, Character target, int comboSwing)
        {

            if (target.CurrentCharacterState == CharacterState.Alive)
            {
                target.TakePhysicalDamage(attacker.AttackPower + (attacker.Inventory.Equipment.Weapon.Damage * comboSwing), attacker);

                if (target.Health <= 0)
                {
                    target.HasDied();
                    attacker.Inventory.AddGold(target.Inventory.Gold, 
                        attacker.Origin - new Vector2(0, attacker.SpriteManager.SpriteBox.Height / 2));
                }
            }

        }



    }
}
