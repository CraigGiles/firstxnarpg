using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Helper
{
    class GamePadInput
    {
        GamePadState currentState, previousState;
        List<Buttons> previousButtonsPressed = new List<Buttons>();
        List<Buttons> currentButtonsPressed = new List<Buttons>();

        GamePadDeadZone deadZone = GamePadDeadZone.Circular;

        public List<Buttons> GetPressedButtons
        {
            get { return currentButtonsPressed; }
        }

        public void Update()
        {
           
            previousButtonsPressed.Clear();

            if (currentState.Buttons.A == ButtonState.Pressed)
                previousButtonsPressed.Add(Buttons.A);

            if (currentState.Buttons.B == ButtonState.Pressed)
                previousButtonsPressed.Add(Buttons.B);

            if (currentState.Buttons.X == ButtonState.Pressed)
                previousButtonsPressed.Add(Buttons.X);

            if (currentState.Buttons.Y == ButtonState.Pressed)
                previousButtonsPressed.Add(Buttons.Y);

            if (currentState.DPad.Down == ButtonState.Pressed)
                previousButtonsPressed.Add(Buttons.DPadDown);

            if (currentState.DPad.Left == ButtonState.Pressed)
                previousButtonsPressed.Add(Buttons.DPadLeft);

            if (currentState.DPad.Up == ButtonState.Pressed)
                previousButtonsPressed.Add(Buttons.DPadUp);

            if (currentState.DPad.Right == ButtonState.Pressed)
                previousButtonsPressed.Add(Buttons.DPadRight);



            previousState = currentState;
            currentState = GamePad.GetState(PlayerIndex.One);
        }

        public bool IsButtonPressed(Buttons button)
        {
            return (currentState.IsButtonDown(button) &&
                !previousButtonsPressed.Contains(button));
        }


        public bool IsButtonHeld(Buttons button)
        {
            return (currentState.IsButtonDown(button) &&
                previousButtonsPressed.Contains(button));
        }

        public bool LeftThumbstickHeld()
        {
            return (currentState.ThumbSticks.Left != Vector2.Zero);
        }

        public Vector2 LeftThumbstick
        {
            get { return new Vector2(currentState.ThumbSticks.Left.X, -currentState.ThumbSticks.Left.Y); }
        }
    }
}
