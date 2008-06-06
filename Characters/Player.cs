using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

        }

        #endregion


        #region Save / Load

        //public void Load()
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(@"Content\Characters\Player\Player.xml");

        //    foreach (XmlNode rootElement in doc.ChildNodes)
        //    {
        //        foreach (XmlNode node in rootElement.ChildNodes)
        //        {
        //            if (node.Name == "Asset")
        //                SetAsset(node);

        //            if (node.Name == "Stats")
        //                SetStats(node);

        //            if (node.Name == "Inventory")
        //                SetInventory(node);

        //            if (node.Name == "Equipment")
        //                SetEquipment(node);

        //            if (node.Name == "Animations")
        //                SetAnimations(node); 
        //        }
        //    }
        //}

        //private void SetAsset(XmlNode node)
        //{
        //    AssetName = node.InnerText;
        //}

        //private void SetStats(XmlNode node)
        //{
        //    foreach (XmlNode n in node.ChildNodes)
        //    {
        //        if (n.Name == "Health")
        //            BaseStats.BaseHealth = int.Parse(n.InnerText);

        //        else if (n.Name == "Mana")
        //            BaseStats.BaseMana = int.Parse(n.InnerText);

        //        else if (n.Name == "Strength")
        //            BaseStats.Strength = int.Parse(n.InnerText);

        //        else if (n.Name == "Stamina")
        //            BaseStats.Stamina = int.Parse(n.InnerText);

        //        else if (n.Name == "Agility")
        //            BaseStats.Agility = int.Parse(n.InnerText);

        //        else if (n.Name == "Dexterity")
        //            BaseStats.Dexterity = int.Parse(n.InnerText);

        //        else if (n.Name == "Intelligence")
        //            BaseStats.Intelligence = int.Parse(n.InnerText);

        //        else if (n.Name == "Wisdom")
        //            BaseStats.Wisdom = int.Parse(n.InnerText);
        //    }
        //}

        //private void SetInventory(XmlNode node)
        //{
        //    foreach (XmlNode n in node.ChildNodes)
        //    {
        //        if (n.Name == "Gold")
        //            Gold = int.Parse(n.InnerText);

        //        else if (n.Name == "Item")
        //            Inventory.AddItem(new Item(n.InnerText));
        //    }
        //}

        //private void SetEquipment(XmlNode node)
        //{
        //    foreach (XmlNode n in node.ChildNodes)
        //    {
        //        if (n.Name == "Weapon")
        //            Equipment.Equip(new Weapon(n.InnerText));

        //        if (n.Name == "Armor")
        //            Equipment.Equip(new Armor(n.InnerText));

        //        if (n.Name == "Accessory")
        //            Equipment.Equip(new Accessory(n.InnerText));
        //    }
        //}

        //private void SetAnimations(XmlNode node)
        //{
        //}

        #endregion


        #region Add Items to inventory


        public void AddItemToInventory(string asset)
        {
        }


        public void AddItemToInventory(Item item)
        {
        }


        public void AddItemToInventory(Equipment equipment)
        {
        }


        #endregion

        
        #region Update


        /// <summary>
        /// Handles the update logic for the player character
        /// </summary>
        public override void Update()
        {
            CheckForMovement();
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
                Globals.Input.IsKeyHeld(Globals.Settings.MoveRight))
            {

                //if move, adjust the players position
                Origin = Globals.TileEngine.CalculateCharacterMovement(
                    Globals.Input.IsKeyHeld(Globals.Settings.MoveDown),
                    Globals.Input.IsKeyHeld(Globals.Settings.MoveLeft),
                    Globals.Input.IsKeyHeld(Globals.Settings.MoveUp),
                    Globals.Input.IsKeyHeld(Globals.Settings.MoveRight),
                    Origin,
                    MovementSpeedModifier);

                //if player is on portal to another map, return new position
                Origin = Globals.TileEngine.CheckForPortalEntry(Origin);

            }//end if

        }


        #endregion


        #region Draw


        /// <summary>
        /// Handles the draw logic for the player character
        /// </summary>
        public override void Draw()
        {
            Globals.Batch.Draw(SpriteSheet, CurrentPosition, Color.White);
        }


        #endregion
    
    }
}
