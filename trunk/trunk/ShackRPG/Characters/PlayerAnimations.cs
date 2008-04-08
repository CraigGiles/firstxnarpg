using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ShackRPG
{
    public class PlayerAnimations
    {
        Animation _CurrentAnimation, _NextAnimation;
        float _AnimationTimer;
        bool _IsTimed = false;

        /* The following lists houses all of the source rectangles used to find
         * the correct frame of animation from the SpriteSheet */
        #region Lists
        List<Rectangle> DownStandingSourceRectangles = new List<Rectangle>();
        List<Rectangle> DownWalkingSourceRectangles = new List<Rectangle>();
        List<Rectangle> DownBattleIdleSourceRectangles = new List<Rectangle>();
        List<Rectangle> DownAttackSourceRectangles = new List<Rectangle>();
        List<Rectangle> DownDefendSourceRectangles = new List<Rectangle>();

        List<Rectangle> LeftStandingSourceRectangles = new List<Rectangle>();
        List<Rectangle> LeftWalkingSourceRectangles = new List<Rectangle>();
        List<Rectangle> LeftBattleIdleSourceRectangles = new List<Rectangle>();
        List<Rectangle> LeftAttackSourceRectangles = new List<Rectangle>();
        List<Rectangle> LeftDefendSourceRectangles = new List<Rectangle>();

        List<Rectangle> UpStandingSourceRectangles = new List<Rectangle>();
        List<Rectangle> UpWalkingSourceRectangles = new List<Rectangle>();
        List<Rectangle> UpBattleIdleSourceRectangles = new List<Rectangle>();
        List<Rectangle> UpAttackSourceRectangles = new List<Rectangle>();
        List<Rectangle> UpDefendSourceRectangles = new List<Rectangle>();

        List<Rectangle> RightStandingSourceRectangles = new List<Rectangle>();
        List<Rectangle> RightWalkingSourceRectangles = new List<Rectangle>();
        List<Rectangle> RightBattleIdleSourceRectangles = new List<Rectangle>();
        List<Rectangle> RightAttackSourceRectangles = new List<Rectangle>();
        List<Rectangle> RightDefendSourceRectangles = new List<Rectangle>();

        List<Rectangle> VictorySourceRectangles = new List<Rectangle>();
        #endregion 

        /* The following is a list of all of the available animations a player can
         * make. */
        #region Animations
        Animation DownStanding;
        Animation LeftStanding;
        Animation UpStanding;
        Animation RightStanding;

        Animation DownWalking;
        Animation LeftWalking;
        Animation UpWalking;
        Animation RightWalking;

        Animation DownBattleIdle;
        Animation LeftBattleIdle;
        Animation UpBattleIdle;
        Animation RightBattleIdle;

        Animation DownAttack;
        Animation LeftAttack;
        Animation UpAttack;
        Animation RightAttack;

        Animation DownDefend;
        Animation LeftDefend;
        Animation UpDefend;
        Animation RightDefend;

        Animation Victory;
        Animation CharacterDead;

        #endregion

        /* The following is all of the source rectangles and their values used to
         * find the correct frame of animation from the SpriteSheet */
        #region Source Rectangles

        #region Misc Animation
        Rectangle
            rVictory1 = new Rectangle(320, 0, 40, 40),
            rVictory2 = new Rectangle(320, 40, 40, 40);

        Rectangle
            rDead = new Rectangle(360, 0, 40, 40);

        #endregion

        #region Facing Down
        Rectangle
            rDownStanding1 = new Rectangle(0, 0, 40, 40),
            rDownStanding2 = new Rectangle(0, 40, 40, 40),
            rDownStanding3 = new Rectangle(0, 80, 40, 40),
            rDownStanding4 = new Rectangle(0, 120, 40, 40),
            rDownStanding5 = new Rectangle(0, 160, 40, 40);

        Rectangle
             rDownWalking1 = new Rectangle(0, 200, 40, 40),
             rDownWalking2 = new Rectangle(0, 240, 40, 40),
             rDownWalking3 = new Rectangle(0, 280, 40, 40),
             rDownWalking4 = new Rectangle(0, 320, 40, 40),
             rDownWalking5 = new Rectangle(0, 360, 40, 40);

        Rectangle
            rDownAttack1 = new Rectangle(160, 40, 40, 40),
            rDownAttack2 = new Rectangle(160, 80, 40, 40),
            rDownAttack3 = new Rectangle(160, 120, 40, 40),
            rDownAttack4 = new Rectangle(160, 160, 40, 40),
            rDownAttack5 = new Rectangle(160, 200, 40, 40);

        Rectangle
            rDownBattleIdle1,
            rDownBattleIdle2,
            rDownBattleIdle3,
            rDownBattleIdle4,
            rDownBattleIdle5;

        Rectangle
            rDownDefend = new Rectangle(160, 0, 40, 40);

        #endregion

        #region Facing Left
        Rectangle
            rLeftStanding1 = new Rectangle(40, 0, 40, 40),
            rLeftStanding2 = new Rectangle(40, 40, 40, 40),
            rLeftStanding3 = new Rectangle(40, 80, 40, 40),
            rLeftStanding4 = new Rectangle(40, 120, 40, 40),
            rLeftStanding5 = new Rectangle(40, 160, 40, 40);

        Rectangle
             rLeftWalking1 = new Rectangle(40, 200, 40, 40),
             rLeftWalking2 = new Rectangle(40, 240, 40, 40),
             rLeftWalking3 = new Rectangle(40, 280, 40, 40),
             rLeftWalking4 = new Rectangle(40, 320, 40, 40),
             rLeftWalking5 = new Rectangle(40, 360, 40, 40);

        Rectangle
            rLeftBattleIdle1,
            rLeftBattleIdle2,
            rLeftBattleIdle3,
            rLeftBattleIdle4,
            rLeftBattleIdle5;

        Rectangle
            rLeftAttack1 = new Rectangle(200, 40, 40, 40),
            rLeftAttack2 = new Rectangle(200, 80, 40, 40),
            rLeftAttack3 = new Rectangle(200, 120, 40, 40),
            rLeftAttack4 = new Rectangle(200, 160, 40, 40),
            rLeftAttack5 = new Rectangle(200, 200, 40, 40);

        Rectangle
            rLeftSpell1 = new Rectangle(200, 240, 40, 40),
            rLeftSpell2 = new Rectangle(200, 280, 40, 40),
            rLeftSpell3 = new Rectangle(200, 320, 40, 40),
            rLeftSpell4 = new Rectangle(200, 360, 40, 40);

        Rectangle
            rLeftDefend = new Rectangle(200, 0, 40, 40);

        #endregion

        #region Facing Up
        Rectangle
            rUpStanding1 = new Rectangle(80, 0, 40, 40),
            rUpStanding2 = new Rectangle(80, 40, 40, 40),
            rUpStanding3 = new Rectangle(80, 80, 40, 40),
            rUpStanding4 = new Rectangle(80, 120, 40, 40),
            rUpStanding5 = new Rectangle(80, 160, 40, 40);

        Rectangle
             rUpWalking1 = new Rectangle(80, 200, 40, 40),
             rUpWalking2 = new Rectangle(80, 240, 40, 40),
             rUpWalking3 = new Rectangle(80, 280, 40, 40),
             rUpWalking4 = new Rectangle(80, 320, 40, 40),
             rUpWalking5 = new Rectangle(80, 360, 40, 40);

        Rectangle
            rUpBattleIdle1,
            rUpBattleIdle2,
            rUpBattleIdle3,
            rUpBattleIdle4,
            rUpBattleIdle5;

        Rectangle
            rUpAttack1 = new Rectangle(240, 40, 40, 40),
            rUpAttack2 = new Rectangle(240, 80, 40, 40),
            rUpAttack3 = new Rectangle(240, 120, 40, 40),
            rUpAttack4 = new Rectangle(240, 160, 40, 40),
            rUpAttack5 = new Rectangle(240, 200, 40, 40);

        Rectangle
            rUpSpell1 = new Rectangle(240, 240, 40, 40),
            rUpSpell2 = new Rectangle(240, 280, 40, 40),
            rUpSpell3 = new Rectangle(240, 320, 40, 40),
            rUpSpell4 = new Rectangle(240, 360, 40, 40);

        Rectangle
            rUpDefend = new Rectangle(240, 0, 40, 40);

        #endregion

        #region Facing Right
        Rectangle
            rRightStanding1 = new Rectangle(120, 0, 40, 40),
            rRightStanding2 = new Rectangle(120, 40, 40, 40),
            rRightStanding3 = new Rectangle(120, 80, 40, 40),
            rRightStanding4 = new Rectangle(120, 120, 40, 40),
            rRightStanding5 = new Rectangle(120, 160, 40, 40);

        Rectangle
             rRightWalking1 = new Rectangle(120, 200, 40, 40),
             rRightWalking2 = new Rectangle(120, 240, 40, 40),
             rRightWalking3 = new Rectangle(120, 280, 40, 40),
             rRightWalking4 = new Rectangle(120, 320, 40, 40),
             rRightWalking5 = new Rectangle(120, 360, 40, 40);

        Rectangle
            rRightAttack1 = new Rectangle(280, 40, 40, 40),
            rRightAttack2 = new Rectangle(280, 80, 40, 40),
            rRightAttack3 = new Rectangle(280, 120, 40, 40),
            rRightAttack4 = new Rectangle(280, 160, 40, 40),
            rRightAttack5 = new Rectangle(280, 200, 40, 40);

        Rectangle
            rRightBattleIdle1,
            rRightBattleIdle2,
            rRightBattleIdle3,
            rRightBattleIdle4,
            rRightBattleIdle5;

        Rectangle
            rRightSpell1 = new Rectangle(280, 240, 40, 40),
            rRightSpell2 = new Rectangle(280, 280, 40, 40),
            rRightSpell3 = new Rectangle(280, 320, 40, 40),
            rRightSpell4 = new Rectangle(280, 360, 40, 40);

        Rectangle
            rRightDefend = new Rectangle(280, 0, 40, 40);

        #endregion

        #endregion

        #region Constructor / Initilize

        /// <summary>
        /// Constructor for the PlayerAnimations helper class
        /// </summary>
        /// <param name="spriteSheet">Players spritesheet</param>
        public PlayerAnimations(Texture2D spriteSheet)
        {
            InitializeLists();
            InitializeAnimations(spriteSheet);

            _CurrentAnimation = DownStanding;
        }

        /// <summary>
        /// Initializes the lists used for managing all of the player 
        /// animations
        /// </summary>
        private void InitializeLists()
        {
            VictorySourceRectangles.Add(rVictory1);
            VictorySourceRectangles.Add(rVictory2);

            DownAnimations();
            LeftAnimations();
            UpAnimations();
            RightAnimations();
        }

        /// <summary>
        /// Creates all of the individual animations a player can perform
        /// </summary>
        /// <param name="spriteSheet">Players SpriteSheet</param>
        private void InitializeAnimations(Texture2D spriteSheet)
        {
            DownStanding = new Animation(spriteSheet, DownStandingSourceRectangles, 1f);
            LeftStanding = new Animation(spriteSheet, LeftStandingSourceRectangles, 1f);
            UpStanding = new Animation(spriteSheet, UpStandingSourceRectangles, 1f);
            RightStanding = new Animation(spriteSheet, RightStandingSourceRectangles, 1f);

            DownWalking = new Animation(spriteSheet, DownWalkingSourceRectangles, .13f);
            LeftWalking = new Animation(spriteSheet, LeftWalkingSourceRectangles, .13f);
            UpWalking = new Animation(spriteSheet, UpWalkingSourceRectangles, .13f);
            RightWalking = new Animation(spriteSheet, RightWalkingSourceRectangles, .13f);

            DownBattleIdle = new Animation(spriteSheet, DownBattleIdleSourceRectangles, .13f);
            LeftBattleIdle = new Animation(spriteSheet, LeftBattleIdleSourceRectangles, .13f);
            UpBattleIdle = new Animation(spriteSheet, UpBattleIdleSourceRectangles, .13f);
            RightBattleIdle = new Animation(spriteSheet, RightBattleIdleSourceRectangles, .13f);

            DownAttack = new Animation(spriteSheet, DownAttackSourceRectangles, .13f);
            LeftAttack = new Animation(spriteSheet, LeftAttackSourceRectangles, .13f);
            UpAttack = new Animation(spriteSheet, UpAttackSourceRectangles, .13f);
            RightAttack = new Animation(spriteSheet, RightAttackSourceRectangles, .13f);

            DownDefend = new Animation(spriteSheet, DownDefendSourceRectangles, .13f);
            LeftDefend = new Animation(spriteSheet, LeftDefendSourceRectangles, .13f);
            UpDefend = new Animation(spriteSheet, UpDefendSourceRectangles, .13f);
            RightDefend = new Animation(spriteSheet, RightDefendSourceRectangles, .13f);

            Victory = new Animation(spriteSheet, VictorySourceRectangles , .75f);
        }

        #region Add Rectangles To List
        private void DownAnimations()
        {
            DownStandingSourceRectangles.Add(rDownStanding1);
            DownStandingSourceRectangles.Add(rDownStanding2);
            DownStandingSourceRectangles.Add(rDownStanding3);
            DownStandingSourceRectangles.Add(rDownStanding4);
            DownStandingSourceRectangles.Add(rDownStanding5);

            DownWalkingSourceRectangles.Add(rDownWalking1);
            DownWalkingSourceRectangles.Add(rDownWalking2);
            DownWalkingSourceRectangles.Add(rDownWalking3);
            DownWalkingSourceRectangles.Add(rDownWalking4);
            DownWalkingSourceRectangles.Add(rDownWalking5);

            DownBattleIdleSourceRectangles.Add(rDownBattleIdle1);
            DownBattleIdleSourceRectangles.Add(rDownBattleIdle2);
            DownBattleIdleSourceRectangles.Add(rDownBattleIdle3);
            DownBattleIdleSourceRectangles.Add(rDownBattleIdle4);
            DownBattleIdleSourceRectangles.Add(rDownBattleIdle5);

            DownAttackSourceRectangles.Add(rDownAttack1);
            DownAttackSourceRectangles.Add(rDownAttack2);
            DownAttackSourceRectangles.Add(rDownAttack3);
            DownAttackSourceRectangles.Add(rDownAttack4);
            DownAttackSourceRectangles.Add(rDownAttack5);

            DownDefendSourceRectangles.Add(rDownDefend);
        }

        private void LeftAnimations()
        {
            LeftStandingSourceRectangles.Add(rLeftStanding1);
            LeftStandingSourceRectangles.Add(rLeftStanding2);
            LeftStandingSourceRectangles.Add(rLeftStanding3);
            LeftStandingSourceRectangles.Add(rLeftStanding4);
            LeftStandingSourceRectangles.Add(rLeftStanding5);

            LeftWalkingSourceRectangles.Add(rLeftWalking1);
            LeftWalkingSourceRectangles.Add(rLeftWalking2);
            LeftWalkingSourceRectangles.Add(rLeftWalking3);
            LeftWalkingSourceRectangles.Add(rLeftWalking4);
            LeftWalkingSourceRectangles.Add(rLeftWalking5);

            LeftBattleIdleSourceRectangles.Add(rLeftBattleIdle1);
            LeftBattleIdleSourceRectangles.Add(rLeftBattleIdle2);
            LeftBattleIdleSourceRectangles.Add(rLeftBattleIdle3);
            LeftBattleIdleSourceRectangles.Add(rLeftBattleIdle4);
            LeftBattleIdleSourceRectangles.Add(rLeftBattleIdle5);

            LeftAttackSourceRectangles.Add(rLeftAttack1);
            LeftAttackSourceRectangles.Add(rLeftAttack2);
            LeftAttackSourceRectangles.Add(rLeftAttack3);
            LeftAttackSourceRectangles.Add(rLeftAttack4);
            LeftAttackSourceRectangles.Add(rLeftAttack5);

            LeftDefendSourceRectangles.Add(rLeftDefend);
        }

        private void UpAnimations()
        {
            UpStandingSourceRectangles.Add(rUpStanding1);
            UpStandingSourceRectangles.Add(rUpStanding2);
            UpStandingSourceRectangles.Add(rUpStanding3);
            UpStandingSourceRectangles.Add(rUpStanding4);
            UpStandingSourceRectangles.Add(rUpStanding5);

            UpWalkingSourceRectangles.Add(rUpWalking1);
            UpWalkingSourceRectangles.Add(rUpWalking2);
            UpWalkingSourceRectangles.Add(rUpWalking3);
            UpWalkingSourceRectangles.Add(rUpWalking4);
            UpWalkingSourceRectangles.Add(rUpWalking5);

            UpBattleIdleSourceRectangles.Add(rUpBattleIdle1);
            UpBattleIdleSourceRectangles.Add(rUpBattleIdle2);
            UpBattleIdleSourceRectangles.Add(rUpBattleIdle3);
            UpBattleIdleSourceRectangles.Add(rUpBattleIdle4);
            UpBattleIdleSourceRectangles.Add(rUpBattleIdle5);

            UpAttackSourceRectangles.Add(rUpAttack1);
            UpAttackSourceRectangles.Add(rUpAttack2);
            UpAttackSourceRectangles.Add(rUpAttack3);
            UpAttackSourceRectangles.Add(rUpAttack4);
            UpAttackSourceRectangles.Add(rUpAttack5);

            UpDefendSourceRectangles.Add(rUpDefend);
        }

        private void RightAnimations()
        {
            RightStandingSourceRectangles.Add(rRightStanding1);
            RightStandingSourceRectangles.Add(rRightStanding2);
            RightStandingSourceRectangles.Add(rRightStanding3);
            RightStandingSourceRectangles.Add(rRightStanding4);
            RightStandingSourceRectangles.Add(rRightStanding5);

            RightWalkingSourceRectangles.Add(rRightWalking1);
            RightWalkingSourceRectangles.Add(rRightWalking2);
            RightWalkingSourceRectangles.Add(rRightWalking3);
            RightWalkingSourceRectangles.Add(rRightWalking4);
            RightWalkingSourceRectangles.Add(rRightWalking5);

            RightBattleIdleSourceRectangles.Add(rRightBattleIdle1);
            RightBattleIdleSourceRectangles.Add(rRightBattleIdle2);
            RightBattleIdleSourceRectangles.Add(rRightBattleIdle3);
            RightBattleIdleSourceRectangles.Add(rRightBattleIdle4);
            RightBattleIdleSourceRectangles.Add(rRightBattleIdle5);

            RightAttackSourceRectangles.Add(rRightAttack1);
            RightAttackSourceRectangles.Add(rRightAttack2);
            RightAttackSourceRectangles.Add(rRightAttack3);
            RightAttackSourceRectangles.Add(rRightAttack4);
            RightAttackSourceRectangles.Add(rRightAttack5);

            RightDefendSourceRectangles.Add(rRightDefend);
        }
        #endregion 

        #endregion

        /// <summary>
        /// Updates the current animation
        /// </summary>
        /// <param name="gameTime">Game Timer</param>
        public void Update(GameTime gameTime)
        {
            if (_IsTimed)
            {
                _AnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_AnimationTimer <= 0)
                {
                    _CurrentAnimation = _NextAnimation;
                }
            }

            _CurrentAnimation.UpdateAnimation(gameTime);
        }

        /// <summary>
        /// Starts the players standing animation
        /// </summary>
        /// <param name="direction">Direction to face</param>
        public void StartStandingAnimation(string direction)
        {
            _IsTimed = false;
            switch (direction)
            {
                case "down":
                    _CurrentAnimation = DownStanding;
                    break;

                case "left":
                    _CurrentAnimation = LeftStanding;
                    break;

                case "up":
                    _CurrentAnimation = UpStanding;
                    break;

                case "right":
                    _CurrentAnimation = RightStanding;
                    break;
            }
        }

        /// <summary>
        /// Starts the players Walking animation
        /// </summary>
        /// <param name="direction">Direction to face</param>
        public void StartWalkingAnimation(string direction)
        {
            _IsTimed = false;
            switch (direction)
            {
                case "down":
                    _CurrentAnimation = DownWalking;
                    break;

                case "left":
                    _CurrentAnimation = LeftWalking;
                    break;

                case "up":
                    _CurrentAnimation = UpWalking;
                    break;

                case "right":
                    _CurrentAnimation = RightWalking;
                    break;
            }
        }

        /// <summary>
        /// Starts the players idle animation while in battle
        /// </summary>
        /// <param name="direction">Direction to face</param>
        public void StartBattleIdleAnimation(string direction)
        {
            _IsTimed = false;
            switch (direction)
            {
                case "down":
                    _CurrentAnimation = DownBattleIdle;
                    break;

                case "left":
                    _CurrentAnimation = LeftBattleIdle;
                    break;

                case "up":
                    _CurrentAnimation = UpBattleIdle;
                    break;

                case "right":
                    _CurrentAnimation = RightBattleIdle;
                    break;
            }
        }

        /// <summary>
        /// Starts the players attack animation while in battle
        /// </summary>
        /// <param name="direction">Direction to face</param>
        public void StartAttackAnimation(string direction)
        {
            _AnimationTimer = .65f;
            _IsTimed = true;
            switch (direction)
            {
                case "down":
                    _CurrentAnimation = DownAttack;
                    _NextAnimation = DownStanding;
                    break;

                case "left":
                    _CurrentAnimation = LeftAttack;
                    _NextAnimation = LeftStanding;
                    break;

                case "up":
                    _CurrentAnimation = UpAttack;
                    _NextAnimation = UpStanding;
                    break;

                case "right":
                    _CurrentAnimation = RightAttack;
                    _NextAnimation = RightStanding;
                    break;
            }                      
        }

        /// <summary>
        /// Starts the players defend animation while in battle
        /// </summary>
        /// <param name="direction">Direction to face</param>
        public void StartDefendAnimation(string direction)
        {
            _IsTimed = false;
            switch (direction)
            {
                case "down":
                    _CurrentAnimation = DownDefend;
                    break;

                case "left":
                    _CurrentAnimation = LeftDefend;
                    break;

                case "up":
                    _CurrentAnimation = UpDefend;
                    break;

                case "right":
                    _CurrentAnimation = RightDefend;
                    break;
            }
        }

        /// <summary>
        /// Starts the players victory animation
        /// </summary>
        public void StartVictoryAnimation()
        {
            _IsTimed = false;
            _CurrentAnimation = Victory;
        }

        /// <summary>
        /// Changes the character sprite to "dead"
        /// </summary>
        public void DeadCharacter()
        {
            _IsTimed = false;
            _CurrentAnimation = CharacterDead;
        }

        /// <summary>
        /// Draws the current animation to screen
        /// </summary>
        /// <param name="batch">Sprite Batch</param>
        /// <param name="CurrentLocation">Current Location to draw sprite</param>
        /// <param name="SpriteWidth">Desired Sprite Width</param>
        /// <param name="SpriteHeight">Desired Sprite Height</param>
        internal void Draw(SpriteBatch batch, Vector2 CurrentLocation, int SpriteWidth, int SpriteHeight)
        {
            _CurrentAnimation.Draw(batch, CurrentLocation, SpriteWidth, SpriteHeight);
        }
    }
}
