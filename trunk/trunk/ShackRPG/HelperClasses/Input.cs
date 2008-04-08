using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShackRPG
{
    public static class Input
    {
        public static KeyboardState curKeyboardState, prevKeyboardState;

        public static bool
            Up = false,
            Down = false,
            Left = false,
            Right = false,
            Run = false;

        public static bool
            GetNext = false,
            GetPrevious = false;

        public static bool
            Attack = false,
            Defend = false,
            Magic = false,
            Item = false;

        public static bool
            Enter = false,
            Escape = false,
            Space = false,
            Shift = false,
            Tilde = false,
            Inventory = false;

        public static void GetUserInput()
        {
            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();

            #region Movement Keys
            if (curKeyboardState.IsKeyDown(Keys.S))
                Down = true;
            else
                Down = false;

            if (curKeyboardState.IsKeyDown(Keys.A))
                Left = true;
            else
                Left = false;

            if (curKeyboardState.IsKeyDown(Keys.W))
                Up = true;
            else
                Up = false;

            if (curKeyboardState.IsKeyDown(Keys.D))
                Right = true;
            else
                Right = false;

            if (curKeyboardState.IsKeyDown(Keys.Space))
                Run = true;
            else
                Run = false;
            #endregion

            #region Various Keys
            if (curKeyboardState.IsKeyUp(Keys.Enter) && prevKeyboardState.IsKeyDown(Keys.Enter))
                Enter = true;
            else
                Enter = false;

            if (curKeyboardState.IsKeyDown(Keys.Escape))
                Escape = true;
            else
                Escape = false;

            if (curKeyboardState.IsKeyDown(Keys.Space))
                Space = true;
            else
                Space = false;

            if (curKeyboardState.IsKeyDown(Keys.LeftShift))
                Shift = true;
            else
                Shift = false;

            if (curKeyboardState.IsKeyUp(Keys.OemTilde) && prevKeyboardState.IsKeyDown(Keys.OemTilde))
                Tilde = true;
            else
                Tilde = false;

            if (curKeyboardState.IsKeyUp(Keys.D) && prevKeyboardState.IsKeyDown(Keys.D))
                GetNext = true;
            else
                GetNext = false;

            if (curKeyboardState.IsKeyUp(Keys.A) && prevKeyboardState.IsKeyDown(Keys.A))
                GetPrevious = true;
            else
                GetPrevious = false;

            if (curKeyboardState.IsKeyUp(Keys.C) && prevKeyboardState.IsKeyDown(Keys.C))
                Inventory = true;
            else
                Inventory = false;
            #endregion

            #region Battle Keys
            if (curKeyboardState.IsKeyDown(Keys.NumPad2))
                Defend = true;
            else
                Defend = false;

            if (curKeyboardState.IsKeyUp(Keys.NumPad0) && prevKeyboardState.IsKeyDown(Keys.NumPad0))
                Attack = true;
            else
                Attack = false;

            if (curKeyboardState.IsKeyUp(Keys.NumPad1) && prevKeyboardState.IsKeyDown(Keys.NumPad1))
                Magic = true;
            else
                Magic = false;
            #endregion
        }


    }
}
