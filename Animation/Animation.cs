using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class Animation
    {
        public bool Paused
        {
            get { return paused; }
            set { paused = value; }
        }
        bool paused = false;


        /// <summary>
        /// The name of the animation
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string name;


        /// <summary>
        /// The name of the animation
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        string type;
        

        /// <summary>
        /// Direction of the animation
        /// </summary>
        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        Direction direction;


        /// <summary>
        /// List of animation frames
        /// </summary>
        public List<Rectangle> Frames
        {
            get { return frame; }
            set { frame = value; }
        }
        List<Rectangle> frame;


        /// <summary>
        /// Width of the sprite
        /// </summary>
        public int Width
        {
            get { return Frames[0].Width; }
        }
        /// <summary>
        /// Height of the sprite
        /// </summary>
        public int Height
        {
            get { return Frames[0].Height; }
        }


        /// <summary>
        /// Interval between animation frames
        /// </summary>
        public float Interval
        {
            get { return interval; }
            set { interval = value; }
        }
        float interval;


        /// <summary>
        /// If true, the animation loops
        /// </summary>
        public bool IsLoop
        {
            get { return isLoop; }
            set { isLoop = value; }
        }
        bool isLoop;


        /// <summary>
        /// if true, the animation pauses at the end
        /// </summary>
        public bool PauseAtEnd
        {
            get { return pauseAtEnd; }
            set { pauseAtEnd = value; }
        }
        bool pauseAtEnd;


        #region Constructors


        /// <summary>
        /// Creates a new Animation object.
        /// </summary>
        public Animation() { }


        /// <summary>
        /// Creates a new Animation object by full specification.
        /// </summary>
        public Animation(string name,Direction direction, List<Rectangle> frames, 
            float interval, bool isLoop, bool pauseAtEnd)
        {
            this.Name = name + direction.ToString();
            this.Type = name;
            this.Direction = direction;
            this.Frames = frames;
            this.Interval = interval;
            this.IsLoop = isLoop;
            this.PauseAtEnd = pauseAtEnd;
        }


        #endregion
    }
}
