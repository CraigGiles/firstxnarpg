using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

using TileEngine;
using Microsoft.Xna.Framework.Graphics;

namespace ShackRPG
{
    public class BaseObjects
    {
        protected int _Id;

        protected Texture2D _Texture;

        protected string _Name;

        #region Properties

        public int Id
        {
            get { return _Id; }
        }

        public string Name
        {
            get { return _Name; }
        }

        public Texture2D Texture
        {
            get { return _Texture; }
        }
        #endregion

    }
}
