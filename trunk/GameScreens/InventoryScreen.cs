using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ActionRPG
{
    public class InventoryScreen : GameScreen
    {
        Player player;

        enum InventoryAction
        {
            Equip,
            Unequip,
            UseItem,
            DiscardItem,            
        }

        #region Constructor(s)


        public InventoryScreen(Player player)
        {
            this.player = player;
            this.Name = "Inventory";
        }


        public override void LoadContent()
        {
            charEquipmentBackground = Globals.Content.Load<Texture2D>(@"Graphics/CharacterEquipment");
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
            if (Globals.Input.IsKeyHeld(Globals.Settings.Cancel) ||
                Globals.Input.IsKeyPressed(Globals.Settings.Inventory))
            {
                ScreenManager.RemoveScreen(this);
            }

            CheckButtonPresses();

            base.Update(coveredByOtherScreen);
        }


        #endregion


        /// <summary>
        /// Checks for a button press and executes
        /// command
        /// </summary>
        private void CheckButtonPresses()
        {

        }

        #region Draw


        /// <summary>
        /// Draw hub, draws all objects to screen
        /// </summary>
        public override void Draw()
        {
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend);

            DrawTopBar();
            DrawStats();
            DrawCharacterSheet();
            DrawInventory();

            Globals.Batch.End();
            base.Draw();
        }



        #region Top Bar


        Vector2
            equipRect = new Vector2(350, 10),
            unEquipRect = new Vector2(450, 10),
            useItemRect = new Vector2(550, 10),
            discardItemRect = new Vector2(650, 10);


        /// <summary>
        /// Draws the top bar portion of the inventory screen
        /// </summary>
        public void DrawTopBar()
        {
            Globals.Batch.DrawString(Globals.Font,
                "Equip",
                equipRect,
                Color.White);

            Globals.Batch.DrawString(Globals.Font,
                "Unequip",
                unEquipRect,
                Color.White);

            Globals.Batch.DrawString(Globals.Font,
                "Use Item",
                useItemRect,
                Color.White);

            Globals.Batch.DrawString(Globals.Font,
                "Discard",
                discardItemRect,
                Color.White);
        }

        #endregion


        #region Stats


        Rectangle statsRect = new Rectangle(10, 10, 300, 120);


        /// <summary>
        /// Draws the character stats to screen
        /// </summary>
        private void DrawStats()
        {
            int agility = player.BaseStats.Agility + player.Inventory.Equipment.AddedAgility;
            int dexterity = player.BaseStats.Dexterity + player.Inventory.Equipment.AddedDexterity;
            int intelligence = player.BaseStats.Intelligence + player.Inventory.Equipment.AddedIntelligence;
            int stamina = player.BaseStats.Stamina + player.Inventory.Equipment.AddedStamina;
            int strength = player.BaseStats.Strength + player.Inventory.Equipment.AddedStrength;
            int wisdom = player.BaseStats.Wisdom + player.Inventory.Equipment.AddedWisdom;


            Globals.Batch.Draw(charEquipmentBackground, statsRect, Color.Black);
        
            DrawString("Health: " + player.Health.ToString() + " / " + player.BaseStats.MaxHealth.ToString(), new Vector2(statsRect.X + 8, statsRect.Y + 2));
            DrawString("Mana: " + player.Mana.ToString() + " / " + player.BaseStats.MaxMana.ToString(), new Vector2(statsRect.X + 8, statsRect.Y + 13));

            DrawString("AGI: " + agility.ToString(), new Vector2(statsRect.X + 8, statsRect.Y + 34));
            DrawString("DEX: " + dexterity.ToString(), new Vector2(statsRect.X + 8, statsRect.Y + 45));
            DrawString("INT: " + intelligence.ToString(), new Vector2(statsRect.X + 8, statsRect.Y + 56));
            DrawString("STA: " + stamina.ToString(), new Vector2(statsRect.X + 8, statsRect.Y + 67));
            DrawString("STR: " + strength.ToString(), new Vector2(statsRect.X + 8, statsRect.Y + 78));
            DrawString("WIS: " + wisdom.ToString(), new Vector2(statsRect.X + 8, statsRect.Y + 89));

            DrawString("Gold: " + player.Inventory.Gold.ToString(), new Vector2(statsRect.X + 162, statsRect.Y + 2));

            DrawString("ATK: " + player.AttackPower.ToString(), new Vector2(statsRect.X + 162, statsRect.Y + 22));
            DrawString("RNG: " + player.AttackRange.ToString(), new Vector2(statsRect.X + 162, statsRect.Y + 33));

            DrawString("PHY DEF: " + player.Defense.ToString(), new Vector2(statsRect.X + 162, statsRect.Y + 55));
            DrawString("MGK DEF: " + player.MagicalDefense.ToString(), new Vector2(statsRect.X + 162, statsRect.Y + 66));
        }

        #endregion
        

        #region Character Sheet

        Texture2D charEquipmentBackground;
        Rectangle charEquipmentRect = new Rectangle(10, 140, 300, 450);


        /// <summary>
        /// Draws the character sheet / equipment
        /// </summary>
        public void DrawCharacterSheet()
        {
            Rectangle
                weaponRect = new Rectangle(9 + charEquipmentRect.X, 55 + charEquipmentRect.Y, 50, 50),
                helmetRect = new Rectangle(102 + charEquipmentRect.X, 45 + charEquipmentRect.Y, 50, 50),
                glovesRect = new Rectangle(38 + charEquipmentRect.X, 135 + charEquipmentRect.Y, 50, 50),
                chestRect = new Rectangle(88 + charEquipmentRect.X, 115 + charEquipmentRect.Y, 50, 50),
                shouldersRect = new Rectangle(144 + charEquipmentRect.X, 97 + charEquipmentRect.Y, 50, 50),
                beltRect = new Rectangle(90 + charEquipmentRect.X, 176 + charEquipmentRect.Y, 50, 50),
                armsRect = new Rectangle(155 + charEquipmentRect.X, 158 + charEquipmentRect.Y, 50, 50),
                shieldRect = new Rectangle(148 + charEquipmentRect.X, 210 + charEquipmentRect.Y, 50, 50),
                legsRect = new Rectangle(130 + charEquipmentRect.X, 308 + charEquipmentRect.Y, 50, 50),
                bootsRect = new Rectangle(135 + charEquipmentRect.X, 395 + charEquipmentRect.Y, 50, 50),
                accessoryRect = new Rectangle(226 + charEquipmentRect.X, 162 + charEquipmentRect.Y, 50, 50);


            Globals.Batch.Draw(charEquipmentBackground, charEquipmentRect, Color.White);

            if (player.Inventory.Equipment.Weapon != null)
                DrawGraphic(player.Inventory.Equipment.Weapon.GraphicIcon, weaponRect);

            if (player.Inventory.Equipment.Helmet != null)
                DrawGraphic(player.Inventory.Equipment.Helmet.GraphicIcon, helmetRect);

            if (player.Inventory.Equipment.Gloves != null)
                DrawGraphic(player.Inventory.Equipment.Gloves.GraphicIcon, glovesRect);

            if (player.Inventory.Equipment.Chest != null)
                DrawGraphic(player.Inventory.Equipment.Chest.GraphicIcon, chestRect);

            if (player.Inventory.Equipment.Shoulders != null)
                DrawGraphic(player.Inventory.Equipment.Shoulders.GraphicIcon, shouldersRect);

            if (player.Inventory.Equipment.Belt != null)
                DrawGraphic(player.Inventory.Equipment.Belt.GraphicIcon, beltRect);

            if (player.Inventory.Equipment.Arms != null)
                DrawGraphic(player.Inventory.Equipment.Arms.GraphicIcon, armsRect);

            if (player.Inventory.Equipment.Shield != null)
                DrawGraphic(player.Inventory.Equipment.Shield.GraphicIcon, shieldRect);

            if (player.Inventory.Equipment.Legs != null)
                DrawGraphic(player.Inventory.Equipment.Legs.GraphicIcon, legsRect);

            if (player.Inventory.Equipment.Boots != null)
                DrawGraphic(player.Inventory.Equipment.Boots.GraphicIcon, bootsRect);

            if (player.Inventory.Equipment.Accessory != null)
                DrawGraphic(player.Inventory.Equipment.Accessory.GraphicIcon, accessoryRect);

        }

        #endregion
        

        /// <summary>
        /// Draws the inventory list
        /// </summary>
        public void DrawInventory()
        {
            //Sets the offsets for items
            int offset = 0;
            int heightOffset = 40;

            //cycle thru items and draw
            foreach (Item item in player.Inventory.Items)
            {
                //draw item graphic
                DrawGraphic(item.GraphicIcon, new Rectangle(charEquipmentRect.Right + 25, 75 + (offset * heightOffset), 35, 35));

                //draw item name
                Globals.Batch.DrawString(Globals.Font, item.AssetName, 
                    new Vector2(charEquipmentRect.Right + 75, 80 + (offset * heightOffset)), Color.White);

                //draw item description
                Globals.Batch.DrawString(Globals.Font, item.Description, 
                    new Vector2(charEquipmentRect.Right + 75, 90 + (offset * heightOffset)), Color.White);
                offset++;
            }
        }


        /// <summary>
        /// Draws text
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <param name="location">Location to draw</param>
        private void DrawString(string text, Vector2 location)
        {
            Globals.Batch.DrawString(Globals.Font, text, location, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Draws graphic
        /// </summary>
        /// <param name="texture">Graphic to draw</param>
        /// <param name="rectangle">Rectangle used to draw</param>
        private void DrawGraphic(Texture2D texture, Rectangle rectangle)
        {
            Globals.Batch.Draw(texture, rectangle, Color.White);
        }


        #endregion
    }
}
