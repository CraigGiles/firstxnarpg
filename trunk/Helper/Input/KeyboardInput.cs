using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Helper
{
    /// <summary>
    /// Helps to manage keyboard input and provides some
    /// methods to check and see if a key was just pressed,
    /// being held down, and conversion methods for converting
    /// a key to a Char
    /// </summary>
    class KeyboardInput
    {
        /// <summary>
        /// Keyboard states, current frame and previous frame
        /// </summary>
        KeyboardState
            _CurrentState,
            _PreviousState;
  
        /// <summary>
        /// List of keys pressed last frame
        /// </summary>
        List<Keys> _PreviousKeysPressed = new List<Keys>();

        /// <summary>
        /// List of keys pressed this frame
        /// </summary>
        List<Keys> _CurrentKeysPressed = new List<Keys>();

        /// <summary>
        /// Gets a list of the currently pressed keys
        /// </summary>
        public List<Keys> GetPressedKeys
        {
            get { return _CurrentKeysPressed; }
        }

        /// <summary>
        /// Updates our keyboard state
        /// </summary>
        public void Update()
        {
            /* fills our list with keys that were pressed in the previous frame */
            _PreviousKeysPressed = new List<Keys>(_CurrentState.GetPressedKeys());

            _PreviousState = _CurrentState;         //Sets previous keyboard state to the current state
            _CurrentState = Keyboard.GetState();    //Refreshes the current state
            _CurrentKeysPressed = new List<Keys>(_CurrentState.GetPressedKeys());
        }

        /// <summary>
        /// Checks to see if a key was just pressed
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>bool</returns>
        internal bool IsKeyPressed(Keys key)
        {
            return (_CurrentState.IsKeyDown(key) && 
                !_PreviousKeysPressed.Contains(key));
        }

        /// <summary>
        /// Checks to see if a key is being held down
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>bool</returns>
        internal bool IsKeyHeld(Keys key)
        {
            return (_CurrentState.IsKeyDown(key) && 
                _PreviousKeysPressed.Contains(key));
        }

        /// <summary>
        /// Converts key to its respected character
        /// </summary>
        /// <param name="key">Key to convert into char</param>
        /// <param name="shiftPressed">Is shift currently pressed</param>
        /// <returns>Char</returns>
        public char KeyToChar(Keys key, bool shiftPressed)
        {
            // If key will not be found, just return space
            char ret = ' ';
            int keyNum = (int)key;
            if (keyNum >= (int)Keys.A && keyNum <= (int)Keys.Z)
            {
                if (shiftPressed)
                    ret = key.ToString()[0];
                else
                    ret = key.ToString().ToLower()[0];
            } // if (keyNum)
            else if (keyNum >= (int)Keys.D0 && keyNum <= (int)Keys.D9 &&
                shiftPressed == false)
            {
                ret = (char)((int)'0' + (keyNum - Keys.D0));
            } // else if
            else if (key == Keys.D1 && shiftPressed)
                ret = '!';
            else if (key == Keys.D2 && shiftPressed)
                ret = '@';
            else if (key == Keys.D3 && shiftPressed)
                ret = '#';
            else if (key == Keys.D4 && shiftPressed)
                ret = '$';
            else if (key == Keys.D5 && shiftPressed)
                ret = '%';
            else if (key == Keys.D6 && shiftPressed)
                ret = '^';
            else if (key == Keys.D7 && shiftPressed)
                ret = '&';
            else if (key == Keys.D8 && shiftPressed)
                ret = '*';
            else if (key == Keys.D9 && shiftPressed)
                ret = '(';
            else if (key == Keys.D0 && shiftPressed)
                ret = ')';
            else if (key == Keys.OemTilde)
                ret = shiftPressed ? '~' : '`';
            else if (key == Keys.OemMinus)
                ret = shiftPressed ? '_' : '-';
            else if (key == Keys.OemPipe)
                ret = shiftPressed ? '|' : '\\';
            else if (key == Keys.OemOpenBrackets)
                ret = shiftPressed ? '{' : '[';
            else if (key == Keys.OemCloseBrackets)
                ret = shiftPressed ? '}' : ']';
            else if (key == Keys.OemSemicolon)
                ret = shiftPressed ? ':' : ';';
            else if (key == Keys.OemQuotes)
                ret = shiftPressed ? '"' : '\'';
            else if (key == Keys.OemComma)
                ret = shiftPressed ? '<' : '.';
            else if (key == Keys.OemPeriod)
                ret = shiftPressed ? '>' : ',';
            else if (key == Keys.OemQuestion)
                ret = shiftPressed ? '?' : '/';
            else if (key == Keys.OemPlus)
                ret = shiftPressed ? '+' : '=';

            // Return result
            return ret;
        }
    }
}
