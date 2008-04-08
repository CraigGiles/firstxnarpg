using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TileEngine;
using ShackRPG.HelperClasses;
using ShackRPG.Enemies;

namespace ShackRPG.GameScreens
{
    class TestGame : IGameScreen
    {
        float AggroRadius = 250f;
        bool bExitedMapEditor = false;
        SpawnEnemy spawnEnemy;
        




            #region Debug - remove when done
            curKeyboardState = Keyboard.GetState();
            #endregion

            //if user preses Space, open a test battle
            if (Input.Space)
                game.AddNewGameScreen(new InventoryScreen(characters[0], game, spriteBatch, font, Content));


            #region MapEditor Debug (change later
            //If user presses E, open the in-game Map Editor
            if (curKeyboardState.IsKeyDown(Keys.E))
            {
                UnloadAllEnemies();
                game.AddNewGameScreen(new MapEditor(Content, spriteBatch, game, font, tileEngine, camera));
                bExitedMapEditor = true;
            }
            #endregion

            foreach (Character player in characters)
                player.Update(gameTime);

            foreach (BaseEnemy enemy in enemies)
            {
                enemy.UpdateOnMap(gameTime);

                foreach (Character character in characters)
                {
                    if (CheckNpcCollisions(character, enemy))
                    {
                        Vector2 d = Vector2.Normalize(enemy.CurrentLocation - character.CurrentLocation);

                        character.CurrentLocation = character.CurrentLocation - (d * ((character.CollisionRadius + enemy.CollisionRadius) ));
                    }
                }
            }



            CheckForBattle();
        }

        public void CheckForBattle()
        {
            bool battle = false;
            Character character = null;

            foreach (Character c in characters)
            {
                foreach (BaseEnemy e in enemies)
                {
                    Vector2 d = c.CurrentLocation - e.CurrentLocation;

                    if (d.Length() < e.BattleRadius)
                    {
                        character = c;
                        battle = true;
                    }
                }
            }

            if (battle)
                StartBattle(character);
        }

        private void StartBattle(Character character)
        {
            List<Character> characterBattle = new List<Character>();
            List<BaseEnemy> enemyBattle = new List<BaseEnemy>();

            characterBattle.Add(character);

            foreach (BaseEnemy e in enemies)
            {
                bool include = EnemyIncludedInBattle(character, e);

                if (include)
                    enemyBattle.Add(e);
            }

            foreach (BaseEnemy e in enemyBattle)
                enemies.Remove(e);

            game.AddNewGameScreen(new Battle(game, tileEngine, camera, characterBattle, enemyBattle, spriteBatch, font, Content));
        }

        public static bool CheckNpcCollisions(Character a, BaseEnemy b)
        {
            Vector2 d = b.CurrentLocation - a.CurrentLocation;

            return (d.Length() < (b.CollisionRadius + a.CollisionRadius));
        }

        public bool EnemyIncludedInBattle(Character engaged, BaseEnemy compare)
        {
            Vector2 distance = engaged.CurrentLocation - compare.CurrentLocation;

            return (distance.Length() < AggroRadius);
        }


        #endregion

        #region Draw Methods
        /// <summary>
        /// Renders all sprites to screen
        /// </summary>
        /// <param name="gameTime">Game Timer</param>
        public void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Black);

            tileEngine.Draw(spriteBatch, camera);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                SpriteSortMode.BackToFront,
                SaveStateMode.None,
                camera.TransformMatrix);


            foreach (BaseEnemy e in enemies)
                e.Draw(spriteBatch, font, gameTime);

            foreach (Character player in characters)
                player.Draw(spriteBatch, font, gameTime);

            #region Debug msg
            spriteBatch.DrawString(font, "Press E to enter map editor", new Vector2(5, 5), Color.Red);

            #endregion

            spriteBatch.End();
        }
        #endregion

        public void PopulateEnemies()
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

        public void SpawnNewEnemy(int index, int tileX, int tileY)
        {
            Vector2 location = new Vector2();

            location.X = (float)tileX * (float)TileEngineV2.TileWidth;
            location.Y = (float)tileY * (float)TileEngineV2.TileHeight;

            BaseEnemy e = spawnEnemy.GetEnemyToSpawn (index);
            Type t = e.GetType();
            e = (BaseEnemy)t.Assembly.CreateInstance(t.FullName);
            e.InitializeEnemy(TileEngineV2.curEnemyTiles[index], location);
            enemies.Add (e); 
        }

        public static void UnloadAllEnemies()
        {
            enemies.Clear();         
        }

  
    }//end TestGame
}//end namespace
