using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ActionRPG
{
    public class SpriteManager
    {

        #region Sprite Data

        /// <summary>
        /// Sprite sheet containing all frames of a character
        /// </summary>
        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }
        Texture2D spriteSheet;


        /// <summary>
        /// Width and Height of the sprite
        /// </summary>
        public Point SpriteDimensions
        {
            get { return spriteDimensions; }
            set { spriteDimensions = value; }
        }
        Point spriteDimensions;


        /// <summary>
        /// Sprite box used for drawing sprite and
        /// collision detection
        /// </summary>
        public Rectangle SpriteBox
        {
            get { return spriteBox; }
            set { spriteBox = value; }
        }
        Rectangle spriteBox;


        /// <summary>
        /// Called once per frame to update the position of the spritebox
        /// </summary>
        /// <param name="position">Characters position vector</param>
        public void UpdateSpriteBox(Vector2 position)
        {
            spriteBox.X = (int)position.X;
            spriteBox.Y = (int)position.Y;

            spriteBox.Width = SpriteDimensions.X;
            spriteBox.Height = SpriteDimensions.Y;
        }

        #endregion


        #region Animation Data

        public string CurrentAnimationName
        {
            get { return currentAnimation.Name; }
        }

        /// <summary>
        /// List of available animations
        /// </summary>
        public List<Animation> Animations
        {
            get { return animations; }
            set { animations = value; }
        }
        List<Animation> animations = new List<Animation>();


        /// <summary>
        /// Rectangle used to get the current sprite
        /// from spritesheet
        /// </summary>
        public Rectangle SourceRectangle
        {
            get 
            {
                if (currentAnimation != null)
                    return currentAnimation.Frames[frame];
                else
                    return new Rectangle(0, 0, 0, 0);
            }
        }


        /// <summary>
        /// Current animation playing by character
        /// </summary>
        Animation currentAnimation = null;

        /// <summary>
        /// Previous animation played by character
        /// </summary>
        Animation previousAnimation = null;


        /// <summary>
        /// Elapsed time since last call
        /// </summary>
        float elapsedTime;


        /// <summary>
        /// Current frame of animation
        /// </summary>
        public int CurrentFrame
        {
            get { return frame; }
        }
        private int Frame
        {
            get { return frame; }
            set { frame = (int)MathHelper.Clamp(value, 0, currentAnimation.Frames.Count - 1); }
        }
        int frame = 0;


        /// <summary>
        /// Last direction moved by the character
        /// </summary>
        public Direction LastDirection
        {
            get { return lastDirection; }
        }
        Direction lastDirection = Direction.Down;

        #endregion             


        #region Animation Playback


        /// <summary>
        /// Starts playback of a specific animation
        /// </summary>
        /// <param name="animation">Animation to begin playing</param>
        public void PlayAnimation(Animation animation)
        {
            currentAnimation = animation;
        }


        /// <summary>
        /// Starts playback of a specific animation based
        /// on the last direction character was moving
        /// </summary>
        /// <param name="name"></param>
        public void PlayAnimation(string name)
        {
            PlayAnimation(name, lastDirection);
        }
        

        /// <summary>
        /// Starts playback of an animation with the name + directions
        /// character is moving
        /// </summary>
        /// <param name="name">Name of animation - IE: "Standing"</param>
        /// <param name="down">Is character moving down</param>
        /// <param name="left">Is character moving left</param>
        /// <param name="up">Is character moving up</param>
        /// <param name="right">Is character moving right</param>
        public void PlayAnimation(string name, bool down, bool left, bool up, bool right)
        {
            if (right)
                PlayAnimation(name, Direction.Right);


            if (left)
                PlayAnimation(name, Direction.Left);

            if (up)
                PlayAnimation(name, Direction.Up);

            if (down)
                PlayAnimation(name, Direction.Down);
        }


        /// <summary>
        /// Starts playback of an animation with the name + direction specified
        /// </summary>
        /// <param name="name">Name of animation - IE: "Standing"</param>
        /// <param name="direction">Direction of animation</param>
        public void PlayAnimation(string name, Direction direction)
        {
            previousAnimation = currentAnimation;
            currentAnimation = SetCurrentAnimation(name + direction.ToString());
                        
            lastDirection = direction;

            if (previousAnimation != null && previousAnimation.Type != currentAnimation.Type)
            {
                elapsedTime = 0;
                frame = 0;
            }

        }


        /// <summary>
        /// Loads an animation for the player
        /// </summary>
        /// <param name="asset">Asset name of animation, IE: "StandingDown"</param>
        /// <returns>Animation</returns>
        private Animation SetCurrentAnimation(string asset)
        {
            foreach (Animation a in animations)
                if (a.Name == asset)
                    return a;

            return null;
        }


        /// <summary>
        /// Jumps to a specific frame in the animation
        /// </summary>
        /// <param name="jumpTo">Frame to skip ahead to</param>
        public void JumpToFrame(int jumpTo)
        {            
            frame = (int)MathHelper.Clamp(jumpTo, 0, currentAnimation.Frames.Count - 1);
        }

        public void AdvanceFrameByOne()
         {
            if (frame++ < currentAnimation.Frames.Count - 1)
                frame = (int)MathHelper.Clamp(frame++, 0, currentAnimation.Frames.Count - 1);
            else
                frame = 0;
        }


        /// <summary>
        /// Resets the animation frame
        /// </summary>
        public void ResetAnimation()
        {
            frame = 0;
        }


        public void PauseAnimation(bool value)
        {
            currentAnimation.Paused = value;
        }


        #endregion


        #region Update Animation


        /// <summary>
        /// Update the current animation.
        /// </summary>
        public void UpdateAnimation()
        {
            if (!currentAnimation.Paused)
            {
                if (IsAnimationComplete)
                {
                    if (currentAnimation.IsLoop)
                        Frame = 0;


                    if (!currentAnimation.PauseAtEnd)
                        PlayAnimation("Standing");

                    return;
                }


                // update the elapsed time
                elapsedTime += Globals.DeltaTime;

                // advance to the next frame if ready
                if (elapsedTime > (float)currentAnimation.Interval)
                {
                    Frame++;
                    //elapsedTime -= (float)currentAnimation.Interval;
                    elapsedTime = 0;
                }
            }
        }


        /// <summary>
        /// Checks to see if animation has finished
        /// </summary>
        public bool IsAnimationComplete
        {
            get
            {
                return Frame >= currentAnimation.Frames.Count - 1;
            }
        }


        #endregion


        #region Draw


        /// <summary>
        /// Renders the sprites current frame to screen
        /// </summary>
        public void DrawAnimation()
        {
            Globals.Batch.Draw(spriteSheet, spriteBox, SourceRectangle, Color.White);
        }


        public void DrawSprite()
        {
            Globals.Batch.Draw(spriteSheet, spriteBox, Color.White);
        }

        #endregion
        
    }
}
