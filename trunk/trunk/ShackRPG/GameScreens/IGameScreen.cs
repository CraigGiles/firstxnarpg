#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
#endregion

namespace ShackRPG.GameScreens
{
	/// <summary>
	/// Game screen helper interface for all game screens of our game.
	/// Helps us to put them all into one list and manage them in our BaseGame.
	/// </summary>
	public interface IGameScreen
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
	} // IGameScreen
} // namespace RocketCommanderXna.GameScreens
