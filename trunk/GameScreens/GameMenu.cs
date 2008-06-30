using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ActionRPG
{
    public class GameMenu : GameScreen
    {
        Player player;

        /// <summary>
        /// Menu selections for the main menu
        /// </summary>
        enum MenuSelections
        {
            Character = 0,
            Equipment = 1,
            Magic = 2,
            Items = 3,
            Quests = 4,
            Exit = 5,
        }
        MenuSelections currentMenuSelection = MenuSelections.Character;

        Texture2D 
            button,
            handIcon;


        #region Constructor(s)

        public GameMenu(Player player)
        {
            this.player = player;
        }

        public override void LoadContent()
        {
            button = Globals.Content.Load<Texture2D>(@"Graphics/Button");
            handIcon = Globals.Content.Load<Texture2D>(@"Graphics/hand");

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            
            base.UnloadContent();
        }

        #endregion


        public override void Update(bool coveredByOtherScreen)
        {
            if (this.HasFocus)
                UpdateScreen();

            base.Update(coveredByOtherScreen);
        }

        private void UpdateScreen()
        {
            if (Globals.Input.IsKeyPressed(Globals.Settings.Cancel) ||
                Globals.Input.IsKeyPressed(Globals.Settings.Inventory) ||
                Globals.Input.IsButtonPressed(Globals.Settings.GamePadCancel) ||
                Globals.Input.IsButtonPressed(Globals.Settings.GamePadInventory))
            {
                ScreenManager.RemoveScreen(this);
            }

            CheckInput();
        }

        private void CheckInput()
        {
            //Next Menu Selection
            if (Globals.Input.IsKeyPressed(Keys.Down) ||
                Globals.Input.IsButtonPressed(Buttons.DPadDown))
            {
                if (currentMenuSelection == MenuSelections.Exit)
                    currentMenuSelection = MenuSelections.Character;
                else
                    currentMenuSelection++;
            }

            //Previous Menu Selection
            if (Globals.Input.IsKeyPressed(Keys.Up) ||
                Globals.Input.IsButtonPressed(Buttons.DPadUp))
            {
                if (currentMenuSelection == MenuSelections.Character)
                    currentMenuSelection = MenuSelections.Exit;
                else
                    currentMenuSelection--;
            }

            //Execute menu button
            if (Globals.Input.IsKeyPressed(Globals.Settings.Okay) ||
                Globals.Input.IsButtonPressed(Globals.Settings.GamePadOkay))
            {
                switch (currentMenuSelection)
                {
                    case MenuSelections.Character:
                        ScreenManager.AddScreen(new CharacterMenu(new Vector2(gameMenuButtons.X + 20, gameMenuButtons.Y + 15), player));
                        break;

                    case MenuSelections.Equipment:
                        ScreenManager.AddScreen(new EquipmentMenu(new Vector2(gameMenuButtons.X + 20, gameMenuButtons.Y + 32), player));
                        break;

                    case MenuSelections.Magic:
                        //ScreenManager.AddScreen(new MagicMenu(player, new Vector2(gameMenuButtons.X + 130, gameMenuButtons.Y + 49)));
                        break;

                    case MenuSelections.Items:
                        //ScreenManager.AddScreen(new ItemsMenu(player, new Vector2(gameMenuButtons.X + 130, gameMenuButtons.Y + 71)));
                        break;

                    case MenuSelections.Quests:
                        //ScreenManager.AddScreen(new QuestsMenu(player, new Vector2(gameMenuButtons.X + 130, gameMenuButtons.Y + 93)));
                        break;

                    case MenuSelections.Exit:
                        ScreenManager.RemoveScreen(this);
                        break;

                }
            }

        }

        public override void Draw()
        {

            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend);

            DrawMenuButtons();

            Globals.Batch.End();

            base.Draw();
        }

        #region Draw Game Menu Buttons

        /// <summary>
        /// Location of the buttons used for the
        /// main game menu
        /// </summary>
        Vector2 gameMenuButtons = new Vector2(20, 5);

        private void DrawMenuButtons()
        {
            //Character
            Globals.Batch.Draw(button, new Rectangle((int)gameMenuButtons.X + 5, (int)gameMenuButtons.Y + 5, 120, 20), Color.White);
            Globals.Batch.DrawString(Globals.Font, "Character", new Vector2((int)gameMenuButtons.X + 15, (int)gameMenuButtons.Y + 7), Color.White);
            if (currentMenuSelection == MenuSelections.Character && this.HasFocus)
                DrawSelectionIcon(new Rectangle((int)gameMenuButtons.X - 15, (int)gameMenuButtons.Y + 7, 25, 20));

            //Equipment
            Globals.Batch.Draw(button, new Rectangle((int)gameMenuButtons.X + 5, (int)gameMenuButtons.Y + 27, 120, 20), Color.White);
            Globals.Batch.DrawString(Globals.Font, "Equipment", new Vector2((int)gameMenuButtons.X + 15, (int)gameMenuButtons.Y + 29), Color.White);
            if (currentMenuSelection == MenuSelections.Equipment && this.HasFocus)
                DrawSelectionIcon(new Rectangle((int)gameMenuButtons.X - 15, (int)gameMenuButtons.Y + 29, 25, 20));

            //Magic
            Globals.Batch.Draw(button, new Rectangle((int)gameMenuButtons.X + 5, (int)gameMenuButtons.Y + 49, 120, 20), Color.White);
            Globals.Batch.DrawString(Globals.Font, "Magic", new Vector2((int)gameMenuButtons.X + 15, (int)gameMenuButtons.Y + 51), Color.White);
            if (currentMenuSelection == MenuSelections.Magic && this.HasFocus)
                DrawSelectionIcon(new Rectangle((int)gameMenuButtons.X - 15, (int)gameMenuButtons.Y + 51, 25, 20));

            //Items
            Globals.Batch.Draw(button, new Rectangle((int)gameMenuButtons.X + 5, (int)gameMenuButtons.Y + 71, 120, 20), Color.White);
            Globals.Batch.DrawString(Globals.Font, "Items", new Vector2((int)gameMenuButtons.X + 15, (int)gameMenuButtons.Y + 73), Color.White);
            if (currentMenuSelection == MenuSelections.Items && this.HasFocus)
                DrawSelectionIcon(new Rectangle((int)gameMenuButtons.X - 15, (int)gameMenuButtons.Y + 73, 25, 20));

            //Quests Menu
            Globals.Batch.Draw(button, new Rectangle((int)gameMenuButtons.X + 5, (int)gameMenuButtons.Y + 93, 120, 20), Color.White);
            Globals.Batch.DrawString(Globals.Font, "Quests", new Vector2((int)gameMenuButtons.X + 15, (int)gameMenuButtons.Y + 95), Color.White);
            if (currentMenuSelection == MenuSelections.Quests && this.HasFocus)
                DrawSelectionIcon(new Rectangle((int)gameMenuButtons.X - 15, (int)gameMenuButtons.Y + 95, 25, 20));

            //Exit Menu
            Globals.Batch.Draw(button, new Rectangle((int)gameMenuButtons.X + 5, (int)gameMenuButtons.Y + 115, 120, 20), Color.White);
            Globals.Batch.DrawString(Globals.Font, "Exit", new Vector2((int)gameMenuButtons.X + 15, (int)gameMenuButtons.Y + 117), Color.White);
            if (currentMenuSelection == MenuSelections.Exit && this.HasFocus)
                DrawSelectionIcon(new Rectangle((int)gameMenuButtons.X - 15, (int)gameMenuButtons.Y + 117, 25, 20));

        }
        

        #endregion


        private void DrawSelectionIcon(Rectangle location)
        {
            Globals.Batch.Draw(handIcon, location, Color.White);
        }
    }
}
