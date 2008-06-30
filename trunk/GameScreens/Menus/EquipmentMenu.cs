using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ActionRPG
{
    public class EquipmentMenu : GameScreen
    {
        Vector2 location;
        Player player;

        Texture2D button, handIcon;

        EquipmentSlot currentMenuSelection = EquipmentSlot.Weapon;


        #region Constructor(s)


        public EquipmentMenu(Vector2 menuLocation, Player player)
        {
            this.location = menuLocation;
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
            button = null;
            handIcon = null;

            base.UnloadContent();
        }

        #endregion


        #region Update


        public override void Update(bool coveredByOtherScreen)
        {
            if (this.HasFocus)
                UpdateScreen();

            base.Update(coveredByOtherScreen);
        }

        private void UpdateScreen()
        {
            if (Globals.Input.IsKeyPressed(Globals.Settings.Cancel) ||
                Globals.Input.IsButtonPressed(Globals.Settings.GamePadCancel))
            {
                ScreenManager.RemoveScreen(this);
            }

            //Next Menu Selection
            if (Globals.Input.IsKeyPressed(Keys.Down) ||
                Globals.Input.IsButtonPressed(Buttons.DPadDown))
            {
                if (currentMenuSelection == EquipmentSlot.Boots)
                    currentMenuSelection = EquipmentSlot.Weapon;
                else
                    currentMenuSelection++;
            }

            //Previous Menu Selection
            if (Globals.Input.IsKeyPressed(Keys.Up) ||
                Globals.Input.IsButtonPressed(Buttons.DPadUp))
            {
                if (currentMenuSelection == EquipmentSlot.Weapon)
                    currentMenuSelection = EquipmentSlot.Boots;
                else
                    currentMenuSelection--;
            }

            if (Globals.Input.IsKeyPressed(Globals.Settings.Okay) ||
                Globals.Input.IsButtonPressed(Globals.Settings.GamePadOkay))
            {
                if (currentMenuSelection == EquipmentSlot.Weapon)
                    ScreenManager.AddScreen(new EquipWeapon(new Vector2(location.X + 130, location.Y), player));

                else if (currentMenuSelection == EquipmentSlot.Accessory)
                {
                    ScreenManager.AddScreen(new EquipAccessory(new Vector2(location.X + 130, location.Y), player));
                }
                else if (currentMenuSelection == EquipmentSlot.Arms ||
                         currentMenuSelection == EquipmentSlot.Belt ||
                         currentMenuSelection == EquipmentSlot.Boots ||
                         currentMenuSelection == EquipmentSlot.Chest ||
                         currentMenuSelection == EquipmentSlot.Gloves ||
                         currentMenuSelection == EquipmentSlot.Helmet ||
                         currentMenuSelection == EquipmentSlot.Legs ||
                         currentMenuSelection == EquipmentSlot.Shield ||
                         currentMenuSelection == EquipmentSlot.Shoulders)
                {
                    ScreenManager.AddScreen(new EquipArmor(new Vector2(location.X + 130, location.Y), player, currentMenuSelection));
                }
            }
        }


        #endregion


        #region Draw

        string[] text = new string[]
        {
            "Weapon",
            "Helmet",
            "Shoulders",
            "Chest",
            "Gloves",
            "Belt",
            "Arms",
            "Accessory",
            "Shield",
            "Legs",
            "Boots", 
        };

        public override void Draw()
        {
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend);

            DrawMenuButtons();

            Globals.Batch.End();

            base.Draw();
        }

        private void DrawMenuButtons()
        {

            for (int i = 0; i < 11; i++)
            {
                Globals.Batch.Draw(button,
                    new Rectangle((int)location.X, (int)location.Y + (i * 22), 120, 20),
                    Color.White);

                Globals.Batch.DrawString(Globals.Font,
                    text[i],
                    new Vector2(location.X + 10, location.Y + ((i * 22) + 2)),
                    Color.White);

                if (currentMenuSelection.ToString() == text[i] && this.HasFocus)
                    DrawMenuIcon(new Rectangle((int)location.X - 20, (int)location.Y + (i * 22), 25, 25));
            }
        }

        private void DrawMenuIcon(Rectangle drawRectangle)
        {
            Globals.Batch.Draw(handIcon,
                drawRectangle,
                Color.White);
        }


        #endregion

    }
}
