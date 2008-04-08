using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ShackRPG.HelperClasses;

using TileEngine;
using ShackRPG.GameScreens;
using ShackRPG.Enemies;
using Microsoft.Xna.Framework.Content;

namespace ShackRPG
{
    public partial class Character
    {
        int _EnemyTarget = 0;
        int _AllyTarget = 0;
        int _SelectedSpell = 0;

        bool _SelectingTarget;
        bool _SelectingSpell;
        

        /// <summary>
        /// List of available Battle Commands
        /// </summary>
        public enum BattleCommands
        {
            Fight,
            Magic,
            Item,
            Defend,
            None,
        }

        /// <summary>
        /// Battle commands available to the character
        /// </summary>
        public BattleCommands
            SelectedBattleCommand = BattleCommands.None,
            ExecuteCommand = BattleCommands.None,
            previousBattleCommand;

        /// <summary>
        /// List of current available battle status
        /// </summary>
        public enum BattleStatus
        {
            Normal,
            Hasted,
            Slowed,
            Poisoned,
            Stopped,
            Silenced,
            Dead,
        }

        /// <summary>
        /// Current battle status of the character
        /// </summary>
        public BattleStatus
            curBattleStatus = BattleStatus.Normal,
            prevBattleStatus;

        public bool SelectingTarget
        {
            get { return _SelectingTarget; }
            set { _SelectingTarget = value; }
        }

        #region Properties
        public bool SelectingSpell
        {
            get { return _SelectingSpell; }
            set { _SelectingSpell = value; }
        }

        #endregion

        #region Methods
        public void StartBattle()
        {
            animation.StartStandingAnimation(LastDirection); //change this to 'battle idle'

            ActionTimer = ActionTimerReset;
            prevCharState = curCharState;           //holds the prev char state
            curCharState = CharacterStates.Battle;  //sets the current state to battle
        }

        public void Victory()
        {
            animation.StartVictoryAnimation();      //starts victory animation
            ExecuteCommand = BattleCommands.None;
            previousBattleCommand = BattleCommands.None;
            SelectedBattleCommand = BattleCommands.None;
        }

        public void EndBattle()
        {
            curCharState = prevCharState;           //reverts back to prev char state
        }

        private void UpdateBattle(GameTime gameTime)
        {
            UpdateBattleTimers(gameTime);
        }

        private void UpdateBattleTimers(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            ActionTimer -= elapsed;

            if (ActionTimer <= 0)
            {
                previousBattleCommand = BattleCommands.None;
                animation.StartStandingAnimation(LastDirection);
            }

            //update any other timers like "poisoned" etc
            //example; 
            //if (poisoned)
            //  PoisonTimer -= elapsed;
                //if (PoisonTimer <= 0)
                    //take poison damage
        }
                
        public BaseEnemy GetBattleTarget(List<BaseEnemy> enemies, GameTime gameTime)
        {
            BaseEnemy selected = null;
            BaseEnemy[] e = enemies.ToArray();

            _EnemyTarget = (int)MathHelper.Clamp(_EnemyTarget, 0, enemies.Count - 1);

                if (Input.GetNext)              //get next enemy
                    GetNextEnemy(enemies);

                else if (Input.GetPrevious)     //get previous enemy
                    GetPreviousEnemy(enemies);

                else if (Input.Enter)           //execute command on selected enemy
                    selected = e[_EnemyTarget];
 
            return selected;
        }

        public Spells GetCastableSpell()
        {
            Spells selected = null;
            Spells[] known = spells.KnownSpells.ToArray();

            _SelectedSpell = (int)MathHelper.Clamp(_SelectedSpell, 0,  spells.KnownSpells.Count - 1);

            if (Input.GetNext)
            {
                _SelectedSpell++;
                _SelectedSpell = (int)MathHelper.Clamp(_SelectedSpell, 0, spells.KnownSpells.Count - 1);
            }

            if (Input.GetPrevious)
            {
                _SelectedSpell--;
                _SelectedSpell = (int)MathHelper.Clamp(_SelectedSpell, 0, spells.KnownSpells.Count - 1);
            }

            if (Input.Enter)
            {
                selected = known[_SelectedSpell];
            }

            return selected;
        }

        private void GetNextEnemy(List<BaseEnemy> enemies)
        {
            _EnemyTarget++;
            _EnemyTarget = (int)MathHelper.Clamp(_EnemyTarget, 0, enemies.Count - 1);

        }

        private void GetPreviousEnemy(List<BaseEnemy> enemies)
        {
            _EnemyTarget--;

            _EnemyTarget = (int)MathHelper.Clamp(_EnemyTarget, 0, enemies.Count - 1);
        }

        public void GetAllyTarget(List<Character> allies)
        {
        }

        public void GetMagicTarget(List<BaseEnemy> enemies, List<Character> characters, GameTime gameTime)
        {         
            //if the spell being cast is a heal, start with allies
            //if the spell being cast is not a heal, start with enemies
        }

        private void GetNextAlly(List<Character> allies)
        {
        }

        private void GetPreviousAlly(List<Character> allies)
        {
        }

        public int Fight(GameTime gameTime)
        {
            int damage = 0;

            RandomHelper.GenerateNewRandomGenerator();
            damage = Strength + inventory.EquippedWeapon.Damage + RandomHelper.GetRandomInt(iLevel, iLevel * AttackDamageMod);

            animation.StartAttackAnimation(LastDirection);

            ActionTimer = ActionTimerReset;
            previousBattleCommand = BattleCommands.Fight;
            return damage;

        }

        public void Defend()
        {
            ActionTimer = ActionTimerReset;
            animation.Defending(LastDirection);
            previousBattleCommand = BattleCommands.Defend;
        }

        public int Magic(Spells spell, GameTime gameTime)
        {
            int damage = 0;

            SelectedBattleCommand = BattleCommands.Magic;
            ActionTimer = ActionTimerReset;
            return damage;
        }

        #endregion

        #region Battle Draw Methods
        /// <summary>
        /// Draws the character to screen
        /// </summary>
        public void Draw(SpriteBatch batch, SpriteFont font, GameTime gameTime, List<BaseEnemy> enemies)
        {
            BaseEnemy[] enemy = enemies.ToArray();
            Rectangle eLocation = new Rectangle(
                enemy[_EnemyTarget].SpriteBoundingBox.X - 30, 
                enemy[_EnemyTarget].SpriteBoundingBox.Y, 
                25, 25);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;   //elapsed time for timers

            batch.Draw(tSpriteSheet, rSpriteRect, animation.CurrentSprite, Color.White);
            batch.Draw(Battle.tHandIcon, eLocation, Color.White);

            DrawTextAnimations(batch, gameTime, font);
        }
              
        #endregion
    }
}
