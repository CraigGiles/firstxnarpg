using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ActionRPG
{
    public class TextManager
    {
        static List<TextAnimation> animations = new List<TextAnimation>();
        static List<TextAnimation> animationsToRemove = new List<TextAnimation>();

        public static void LaunchAnimation(string text, Vector2 position, float velocity, Color color, float timer)
        {
            //adjusts the position by char length (Assuming Font Size = 10)
            position.X -= (text.Length * 10) / 2;

            //Adds animation to the list of animations
            animations.Add(new TextAnimation(text, position, velocity, color, timer));
        }

        public static void Update()
        {
            foreach (TextAnimation animation in animations)
            {
                animation.Update();

                if (!animation.IsActive)
                    animationsToRemove.Add(animation);
            }

            RemoveFinishedAnimations();
        }

        private static void RemoveFinishedAnimations()
        {
            foreach (TextAnimation a in animationsToRemove)
                animations.Remove(a);

            animationsToRemove.Clear();
        }

        public static void Draw()
        {

            foreach (TextAnimation a in animations)
            {
                a.Draw();
            }
        }

    }
}
