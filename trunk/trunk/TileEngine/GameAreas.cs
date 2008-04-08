using System;
using System.Collections.Generic;
using System.Text;

namespace TileEngine
{
    public class GameArea
    {
        public enum GameAreas
        {
            NewGame,
            TestField1,
            TestField2,
            TestField3,
        }

        public enum EnemiesField
        {
            Flower,
        }

        public static GameAreas
            curGameArea = GameAreas.NewGame,
            prevGameArea;


    }
}
