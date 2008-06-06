using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public class Animation
    {
        /// <summary>
        /// The name of the animation.
        /// </summary>
        private string name;

        /// <summary>
        /// The name of the animation.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }



        /// <summary>
        /// The first frame of the animation.
        /// </summary>
        private int startingFrame;

        /// <summary>
        /// The first frame of the animation.
        /// </summary>
        public int StartingFrame
        {
            get { return startingFrame; }
            set { startingFrame = value; }
        }


        /// <summary>
        /// The last frame of the animation.
        /// </summary>
        private int endingFrame;

        /// <summary>
        /// The last frame of the animation.
        /// </summary>
        public int EndingFrame
        {
            get { return endingFrame; }
            set { endingFrame = value; }
        }


        /// <summary>
        /// The interval between frames of the animation.
        /// </summary>
        private int interval;

        /// <summary>
        /// The interval between frames of the animation.
        /// </summary>
        public int Interval
        {
            get { return interval; }
            set { interval = value; }
        }


        /// <summary>   
        /// If true, the animation loops.
        /// </summary>
        private bool isLoop;

        /// <summary>
        /// If true, the animation loops.
        /// </summary>
        public bool IsLoop
        {
            get { return isLoop; }
            set { isLoop = value; }
        }


        #region Constructors


        /// <summary>
        /// Creates a new Animation object.
        /// </summary>
        public Animation() { }


        /// <summary>
        /// Creates a new Animation object by full specification.
        /// </summary>
        public Animation(string name, int startingFrame, int endingFrame, int interval,
            bool isLoop)
        {
            this.Name = name;
            this.StartingFrame = startingFrame;
            this.EndingFrame = endingFrame;
            this.Interval = interval;
            this.IsLoop = isLoop;
        }


        #endregion
    }
}
