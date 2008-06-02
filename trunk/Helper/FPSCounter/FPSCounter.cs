using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;
using ActionRPG;

namespace Helper
{
    /// <summary>
    /// Frames Per Second counter class used to display the
    /// current frames per second.
    /// </summary>
    public class FPSCounter
    {
        /// <summary>
        /// Value used by in-game console to draw / hide FPS
        /// </summary>
        public static int ConsoleValue
        {
            get
            {
                if (_drawFps)
                    return 1;
                else
                    return 0;
            }
            set
            {
                if (value == 1)
                    _drawFps = true;
                else if (value == 0)
                    _drawFps = false;
            }
        }

        /// <summary>
        /// Gets or sets the Draw FPS command
        /// </summary>
        public static bool DrawFramesPerSecond
        {
            get { return _drawFps; }
            set { _drawFps = value; }
        }
        static bool _drawFps = true;
        
        /// <summary>
        /// Gets or sets the draw scale for the FPS text
        /// </summary>
        public static float DrawScale
        {
            get { return _drawScale; }
            set { _drawScale = value; }
        }
        static float _drawScale = 1.0f;


        /// <summary>
        /// Total frames between resets
        /// </summary>
        int _totalFrames;


        /// <summary>
        /// Current Frames Per Second
        /// </summary>
        int _fps;


        /// <summary>
        /// Elapsed Time since last call
        /// </summary>
        float _elapsedTime;

        
        /// <summary>
        /// Updates the Frames Per Second counter
        /// </summary>
        public void Update()
        {
            _elapsedTime += Globals.DeltaTime;

            _totalFrames++;

            if (_elapsedTime >= 1.0f)
            {
                _fps = _totalFrames;
                _totalFrames = 0;
                _elapsedTime = 0;
            }
        }

        
        /// <summary>
        /// Draws the current Frames Per Second to screen
        /// </summary>
        public void Draw()
        {
            if (DrawFramesPerSecond)
            {
                Globals.Batch.DrawString(
                    Globals.Font,
                    "FPS: " + _fps.ToString(),
                    new Vector2(
                    Globals.Graphics.GraphicsDevice.Viewport.Width - 75, 5),
                    Color.Yellow,
                    0f,
                    Vector2.Zero,
                    DrawScale,
                    SpriteEffects.None,
                    0f);
            }
        }

    }//end FPSCounter
}//end Namespace
