using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Helper;

namespace ActionRPG
{
    public class Character
    {
        #region Combat

        public Character CurrentTarget
        {
            get { return currentTarget; }
            set { currentTarget = value; }
        } 
        Character currentTarget;

        #endregion

        #region Animations + Sprite Manager


        /// <summary>
        /// Sprite manager for handling animation frames
        /// </summary>
        public SpriteManager SpriteManager
        {
            get { return spriteManager; }
        }
        SpriteManager spriteManager = new SpriteManager();


        /// <summary>
        /// Is the character animated
        /// </summary>
        public bool IsAnimated
        {
            get { return isAnimated; }
            set { isAnimated = value; }
        }
        bool isAnimated = false;


        #endregion


        ///// <summary>
        ///// Will the NPC attack if you if in range
        ///// </summary>
        //public bool Hostile
        //{
        //    get { return hostile; }
        //    set { hostile = value; }
        //}
        //bool hostile;


        public CharacterState CurrentCharacterState
        {
            get { return currentCharacterState; }
            set { currentCharacterState = value; }
        }
        CharacterState currentCharacterState = CharacterState.Alive;

        /// <summary>
        /// List of Gear items that can be dropped when killed
        /// </summary>
        public List<Gear> LootTable
        {
            get { return lootTable; }
            set { lootTable = value; }
        }
        List<Gear> lootTable = new List<Gear>();


        #region Character Actions


        /// <summary>
        /// Characters current action
        /// </summary>
        public Actions CurrentAction
        {
            get { return currentAction; }
            set { currentAction = value; }
        }
        Actions currentAction = Actions.Traveling;


        /// <summary>
        /// Characters previous action
        /// </summary>
        public Actions PreviousAction
        {
            get { return previousAction; }
            set { previousAction = value; }
        }
        Actions previousAction = Actions.Traveling;


        /// <summary>
        /// All timers associated with the character
        /// </summary>
        public Timers Timers
        {
            get { return timers; }
        }
        Timers timers = new Timers();


        #endregion


        #region Asset Name


        /// <summary>
        /// Asset name of character
        /// </summary>
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        string assetName;


        #endregion


        #region Stats


