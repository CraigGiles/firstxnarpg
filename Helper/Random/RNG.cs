using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Helper
{
    /// <summary>
    /// Random Helper class used to streamline getting new 
    /// random values for variables such as int, float, and
    /// vectors
    /// </summary>
    public static class RNG
    {
        #region Variables

        public static Random globalRandomGenerator = GenerateNewRandomGenerator();

        #endregion

        /// <summary>
        /// Generates a new random generator based on the DateTime
        /// </summary>
        /// <returns></returns>
        public static Random GenerateNewRandomGenerator()
        {
            return globalRandomGenerator =
                new Random((int)DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Generates a random int
        /// </summary>
        /// <param name="max">Greatest value for int</param>
        /// <returns>int</returns>
        public static int GetRandomInt(int max)
        {
            return globalRandomGenerator.Next(max);
        }

        /// <summary>
        /// Generates a random float between two values
        /// </summary>
        /// <param name="min">lowest value</param>
        /// <param name="max">highest value</param>
        /// <returns>float</returns>
        public static float GetRandomFloat(float min, float max)
        {
            return (float)globalRandomGenerator.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Generates a random byte between two values
        /// </summary>
        /// <param name="min">lowest value</param>
        /// <param name="max">highest value</param>
        /// <returns>byte</returns>
        public static byte GetRandomByte(byte min, byte max)
        {
            return (byte)globalRandomGenerator.Next(min, max);
        }

        /// <summary>
        /// Generates a random Vector2
        /// </summary>
        /// <param name="min">lowest value</param>
        /// <param name="max">highest value</param>
        /// <returns>Vector2</returns>
        public static Vector2 GetRandomVector2(float min, float max)
        {
            return new Vector2(
                GetRandomFloat(min, max),
                GetRandomFloat(min, max));
        }

        /// <summary>
        /// Generates a random Vector3
        /// </summary>
        /// <param name="min">lowest value</param>
        /// <param name="max">highest value</param>
        /// <returns>Vector3</returns>
        public static Vector3 GetRandomVector3(float min, float max)
        {
            return new Vector3(
                GetRandomFloat(min, max),
                GetRandomFloat(min, max),
                GetRandomFloat(min, max));
        }

        /// <summary>
        /// Get random color
        /// </summary>
        /// <returns>Color</returns>
        public static Color GetRandomColor()
        {
            return new Color(new Vector3(
                GetRandomFloat(0.25f, 1.0f),
                GetRandomFloat(0.25f, 1.0f),
                GetRandomFloat(0.25f, 1.0f)));
        } 

    }
}
