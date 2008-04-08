using System;
using System.Collections.Generic;
using System.Text;

using TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ShackRPG.GameScreens;
namespace ShackRPG
{
    public class Inventory
    {
        #region Variables

        /// <summary>
        /// A list of all inventory items inside the characters inventory
        /// </summary>
        List<BaseObjects> _Inventory = new List<BaseObjects>();

        /// <summary>
        /// Characters current weapon
        /// </summary>
        Weapon _EquippedWeapon = new Weapon();

        /// <summary>
        /// Characters current armor
        /// </summary>
        Armor _EquippedArmor = new Armor();

        /// <summary>
        /// Characters current accessory
        /// </summary>
        Accessory _EquippedAccessory = new Accessory();

        #endregion

        #region Properties

        /// <summary>
        /// List of all objects in characters inventory
        /// </summary>
        public List<BaseObjects> CurrentInventory
        {
            get { return _Inventory; }
        }

        /// <summary>
        /// Current weapon equipped by the character
        /// </summary>
        public Weapon EquippedWeapon
        {
            get { return _EquippedWeapon; }
            set { _EquippedWeapon = value; }
        }

        /// <summary>
        /// Current armor equipped by the character
        /// </summary>
        public Armor EquippedArmor
        {
            get { return _EquippedArmor; }
            set { _EquippedArmor = value; }
        }

        /// <summary>
        /// Current accessory equipped by the character
        /// </summary>
        public Accessory EquippedAccessory
        {
            get { return _EquippedAccessory; }
            set { _EquippedAccessory = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an item to characters inventory
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void AddItemToInventory(BaseObjects item)
        {
            _Inventory.Add(item);
        }

        /// <summary>
        /// Removes an item from characters inventory
        /// </summary>
        /// <param name="item">Item to be removed</param>
        public void DeleteItemFromInventory(BaseObjects item)
        {
            _Inventory.Remove(item);
        }

        /// <summary>
        /// Takes an weapon from characters inventory, and equips it
        /// </summary>
        /// <param name="item">Item to be equipped</param>
        public void EquipWeapon(Weapon item)
        {
            if (_EquippedWeapon != null)        //if something is equipped
                UnEquipWeapon(_EquippedWeapon);   //remove it before equipping new weapon

            _EquippedWeapon = item;             //equip new weapon
            _Inventory.Remove(item);            //remove from inventory list
        }

        /// <summary>
        /// Takes an Accessory from characters inventory, and equips it
        /// </summary>
        /// <param name="item">Item to be equipped</param>
        public void EquipAccessory(Accessory item)
        {
            if (_EquippedAccessory != null)
                UnEquipAccessory(_EquippedAccessory);

            _EquippedAccessory = item;
            _Inventory.Remove(item);
        }

        /// <summary>
        /// Takes an Armor from characters inventory, and equips it
        /// </summary>
        /// <param name="item">Item to be equipped</param>
        public void EquipArmor(Armor item)
        {
            if (_EquippedArmor != null)
                UnEquipArmor(_EquippedArmor);

            _EquippedArmor = item;
            _Inventory.Remove(item);
        }

        /// <summary>
        /// Unequips the current weapon and places it back into inventory
        /// </summary>
        /// <param name="item">Weapon to be unequipped</param>
        public void UnEquipWeapon(Weapon item)
        {
            _EquippedWeapon = null;
            _Inventory.Add(item);
        }

        /// <summary>
        /// Unequips the current Armor and places it back into inventory
        /// </summary>
        /// <param name="item">Armor to be unequipped</param>
        public void UnEquipArmor(Armor item)
        {
            _EquippedArmor = null;
            _Inventory.Add(item);
        }

        /// <summary>
        /// Unequips the current Accessory and places it back into inventory
        /// </summary>
        /// <param name="item">Accessory to be unequipped</param>
        public void UnEquipAccessory(Accessory item)
        {
            _EquippedAccessory = null;
            _Inventory.Add(item);
        }
        
        #endregion

        #region Draw Methods
        public void DrawEquippedWeapon(SpriteBatch batch, SpriteFont font, Weapon ItemToDraw, Rectangle DrawRectangle, Color DrawColor)
        {
            batch.Draw(ItemToDraw.Texture, DrawRectangle, DrawColor);

            // name
            batch.DrawString(
                font,
                ItemToDraw.Name,
                new Vector2(DrawRectangle.X + 30, DrawRectangle.Y),
                DrawColor,
                0f,
                Vector2.Zero,
                .50f,
                SpriteEffects.None,
                0f);

            // damage
            batch.DrawString(
                font,
                "Damage: " + ItemToDraw.Damage.ToString(),
                new Vector2(DrawRectangle.X + 35, DrawRectangle.Y + 20),
                DrawColor,
                0f,
                Vector2.Zero,
                .50f,
                SpriteEffects.None,
                0f);

            // delay
            batch.DrawString(
                font,
                "Delay: " + ItemToDraw.Delay.ToString(),
                new Vector2(DrawRectangle.X + 55, DrawRectangle.Y + 20),
                DrawColor,
                0f,
                Vector2.Zero,
                .50f,
                SpriteEffects.None,
                0f);

        }

        public void DrawEquippedArmor(SpriteBatch batch, SpriteFont font, Armor ItemToDraw, Rectangle DrawRectangle, Color DrawColor)
        {
            batch.Draw(ItemToDraw.Texture, DrawRectangle, DrawColor);

            // name
            batch.DrawString(
                font,
                ItemToDraw.Name,
                new Vector2(DrawRectangle.X + 30, DrawRectangle.Y),
                DrawColor,
                0f,
                Vector2.Zero,
                .50f,
                SpriteEffects.None,
                0f);

            // damage
            batch.DrawString(
                font,
                "Defense: " + ItemToDraw.Defense.ToString(),
                new Vector2(DrawRectangle.X + 35, DrawRectangle.Y + 20),
                DrawColor,
                0f,
                Vector2.Zero,
                .50f,
                SpriteEffects.None,
                0f);
        }

        public void DrawEquippedAccessory(SpriteBatch batch, SpriteFont font, Accessory ItemToDraw, Rectangle DrawRectangle, Color DrawColor)
        {
            batch.Draw(ItemToDraw.Texture, DrawRectangle, DrawColor);

            // name
            batch.DrawString(
                font,
                ItemToDraw.Name,
                new Vector2(DrawRectangle.X + 30, DrawRectangle.Y),
                DrawColor,
                0f,
                Vector2.Zero,
                .50f,
                SpriteEffects.None,
                0f);

            batch.DrawString(
                font,
                ItemToDraw.Effect,
                new Vector2(DrawRectangle.X + 35, DrawRectangle.Y + 20),
                DrawColor,
                0f,
                Vector2.Zero,
                .50f,
                SpriteEffects.None,
                0f);
        }

        public void DrawInventoryList(SpriteBatch batch, SpriteFont font, Rectangle DrawRectangle)
        {
            int offsetY = 15;
            int offset = 0;

            foreach (BaseObjects item in _Inventory)
            {
                batch.Draw(item.Texture, DrawRectangle, Color.White);
                batch.DrawString(font, item.Name,
                    new Vector2(DrawRectangle.X + 30, DrawRectangle.Y + (offsetY * offset)),
                    Color.White);

                offset++;
            }
        }
        #endregion
    }
}
