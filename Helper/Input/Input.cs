using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Helper
{
    /// <summary>
    /// Input class is used to simplify the input code from
    /// within the game by handling all of the logic inside
    /// the helper class.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// keyboard object
        /// </summary>
        KeyboardInput keyboard = new KeyboardInput();

        /// <summary>
        /// Mouse object
        /// </summary>
        MouseInput mouse = new MouseInput();

        /// <summary>
        /// Updates the user input for this frame
        /// </summary>
        public void GetUserInput(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            keyboard.Update();
            mouse.Update();
        }

        #region Mouse
        /// <summary>
        /// Returns the current Cursor location Vector2
        /// </summary>
        public Vector2 MouseLocation
        {
            get { return mouse._CursorPosition; }
        }

        /// <summary>
        /// Returns the change in mouse Scroll Wheel divided by 100
        /// </summary>
        public int MouseScrollWheel
        {
            get { return mouse.ScrollWheel / 100; }
        }

        /// <summary>
        /// Returns if the left mouse button is pressed
        /// </summary>
        /// <returns>bool</returns>
        public bool IsMouseButtonPressedLeft()
        {
            return mouse.LeftButtonPressed();
        }
        
        /// <summary>
        /// Returns if the right mouse button is pressed
        /// </summary>
        /// <returns>bool</returns>
        public bool IsMouseButtonPressedRight()
        {
            return mouse.RightButtonPressed();
        }

        /// <summary>
        /// Returns if the left mouse button is held down
        /// </summary>
        /// <returns>bool</returns>
        public bool IsMouseButtonHeldLeft()
        {
            return mouse.LeftButtonHeld();
        }
        
        /// <summary>
        /// Returns if the right mouse button is held down
        /// </summary>
        /// <returns>bool</returns>
        public bool IsMouseButtonHeldRight()
        {
            return mouse.RightButtonHeld();
        }

        #endregion

        #region Keyboard
        /// <summary>
        /// Checks to see if a key was newly pressed
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True: Key is pressed - False: Key is not pressed, or key is held</returns>
        public bool IsKeyPressed(Keys key)
        {
            return keyboard.IsKeyPressed(key);
        }

        /// <summary>
        /// Checks to see if a key is being held down
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True: Key is being held down - False: Key is not being held down, or was pressed</returns>
        public bool IsKeyHeld(Keys key)
        {
            return keyboard.IsKeyHeld(key);
        }

        /// <summary>
        /// Converts a key on the keyboard to a Char
        /// </summary>
        /// <param name="key">key to convert</param>
        /// <returns>Char</returns>
        public Char ConvertKeyToChar(Keys key)
        {
            bool shiftPressed = (keyboard.IsKeyHeld(Keys.LeftShift) ||
                                 keyboard.IsKeyHeld(Keys.RightShift));

            return keyboard.KeyToChar(key, shiftPressed);
        }

        public List<Keys> GetPressedKeys
        {
            get
            {
                if (timer >= .01f)
                {
                    timer = 0;
                    return keyboard.GetPressedKeys;
                }
                else return new List<Keys>();
            }
        }
        float timer = 0;

        #endregion

    }//end class
}//end namespace
