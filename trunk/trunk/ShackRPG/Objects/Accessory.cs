using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ShackRPG
{
    public class Accessory : BaseObjects
    {
        string _Effect;

        int _Defense = 0,
            _Strength = 0,
            _Health = 0,
            _Mana = 0,
            _Haste;

        #region Properties

        /// <summary>
        /// Description of the effect the accessory grants
        /// </summary>
        public string Effect
        {
            get { return _Effect; }
        }

        /// <summary>
        /// Amount of Defense added by current accessory
        /// </summary>
        public int Defense
        {
            get { return _Defense; }
        }

        /// <summary>
        /// Amount of Strength added by current accessory
        /// </summary>
        public int Strength
        {
            get { return _Strength; }
        }

        /// <summary>
        /// Amount of Health added by current accessory
        /// </summary>
        public int Health
        {
            get { return _Health; }
        }

        /// <summary>
        /// Amount of Mana added by current accessory
        /// </summary>
        public int Mana
        {
            get { return _Mana; }
        }

        /// <summary>
        /// Haste value added
        /// </summary>
        public int Haste
        {
            get { return _Haste; }
        }

        #endregion
        public Accessory()
        {
        }

        public Accessory(
            int id,
            Texture2D texture,
            string name,
            string effect,
            int defenseAdded,
            int strengthAdded,
            int healthAdded,
            int manaAdded,
            int hasteAdded)
        {
            _Id = id;
            _Texture = texture;
            _Name = name;
            _Effect = effect;

            _Defense = defenseAdded;
            _Strength = strengthAdded;
            _Health = healthAdded;
            _Mana = manaAdded;
            _Haste = hasteAdded;
        }

    }
}
