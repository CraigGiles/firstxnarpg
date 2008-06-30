using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ActionRPG
{
    class EquipAccessory : GameScreen
    {
        List<Accessory> unEquippedAccessory = new List<Accessory>();
        int chosenAccessory = 0;

        bool noAccessoryInInventory = false;

        Player player;
        Vector2 location;

        Texture2D blueButton, handIcon, blueBackground;

        #region Constructor(s)


        public EquipAccessory(Vector2 menuLocation, Player player)
        {
            this.location = menuLocation;
            this.player = player;
        }

        public override void LoadContent()
        {
            handIcon = Globals.Content.Load<Texture2D>(@"Graphics/hand");
            blueButton = Globals.Content.Load<Texture2D>(@"Graphics/button");
            blueBackground = Globals.Content.Load<Texture2D>(@"Graphics/blueBackground");

            UpdateUnequippedAccessories();
            base.LoadContent();
        }

        public override void UnloadContent()
        {

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
                Globals.Input.IsButtonPressed(Buttons.DPadDown) &&
                !noAccessoryInInventory)
            {
                if (chosenAccessory == unEquippedAccessory.Count - 1)
                    chosenAccessory = (int)MathHelper.Clamp(0, 0, unEquippedAccessory.Count - 1);
                else
                    chosenAccessory++;
            }

            //Previous Menu Selection
            if (Globals.Input.IsKeyPressed(Keys.Up) ||
                Globals.Input.IsButtonPressed(Buttons.DPadUp))
            {
                if (chosenAccessory == 0)
                    chosenAccessory = (int)MathHelper.Clamp(unEquippedAccessory.Count - 1, 0, unEquippedAccessory.Count - 1);
                else
                    chosenAccessory--;
            }

            if (Globals.Input.IsKeyPressed(Globals.Settings.Okay) ||
                Globals.Input.IsButtonPressed(Globals.Settings.GamePadOkay) &&
                unEquippedAccessory.Count > 0)
            {
                player.Inventory.Equip(unEquippedAccessory[chosenAccessory]);
                unEquippedAccessory.Remove(unEquippedAccessory[chosenAccessory]);
                UpdateUnequippedAccessories();
            }
        }


        #endregion

        private void UpdateUnequippedAccessories()
        {
            unEquippedAccessory.Clear();

            foreach (Item item in player.Inventory.Items)
            {
                if (item.Slot == EquipmentSlot.Accessory)
                {
                    unEquippedAccessory.Add(item as Accessory);
                }
            }
        }


        #region Draw
               

        public override void Draw()
        {
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend);

            DrawEquipmentList();
            DrawHandIcon();
            DrawCurrentAccessory();
            DrawHighlightedWeapon();

            Globals.Batch.End();
            base.Draw();
        }


        private void DrawEquipmentList()
        {
            int count = 0;

            foreach (Accessory weapon in unEquippedAccessory)
            {
                Globals.Batch.Draw(blueButton, new Rectangle((int)location.X, (int)location.Y + (count * 22), 250, 20), Color.White);
                Globals.Batch.DrawString(Globals.Font,"Equip: " + weapon.AssetName, new Vector2(location.X + 10, location.Y + ((count * 22) + 2)), Color.White);

                count++;
            }

            if (count == 0)
            {
                noAccessoryInInventory = true;
                Globals.Batch.Draw(blueButton, new Rectangle((int)location.X, (int)location.Y + (count * 22), 250, 20), Color.White);
                Globals.Batch.DrawString(Globals.Font, "No weapons in inventory", new Vector2(location.X + 10, location.Y + ((count * 22) + 2)), Color.White);
            }
            else
                noAccessoryInInventory = false;
        }


        private void DrawHandIcon()
        {
            Globals.Batch.Draw(handIcon, new Rectangle((int)location.X - 20, (int)location.Y + (chosenAccessory * 22), 25, 25), Color.White);
        }


        private void DrawCurrentAccessory()
        {
            Rectangle currentlyEquippedBackground = new Rectangle((int)location.X + 260, (int)location.Y, 300, 250);
            Globals.Batch.Draw(blueBackground, currentlyEquippedBackground, Color.White);

            Globals.Batch.DrawString(Globals.Font,
                "Currently Equipped",
                new Vector2(location.X + 270, location.Y),
                Color.White);

                       
            DrawWeapon(player.Inventory.Equipment.Accessory, new Vector2(location.X + 270, location.Y + 15));
            DrawStats(player.Inventory.Equipment.Accessory, new Vector2(location.X + 270, location.Y + 80));
        }


        private void DrawHighlightedWeapon()
        {
            Rectangle currentlyEquippedBackground = new Rectangle((int)location.X + 260, (int)location.Y + 260, 300, 250);
            Globals.Batch.Draw(blueBackground, currentlyEquippedBackground, Color.White);

            Globals.Batch.DrawString(Globals.Font,
                "Highlighted Accessory",
                new Vector2(location.X + 270, location.Y + 260),
                Color.White);

            if (!noAccessoryInInventory)
            {
                DrawWeapon(unEquippedAccessory[chosenAccessory], new Vector2(location.X + 270, location.Y + 275));
                DrawStats(unEquippedAccessory[chosenAccessory], new Vector2(location.X + 270, location.Y + 340));
            }
        }


        private void DrawWeapon(Accessory gear, Vector2 location)
        {
            Globals.Batch.Draw(gear.GraphicIcon,
                new Vector2(location.X - 5, location.Y),
                Color.White);

            Globals.Batch.DrawString(
                Globals.Font,
                "Name: " + gear.AssetName,
                new Vector2(location.X + 60, location.Y),
                Color.White);

            Globals.Batch.DrawString(
                Globals.Font,
                gear.Description,
                new Vector2(location.X + 75, location.Y + 15),
                Color.White);

        }


        private void DrawStats(Accessory gear, Vector2 location)
        {
            Color color = Color.White;

            Globals.Batch.DrawString(Globals.Font, "Added Stats: ", new Vector2(location.X + 80, location.Y + 15), Color.White);

            Globals.Batch.DrawString(Globals.Font, "Health: " + gear.AddedStats.Health.ToString(), new Vector2(location.X, location.Y + 30), CompareStats(gear.AddedStats.Health, 0));
            Globals.Batch.DrawString(Globals.Font, "Mana: " + gear.AddedStats.Mana.ToString(), new Vector2(location.X, location.Y + 45), CompareStats(gear.AddedStats.Mana, 0));
            Globals.Batch.DrawString(Globals.Font, "Haste: " + gear.AddedStats.Haste.ToString(), new Vector2(location.X, location.Y + 60), CompareStats((int)gear.AddedStats.Haste, 0));

            Globals.Batch.DrawString(Globals.Font, "STR: " + gear.AddedStats.Strength.ToString(), new Vector2(location.X + 150, location.Y + 30), CompareStats(gear.AddedStats.Strength, 0));
            Globals.Batch.DrawString(Globals.Font, "STA: " + gear.AddedStats.Stamina.ToString(), new Vector2(location.X + 150, location.Y + 45), CompareStats(gear.AddedStats.Stamina, 0));
            Globals.Batch.DrawString(Globals.Font, "INT: " + gear.AddedStats.Intelligence.ToString(), new Vector2(location.X + 150, location.Y + 60), CompareStats(gear.AddedStats.Intelligence, 0));
            Globals.Batch.DrawString(Globals.Font, "WIS: " + gear.AddedStats.Wisdom.ToString(), new Vector2(location.X + 150, location.Y + 75), CompareStats(gear.AddedStats.Wisdom, 0));
            Globals.Batch.DrawString(Globals.Font, "AGI: " + gear.AddedStats.Agility.ToString(), new Vector2(location.X + 150, location.Y + 90), CompareStats(gear.AddedStats.Agility, 0));
            Globals.Batch.DrawString(Globals.Font, "DEX: " + gear.AddedStats.Dexterity.ToString(), new Vector2(location.X + 150, location.Y + 105), CompareStats(gear.AddedStats.Dexterity, 0));
        }


        private Color CompareStats(int gear, int playerStat)
        {
            if (gear > playerStat)
                return Color.LimeGreen;

            else if (gear < playerStat)
                return Color.Red;

            else
                return Color.White;
        }


        #endregion

    }
    
}
