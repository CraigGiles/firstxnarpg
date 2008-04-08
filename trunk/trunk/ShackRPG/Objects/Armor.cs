using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

using TileEngine;
using Microsoft.Xna.Framework.Graphics;

namespace ShackRPG
{
    public class Armor : BaseObjects
    {
        int _Defense = 0;

        public int Defense
        {
            get { return _Defense; }
        }

        public Armor()
        {
        }

        public Armor(
            int id,
            Texture2D texture,
            string name,
            int defense)
        {
            _Id = id;
            _Texture = texture;

            _Name = name;          
            _Defense = defense;
        }

    }
}
