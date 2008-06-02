using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class Globals
    {
        public static BaseGame Game;
        public static ContentManager Content;
        public static SpriteBatch Batch;
        public static SpriteFont Font;
        public static GraphicsDeviceManager Graphics;
        public static Helper.Input Input;

        public static GameTime GameTime;
        public static float DeltaTime;

        public static TileEngine TileEngine;
        public static Camera2D Camera;

    }
}
