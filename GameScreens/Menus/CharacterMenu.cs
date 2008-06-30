using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ActionRPG
{
    public class CharacterMenu : GameScreen
    {
        Vector2 location;
        Player player;

        Texture2D
            button,
            handIcon,
            characterBackground,
            blueBackground;


        #region Constructor(s)

        public CharacterMenu(Vector2 menuLocation, Player player)
        {
            this.location = menuLocation;
            this.player = player;

            characterEquipmentRect.X = (int)menuLocation.X;
            characterEquipmentRect.Y = (int)menuLocation.Y;
        }

        public override void LoadContent()
        {
            button = Globals.Content.Load<Texture2D>(@"Graphics/Button");
            handIcon = Globals.Content.Load<Texture2D>(@"Graphics/hand");
            characterBackground = Globals.Content.Load<Texture2D>(@"Graphics/CharacterEquipment");
            blueBackground = Globals.Content.Load<Texture2D>(@"Graphics/blueBackground");

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            button = null;
            handIcon = null;
            characterBackground = null;
            blueBackground = null;

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
        }


        #endregion


        #region Draw

        Rectangle characterEquipmentRect = new Rectangle(0, 0, 300, 450);


        public override void Draw()
        {
            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend);

            DrawCharacterEquipment();
            DrawCharacterStats();

            Globals.Batch.End();

            base.Draw();
        }

        Rectangle
            weapon = new Rectangle(44, 75, 50, 50),
            helmet = new Rectangle(137, 65, 50, 50),
            gloves = new Rectangle(72, 159, 50, 50),
            chest = new Rectangle(124, 135, 50, 50),
            shoulders = new Rectangle(179, 117, 50, 50),
            belt = new Rectangle(125, 196, 50, 50),
            arms = new Rectangle(190, 178, 50, 50),
            shield = new Rectangle(183, 230, 50, 50),
            legs = new Rectangle(165, 329, 50, 50),
            boots = new Rectangle(170, 415, 50, 50),
            accessory = new Rectangle(265, 182, 50, 50);

        private void DrawCharacterEquipment()
        {
            Globals.Batch.Draw(characterBackground, characterEquipmentRect, Color.White);

            if (player.Inventory.Equipment.Weapon != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Weapon.GraphicIcon, weapon, Color.White);

            if (player.Inventory.Equipment.Helmet != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Helmet.GraphicIcon, helmet, Color.White);

            if (player.Inventory.Equipment.Gloves != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Gloves.GraphicIcon, gloves, Color.White);

            if (player.Inventory.Equipment.Chest != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Chest.GraphicIcon, chest, Color.White);

            if (player.Inventory.Equipment.Shoulders != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Shoulders.GraphicIcon, shoulders, Color.White);

            if (player.Inventory.Equipment.Belt != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Belt.GraphicIcon, belt, Color.White);

            if (player.Inventory.Equipment.Arms != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Arms.GraphicIcon, arms, Color.White);

            if (player.Inventory.Equipment.Shield != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Shield.GraphicIcon, shield, Color.White);

            if (player.Inventory.Equipment.Legs != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Legs.GraphicIcon, legs, Color.White);

            if (player.Inventory.Equipment.Boots != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Boots.GraphicIcon, boots, Color.White);

            if (player.Inventory.Equipment.Accessory != null)
                Globals.Batch.Draw(player.Inventory.Equipment.Accessory.GraphicIcon, accessory, Color.White);

        }

        private void DrawCharacterStats()
        {
            Rectangle statsRect = new Rectangle(characterEquipmentRect.X + characterEquipmentRect.Width + 10,
                    characterEquipmentRect.Y, characterEquipmentRect.Width, characterEquipmentRect.Height);

            Vector2
                healthText = new Vector2(statsRect.X + 5, statsRect.Y + 5),
                manaText = new Vector2(statsRect.X + 5, statsRect.Y + 20);

            Vector2
                goldText = new Vector2(statsRect.X + 5, statsRect.Y + 50),
                attackPowerText = new Vector2(statsRect.X + 5, statsRect.Y + 80),
                attackRangeText = new Vector2(statsRect.X + 5, statsRect.Y + 95),
                physicalDefenseText = new Vector2(statsRect.X + 5, statsRect.Y + 110),
                magicalDefenseText = new Vector2(statsRect.X + 5, statsRect.Y + 125);

            Vector2
                strengthText = new Vector2(statsRect.X + 5, statsRect.Y + 155),
                staminaText = new Vector2(statsRect.X + 5, statsRect.Y + 170),
                intelligenceText = new Vector2(statsRect.X + 5, statsRect.Y + 185),
                wisdomText = new Vector2(statsRect.X + 5, statsRect.Y + 200),
                agilityText = new Vector2(statsRect.X + 5, statsRect.Y + 215),
                dexterityText = new Vector2(statsRect.X + 5, statsRect.Y + 230);

            
            int agi = player.BaseStats.Agility + player.Inventory.Equipment.AddedAgility;
            int dex = player.BaseStats.Dexterity + player.Inventory.Equipment.AddedDexterity;
            int intel = player.BaseStats.Intelligence + player.Inventory.Equipment.AddedIntelligence;
            int sta = player.BaseStats.Stamina + player.Inventory.Equipment.AddedStamina;
            int str = player.BaseStats.Strength + player.Inventory.Equipment.AddedStrength;
            int wis = player.BaseStats.Wisdom + player.Inventory.Equipment.AddedWisdom;




            Globals.Batch.Draw(blueBackground, statsRect, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Health: " + player.Health.ToString() + " / " + player.BaseStats.MaxHealth.ToString(), healthText, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Mana: " + player.Mana.ToString() + " / " + player.BaseStats.MaxMana.ToString(), manaText, Color.White);

            Globals.Batch.DrawString(Globals.Font, "Gold: " + player.Inventory.Gold.ToString(), goldText, Color.White);

            Globals.Batch.DrawString(Globals.Font, "Strength: " + str.ToString(), strengthText, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Stamina: " + sta.ToString(), staminaText, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Intelligence: " + intel.ToString(), intelligenceText, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Wisdom: " + wis.ToString(), wisdomText, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Agility: " + agi.ToString(), agilityText, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Dexterity: " + dex.ToString(), dexterityText, Color.White);


            Globals.Batch.DrawString(Globals.Font, "Attack Power: " + player.AttackPower.ToString(), attackPowerText, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Attack Range: " + player.AttackRange.ToString(), attackRangeText, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Physical Defense: " + player.Defense.ToString(), physicalDefenseText, Color.White);
            Globals.Batch.DrawString(Globals.Font, "Magical Defense: " + player.MagicalDefense.ToString(), magicalDefenseText, Color.White);

        }

        #endregion

    }
}
