using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShackRPG.GameComponents
{
    public class FPSCounter : GameComponent
    {
        float fps = 0;
        float totalTime = 0;
        float frames = 0;

        public FPSCounter(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            totalTime += (float)gameTime.ElapsedRealTime.TotalSeconds;
            frames++;

            if (totalTime >= 1.0f)
            {
                totalTime = totalTime - (float)((int)totalTime);

                fps = frames;

                frames = 0;

                base.Game.Window.Title = "FPS: " + fps.ToString();
                //System.Diagnostics.Debug.WriteLine("FPS: " + fps.ToString());
            }
            base.Update(gameTime);
        }
   }
}
