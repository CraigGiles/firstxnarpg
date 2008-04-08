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
    public class Player : BaseCharacter
    {
        #region Player Objects

        Camera camera;
        Inventory inventory;
        PlayerAnimations animation;
        BattleManager battle;

        //If i have time, i'll impliment the following
        //Skills system
        //Magic system

        #endregion        

        bool _InBattle = false; 

        #region Player variables

        /// <summary>
        /// Characters level, current exp, and gold
        /// </summary>
        int _Level,
            _Exp,
            _Gold;

        /// <summary>
        /// Characters bonuses attained on levelup
        /// </summary>
        int _LevelBonusHealth,
            _LevelBonusMana;

        /// <summary>
        /// Max level and exp table
        /// </summary>
        const int _MaxLevel = 60;
        int[] _ExpTable = new int[_MaxLevel + 1];

        /// <summary>
        /// Used to determine the last direction the character was
        /// moving
        /// </summary>
        bool
            _MoveDown = true,
            _MoveLeft = false,
            _MoveUp = false,
            _MoveRight = false;

        /// <summary>
        /// Used to determine if the player is currently in a defensive
        /// state in battle
        /// </summary>
        bool _IsDefending = false;

        /// <summary>
        /// Source Rectangle for the characters Portrate picture
        /// </summary>
        Rectangle
            _Portrait = new Rectangle(760, 0, 40, 40);

        #endregion
        
        #region Properties
        /// <summary>
        /// characters current level
        /// </summary>
        public int Level
        {
            get { return _Level; }
            set { _Level = (int)MathHelper.Clamp(value, 1, _MaxLevel); }
        }

        /// <summary>
        /// characters current exp
        /// </summary>
        public int Exp
        {
            get { return _Exp; }
            set { _Exp = (int)Math.Max(value, 1); }
        }

        /// <summary>
        /// Exp needed until a level is gained
        /// </summary>
        public int ExpToLevel
        {
            get { return _ExpTable[Level]; }
        }

        /// <summary>
        /// Health awarded on level up
        /// </summary>
        public int LevelBonusHealth
        {
            get { return _LevelBonusHealth; }
            set { _LevelBonusHealth = (int)Math.Max(value, 0); }
        }

        /// <summary>
        /// Mana awarded on level up
        /// </summary>
        public int LevelBonusMana
        {
            get { return _LevelBonusMana; }
            set { _LevelBonusMana = (int)Math.Max(value, 0); }
        }

        /// <summary>
        /// Characters current gold
        /// </summary>
        public int Gold
        {
            get { return _Gold; }
            set { _Gold = value; }
        }

        /// <summary>
        /// Characters current armor value
        /// </summary>
        public int Armor
        {
            get
            {
                if (_IsDefending)
                    return
                        BaseArmor +
                        inventory.EquippedArmor.Defense +
                        inventory.EquippedAccessory.Defense +
                        Level * 25;
                else
                    return
                        BaseArmor +
                        inventory.EquippedArmor.Defense +
                        inventory.EquippedAccessory.Defense;
            }
        }

        /// <summary>
        /// Characters current strength value
        /// </summary>
        public int Strength
        {
            get
            {
                return 
                    BaseStrength +
                    inventory.EquippedAccessory.Strength;
            }
        }

        /// <summary>
        /// Characters current attack power
        /// </summary>
        public int AttackPower
        {
            get
            {
                return
                    Strength;
            }
        }

        /// <summary>
        /// Characters Action Timer Reset value
        /// </summary>
        public override float ActionTimerReset
        {
            get
            {
                return
                    base.ActionTimerReset +
                    inventory.EquippedWeapon.Delay -
                    inventory.EquippedAccessory.Haste;
            }
        }

        #region Sprite Properties
        /// <summary>
        /// The collision radius of your sprite
        /// </summary>
        public float CollisionRadius
        {
            get { return Radius; }
        }

        /// <summary>
        /// The agro radius of your sprite
        /// </summary>
        public float BattleRadius
        {
            get { return Radius * 10; }
        }

        /// <summary>
        /// Bounding box used for collision on tiles
        /// </summary>
        public Rectangle SpriteBoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)FootLocation.X,
                    (int)FootLocation.Y,
                    SpriteBox.Width / 3,
                    SpriteBox.Height / 3);
            }
        }

        /// <summary>
        /// Returns the characters Portrate
        /// </summary>
        public Rectangle Portrait
        {
            get { return _Portrait; }
        }

        /// <summary>
        /// The location of your sprites feet
        /// </summary>
        public Vector2 FootLocation
        {
            get
            {
                return CurrentLocation + new Vector2(SpriteBox.Width / 2, SpriteBox.Height / 2 + 10);
            }
        }

        /// <summary>
        /// Characters last direction while moving
        /// </summary>
        public string LastMovementDirection
        {
            get
            {
                string direction = "";

                if (_MoveDown == true)
                    direction = "down";
                if (_MoveLeft == true)
                    direction = "left";
                if (_MoveUp == true)
                    direction = "up";
                if (_MoveRight == true)
                    direction = "right";

                return direction;
            }
        }

        #endregion

        #endregion

        #region Properties - Inventory
        public List<BaseObjects> Inventory
        {
            get { return inventory.CurrentInventory; }
        }

        public Weapon CurrentWeapon
        {
            get { return inventory.EquippedWeapon; }
        }

        public Armor CurrentArmor
        {
            get { return inventory.EquippedArmor; }
        }

        public Accessory CurrentAccessory
        {
            get { return inventory.EquippedAccessory; }
        }
        #endregion

        #region Constructor / Initialize Methods
        public Player(ContentManager content, Texture2D SpriteSheet, Camera camera)
        {
            this.camera = camera;
            base.SpriteSheet = SpriteSheet;

            Initialize(content);
        }
        
        private void Initialize(ContentManager content)
        {
            SetStartingStats();     //sets up your starting stats
            SetExpTable();          //Sets up the players exp table

            base.Initialize();      //sets current Stats to their Max stats

            CurrentLocation = new Vector2(250, 250);
            SpriteHeight = 80;
            SpriteWidth = 80;

            animation = new PlayerAnimations(SpriteSheet);
            inventory = new Inventory();
            battle = new BattleManager();

            //Just some random weapon that i equipped for testing puropses
            inventory.EquipWeapon(
                new Weapon(999,
                content.Load<Texture2D>(@"Battle/BattleCommandFight"),
                "Sword of Doom",
                30,
                1));

            inventory.EquipArmor(
                new Armor(998,
                content.Load<Texture2D>(@"Battle/BattleCommandDefend"),
                "Armor of Doom",
                5));

            inventory.EquipAccessory(
                new Accessory(997,
                content.Load<Texture2D>(@"Battle/BattleCommandDefend"),
                "Accessory of Remo",
                "Adds health to Remo's Fanboi",
                0,
                0,
                100,
                0,
                0));

            Accessory item = new Accessory(997,
                content.Load<Texture2D>(@"Battle/BattleCommandDefend"),
                "Accessory of Remo",
                "Adds health to Remo's Fanboi",
                0,
                0,
                100,
                0,
                0);

            inventory.AddItemToInventory( item );
        }

        private void SetStartingStats()
        {
            Name = "sCary";
            Level = 1;
            Gold = 100;

            MaxHealth = 300;
            MaxMana = 250;

            Health = MaxHealth;
            Mana = MaxMana;

            BaseArmor = 5;
            BaseStrength = 150;

            ActionTimerReset = 1;
            TravelSpeed = 3.2f;
            Radius = 8f;
        }

        public void SetExpTable()
        {
            for (int i = 0; i < (_MaxLevel + 1); i++)
            {
                int exp;                //declares the exp variable

                exp = i * 350;          //sets the amnt of exp needed for that level
                _ExpTable[i] = exp;     //fills in the exp table with the exp needed
            }
        }

        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            if (_InBattle)
            {
                UpdateBattle(gameTime);
            }
            else
            {
                CheckForButtonPress(gameTime);      //checks to see if any buttons were pressed
                CheckForMovement(gameTime);         //checks to see if character has moved
            }

            animation.Update(gameTime);

            PreviousLocation = CurrentLocation; //sets the previous location to current location
        }

        private void UpdateBattle(GameTime gameTime)
        {
            ActionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ActionTimer <= 0 && _IsDefending)
            {
                _IsDefending = false;
                animation.StartStandingAnimation(LastMovementDirection);
            }
        }

        private void CheckForButtonPress(GameTime gameTime)
        {
        }

        #region Movement and Collision Methods

        /// <summary>
        /// Checks the input for movement key presses, and moves the
        /// character sprite as intended
        /// </summary>
        private void CheckForMovement(GameTime gameTime)
        {
            float _RunSpeed = 0f;

            /* If the run button is held down, increase _RunSpeed
             * to something other than 0 */ 
            if (Input.Run)
                _RunSpeed = 3f;

            if (Input.Up)
            {
                CurrentLocation -= new Vector2(0, TravelSpeed + _RunSpeed);
                _MoveDown = false;
                _MoveLeft = false;
                _MoveUp = true;
                _MoveRight = false;
            }
            else if (Input.Down)
            {
                CurrentLocation += new Vector2(0, TravelSpeed + _RunSpeed);
                _MoveDown = true;
                _MoveLeft = false;
                _MoveUp = false;
                _MoveRight = false;
            }
            else if (Input.Left)
            {
                CurrentLocation -= new Vector2(TravelSpeed + _RunSpeed, 0);
                _MoveDown = false;
                _MoveLeft = true;
                _MoveUp = false;
                _MoveRight = false;
            }
            else if (Input.Right)
            {
                CurrentLocation += new Vector2(TravelSpeed + _RunSpeed, 0);
                _MoveDown = false;
                _MoveLeft = false;
                _MoveUp = false;
                _MoveRight = true;
            }

            if (CurrentLocation != PreviousLocation)
            {
                CheckForCollisions(TravelSpeed + _RunSpeed);
                animation.StartWalkingAnimation(LastMovementDirection);
            }
            else
            {
                animation.StartStandingAnimation(LastMovementDirection);
            }
        }

        /// <summary>
        /// Checks for collisions between sprite and world objects
        /// </summary>
        private void CheckForCollisions(float speed)
        {
            MapCollisions(speed);
            NpcCollisions();
        }

                /// <summary>
                /// Checks for collisions on map tiles
                /// </summary>
                private void MapCollisions(float speed)
                {
                    Rectangle[,] CollisionLayer = (Rectangle[,])TileEngineV2.rUnwalkable.Clone();
                    int height = CollisionLayer.GetLength(0);
                    int width = CollisionLayer.GetLength(1);

                    Point spriteCell = TileEngineV2.ConvertPositionToCell(FootLocation);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (_MoveDown && SpriteBoundingBox.Intersects(CollisionLayer[y, x]))
                                CurrentLocation -= new Vector2(0, speed);

                            if (_MoveLeft && SpriteBoundingBox.Intersects(CollisionLayer[y, x]))
                                CurrentLocation += new Vector2(speed, 0);

                            if (_MoveUp && SpriteBoundingBox.Intersects(CollisionLayer[y, x]))
                                CurrentLocation += new Vector2(0, speed);

                            if (_MoveRight && SpriteBoundingBox.Intersects(CollisionLayer[y, x]))
                                CurrentLocation -= new Vector2(speed, 0);

                        }
                    }
                }

                /// <summary>
                /// Checks for collisions with other characters
                /// </summary>
                private void NpcCollisions()
                {
                }

        /// <summary>
        /// Issues the attack command in battle
        /// </summary>
        /// <returns>Damage delt to the opponent</returns>
        public int Attack()
        {
            animation.StartAttackAnimation(LastMovementDirection);
            ActionTimer = ActionTimerReset;
            //sound.PlayAttackSound();
            return battle.Attack(AttackPower);
        }

        /// <summary>
        /// Issues the defend command in battle
        /// </summary>
        public void Defend()
        {
            _IsDefending = true;
            ActionTimer = ActionTimerReset;
            animation.StartDefendAnimation(LastMovementDirection);            
        }

        /// <summary>
        /// issues the run away command in battle
        /// </summary>
        /// <returns>True: Ran away - False: Attempt failed</returns>
        public bool Run()
        {
            ActionTimer = ActionTimerReset;
            return battle.RunAway();
        }

        /// <summary>
        /// Damages the player
        /// </summary>
        /// <param name="damage">Amount of damage issued to the player</param>
        public void TakePhysicalDamage(int damage)
        {
            Health -= battle.TakePhysicalDamage(damage, Armor);
            if (Health <= 0)
                PlayerHasDied();
        }

        /// <summary>
        /// Sets the players status to dead
        /// </summary>
        private void PlayerHasDied()
        {
            //sound.PlayerDied();
            animation.DeadCharacter();
        }

        #endregion

        #region Alter Player Stats

        /// <summary>
        /// Adds health to the character
        /// </summary>
        /// <param name="health">health added</param>
        public void AddHealth(int health)
        {
            int added = (int)Math.Max(health, 0);
            Health += added;
        }

        /// <summary>
        /// reduces health from the character
        /// </summary>
        /// <param name="damage">damage to be taken</param>
        public void TakeDamage(int damage)
        {
            int dmg = (int)Math.Max(damage, 0);
            Health -= (dmg - Armor);

            if (Health <= 0)
                PlayerHasDied();
        }

        /// <summary>
        /// Adds mana to the character
        /// </summary>
        /// <param name="mana">mana added</param>
        public void AddMana(int mana)
        {
            int added = (int)Math.Max(mana, 0);
            Mana += added;
        }

        /// <summary>
        /// Adds gold to the character
        /// </summary>
        /// <param name="gold">gold to be added</param>
        public void AddGold(int gold)
        {
            int added = (int)Math.Max(gold, 0);
            Gold += added;
        }

        /// <summary>
        /// Adds exp to the character
        /// </summary>
        /// <param name="exp">exp to be added</param>
        public void AddExp(int exp)
        {
            int added = (int)Math.Max(exp, 0);
            Exp += added;

            if (Exp >= ExpToLevel)
                LevelUp();
        }

        /// <summary>
        /// Levels up the character
        /// </summary>
        private void LevelUp()
        {
            Level++;                                //increases player level by 1

            MaxHealth += LevelBonusHealth;
            MaxMana += LevelBonusMana;

            Health = MaxHealth;                     //sets the players cur health to max health
            Mana = MaxMana;

            _Exp = 0;                            //Sets the current exp to 0
            //audio.PlayLevelUp();
        }

        #endregion

        public void StartBattle()
        {
            animation.StartStandingAnimation(LastMovementDirection);
            ActionTimer = ActionTimerReset;
            _InBattle = true;
        }

        public void Victory()
        {            
            animation.StartVictoryAnimation();
        }

        public void EndBattle()
        {
            _InBattle = false;
            animation.StartStandingAnimation(LastMovementDirection);
        }
        #endregion

        #region Draw Methods

        /// <summary>
        /// Draws the character to screen
        /// </summary>
        public void Draw(SpriteBatch batch, SpriteFont font, GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;   //elapsed time for timers

            animation.Draw(batch, CurrentLocation, SpriteWidth, SpriteHeight);
        }

        #endregion

    }
}
