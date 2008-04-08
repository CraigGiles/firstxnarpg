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
    class Battle : IGameScreen, IDisposable
    {
        int _SelectedTarget = 0;

        bool victory = false;
        bool _RanAway = false;
        float victoryTimer = 1.5f;
        float _LastAnimationTimer = .75f;

        BattleManager.BattleCommands
            _HighlightedCommand = BattleManager.BattleCommands.None,
            _PlayersCommand = BattleManager.BattleCommands.None;

        #region System Variables

        BaseGame game;
        TileEngineV2 tileEngine;
        Camera camera;
        SpriteBatch batch;
        SpriteFont font;

        Player player;
        List<Enemy> enemies = new List<Enemy>();

        #endregion

        #region Textures
        public Texture2D
            tHudStrip,
            tPlayerHudBorder,
            tPlayerBarBorder,
            tPlayerBarProgress,
            tBattleCommandFight,
            tBattleCommandDefend,
            tBattleCommandRun,
            tIconBorder;
        static public Texture2D tHandIcon;

        #endregion

        #region Draw Rectangles

        public Rectangle rPlayerHud;

        #endregion

        #region Properties
        #endregion

        #region Constructors / Initialize

        public Battle(BaseGame game, TileEngineV2 tileEngine, Camera camera, Player Player, List<Enemy> enemies, SpriteBatch batch, SpriteFont font, ContentManager content)
        {
            this.game = game;
            this.tileEngine = tileEngine;
            this.camera = camera;
            this.player = Player;
            this.enemies = enemies;
            this.batch = batch;
            this.font = font;

            Initialize(batch, content);
        }

        private void Initialize(SpriteBatch batch, ContentManager content)
        {
            rPlayerHud = new Rectangle(0, 460, 200, 125);
            LoadTextures(content);

            player.StartBattle();
            InitializeEnemies();
        }

        private void LoadTextures(ContentManager content)
        {
            tHandIcon = content.Load<Texture2D>(@"Battle/Hand");

            tIconBorder = content.Load<Texture2D>(@"MapEditor/TexturePreview");
            tBattleCommandFight = content.Load<Texture2D>(@"Battle/BattleCommandFight");
            tBattleCommandDefend = content.Load<Texture2D>(@"Battle/BattleCommandDefend");
            tBattleCommandRun = content.Load<Texture2D>(@"Battle/BattleCommandRun");

            tPlayerBarProgress = content.Load<Texture2D>(@"Battle/PlayerBarProgress");
            tPlayerBarBorder = content.Load<Texture2D>(@"Battle/PlayerBarBorder");
            tPlayerHudBorder = content.Load<Texture2D>(@"Battle/PlayerHudBackground");
            tHudStrip = content.Load<Texture2D>(@"Battle/HudBorder");
        }

        private void InitializeEnemies()
        {
            foreach (Enemy e in enemies)
                e.StartBattle();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Updates all logic of the battle
        /// </summary>
        /// <param name="gameTime">Game Timer</param>
        public void Update(GameTime gameTime)
        {
            RandomHelper.GenerateNewRandomGenerator();

            if (Input.Tilde)
                game.RemoveCurrentGameScreen();

            CheckEndingConditions(gameTime);

            if (!victory && !_RanAway)
            {
                UpdatePlayer(gameTime);
                UpdateEnemies(gameTime);
            }

            tileEngine.Update();
        }

        /// <summary>
        /// Checks to see if all enemies have died
        /// </summary>
        /// <param name="gameTime"></param>
        private void CheckEndingConditions(GameTime gameTime)
        {
            if (!victory)
            {
                if (enemies.Count <= 0)
                {
                    _LastAnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_LastAnimationTimer <= 0)
                    {
                        player.Victory();
                        player.Update(gameTime);
                        victory = true;
                    }
                }
            }
            else
            {
                victoryTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (victoryTimer <= 0)
                {
                    player.EndBattle();                    
                    game.RemoveCurrentGameScreen();
                }
            }
        }

        #region Update Players

        /// <summary>
        /// Updates all Players inside the battle
        /// </summary>
        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            /* if action timer is expired, either get the players battle
             * command or execute the players battle command */
            if (player.ActionTimer <= 0)
            {
                if (_PlayersCommand == BattleManager.BattleCommands.None)
                    GetBattleCommand(gameTime);
                else if (_PlayersCommand != BattleManager.BattleCommands.None)
                    ExecuteBattleCommand(gameTime);
            }
        }

        /// <summary>
        /// Gets the desired battle command for the Player
        /// </summary>
        /// <param name="Player">Player Player to get the battle command for</param>
        /// <param name="gameTime">Game Timer</param>
        private void GetBattleCommand(GameTime gameTime)
        {
            /* Each battle command is mapped to a specific direction, so to
             * select a different battle command, the player needs to only
             * press the coorisponding direction on their input device. */
            if (Input.Up)
                _HighlightedCommand = BattleManager.BattleCommands.Fight;
            else if (Input.Left)
                _HighlightedCommand = BattleManager.BattleCommands.Defend;
            else if (Input.Right)
                _HighlightedCommand = BattleManager.BattleCommands.Run;
            else if (Input.Down)
                _HighlightedCommand = BattleManager.BattleCommands.None;

            else if (Input.Enter)
                _PlayersCommand = _HighlightedCommand;
        }

        /// <summary>
        /// Executes the desired battle command for the player charcter
        /// </summary>
        /// <param name="Player">Player Player to execute the battle command for</param>
        /// <param name="gameTime">Game Timer</param>
        private void ExecuteBattleCommand(GameTime gameTime)
        {
            switch (_PlayersCommand)
            {
                case BattleManager.BattleCommands.Fight:
                    Fight(gameTime);
                    break;

                case BattleManager.BattleCommands.Defend:
                    Defend(gameTime);
                    break;

                case BattleManager.BattleCommands.Run:
                    RunAway(gameTime);                    
                    break;
            }
        }

        #region Battle Commands

        /// <summary>
        /// Enables a Player to attack a monster
        /// </summary>
        private void Fight(GameTime gameTime)
        {
            Enemy target = GetSelectedEnemy();
            /* if player has chosen a target, the target will
             * take physical damage based on the characters attack */
            if (target != null)
            {
                target.TakePhysicalDamage(player.Attack());
                _PlayersCommand = BattleManager.BattleCommands.None;

                if (target.Health == 0)
                    EnemyHasDied(target);
            }
        }

        /// <summary>
        /// Puts the Player into a defensive state
        /// </summary>
        private void Defend(GameTime gameTime)
        {
            player.Defend();    //put player in defensive mode
            _PlayersCommand = BattleManager.BattleCommands.None;
        }

        /// <summary>
        /// Checks to see if the players Run attempt was successful
        /// </summary>
        private void RunAway(GameTime gameTime)
        {
            victory = player.Run();
            _PlayersCommand = BattleManager.BattleCommands.None;
        }

        /// <summary>
        /// Targets a specific enemy for battle
        /// </summary>
        /// <returns>Targeted enemy</returns>
        private Enemy GetSelectedEnemy()
        {
            Enemy target = null;

            if (Input.GetNext)
                _SelectedTarget = (int)MathHelper.Clamp(_SelectedTarget++, 0, enemies.Count - 1);                
            
            else if (Input.GetPrevious)
                _SelectedTarget = (int)MathHelper.Clamp(_SelectedTarget--, 0, enemies.Count - 1);  
            
            else if (Input.Enter)
                target = enemies[_SelectedTarget];

            return target;
        }

        #endregion

        #endregion

        #region Update Enemies
        /// <summary>
        /// Updates all enemy logic
        /// </summary>
        private void UpdateEnemies(GameTime gameTime)
        {
            foreach (Enemy e in enemies)
            {
                e.Update(gameTime);

                if (e.ActionTimer <= 0)
                {
                    GetEnemyAction(e, gameTime);
                }
            }
        }

        private void GetEnemyAction(Enemy e, GameTime gameTime)
        {
            bool act = (RandomHelper.GetRandomInt(1000) < 125);

            if (act)
            {
                EnemyFight(e, gameTime);
            }
        }
        
        private void EnemyFight(Enemy e, GameTime gameTime)
        {
            player.TakePhysicalDamage(e.Attack());
        }

        private void EnemyDefend(Enemy e, GameTime gameTime)
        {
            e.Defend();
        }

        
        /// <summary>
        /// Kills the enemy, grants the Players experience, and removes
        /// the enemy from the battle list
        /// </summary>
        /// <param name="enemy"></param>
        private void EnemyHasDied(Enemy enemy)
        {
            player.AddExp( enemy.ExpBonus );
            enemies.Remove(enemy);
        }

        #endregion

        #endregion

        #region Draw Methods

        /// <summary>
        /// Renders the battle to screen
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            tileEngine.Draw(batch, camera);

            batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, camera.TransformMatrix);

            DrawEnemies(gameTime);
            DrawPlayers(gameTime);

            batch.End();

            DrawBattleHud(batch, gameTime);
        }

        /// <summary>
        /// Draws enemies to the screen
        /// </summary>
        /// <param name="gameTime"></param>
        private void DrawEnemies(GameTime gameTime)
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(batch, font, gameTime);

                //Player health bar
                for (int i = 0; i < e.HealthPercentValue; i++)
                {
                    int percentOffset = i;
                    batch.Draw(tPlayerBarProgress,
                        new Rectangle(((int)e.CurrentLocation.X + (e.SpriteWidth / 2)) - 50 + i, ((int)e.CurrentLocation.Y + e.SpriteHeight + 11), 1, 10),
                        null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, .0f);
                }

                //Player Health Bar Border
                batch.Draw(tPlayerBarBorder,
                    new Rectangle(((int)e.CurrentLocation.X + (e.SpriteWidth / 2)) - 50, ((int)e.CurrentLocation.Y + e.SpriteHeight + 11), 100, 10),
                    null, Color.DarkGray, 0f, Vector2.Zero, SpriteEffects.None, .0f);
            }
        }

        /// <summary>
        /// Draws player Players to the screen
        /// </summary>
        /// <param name="gameTime"></param>
        private void DrawPlayers(GameTime gameTime)
        {
            player.Draw(batch, font, gameTime);

            if (player.ActionTimer <= 0)
            {
                #region Draw Battle Command Icons
                batch.Draw(tBattleCommandFight,
                        new Rectangle(player.SpriteBox.X + (player.SpriteBox.Width / 2) - 12, player.SpriteBox.Y - 57, 25, 25),
                        Color.White);

                batch.Draw(tBattleCommandDefend,
                        new Rectangle(player.SpriteBox.X, player.SpriteBox.Y - 30, 25, 25),
                        null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

                batch.Draw(tBattleCommandRun,
                        new Rectangle(player.SpriteBox.X + (player.SpriteBox.Width - 24), player.SpriteBox.Y - 30, 25, 25),
                        null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                #endregion

                #region Frame Selected Command
                if (_HighlightedCommand == BattleManager.BattleCommands.Fight)
                    batch.Draw(tBattleCommandFight,
                        new Rectangle(player.SpriteBox.X + (player.SpriteBox.Width / 2) - 12, player.SpriteBox.Y - 57, 25, 25),
                        Color.Red);

                else if (_HighlightedCommand == BattleManager.BattleCommands.Defend)
                    batch.Draw(tBattleCommandDefend,
                        new Rectangle(player.SpriteBox.X, player.SpriteBox.Y - 30, 25, 25),
                        null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0f);

                else if (_HighlightedCommand == BattleManager.BattleCommands.Run)
                    batch.Draw(tBattleCommandRun,
                        new Rectangle(player.SpriteBox.X + (player.SpriteBox.Width - 24), player.SpriteBox.Y - 30, 25, 25),
                        null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                
                #endregion

                #region Draw "Select Enemy" icon

                if (_PlayersCommand == BattleManager.BattleCommands.Fight)
                {
                    batch.Draw(tHandIcon,
                        new Vector2(enemies[_SelectedTarget].CurrentLocation.X - 30,
                                    enemies[_SelectedTarget].CurrentLocation.Y + 25),
                        Color.White);
                }
                #endregion
            }
        }

        /// <summary>
        /// Renders the user interface HUD to screen
        /// </summary>
        private void DrawBattleHud(SpriteBatch batch, GameTime gameTime)
        {
            batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);

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
                    new Vector2((rPlayerHud.X ) + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height + 25),
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

                #region ActionTimer
                //Player ATB text
                batch.DrawString(font, "ATB",
                    new Vector2((rPlayerHud.X) + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height + 53),
                    Color.White, 0.0f, Vector2.Zero, .50f, SpriteEffects.None, 0f);

                //Player ATB bar
                for (int i = 0; i < player.ActionTimerPercentValue; i++)
                {
                    int percentOffset = i;
                    batch.Draw(tPlayerBarProgress,
                        new Rectangle((rPlayerHud.X) + percentOffset + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height + 70, 1, 10),
                        null, Color.LightGreen, 0f, Vector2.Zero, SpriteEffects.None, .2f);
                }

                batch.Draw(tPlayerBarBorder,
                    new Rectangle((rPlayerHud.X) + player.Portrait.Width + 5, rPlayerHud.Y + player.Portrait.Height + 70, 100, 10),
                    null, Color.DarkGray, 0f, Vector2.Zero, SpriteEffects.None, .1f);
                #endregion    

            batch.End();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
