using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

using TileEngine;
using Microsoft.Xna.Framework.Graphics;

namespace ShackRPG
{
    public class Weapon : BaseObjects
    {
        int _Damage = 0;
        int _Delay = 0;

        public int Damage
        {
            get { return _Damage; }                
        }

        public int Delay
        {
            get { return _Delay; }
        }

        public Weapon()
        {
        }

        public Weapon(
            int id,
            Texture2D texture,
            string name,
            int damage,
            int delay)
        {
            _Id = id;
            _Texture = texture;
            _Name = name;
            
            _Damage = damage;
            _Delay = delay;
        }




    }
}
