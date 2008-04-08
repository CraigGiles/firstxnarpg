using System;
using System.Collections.Generic;
using System.Text;

using TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ShackRPG.GameScreens;

namespace ShackRPG
{
    public class Animation
    {       
 
        /// <summary>
        /// Bools indicating which animation system is being used
        /// </summary>
        bool _IsTimed;          //currently unused
        bool _IsSpriteSheet;    //True: SpriteSheet - False: Seperate Textures
        int _SpriteIndex;       //Current frame to be drawn

        float _TimePerFrame;    //Amnt of time a frame is drawn on screen before changing
        float _Time;            //Timer for animations

        /// <summary>
        /// For use with animations not using a spritesheet
        /// </summary>
        Texture2D _CurrentTexture;
        List<Texture2D> _Textures = new List<Texture2D>();

        /// <summary>
        /// for use with animations using a spritesheet and
        /// source rectangles
        /// </summary>
        Rectangle _SpriteToDraw;
        Texture2D _SpriteSheet;
        List<Rectangle> _SourceRectangle = new List<Rectangle>();

        /// <summary>
        /// Creates an animation with a list of textures
        /// </summary>
        /// <param name="textures">Textures to use for animation</param>
        /// <param name="timePerFrame">Time to show the current frame</param>
        public Animation(List<Texture2D> textures, float timePerFrame)
        {
            _Textures = textures;
            _TimePerFrame = timePerFrame;

            _CurrentTexture = textures[0];

            _IsSpriteSheet = false;
            _IsTimed = true;
            _SourceRectangle = null;
        }

        /// <summary>
        /// Creates an animation with a sprite sheet
        /// </summary>
        /// <param name="spriteSheet">Sprite Sheet used for animation</param>
        /// <param name="sourceRects">Source Rectangles to determine current frame on sprite sheet</param>
        /// <param name="timePerFrame">Time to show the current frame</param>
        public Animation(Texture2D spriteSheet, List<Rectangle> sourceRects, float timePerFrame)
        {
            _SpriteSheet = spriteSheet;
            _SourceRectangle = sourceRects;
            _TimePerFrame = timePerFrame;

            _SpriteToDraw = sourceRects[0];
            _IsSpriteSheet = true;
            _IsTimed = true;
        }

        /// <summary>
        /// Updates the animation
        /// </summary>
        public void UpdateAnimation(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_IsTimed)                       //if the animation is timed
            {
                if ((_Time -= elapsed) <= 0)    //if the timer reaches 0
                {
                    _SpriteIndex++;             //increase the sprite index
                    CheckForIndexOutOfRange();  //ensure index is valid
                    _Time = _TimePerFrame;      //reset timer
                }
            }
            else
            {
                /* currently unused */
            }

            /* Updates the sprite sheet animation */
            if (_IsSpriteSheet)
                _SpriteToDraw = _SourceRectangle[_SpriteIndex];

            /* Updates the seperate textures animation */
            else
                _CurrentTexture = _Textures[_SpriteIndex];
        }

        /// <summary>
        /// Checks to see if the Sprite Index is out of range and
        /// resets it back to 0 if it is, looping the animation
        /// </summary>
        private void CheckForIndexOutOfRange()
        {
            if (_IsSpriteSheet)
            {
                if (_SpriteIndex > _SourceRectangle.Count - 1)
                    _SpriteIndex = 0;
            }
            else
            {
                if (_SpriteIndex > _Textures.Count - 1)
                    _SpriteIndex = 0;
            }
        }

        /// <summary>
        /// Draws the current animation to screen
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        /// <param name="location">Location of the sprite to draw</param>
        /// <param name="width">Width of the sprite</param>
        /// <param name="height">Height of the sprite</param>
        public void Draw(SpriteBatch batch, Vector2 location, int width, int height)
        {
            /* Draws the spritesheet animation */
            if (_IsSpriteSheet)
            {
                batch.Draw(_SpriteSheet,
                    new Rectangle((int)location.X, (int)location.Y, width, height),
                    _SpriteToDraw,
                    Color.White);
            }

            /* Draws the texture animation */
            else
            {
                batch.Draw(_CurrentTexture,
                    new Rectangle((int)location.X, (int)location.Y, width, height),
                    Color.White);
            }
        }//end Draw

    }//end class
}//end namespace
