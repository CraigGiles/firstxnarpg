using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ActionRPG
{
    public class Player : Character
    {

        /// <summary>
        /// Gets or sets the name of the player
        /// </summary>
        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
        string playerName = "Demius";


        #region Constructor(s)

        public Player()
        {
            //Load player information
            base.Initialize("Player");

            this.Initialize();
        }

        private void Initialize()
        {
            this.SpawnLocationTile = Globals.TileEngine.Map.PlayerRespawnTile;
            this.SpawnLocationVector = Globals.TileEngine.Map.PlayerRespawnVector;
            this.CurrentPosition = this.SpawnLocationVector;

            IsAnimated = true;
            SpriteManager.PlayAnimation("Standing", Direction.Down);
        }

        #endregion
        
        
        #region Update


        /// <summary>
        /// Handles the update logic for the player character
        /// </summary>
        public override void Update()
        {

            switch (CurrentAction)
            {
                case Actions.Traveling:
                    UpdateTraveling();
                    break;

                case Actions.Attack:
                    UpdateAttack();
                    break;

            }//end switch

            base.Update();
        }


        /// <summary>
        /// Update call when Main Game screen is
        /// not in focus
        /// </summary>
        public void UpdateInactive()
        {
            switch (CurrentAction)
            {
                case Actions.Traveling:
                    //SpriteManager.PlayAnimation("Standing");
                    UpdateTraveling();
                    break;

                case Actions.Attack:
                    UpdateAttack();
                    break;
            }



            base.Update();
        }

        #region Update Traveling


        private void UpdateTraveling()
        {
            CheckForMovement();

            if (Timers.CanAttack)
                CheckForAttack();
        }


        /// <summary>
        /// Checks if a movement key is pressed and moves
        /// the character accordingly
        /// </summary>
        private void CheckForMovement()
        {
            //Checks to see if the movement keys are held
            if (Globals.Input.IsKeyHeld(Globals.Settings.MoveDown) ||
                Globals.Input.IsKeyHeld(Globals.Settings.MoveLeft) ||
                Globals.Input.IsKeyHeld(Globals.Settings.MoveUp) ||
                Globals.Input.IsKeyHeld(Globals.Settings.MoveRight) ||
                Globals.Input.IsLeftThumbstickHeld())
            {

                if (!Globals.Input.IsLeftThumbstickHeld())
                {
                    //if move, adjust the players position
                    SpriteManager.SpriteBox = Globals.TileEngine.CalculateCharacterMovement(
                        Globals.Input.IsKeyHeld(Globals.Settings.MoveDown),
                        Globals.Input.IsKeyHeld(Globals.Settings.MoveLeft),
                        Globals.Input.IsKeyHeld(Globals.Settings.MoveUp),
                        Globals.Input.IsKeyHeld(Globals.Settings.MoveRight),
                        SpriteManager.SpriteBox,
                        MovementSpeedModifier);

                    //adjust the players animation
                    SpriteManager.PlayAnimation("Walking",
                        Globals.Input.IsKeyHeld(Globals.Settings.MoveDown),
                        Globals.Input.IsKeyHeld(Globals.Settings.MoveLeft),
                        Globals.Input.IsKeyHeld(Globals.Settings.MoveUp),
                        Globals.Input.IsKeyHeld(Globals.Settings.MoveRight));

                }
                else
                {
                    //if move, adjust the players position
                    SpriteManager.SpriteBox = Globals.TileEngine.CalculateCharacterMovement(
                        Globals.Input.IsButtonPressed(Buttons.LeftThumbstickDown),
                        Globals.Input.IsButtonPressed(Buttons.LeftThumbstickLeft),
                        Globals.Input.IsButtonPressed(Buttons.LeftThumbstickUp),
                        Globals.Input.IsButtonPressed(Buttons.LeftThumbstickRight),
                        SpriteManager.SpriteBox,
                        MovementSpeedModifier);

                    //adjust the players animation
                    SpriteManager.PlayAnimation("Walking",
                        Globals.Input.IsButtonPressed(Buttons.LeftThumbstickDown),
                        Globals.Input.IsButtonPressed(Buttons.LeftThumbstickLeft),
                        Globals.Input.IsButtonPressed(Buttons.LeftThumbstickUp),
                        Globals.Input.IsButtonPressed(Buttons.LeftThumbstickRight));
                }




                //if player is on portal to another map, return new position
                Vector2 newMapPosition = Vector2.Zero;
                Globals.TileEngine.CheckForPortalEntry(FootLocation, out newMapPosition);

                if (newMapPosition != Vector2.Zero)
                    CurrentPosition = newMapPosition;

            }//end if
            else
            {
                SpriteManager.PlayAnimation("Standing");
            }

        }//end CheckForMovement


        /// <summary>
        /// Checks if the attack key is pressed and
        /// launches character attack if true
        /// </summary>
        private void CheckForAttack()
        {
            if (Globals.Input.IsKeyPressed(Globals.Settings.KeyboardAttack) ||
                Globals.Input.IsButtonPressed(Globals.Settings.GamePadAttack))
            {
                if (CurrentAction != Actions.Attack)
                {
                    //sets the previous and current actions
                    PreviousAction = CurrentAction;
                    CurrentAction = Actions.Attack;
                }

                SpriteManager.PlayAnimation("Attack");

                //pause the animation 
                SpriteManager.PauseAnimation(true);

                //set attack timer
                Timers.Attack(Inventory.Equipment.Weapon.Delay - Inventory.Equipment.AddedHaste);

                foreach (NPC npc in Globals.TileEngine.Map.NPCs)
                    Globals.Battle.Attack(this, npc, combo);

            }
        }


        #endregion


        #region Update Attacking

        /// <summary>
        /// Time added to weapon delay to ensure combo attack
        /// </summary>
        float resetCombo = .30f;

        /// <summary>
        /// Timer for combo
        /// </summary>
        float attackCombo = .80f;

        /// <summary>
        /// Combo swing number
        /// </summary>
        int combo = 0;

        /// <summary>
        /// Updates attack logic
        /// </summary>
        /// <remarks>Checks to see if attack animation is complete</remarks>
        private void UpdateAttack()
        {
            //if you're within current attack combo
            if (attackCombo > 0 && combo < Inventory.Equipment.Weapon.MaxCombo)
            {
                //update attack combo timer
                attackCombo -= Globals.DeltaTime;

                //if the current character can attack and attack key was pressed
                if (Globals.Input.IsKeyPressed(Globals.Settings.KeyboardAttack) && Timers.CanAttack ||
                Globals.Input.IsButtonPressed(Globals.Settings.GamePadAttack) && Timers.CanAttack)
                {
                    //reset attack combo timer
                    attackCombo = (Inventory.Equipment.Weapon.Delay - Inventory.Equipment.AddedHaste) + resetCombo; ;

                    //set attack timer
                    Timers.Attack(Inventory.Equipment.Weapon.Delay - Inventory.Equipment.AddedHaste);
                   

                    //see if attack lands on target
                    foreach (NPC npc in Globals.TileEngine.Map.NPCs)
                        Globals.Battle.Attack(this, npc, combo);

                    //pause current animation
                    SpriteManager.PauseAnimation(true);

                    //advance frame by one
                    SpriteManager.AdvanceFrameByOne();

                    combo++;
                }

            }
            else
            {
                attackCombo = (Inventory.Equipment.Weapon.Delay - Inventory.Equipment.AddedHaste) + resetCombo;
                combo = 0;
                CurrentAction = Actions.Traveling;
            }
        }

        #endregion


        #endregion


        #region Draw


        #endregion

    }
}
