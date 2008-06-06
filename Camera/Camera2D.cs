using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ActionRPG
{
	public class Camera2D
	{
        public float Speed = 5;
		public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// Matrix used for drawing in-game WorldItems
        /// </summary>
        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0f));
            }
        }

        /// <summary>
        /// Matrix used in map editor for drawing the UI overlay
        /// </summary>
        public Matrix MapEditorTransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(Position, 0f));
            }
        }

	}//end Camera Class
}//end TileEngine
