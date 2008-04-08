using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

using TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ShackRPG.GameScreens;

namespace ShackRPG
{
    public class Enemy : BaseCharacter
    {
        #region Enemy Objects

        Camera camera;
        Inventory inventory;
        BattleManager battle;

        //If i have time, i'll impliment the following
        //Skills system
        //Magic system

        #endregion

        bool _InBattle = false;

        #region Sprite Animation Frames
        List<Rectangle> animation_rects = new List<Rectangle>();

        Rectangle
            frame1 = new Rectangle(0, 0, 40, 40),
            frame2 = new Rectangle(40, 0, 40, 40),
            frame3 = new Rectangle(80, 0, 40, 40),
            frame4 = new Rectangle(120, 0, 40, 40);

        Animation animation;

        #endregion

        #region Enemy Variables

        /// <summary>
        /// Characters level and gold
        /// </summary>
        int _Level,
            _Exp,
            _Gold;

        int _MaxLevel = 60;

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
        /// Bonus exp given on death 
        /// </summary>
        public int ExpBonus
        {
            get { return _Exp; }
            set { _Exp = (int)Math.Max(value, 1); }
        }

        /// <summary>
        /// Gold given on death
        /// </summary>
        public int Gold
        {
            get { return _Gold; }
            set { _Gold = (int)Math.Max(value, 1); }
        }

        /// <summary>
        /// Enemies current armor values
        /// </summary>
        public int Armor
        {
            get
            {
                if (_IsDefending)
                    return
                        BaseArmor +
                        Level * 25;
                else
                    return
                        BaseArmor;
            }
        }

        /// <summary>
        /// Enemies current strength value
        /// </summary>
        public int Strength
        {
            get
            {
                return
                    BaseStrength;
            }
        }

        /// <summary>
        /// Enemies current attack power (Right now it
        /// only returns the Strength, but this will change
        /// later)
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
            get { return Radius * 15; }
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

        #region Constructor / Initialize
        public Enemy(string name, ContentManager content, Camera camera, Vector2 location)
        {
            this.camera = camera;
            base.CurrentLocation = location;

            LoadEnemyFromFile(name + ".mob", content);

            Initialize(content);
        }

        /// <summary>
        /// Loads up the enemy.mob file and sets the enemy stats accordingly
        /// </summary>
        /// <param name="fileName">filename to open</param>
        /// <param name="content">content manager for loading enemy spritesheet</param>
        private void LoadEnemyFromFile(string fileName, ContentManager content)
        {
            using (StreamReader reader = new StreamReader("Content/" + fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("Name:"))
                        Name = line.Remove(0, 5);

                    else if (line.Contains("Texture:"))
                        SpriteSheet = content.Load<Texture2D>(line.Remove(0, 8));

                    else if (line.Contains("Level:"))
                        Level = int.Parse(line.Remove(0, 6));

                    else if (line.Contains("Exp:"))
                        ExpBonus = int.Parse(line.Remove(0, 4));

                    else if (line.Contains("Gold:"))
                        Gold = int.Parse(line.Remove(0, 5));

                    else if (line.Contains("Speed:"))
                        TravelSpeed = float.Parse(line.Remove(0, 6));

                    else if (line.Contains("Radius:"))
                        Radius = float.Parse(line.Remove(0, 7));

                    else if (line.Contains("ActionTimerReset:"))
                        ActionTimerReset = float.Parse(line.Remove(0, 17));

                    else if (line.Contains("Health:"))
                        MaxHealth = int.Parse(line.Remove(0, 7));

                    else if (line.Contains("Mana:"))
                        MaxMana = int.Parse(line.Remove(0, 5));

                    else if (line.Contains("Strength:"))
                        BaseStrength = int.Parse(line.Remove(0, 9));

                    else if (line.Contains("Armor:"))
                        BaseArmor = int.Parse(line.Remove(0, 6));

                    else if (line.Contains("SpriteWidth:"))
                        SpriteWidth = int.Parse(line.Remove(0, 12));

                    else if (line.Contains("SpriteHeight:"))
                        SpriteHeight = int.Parse(line.Remove(0, 13));

                }
            }
        }

        private void Initialize(ContentManager content)
        {            
            base.Initialize();      //sets current Stats to their Max stats
            battle = new BattleManager();

            animation_rects.Add(frame1);
            animation_rects.Add(frame2);
            animation_rects.Add(frame3);
            animation_rects.Add(frame4);

            animation = new Animation(SpriteSheet, animation_rects, 0.20f);
        }

        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            if (!_InBattle)
                CheckForMovement(gameTime);

            else
                UpdateBattle(gameTime);

            animation.UpdateAnimation(gameTime);
        }

        private void CheckForMovement(GameTime gameTime)
        {
        }

        private void UpdateBattle(GameTime gameTime)
        {
            ActionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void StartBattle(GameTime gameTime)
        {
        }

        /// <summary>
        /// Checks for collisions between sprite and world objects
        /// </summary>
        private void CheckForCollisions()
        {
            MapCollisions();
            NpcCollisions();
        }

                /// <summary>
                /// Checks for collisions on map tiles
                /// </summary>
                private void MapCollisions()
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
                                CurrentLocation -= new Vector2(0, TravelSpeed);

                            if (_MoveLeft && SpriteBoundingBox.Intersects(CollisionLayer[y, x]))
                                CurrentLocation += new Vector2(TravelSpeed, 0);

                            if (_MoveUp && SpriteBoundingBox.Intersects(CollisionLayer[y, x]))
                                CurrentLocation += new Vector2(0, TravelSpeed);

                            if (_MoveRight && SpriteBoundingBox.Intersects(CollisionLayer[y, x]))
                                CurrentLocation -= new Vector2(TravelSpeed, 0);

                        }
                    }
                }

                /// <summary>
                /// Checks for collisions with other characters
                /// </summary>
                private void NpcCollisions()
                {
                }

        public void StartBattle()
        {
            _InBattle = true;
        }

        /// <summary>
        /// Issues the attack command in battle
        /// </summary>
        /// <returns>Damage delt to the opponent</returns>
        public int Attack()
        {
            //sound.PlayAttackSound();
            ActionTimer = ActionTimerReset;
            return battle.Attack(AttackPower);
        }

        /// <summary>
        /// Issues the defend command in battle
        /// </summary>
        public void Defend()
        {
            ActionTimer = ActionTimerReset;
            _IsDefending = true;
        }

        /// <summary>
        /// issues the run away command in battle
        /// </summary>
        /// <returns>True: Ran away - False: Attempt failed</returns>
        public bool Run()
        {
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
                EnemyHasDied();
        }

        /// <summary>
        /// Sets the players status to dead
        /// </summary>
        private void EnemyHasDied()
        {
            //sound.EnemyDied();
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the enemy to screen
        /// </summary>
        public void Draw(SpriteBatch batch, SpriteFont font, GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;   //elapsed time for timers

            animation.Draw(batch, CurrentLocation, SpriteWidth, SpriteHeight);
        }

        public void DrawHealthBar(SpriteBatch batch, SpriteFont font)
        {

        }

        #endregion

    }
}
