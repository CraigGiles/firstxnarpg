using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ActionRPG
{
    public class TextAnimation
    {

        /// <summary>
        /// Text to render 
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        string text;


        /// <summary>
        /// Location of the text animation
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;


        /// <summary>
        /// Velocity of text animation
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;


        /// <summary>
        /// Color of text animation
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        Color color;


        /// <summary>
        /// Time text animation stays on screen
        /// </summary>
        public float Timer
        {
            get { return timer; }
            set { timer = (float)Math.Max(value, 0); }
        }
        float timer;


        /// <summary>
        /// If the animation is active and rendering
        /// to screen
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = true; }
        }
        bool isActive = false;


        /// <summary>
        /// Alpha variable for text 
        /// </summary>
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }
        float alpha = 255.0f;


        /// <summary>
        /// Creates a new text animation object
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="position">starting position of text</param>
        /// <param name="velocity">speed to travel</param>
        /// <param name="color">color of text</param>
        /// <param name="timer">time to stay active</param>
        public TextAnimation(string text, Vector2 position, float velocity, Color color, float timer)
        {
            this.text = text;
            this.position = position;
            this.velocity = new Vector2(0, -velocity);
            this.color = color;
            this.timer = timer;
            this.isActive = true;
        }


        /// <summary>
        /// Updates the text animation position
        /// </summary>
        public void Update()
        {
            position += velocity;
            timer -= Globals.DeltaTime;

            if (timer <= 0)
                isActive = false;

            velocity *= .99f;
            alpha *= .995f;
        }


        /// <summary>
        /// Renders the text animation to screen
        /// </summary>
        public void Draw()
        {
            Globals.Batch.DrawString(
                Globals.Font,
                Text,
                Position,
                new Color(color.R, color.G, color.B, (byte)alpha),
                0f,
                Vector2.Zero,
                1.5f,
                SpriteEffects.None,
                0f);

        }

    }
}
