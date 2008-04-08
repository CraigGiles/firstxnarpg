using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TileEngine;

namespace ShackRPG.GameScreens
{
    class InventoryScreen : IGameScreen
    {
        BaseGame game;
        SpriteBatch batch;
        SpriteFont font;

        Player player;

        Rectangle rPlayerHud = new Rectangle(625, 25, 200, 125),
            rPlayerEquipment = new Rectangle(25, 25, 200, 200),
            rPlayerInventory = new Rectangle(25, 225, 200, 350);

        Texture2D
            tHandIcon,
            tIconBorder,
            tPlayerBarProgress,
            tPlayerBarBorder,
            tPlayerHudBorder,
            tHudStrip,
            tDimScreen;

        public InventoryScreen(Player character, BaseGame game, SpriteBatch batch, SpriteFont font, ContentManager content)
        {
            this.player = character;
            this.game = game;
            this.batch = batch;
            this.font = font;

            Initialize(batch, content);
        }

        private void Initialize(SpriteBatch batch, ContentManager content)
        {
            tHandIcon = content.Load<Texture2D>(@"Battle/Hand");

            tIconBorder = content.Load<Texture2D>(@"MapEditor/TexturePreview");

            tDimScreen = content.Load<Texture2D>(@"DimBackground");

            tPlayerBarProgress = content.Load<Texture2D>(@"Battle/PlayerBarProgress");
            tPlayerBarBorder = content.Load<Texture2D>(@"Battle/PlayerBarBorder");
            tPlayerHudBorder = content.Load<Texture2D>(@"Battle/PlayerHudBackground");
            tHudStrip = content.Load<Texture2D>(@"Battle/HudBorder");
        }

        public void Update(GameTime gameTime)
        {
            if (Input.Escape)
                game.RemoveCurrentGameScreen();
        }

        public void Draw(GameTime gameTime)
        {

            batch.Begin(SpriteBlendMode.AlphaBlend);

            batch.Draw(tDimScreen,
                new Rectangle(0, 0, 800, 600),
                Color.White);

            DrawCharacterStats();
            DrawCharacterEquipment();
            DrawCharacterInventory();


            batch.End();
        }

        private void DrawCharacterStats()
        {

            //Player portrait
            batch.Draw(player.SpriteSheet,
               new Rectangle((rPlayerHud.X) + 2, rPlayerHud.Y + 2, player.Portrait.Width, player.Portrait.Height),
               player.Portrait, Color.White);

            //Player Name
            batch.DrawString(font, player.Name, new Vector2((rPlayerHud.X) + player.Portrait.Width + 5, rPlayerHud.Y),
                Color.White, 0.0f, Vector2.Zero, .60f, SpriteEffects.None, 0f);

            #region Health
            //Player health as value
            batch.DrawString(font, player.Health.ToString() + "/" + player.MaxHealth.ToString(),
                new Vector2((rPlayerHud.X) + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height - 2),
                Color.White, 0.0f, Vector2.Zero, .60f, SpriteEffects.None, 0f);

            //Player health bar
            for (int i = 0; i < player.HealthPercentValue; i++)
            {
                int percentOffset = i;
                batch.Draw(tPlayerBarProgress,
                    new Rectangle((rPlayerHud.X) + percentOffset + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height + 17, 1, 10),
                    null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, .2f);
            }

            //Player Health Bar Border
            batch.Draw(tPlayerBarBorder,
                new Rectangle((rPlayerHud.X) + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height + 17, 100, 10),
                null, Color.DarkGray, 0f, Vector2.Zero, SpriteEffects.None, .1f);

            #endregion

            #region Mana
            //Player mana as value
            batch.DrawString(font, player.Mana.ToString() + "/" + player.MaxMana.ToString(),
                new Vector2((rPlayerHud.X) + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height + 25),
                Color.White, 0.0f, Vector2.Zero, .60f, SpriteEffects.None, 0f);

            //Player mana bar
            for (int i = 0; i < (int)player.ManaPercentValue; i++)
            {
                int percentOffset = i;
                batch.Draw(tPlayerBarProgress,
                    new Rectangle((rPlayerHud.X) + percentOffset + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height + 45, 1, 10),
                    null, Color.Blue, 0f, Vector2.Zero, SpriteEffects.None, .2f);
            }

            //Player mana bar border
            batch.Draw(tPlayerBarBorder,
                new Rectangle((rPlayerHud.X) + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height + 45, 100, 10),
                null, Color.DarkGray, 0f, Vector2.Zero, SpriteEffects.None, .1f);

            #endregion

            //Level
            batch.DrawString(font, "Level: " + player.Level.ToString(),
                new Vector2(rPlayerHud.X+ 5, rPlayerHud.Y + 100),
                Color.White, 0f, Vector2.Zero, .50f, SpriteEffects.None, 0f);

            //Exp
            batch.DrawString(font, "Exp: " + player.Level.ToString() + "/" + player.ExpToLevel.ToString(),
                new Vector2(rPlayerHud.X + 5, rPlayerHud.Y + 120),
                Color.White, 0f, Vector2.Zero, .50f, SpriteEffects.None, 0f);

            //Gold
            batch.DrawString(font, "Gold: " + player.Gold.ToString(),
                new Vector2(rPlayerHud.X + 5, rPlayerHud.Y + 140),
                Color.White, 0f, Vector2.Zero, .50f, SpriteEffects.None, 0f);

            //Strength
            batch.DrawString(font, "Strength: " + player.Strength.ToString(),
                new Vector2(rPlayerHud.X +5, rPlayerHud.Y + 160),
                Color.White, 0f, Vector2.Zero, .50f, SpriteEffects.None, 0f);

            //Armor
            batch.DrawString(font, "Armor: " + player.Armor.ToString(),
                new Vector2(rPlayerHud.X + 5, rPlayerHud.Y + 180),
                Color.White, 0f, Vector2.Zero, .50f, SpriteEffects.None, 0f);





        }

