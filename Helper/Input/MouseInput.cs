using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Helper
{
    class MouseInput
    {
        public MouseState 
            _CurrentState,
            _PreviousState;

        public Vector2 _CursorPosition;
        public Rectangle _ClickRectangle;

        int _CurrentScrollValue,
            _PreviousScrollValue;

        public int ScrollWheel
        {
            get { return _CurrentScrollValue; }
        }

        public void Update()
        {
            _PreviousState = _CurrentState;
            _CurrentScrollValue = 0;
            _CurrentState = Mouse.GetState();

            _CursorPosition.X = Mouse.GetState().X;
            _CursorPosition.Y = Mouse.GetState().Y;

            _CurrentScrollValue += _CurrentState.ScrollWheelValue - _PreviousState.ScrollWheelValue;
        }

        public bool LeftButtonPressed()
        {
            if (_CurrentState.LeftButton == ButtonState.Pressed &&
                _PreviousState.LeftButton == ButtonState.Released)
                return true;
            else
                return false;

        }

        public bool LeftButtonHeld()
        {
            if (_CurrentState.LeftButton == ButtonState.Pressed &&
                _PreviousState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;

        }

        public bool RightButtonPressed()
        {
            if (_CurrentState.RightButton == ButtonState.Pressed &&
                _PreviousState.RightButton == ButtonState.Released)
                return true;
            else
                return false;

        }

        public bool RightButtonHeld()
        {
            if (_CurrentState.RightButton == ButtonState.Pressed &&
                _PreviousState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;

        }
    }
}
