using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class MessageBox : GameScreen
    {
        float timer;

        Vector2 playerLocation;
        Texture2D box;

        Point window = new Point(
            Globals.Graphics.GraphicsDevice.Viewport.Width, 
            100);
        string text;

        public MessageBox(string text,float timer)
        {
            this.text = text;
            this.timer = timer;
        }

        public override void LoadContent()
        {
            box = Globals.Content.Load<Texture2D>(@"Graphics/Debug/Console");

            base.LoadContent();
        }

        public override void Update(bool coveredByOtherScreen)
        {
            timer -= Globals.DeltaTime;

            if (Globals.Input.IsKeyPressed(Globals.Settings.Okay) ||
                timer <= 0)
            {
                ScreenManager.RemoveScreen(this);
            }

            base.Update(coveredByOtherScreen);
        }

        public override void Draw()
        {

            Globals.Batch.Begin(SpriteBlendMode.AlphaBlend);

            Globals.Batch.Draw(
                box, 
                new Rectangle(0,0, window.X, window.Y), 
                new Color(Color.Black.R, Color.Black.G, Color.Black.B, (byte)225));

            Globals.Batch.DrawString(Globals.Font,
                text,
                new Vector2(10, 25),
                Color.White);

            Globals.Batch.End();
            base.Draw();
        }
    }
}