        private void DrawCharacterEquipment()
        {
            #region Weapon
            batch.Draw(player.CurrentWeapon.Texture,
                new Vector2(rPlayerEquipment.X, rPlayerEquipment.Y),
                Color.White);

            batch.DrawString(font, player.CurrentWeapon.Name,
                new Vector2(rPlayerEquipment.X + 30, rPlayerEquipment.Y),
                Color.White, 0f, Vector2.Zero, .50f, SpriteEffects.None, 0f);

            batch.DrawString(font, "DMG: " + player.CurrentWeapon.Damage.ToString() + "  Delay: " + player.CurrentWeapon.Delay.ToString(),
                new Vector2(rPlayerEquipment.X + 30, rPlayerEquipment.Y + 15),
                Color.White, 0f, Vector2.Zero, .40f, SpriteEffects.None, 0f);

            #endregion

            #region Armor

            batch.Draw(player.CurrentArmor.Texture,
                new Vector2(rPlayerEquipment.X, rPlayerEquipment.Y + 35),
                Color.White);

            batch.DrawString(font, player.CurrentArmor.Name,
                new Vector2(rPlayerEquipment.X + 30, rPlayerEquipment.Y + 35),
                Color.White, 0f, Vector2.Zero, .50f, SpriteEffects.None, 0f);

            batch.DrawString(font, "Defense: " + player.CurrentArmor.Defense.ToString(),
                new Vector2(rPlayerEquipment.X + 30, rPlayerEquipment.Y + 50),
                Color.White, 0f, Vector2.Zero, .40f, SpriteEffects.None, 0f);

            #endregion

            #region Accessory

            batch.Draw(player.CurrentAccessory.Texture,
                new Vector2(rPlayerEquipment.X, rPlayerEquipment.Y + 70),
                Color.White);

            batch.DrawString(font, player.CurrentAccessory.Name,
                new Vector2(rPlayerEquipment.X + 30, rPlayerEquipment.Y + 70),
                Color.White, 0f, Vector2.Zero, .50f, SpriteEffects.None, 0f);

            batch.DrawString(font, "Effect: " + player.CurrentAccessory.Effect,
                new Vector2(rPlayerEquipment.X + 30, rPlayerEquipment.Y + 85),
                Color.White, 0f, Vector2.Zero, .40f, SpriteEffects.None, 0f);

            #endregion
        }

        private void DrawCharacterInventory()
        {
        }
    }
}
