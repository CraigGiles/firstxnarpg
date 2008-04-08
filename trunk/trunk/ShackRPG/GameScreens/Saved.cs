using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TileEngine;

namespace ShackRPG.GameScreens 
{
    class Saved : IGameScreen
    {
        ContentManager content;
        SpriteBatch batch;
        BaseGame game;
        SpriteFont font;

        Texture2D tBackground;

        bool draw = true;

        public Saved(ContentManager sContent, SpriteBatch sSpriteBatch, BaseGame sGame, SpriteFont sFont)
        {
            this.content = sContent;
            this.batch = sSpriteBatch;
            this.game = sGame;
            this.font = sFont;

            tBackground = content.Load<Texture2D>(@"DimBackground");
        }

        public void Update(GameTime gameTime)
        {
            if (Input.Enter)
                game.RemoveCurrentGameScreen();
        }

        public void Draw(GameTime gameTime)
        {
            if (draw)
            {
                batch.Begin(SpriteBlendMode.AlphaBlend);
                batch.Draw(tBackground, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height), Color.White);
                batch.DrawString(font, "Progress Saved", new Vector2(260, 250), Color.DarkCyan);
                batch.DrawString(font, " Press Enter", new Vector2(275, 300), Color.DarkCyan);

                batch.End();
                draw = false;
            }
        }

    }
}
