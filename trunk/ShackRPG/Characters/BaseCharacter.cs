using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TileEngine;
using ShackRPG.GameScreens;
using Microsoft.Xna.Framework.Content;

namespace ShackRPG
{
    public class BaseCharacter
    {
        #region Variables
        /// <summary>
        /// Characters name
        /// </summary>
        string _Name;

        /// <summary>
        /// Characters stats
        /// </summary>
        int _Health,            //Current Health
            _HealthMax,         //Max Health for Level
            _Mana,              //current mana
            _ManaMax,           //max mana for level
            _StrengthBase,      //STR not including buffs or +STR items
            _ArmorBase;         //Armor not including buffs or +Armor items
                    

        /// <summary>
        /// Characters walking speed on the map
        /// </summary>
        float _TravelSpeed;

        /// <summary>
        /// Radius of the sprite
        /// </summary>
        float _Radius;

        /// <summary>
        /// Current sprite location
        /// </summary>
        Rectangle _SpriteBox;

        /// <summary>
        /// Sprites texture sheet
        /// </summary>
        Texture2D _SpriteSheet;

        /// <summary>
        /// Sprites current and previous locations
        /// </summary>
        Vector2
            _CurrentLocation,
            _PreviousLocation;

        /// <summary>
        /// Limits the amount of time a character can act
        /// per round of battle
        /// </summary>
        float
            _ActionTimer,
            _ActionTimerReset;

        #endregion

        #region Properties

        /// <summary>
        /// Characters name
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Sprites Radius
        /// </summary>
        public float Radius
        {
            get { return _Radius; }
            set { _Radius = (float)Math.Max(value, 0f); }
        }

        /// <summary>
        /// The location on screen where the sprite is to be
        /// drawn
        /// </summary>
        public Rectangle SpriteBox
        {
            get { return _SpriteBox; }            
        }

        /// <summary>
        /// Sprites width
        /// </summary>
        public int SpriteWidth
        {
            get { return _SpriteBox.Width; }
            set { _SpriteBox.Width = (int)Math.Max(value, 1); }
        }

        /// <summary>
        /// Sprites Height
        /// </summary>
        public int SpriteHeight
        {
            get { return _SpriteBox.Height; }
            set { _SpriteBox.Height = (int)Math.Max(value, 1); }
        }

        /// <summary>
        /// Sprites current location
        /// </summary>
        public Vector2 CurrentLocation
        {
            get { return _CurrentLocation; }
            set 
            { 
                _CurrentLocation = value;
                _SpriteBox.X = (int)_CurrentLocation.X;
                _SpriteBox.Y = (int)_CurrentLocation.Y;
            }
        }

        /// <summary>
        /// Sprites Previous Location
        /// </summary>
        public Vector2 PreviousLocation
        {
            get { return _PreviousLocation; }
            set { _PreviousLocation = value; }
        }

        /// <summary>
        /// Sprites Travel speed
        /// </summary>
        public float TravelSpeed
        {
            get { return _TravelSpeed; }
            set { _TravelSpeed = (float)MathHelper.Clamp(value, 0, 3.2f); }
        }

        /// <summary>
        /// Sprites texture sheet
        /// </summary>
        public Texture2D SpriteSheet
        {
            get { return _SpriteSheet; }
            set { _SpriteSheet = value; }
        }

        /// <summary>
        /// Current Action Timer Value
        /// </summary>
        public float ActionTimer
        {
            get { return _ActionTimer; }
            set { _ActionTimer = (float)MathHelper.Clamp(value, 0, ActionTimerReset); }
        }

        /// <summary>
        /// Characters Action Timer Reset value.
        /// </summary>
        public virtual float ActionTimerReset
        {
            get { return _ActionTimerReset; }
            set { _ActionTimerReset = (float)Math.Max(value, 0f); }
        }

        /// <summary>
        /// Returns the % value of the current character Action Timer
        /// </summary>
        public int ActionTimerPercentValue
        {
            get
            {
                int text = 0;
                text = Math.Abs((int)(((ActionTimer - ActionTimerReset) / ActionTimerReset) * 100));
                if (text >= 100)
                    text = 100;

                return text;
            }
        }

        /// <summary>
        /// Current Health
        /// </summary>
        public int Health
        {
            get { return _Health; }
            set { _Health = (int)MathHelper.Clamp(value, 0, MaxHealth); }
        }

        /// <summary>
        /// Max available Health
        /// </summary>
        public int MaxHealth
        {
            get { return _HealthMax; }
            set { _HealthMax = (int)Math.Max(value, 0); }
        }

        /// <summary>
        /// Returns the current characters health value
        /// in percent form
        /// </summary>
        public float HealthPercentValue
        {
            get
            {
                return ((float)_Health / (float)_HealthMax) * (int)100;
            }
        }

        /// <summary>
        /// Current mana
        /// </summary>
        public int Mana
        {
            get { return _Mana; }
            set { _Mana = (int)MathHelper.Clamp(value, 0, MaxMana); }
        }

        /// <summary>
        /// Max available mana
        /// </summary>
        public int MaxMana
        {
            get { return _ManaMax; }
            set { _ManaMax = (int)Math.Max(value, 0); }
        }

        /// <summary>
        /// Returns the current characters health value
        /// in percent form
        /// </summary>
        public float ManaPercentValue
        {
            get
            {
                return ((float)_Mana / (float)_ManaMax) * (int)100;
            }
        }

        /// <summary>
        /// Characters base armor value
        /// </summary>
        public int BaseArmor
        {
            get { return _ArmorBase; }
            set { _ArmorBase = (int)Math.Max(value, 1); }
        }

        /// <summary>
        /// Characters Base Strength
        /// </summary>
        public int BaseStrength
        {
            get { return _StrengthBase; }
            set { _StrengthBase = (int)Math.Max(value, 1); }
        }

        #endregion

        #region Initialize
        public void Initialize()
        {
            _Health = _HealthMax;
            _Mana = _ManaMax;
        }
        #endregion
    }
}
