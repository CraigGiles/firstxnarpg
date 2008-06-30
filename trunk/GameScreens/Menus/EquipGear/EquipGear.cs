using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ActionRPG
{
    public class EquipGear : GameScreen
    {
        Vector2 location;
        Player player;
        EquipmentSlot slot;

        Texture2D button, handIcon, blueBackground;

        int currentSelection = 0;
        
        List<Gear> equippable = new List<Gear>();

        #region Constructor(s)


        public EquipGear(Vector2 menuLocation, Player player, EquipmentSlot slot)
        {
            this.location = menuLocation;
            this.player = player;
            this.slot = slot;
        }


        public override void LoadContent()
        {
            button = Globals.Content.Load<Texture2D>(@"Graphics/Button");
            handIcon = Globals.Content.Load<Texture2D>(@"Graphics/hand");
            blueBackground = Globals.Content.Load<Texture2D>(@"Graphics/blueBackground");

            foreach (Item i in player.Inventory.Items)
            {
                if (IsItemEquippable(i))
                {
                    Gear item = i as Gear;
                    equippable.Add(item);
                    itemsInInventory++;
                }
            }

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

        int itemsInInventory = 0;

        private void UpdateScreen()
        {
            if (Globals.Input.IsKeyPressed(Globals.Settings.Cancel))
            {
                ScreenManager.RemoveScreen(this);
            }

            //Next Menu Selection
            if (Globals.Input.IsKeyPressed(Globals.Settings.MoveDown))
            {
                if (currentSelection == itemsInInventory)
                    currentSelection = 0;
                else
                    currentSelection++;
            }

            //Previous Menu Selection
            if (Globals.Input.IsKeyPressed(Globals.Settings.MoveUp))
            {
                if (currentSelection == 0)
                    currentSelection = itemsInInventory;
                else
                    currentSelection--;
            }

            if (Globals.Input.IsKeyPressed(Globals.Settings.Okay) &&
                equippable.Count != 0)
            {
                if (equippable[currentSelection].Slot == EquipmentSlot.Weapon)
                {
                    Weapon equip = equippable[currentSelection] as Weapon;
                    equippable.Remove(equippable[currentSelection]);
                    player.Inventory.Equip(equip);
                }
                else if (equippable[currentSelection].Slot == EquipmentSlot.Accessory)
                {
                    Accessory equip = equippable[currentSelection] as Accessory;
                    equippable.Remove(equippable[currentSelection]);
                    player.Inventory.Equip(equip);
                }
                else
                {
                    Armor equip = equippable[currentSelection] as Armor;
                    equippable.Remove(equippable[currentSelection]);
                    player.Inventory.Equip(equip);
                }

            }
        }


        #endregion


        #region Draw


        public override void Draw()
        {
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend);

            DrawMenuButtons();
            DrawEquipmentPreview();

            Globals.Batch.End();

            base.Draw();
        }

        private void DrawMenuButtons()
        {
            int count = 0;

            foreach (Gear item in equippable)
            {
                Globals.Batch.Draw(button,
                    new Rectangle((int)location.X, (int)location.Y + (count * 22), 250, 20),
                    Color.White);

                Globals.Batch.DrawString(Globals.Font,
                    "Equip: " + item.AssetName,
                    new Vector2(location.X + 10, location.Y + ((count * 22) + 2)),
                    Color.White);

                count++;
            }

            Globals.Batch.Draw(button,
                new Rectangle((int)location.X, (int)location.Y + (count * 22), 250, 20),
                Color.White);

            Globals.Batch.DrawString(Globals.Font,
                "Unequip",
                new Vector2(location.X + 10, location.Y + ((count * 22) + 2)),
                Color.White);

            DrawMenuIcon(new Rectangle((int)location.X - 20, (int)location.Y + (currentSelection * 22), 25, 25));
        }

        private void DrawMenuIcon(Rectangle drawRectangle)
        {
            Globals.Batch.Draw(handIcon,
                drawRectangle,
                Color.White);
        }

        private void DrawEquipmentPreview()
        {
            Vector2 preview = new Vector2(location.X + 260, location.Y);
            
            DrawCurrentlyEquipped();
            DrawHighlighted();
            
        }

        /// <summary>
        /// Draws the currently equipped items stats
        /// </summary>
        private void DrawCurrentlyEquipped()
        {
            Rectangle currentlyEqiuppedBackground = new Rectangle((int)location.X + 260, (int)location.Y, 300, 250);
            Globals.Batch.Draw(blueBackground, currentlyEqiuppedBackground, Color.White);

            Globals.Batch.DrawString(Globals.Font,
                "Currently Equipped",
                new Vector2(location.X + 270, location.Y),
                Color.White);

            foreach (Gear gear in player.Inventory.Equipment.EquippedItems)
            {
                if (gear.Slot == slot && gear.Slot == EquipmentSlot.Weapon)
                {
                    Weapon weapon = gear as Weapon;
                    DrawWeapon(weapon, new Vector2(location.X + 270, location.Y + 15));
                    DrawAddedStats(weapon, new Vector2(location.X + 270, location.Y + 80));
                }
                else if (gear.Slot == slot && gear.Slot == EquipmentSlot.Accessory)
                {
                    Accessory accessory = gear as Accessory;
                    DrawAccessory(accessory, new Vector2(location.X + 270, location.Y + 15));
                    DrawAddedStats(accessory, new Vector2(location.X + 270, location.Y + 80));
                }
                else if (gear.Slot == slot)
                {
                    Armor armor = gear as Armor;
                    DrawArmor(armor, new Vector2(location.X + 270, location.Y + 15));
                    DrawAddedStats(armor, new Vector2(location.X + 270, location.Y + 80));
                }
            }
            
        }


        /// <summary>
        /// Draws the highlighted items stats
        /// </summary>
        private void DrawHighlighted()
        {
            if (equippable.Count > 0 && currentSelection != itemsInInventory)
            {
                Rectangle highlightedBackground = new Rectangle((int)location.X + 260, (int)location.Y + 260, 300, 250);
                Globals.Batch.Draw(blueBackground, highlightedBackground, Color.White);

                Globals.Batch.DrawString(Globals.Font,
                    "Selected Item",
                    new Vector2(location.X + 270, location.Y + 260),
                    Color.White);



                if (equippable[currentSelection].Slot == EquipmentSlot.Weapon && currentSelection != itemsInInventory - 1)
                {
                    Weapon weapon = equippable[currentSelection] as Weapon;
                    DrawWeapon(weapon, new Vector2(location.X + 270, location.Y + 275));
                }

                else if (equippable[currentSelection].Slot == EquipmentSlot.Accessory)
                {
                    Accessory accessory = equippable[currentSelection] as Accessory;
                    DrawAccessory(accessory, new Vector2(location.X + 270, location.Y + 275));
                }

                else if (equippable[currentSelection].Slot == EquipmentSlot.Arms ||
               equippable[currentSelection].Slot == EquipmentSlot.Belt ||
               equippable[currentSelection].Slot == EquipmentSlot.Boots ||
               equippable[currentSelection].Slot == EquipmentSlot.Chest ||
               equippable[currentSelection].Slot == EquipmentSlot.Gloves ||
               equippable[currentSelection].Slot == EquipmentSlot.Helmet ||
               equippable[currentSelection].Slot == EquipmentSlot.Legs ||
               equippable[currentSelection].Slot == EquipmentSlot.Shield ||
               equippable[currentSelection].Slot == EquipmentSlot.Shoulders)
                {
                    Armor armor = equippable[currentSelection] as Armor;
                    DrawArmor(armor, new Vector2(location.X + 270, location.Y + 275));

                }

                DrawAddedStats(equippable[currentSelection], new Vector2(location.X + 270, location.Y + 335));
            }
        }

        private void DrawWeapon(Weapon gear, Vector2 location)
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

            Globals.Batch.DrawString(
                Globals.Font,
                "Damage: " + gear.Damage.ToString(),
                new Vector2(location.X + 60, location.Y + 45),
                Color.White);

            Globals.Batch.DrawString(
                Globals.Font,
                "Delay: " + gear.Damage.ToString(),
                new Vector2(location.X + 60, location.Y + 60),
                Color.White);
                      
        }

        private void DrawAccessory(Accessory gear, Vector2 location)
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

        private void DrawArmor(Armor gear, Vector2 location)
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

            Globals.Batch.DrawString(
                Globals.Font,
                gear.Description,
                new Vector2(location.X + 75, location.Y + 15),
                Color.White);

        }

        private void DrawAddedStats(Gear gear, Vector2 location)
        {
            Globals.Batch.DrawString(Globals.Font, "Added Stats: ", new Vector2(location.X + 80, location.Y + 15), Color.White);

            Globals.Batch.DrawString(Globals.Font, "Health: " + gear.AddedStats.Health.ToString(), new Vector2(location.X, location.Y + 30), Color.White);
            Globals.Batch.DrawString(Globals.Font, "Mana: " + gear.AddedStats.Mana.ToString(), new Vector2(location.X, location.Y + 45), Color.White);
            Globals.Batch.DrawString(Globals.Font, "Haste: " + gear.AddedStats.Haste.ToString(), new Vector2(location.X, location.Y + 60), Color.White);

            Globals.Batch.DrawString(Globals.Font, "STR: " + gear.AddedStats.Strength.ToString(), new Vector2(location.X + 150, location.Y + 30), Color.White);
            Globals.Batch.DrawString(Globals.Font, "STA: " + gear.AddedStats.Stamina.ToString(), new Vector2(location.X + 150, location.Y + 45), Color.White);
            Globals.Batch.DrawString(Globals.Font, "INT: " + gear.AddedStats.Intelligence.ToString(), new Vector2(location.X + 150, location.Y + 60), Color.White);
            Globals.Batch.DrawString(Globals.Font, "WIS: " + gear.AddedStats.Wisdom.ToString(), new Vector2(location.X + 150, location.Y + 75), Color.White);
            Globals.Batch.DrawString(Globals.Font, "AGI: " + gear.AddedStats.Agility.ToString(), new Vector2(location.X + 150, location.Y + 90), Color.White);
            Globals.Batch.DrawString(Globals.Font, "DEX: " + gear.AddedStats.Dexterity.ToString(), new Vector2(location.X + 150, location.Y + 105), Color.White);
        }
        #endregion

        private bool IsItemEquippable(Item item)
        {
            if (item.ItemType == ItemType.Gear)
            {
                Gear gear = item as Gear;

                if (gear.Slot == slot)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
