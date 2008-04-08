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
    public class GameScreen : IGameScreen
    {
        #region Variables
        Camera camera = new Camera();
        TileEngineV2 tileEngine;
        ContentManager content;
        SpriteBatch batch;
        SpriteFont font;
        BaseGame game;

        bool _MapEditor = false;

        List<Enemy> enemies = new List<Enemy>();
        float _AggroRadius = 200f;

        Player player;

        #endregion

        #region Properties
        #endregion

        #region Constructors / Initialize

        public GameScreen(ContentManager content, SpriteBatch batch,
                        SpriteFont font, BaseGame game)
        {
            this.content = content;
            this.batch = batch;
            this.font = font;
            this.game = game;

            Initialize();
        }

        private void Initialize()
        {
            tileEngine = new TileEngineV2(content);
            player = new Player(content, content.Load<Texture2D>(@"Characters/Hero/Crono"), camera);

            SpawnEnemies(content);
        }

        /// <summary>
        /// Cycles through all tiles on the map and if a tile should
        /// spawn an enemy, spaw the enemy.
        /// </summary>
        private void SpawnEnemies(ContentManager content)
        {
          
            int width = tileEngine.GetWidthInTiles;
            int height = tileEngine.GetHeightInTiles;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (TileEngineV2.enemyLayer[y, x] != -1)
                    {
                        int index = TileEngineV2.enemyLayer[y, x];
                        SpawnNewEnemy(index, x, y);
                    }
                }
            }
        }

        /// <summary>
        /// Spawns an enemy of the selected index on a specific tile
        /// </summary>
        /// <param name="index">Enemy Index taken from Tile Engine</param>
        /// <param name="tileX">Tile on the X plane</param>
        /// <param name="tileY">Tile on the Y plane</param>
        public void SpawnNewEnemy(int index, int tileX, int tileY)
        {
            Vector2 location = new Vector2();

            location.X = (float)tileX * (float)TileEngineV2.TileWidth;
            location.Y = (float)tileY * (float)TileEngineV2.TileHeight;

            Enemy e = new Enemy(TileEngineV2.curEnemyTileNames[index], content, camera, location);

            enemies.Add(e);
       }

        #endregion

        #region Methods

        /// <summary>
        /// Handles all the update logic for the current game screen
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (GameArea.curGameArea != GameArea.prevGameArea)
                tileEngine.LoadMap(content, GameArea.curGameArea + ".map");

            if (_MapEditor)
            {
                enemies.Clear();
                SpawnEnemies(content);
                _MapEditor = false;
            }

            CheckButtonPresses();

            foreach (Enemy e in enemies)
                e.Update(gameTime);

            player.Update(gameTime);

            CheckForBattle();

            #region Boundry Checks
            int _TileMapWidth = tileEngine.GetWidthInPixels;
            int _TileMapHeight = tileEngine.GetHeightInPixels;
            int _ScreenWidth = game.GraphicsDevice.Viewport.Width;
            int _ScreenHeight = game.GraphicsDevice.Viewport.Height;

            ClampCamera(_ScreenWidth, _ScreenHeight);
            BoundryCheckPlayer(_TileMapWidth, _TileMapHeight);
            BoundryCheckEnemies(_TileMapWidth, _TileMapHeight);
            BoundryCheckCamera(_TileMapWidth, _TileMapHeight, _ScreenWidth, _ScreenHeight);
            #endregion            
        }

        /// <summary>
        /// Checks for button input
        /// </summary>
        private void CheckButtonPresses()
        {
            if (Input.Tilde)
            {
                _MapEditor = true;
                game.AddNewGameScreen(new MapEditor(content, batch, game, font, tileEngine, camera));
            }

            if (Input.Inventory)
            {
                game.AddNewGameScreen(new InventoryScreen(player, game, batch, font, content));
            }
        }

        /// <summary>
        /// Clamps the camera to the current character. 
        /// </summary>
        /// <param name="iScreenWidth">Screen Width</param>
        /// <param name="iScreenHeight">Screen Height</param>
        private void ClampCamera(int _ScreenWidth, int _ScreenHeight)
        {
            camera.Position.X = player.CurrentLocation.X +
                (player.SpriteBoundingBox.Width / 2) - (_ScreenWidth / 2);
            camera.Position.Y = player.CurrentLocation.Y +
                (player.SpriteBoundingBox.Height / 2) - (_ScreenHeight / 2);
        }

        /// <summary>
        /// Clamps camera to the gameworld, ensuring it does not leave the
        /// boundries of the game map
        /// </summary>
        private void BoundryCheckCamera(int _TileMapWidth, int _TileMapHeight, int _ScreenWidth, int _ScreenHeight)
        {
            if (camera.Position.X < 0)
                camera.Position.X = 0;
            if (camera.Position.Y < 0)
                camera.Position.Y = 0;

            if (camera.Position.X > _TileMapWidth - _ScreenWidth)
                camera.Position.X = _TileMapWidth - _ScreenWidth;
            if (camera.Position.Y > _TileMapHeight - _ScreenHeight)
                camera.Position.Y = _TileMapHeight - _ScreenHeight;
        }

        /// <summary>
        /// Ensures the player can not walk off the screen
        /// </summary>
        private void BoundryCheckPlayer(int _TileMapWidth, int _TileMapHeight)
        {
            if (player.CurrentLocation.X <= -25)
                player.CurrentLocation = new Vector2(-25, player.CurrentLocation.Y);
            if (player.CurrentLocation.Y <= -25)
                player.CurrentLocation = new Vector2(player.CurrentLocation.X, -25);

            if (player.CurrentLocation.X >= _TileMapWidth - player.SpriteBoundingBox.Width * 2)
                player.CurrentLocation = new Vector2(_TileMapWidth - player.SpriteBoundingBox.Width * 2, player.CurrentLocation.Y);
            if (player.CurrentLocation.Y >= _TileMapHeight - player.SpriteBoundingBox.Height - 50)
                player.CurrentLocation = new Vector2(player.CurrentLocation.X, _TileMapHeight - player.SpriteBoundingBox.Height - 50);
        }

        /// <summary>
        /// Ensures the enemies can not walk off screen
        /// </summary>
        /// <param name="_TileMapWidth"></param>
        /// <param name="_TileMapHeight"></param>
        private void BoundryCheckEnemies(int _TileMapWidth, int _TileMapHeight)
        {
            foreach (Enemy e in enemies)
            {
                if (e.CurrentLocation.X <= -25)
                    e.CurrentLocation = new Vector2(-25, e.CurrentLocation.Y);
                if (e.CurrentLocation.Y <= -25)
                    e.CurrentLocation = new Vector2(e.CurrentLocation.X, -25);

                if (e.CurrentLocation.X >= _TileMapWidth - e.SpriteBoundingBox.Width * 2)
                    e.CurrentLocation = new Vector2(_TileMapWidth - e.SpriteBoundingBox.Width * 2, e.CurrentLocation.Y);
                if (e.CurrentLocation.Y >= _TileMapHeight - e.SpriteBoundingBox.Height - 50)
                    e.CurrentLocation = new Vector2(e.CurrentLocation.X, _TileMapHeight - e.SpriteBoundingBox.Height - 50);
            }
        }

        /// <summary>
        /// Checks the distance between player and all enemies on screen to
        /// determine if an enemy has agro'd the player and battle is initiated
        /// </summary>
        private void CheckForBattle()
        {
            bool battle = false;

            foreach (Enemy e in enemies)
            {
                Vector2 distance = player.CurrentLocation - e.CurrentLocation;

                if (distance.Length() < e.BattleRadius)
                    battle = true;
            }

            if (battle)
                InitiateBattle();
        }

        /// <summary>
        /// Starts the battle once a player has agro'd an enemy
        /// </summary>
        public void InitiateBattle()
        {
            List<Enemy> enemyTargets = new List<Enemy>();

            foreach (Enemy e in enemies)
            {
                bool include = EnemyIncludedInBattle(e);

                if (include)
                    enemyTargets.Add(e);
            }

            foreach (Enemy e in enemyTargets)
                enemies.Remove(e);

            game.AddNewGameScreen(new Battle(game, tileEngine, camera,
                player, enemyTargets, batch, font, content));
        }

        /// <summary>
        /// Cycles through all enemies to see if they should be included
        /// in the battle
        /// </summary>
        /// <param name="compare">Enemy to compare</param>
        /// <returns>True: Yes, add to battle - False: No, stay on map</returns>
        public bool EnemyIncludedInBattle(Enemy compare)
        {
            Vector2 distance = player.CurrentLocation - compare.CurrentLocation;

            return (distance.Length() < _AggroRadius);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Renders the current game screen
        /// </summary>
        /// <param name="gameTime">Game Timer</param>
        public void Draw(GameTime gameTime)
        {
            
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            tileEngine.Draw(batch, camera);

            batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, camera.TransformMatrix);

            foreach (Enemy e in enemies)
                e.Draw(batch, font, gameTime);

            player.Draw(batch, font, gameTime);

            batch.End();
        }

        #endregion

    }//end class
}//end namespace