        /// <summary>
        /// Base statistics that make up the character
        /// </summary>
        public Stats BaseStats
        {
            get { return baseStats; }
            set { baseStats = value; }
        }
        Stats baseStats = new Stats();

        
        /// <summary>
        /// Characters current health
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = (int)MathHelper.Clamp(value, 0, BaseStats.MaxHealth); }
        }
        int health;


        /// <summary>
        /// Characters current mana
        /// </summary>
        public int Mana
        {
            get { return mana; }
            set { mana = (int)MathHelper.Clamp(value, 0, BaseStats.MaxMana); }
        }
        int mana;


        /// <summary>
        /// Movement Speed Modifier
        /// </summary>
        public float MovementSpeedModifier
        {
            get { return movementSpeedModifier; }
            set { movementSpeedModifier = value; }
        }
        float movementSpeedModifier = 1.0f;


        /// <summary>
        /// Returns characters current attack power
        /// </summary>
        public int AttackPower
        {
            get
            {
                return baseStats.Strength + Inventory.Equipment.AddedStrength;
            }
        }


        /// <summary>
        /// How much Defense a character has; 
        /// Armor + (Agility / 10)
        /// </summary>
        public int Defense
        {
            get
            {
                return (int)(Inventory.Equipment.Defense + ((BaseStats.Agility + Inventory.Equipment.AddedAgility) / 10));
            }
        }


        /// <summary>
        /// How much magical defense a character has
        /// Armor + (Wisdom / 10)
        /// </summary>
        public int MagicalDefense
        {
            get
            {
                return (int)(Inventory.Equipment.Defense + ((BaseStats.Wisdom + Inventory.Equipment.AddedWisdom) / 10));
            }
        }


        #endregion


        #region Equipment / Inventory


        /// <summary>
        /// Characters current inventory
        /// </summary>
        public Inventory Inventory
        {
            get { return inventory; }
        }
        Inventory inventory = new Inventory();


        #endregion


        #region Position Information


        /// <summary>
        /// Characters current position
        /// </summary>
        public Vector2 CurrentPosition
        {
            get { return new Vector2(spriteManager.SpriteBox.X, spriteManager.SpriteBox.Y); }
            set { spriteManager.UpdateSpriteBox(value); }
        }


        /// <summary>
        /// Returns characters last direction
        /// </summary>
        public Direction LastDirection
        {
            get { return SpriteManager.LastDirection; }
        }


        /// <summary>
        /// Center of sprite
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                return new Vector2(
                    CurrentPosition.X + (SpriteManager.SpriteDimensions.X / 2),
                    CurrentPosition.Y + (SpriteManager.SpriteDimensions.Y / 2));
            }
            set
            {
                CurrentPosition = new Vector2(
                    value.X - (SpriteManager.SpriteDimensions.X / 2),
                    value.Y - (SpriteManager.SpriteDimensions.Y / 2));
            }
        }


        /// <summary>
        /// Foot location of sprite. Bottom Center
        /// </summary>
        public Vector2 FootLocation
        {
            get
            {
                return new Vector2(
                    CurrentPosition.X + (SpriteManager.SpriteDimensions.X / 2),
                    CurrentPosition.Y + (SpriteManager.SpriteDimensions.Y - 5));
            }
        }


        /// <summary>
        /// Current velocity
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;


        /// <summary>
        /// Spawn location Vector2
        /// </summary>
        public Vector2 SpawnLocationVector
        {
            get { return spawnLocationVector; }
            set { spawnLocationVector = value; }
        }
        Vector2 spawnLocationVector;


        /// <summary>
        /// Spawn tile located on map
        /// </summary>
        public Point SpawnLocationTile
        {
            get { return spawnLocationTile; }
            set { spawnLocationTile = value; }
        }
        Point spawnLocationTile;


        /// <summary>
        /// Radius of the sprite
        /// </summary>
        public float CollisionRadius
        {
            get { return SpriteManager.SpriteBox.Width / 4; }
        }


        /// <summary>
        /// Radius of interaction with sprite
        /// </summary>
        public float InteractionRadius
        {
            get { return CollisionRadius * 3; }
        }


        ///// <summary>
        ///// Aggro radius of character
        ///// </summary>
        //public float AggroRadius
        //{
        //    get 
        //    {
        //        if (Hostile)
        //            return aggroRadius * CollisionRadius;
        //        else
        //            return 0f;
        //    }
        //    set { aggroRadius = value; }
        //}
        //float aggroRadius = 0.0f;


        /// <summary>
        /// Range of attack for current weapon
        /// </summary>
        public float AttackRange
        {
            get 
            { 
                if (inventory.Equipment.Weapon != null)
                    return Inventory.Equipment.Weapon.Range + spriteManager.SpriteBox.Width / 2; 
                else
                    return spriteManager.SpriteBox.Width / 2;
            }
        }


        #endregion


        #region Sprite Sheet

        /// <summary>
        /// Sprite sheet of the character
        /// </summary>
        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }
        Texture2D spriteSheet;

        #endregion


        #region Load character


        /// <summary>
        /// Initializes character stats and settings from the
        /// characters xml file
        /// </summary>
        /// <param name="asset">Characters asset name</param>
        public virtual void Initialize(string asset)
        {
            Load(asset);
            ResetStats();
        }

        private void Load(string asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content\Characters\" + asset + ".xml");

            foreach (XmlNode rootElement in doc.ChildNodes)
            {
                foreach (XmlNode node in rootElement.ChildNodes)
                {
                    if (node.Name == "Asset")
                        SetAsset(node);

                    if (node.Name == "Stats")
                        SetStats(node);

                    if (node.Name == "Inventory")
                        SetInventory(node);

                    if (node.Name == "Equipment")
                        SetEquipment(node);

                    if (node.Name == "Animation")
                        SetAnimations(node);

                    if (node.Name == "IsAnimated")
                        IsAnimated = bool.Parse(node.InnerText);

                    if (node.Name == "Sprite")
                    {
                         SpriteManager.SpriteDimensions = new Point( 
                             int.Parse(node.Attributes["width"].Value),
                             int.Parse(node.Attributes["height"].Value));
                    }//end sprite

                    //if (node.Name == "Aggro")
                    //    aggroRadius = float.Parse(node.InnerText);

                }
            }

            LoadCharacterSprites();
        }

        private void LoadCharacterSprites()
        {
            SpriteSheet = Globals.Content.Load<Texture2D>(@"Graphics/Characters/" + AssetName);
            SpriteManager.SpriteSheet = SpriteSheet;
        }

        private void SetAsset(XmlNode node)
        {
            AssetName = node.InnerText;
        }

        private void SetStats(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Health")
                    BaseStats.BaseHealth = int.Parse(n.InnerText);

                else if (n.Name == "Mana")
                    BaseStats.BaseMana = int.Parse(n.InnerText);

                else if (n.Name == "Strength")
                    BaseStats.Strength = int.Parse(n.InnerText);

                else if (n.Name == "Stamina")
                    BaseStats.Stamina = int.Parse(n.InnerText);

                else if (n.Name == "Agility")
                    BaseStats.Agility = int.Parse(n.InnerText);

                else if (n.Name == "Dexterity")
                    BaseStats.Dexterity = int.Parse(n.InnerText);

                else if (n.Name == "Intelligence")
                    BaseStats.Intelligence = int.Parse(n.InnerText);

                else if (n.Name == "Wisdom")
                    BaseStats.Wisdom = int.Parse(n.InnerText);
            }
        }

        private void SetInventory(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Gold")
                    inventory.Gold = int.Parse(n.InnerText);

                else if (n.Name == "Item")
                    Inventory.AddItem(new Item(n.InnerText));
            }
        }

        private void SetEquipment(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Weapon")
                    Inventory.Equip(new Weapon(n.InnerText));

                if (n.Name == "Armor")
                    Inventory.Equip(new Armor(n.InnerText));

                if (n.Name == "Accessory")
                    Inventory.Equip(new Accessory(n.InnerText));
            }
        }

        private void SetAnimations(XmlNode node)
        {
            string tempName = string.Empty;
            Direction tempDirection = Direction.Down;
            List<Rectangle> tempFrames = new List<Rectangle>();
            float tempInterval = 0f;
            bool tempIsLoop = false;
            bool tempPauseAtEnd = false;

            tempName = node.Attributes["Name"].Value;
            tempDirection = GetDirection(node.Attributes["Direction"].Value);

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "Frame")
                {
                    tempFrames.Add(new Rectangle());
                    tempFrames[int.Parse(n.Attributes["index"].Value)] = new Rectangle(
                        int.Parse(n.Attributes["x"].Value),
                        int.Parse(n.Attributes["y"].Value),
                        int.Parse(n.Attributes["width"].Value),
                        int.Parse(n.Attributes["height"].Value));
                }

                else if (n.Name == "Interval")
                {
                    tempInterval = float.Parse(n.InnerText);
                }

                else if (n.Name == "IsLoop")
                {
                    tempIsLoop = bool.Parse(n.InnerText);
                }

                else if (n.Name == "PauseAtEnd")
                {
                    tempPauseAtEnd = bool.Parse(n.InnerText);
                }

            }

            spriteManager.Animations.Add(new Animation(
                tempName, tempDirection, tempFrames,
                tempInterval, tempIsLoop, tempPauseAtEnd));
        }

        private Direction GetDirection(string direction)
        {
            switch (direction)
            {
                case "Down":
                    return Direction.Down;

                case "Left":
                    return Direction.Left;

                case "Up":
                    return Direction.Up;

                case "Right":
                    return Direction.Right;

                default:
                    return Direction.Down;
            }
        }

        public void ResetStats()
        {
            health = BaseStats.MaxHealth;
            mana = BaseStats.MaxMana;
        }

        #endregion


        #region Update

        public virtual void Update()
        {
            if (IsAnimated)
                spriteManager.UpdateAnimation();

            spriteManager.UpdateSpriteBox(CurrentPosition);

            timers.Update();
        }

        #endregion


        #region Draw


        public virtual void Draw()
        {
            if (!IsAnimated)
                SpriteManager.DrawSprite();

            else
                SpriteManager.DrawAnimation();
        }


        #endregion


        #region Character Damage


        /// <summary>
        /// Called when physical damage is delt to the character
        /// </summary>
        /// <param name="damage">Damage delt</param>
        /// <param name="attacker">Attacker doing damage</param>
        public void TakePhysicalDamage(int damage, Character attacker)
        {
            //if attack is evaded, launch "Evade!" text animation and quit
            #region Evade
            if (IsAttackEvaded(attacker, DamageType.Physical))
            {
                TextManager.LaunchAnimation("Evade!",
                   new Vector2(Origin.X, Origin.Y - spriteManager.SpriteBox.Height),
                   .7f,
                   Color.Yellow,
                   1.5f);

                return;
            }

            #endregion

            //if attack is blocked, launch "Blocked!" text animation and quit
            #region Blocked
            if (IsAttackBlocked(attacker, DamageType.Physical))
            {
                TextManager.LaunchAnimation("Blocked!",
                   new Vector2(Origin.X, Origin.Y - spriteManager.SpriteBox.Height),
                   .7f,
                   Color.Yellow,
                   1.5f);

                return;
            }
            #endregion

            //calculate damage reduction from gear
            damage = CalculateDamageReduction(damage, DamageType.Physical);

            //adjust health
            Health -= damage;

            //launch damage animation
            //SpriteManager.PlayAnimation("TakeDamage");

            //launch text animation 
            TextManager.LaunchAnimation(
                damage.ToString(),
                new Vector2(Origin.X, Origin.Y - spriteManager.SpriteBox.Height),
                .7f,
                Color.Red,
                1.5f);
        }

        
        /// <summary>
        /// Called when magical damage is delt to the character
        /// </summary>
        /// <param name="damage">Damage delt</param>
        /// <param name="attacker">Attacker doing damage</param>
        public void TakeMagicalDamage(int damage, Character attacker)
        {
            //if attack is evaded, launch "Evade!" text animation and quit
            #region Evade
            if (IsAttackEvaded(attacker, DamageType.Magical))
            {
                TextManager.LaunchAnimation("Evade!",
                   new Vector2(Origin.X, Origin.Y - spriteManager.SpriteBox.Height),
                   .7f,
                   Color.Yellow,
                   1.5f);

                return;
            }

            #endregion

            //if attack is blocked, launch "Blocked!" text animation and quit
            #region Blocked
            if (IsAttackBlocked(attacker, DamageType.Magical))
            {
                TextManager.LaunchAnimation("Blocked!",
                   new Vector2(Origin.X, Origin.Y - spriteManager.SpriteBox.Height),
                   .7f,
                   Color.Yellow,
                   1.5f);

                return;
            }
            #endregion

            //calculate damage reduction from gear
            damage = CalculateDamageReduction(damage, DamageType.Magical);

            //adjust health
            Health -= damage;

            //launch damage animation
            //SpriteManager.PlayAnimation("TakeDamage");

            //launch text animation 
            TextManager.LaunchAnimation(
                damage.ToString(),
                new Vector2(Origin.X, Origin.Y - spriteManager.SpriteBox.Height),
                .7f,
                Color.Red,
                1.5f);

        }


        /// <summary>
        /// Reduces the damage based on armor and stats of the target
        /// </summary>
        /// <param name="damage">Base Damage done</param>
        /// <returns>int</returns>
        public int CalculateDamageReduction(int damage, DamageType damageType)
        {
            if (damageType == DamageType.Physical)
                damage -= Defense;

            else if (damageType == DamageType.Magical)
                damage -= MagicalDefense;



            return damage;
        }


        /// <summary>
        /// Returns true of the attack is evaded
        /// </summary>
        public bool IsAttackEvaded(Character attacker, DamageType damageType)
        {
            int targetAttribute = BaseStats.Agility + Inventory.Equipment.AddedAgility;
            int attackerAttribute = attacker.BaseStats.Agility + attacker.Inventory.Equipment.AddedAgility;

            switch (damageType)
            {
                case DamageType.Physical:
                    targetAttribute = BaseStats.Agility + Inventory.Equipment.AddedAgility;
                    attackerAttribute = attacker.BaseStats.Agility + attacker.Inventory.Equipment.AddedAgility;
                    break;

                case DamageType.Magical:
                    targetAttribute = BaseStats.Wisdom + Inventory.Equipment.AddedWisdom;
                    attackerAttribute = attacker.BaseStats.Wisdom + attacker.Inventory.Equipment.AddedWisdom;
                    break;

            }

            //if agility is greater than attackers agility, check for evasion
            if (targetAttribute > attackerAttribute)
            {
                //target - attacker agil = Evade %
                return (targetAttribute - attackerAttribute) > RNG.GetRandomInt(100);
            }

            return false;
        }


        /// <summary>
        /// Returns true if the attack is blocked
        /// </summary>
        public bool IsAttackBlocked(Character attacker, DamageType damageType)
        {
            int enemyAttribute = 0;

            switch (damageType)
            {
                case DamageType.Physical:
                    enemyAttribute = attacker.BaseStats.Dexterity + attacker.Inventory.Equipment.AddedDexterity;
                    break;

                case DamageType.Magical:
                    enemyAttribute = attacker.BaseStats.Wisdom + attacker.Inventory.Equipment.AddedWisdom;
                    break;
            }

            //if no shield is equipped, attack can not be blocked
            if (Inventory.Equipment.Shield != null &&
                BaseStats.Dexterity + Inventory.Equipment.AddedDexterity + Inventory.Equipment.Shield.Block >
                attacker.BaseStats.Dexterity + attacker.Inventory.Equipment.AddedDexterity)
            {
                return (BaseStats.Dexterity + Inventory.Equipment.AddedDexterity + Inventory.Equipment.Shield.Block -
                            enemyAttribute) > 
                            RNG.GetRandomInt(100);
            }

            return false;
        }


        #endregion


        #region Character Died


        public virtual void HasDied()
        {
            //Play Death Animation
        }

        #endregion

    }//end class
}//end namespace
