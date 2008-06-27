using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Helper;

namespace ActionRPG
{
    public class NPC : Character, IMindset
    {
        /// <summary>
        /// Will the NPC attack if you if in range
        /// </summary>
        public bool Hostile
        {
            get { return hostile; }
            set { hostile = value; }
        }
        bool hostile;

        /// <summary>
        /// Aggro radius of character
        /// </summary>
        public float AggroRadius
        {
            get
            {
                if (Hostile)
                    return aggroRadius * CollisionRadius;
                else
                    return 0f;
            }
            set { aggroRadius = value; }
        }
        float aggroRadius = 0.0f;

        AI ai;

        #region Data

        /// <summary>
        /// Default spawn timer of the NPC
        /// </summary>
        public float SpawnTimer
        {
            get { return spawnTimer; }
            set { spawnTimer = value; }
        }
        float spawnTimer;

        /// <summary>
        /// Returns true if NPC is already in the dead state
        /// </summary>
        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }
        bool dead;

        #endregion


        #region Constructor(s) / Load

        public NPC()
        {
        }

        public override void Initialize(string asset)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content\Characters\" + asset + ".xml");

            foreach (XmlNode rootElement in doc.ChildNodes)
            {
                foreach (XmlNode node in rootElement.ChildNodes)
                {

                    if (node.Name == "Hostile")
                        Hostile = bool.Parse(node.InnerText);

                    if (node.Name == "Respawn")
                        spawnTimer = float.Parse(node.InnerText);

                    if (node.Name == "Loot")
                        LoadLootTable(node);

                    if (node.Name == "Aggro")
                        aggroRadius = float.Parse(node.InnerText);
                }
            }

            base.Initialize(asset);

            ai = new AI(this);
            SpriteManager.PlayAnimation("Standing", Direction.Down);
        }
        
        private void LoadLootTable(XmlNode n)
        {

                switch (n.Attributes["Type"].Value)
                {
                    case "Weapon":
                        LootTable.Add(new Weapon(n.Attributes["Asset"].Value, int.Parse(n.Attributes["Roll"].Value)));
                        break;

                    case "Armor":
                        LootTable.Add(new Armor(n.Attributes["Asset"].Value, int.Parse(n.Attributes["Roll"].Value)));
                        break;

                    case "Accessory":
                        LootTable.Add(new Accessory(n.Attributes["Asset"].Value, int.Parse(n.Attributes["Roll"].Value)));
                        break;
                }

            }

        #endregion


        #region Update


        /// <summary>
        /// Updates NPC actions based on current character state
        /// </summary>
        public void Update(Player player)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Alive:
                    UpdateAlive(player);
                    break;

                case CharacterState.Dying:
                    UpdateDying();
                    break;

                case CharacterState.Dead:
                    UpdateDead();
                    break;
            }


            base.Update();
        }


        #region Update Alive

        /// <summary>
        /// Updates all NPC actions and checks for death
        /// </summary>
        private void UpdateAlive(Player player)
        {
            //update AI
            ai.Update(player);
            
            //update npc actions based on mindset
            switch (ai.CurrentMindset)
            {
                case AI.Mindset.Standing:
                    Standing(player);
                    break;

                case AI.Mindset.Guarding:
                    Guarding(player);
                    break;

                case AI.Mindset.Roaming:
                    Roaming(player);
                    break;

                case AI.Mindset.Patroling:
                    Patroling(player);
                    break;

                case AI.Mindset.Aggressive:
                    Aggressive(player);
                    break;

                case AI.Mindset.Fleeing:
                    Fleeing(player);
                    break;
            }

            CheckDeathConditions();
        }


        private void UpdateTraveling()
        {

        }


        #endregion


        #region Update Dying / Dead

        /// <summary>
        /// Checks to see if death animation is complete, and removes
        /// NPC from world map
        /// </summary>
        private void UpdateDying()
        {
            if (SpriteManager.IsAnimationComplete)
            {
                CurrentCharacterState = CharacterState.Dead;
                CurrentPosition = new Vector2(-100, -100);
            }
        }


        /// <summary>
        /// Checks to see if NPC can respawn
        /// </summary>
        private void UpdateDead()
        {
            if (Timers.Respawn <= 0)
            {
                RespawnNpc();
            }
        }

        #endregion


        #endregion
        

        #region Death and Dying


        /// <summary>
        /// Checks various death conditions 
        /// </summary>
        private void CheckDeathConditions()
        {

            if (Health <= 0)
                HasDied();
        
        }


        /// <summary>
        /// Executes all death actions when NPC dies
        /// </summary>
        public override void HasDied()
        {
            SpriteManager.PlayAnimation("Dying");
            CurrentCharacterState = CharacterState.Dying;

            Timers.Respawn = spawnTimer;

            SpawnLootFromTable();

            base.HasDied();
        }


        /// <summary>
        /// Grabs a random item from NPCs loot table and spawns it in the world
        /// </summary>
        private void SpawnLootFromTable()
        {
            //Random number to determine item drop
            int random = RNG.GetRandomInt(100);

            //drop any 100% items
            foreach (Item i in LootTable)
                if (i.Percent == 0)
                    Globals.TileEngine.Map.SpawnTreasure(i, CurrentPosition - RNG.GetRandomVector2(-30, 30));

            //if random was high enough to drop item, drop it
            foreach (Item i in LootTable)
                if (random > i.Percent && i.Percent != 0)
                {
                    Globals.TileEngine.Map.SpawnTreasure(i, CurrentPosition - RNG.GetRandomVector2(-30, 30));
                    break;
                }


        }
        

        /// <summary>
        /// Respawns the NPC when its Spawn Timer has expired
        /// </summary>
        private void RespawnNpc()
        {
            CurrentCharacterState = CharacterState.Alive;
            CurrentPosition = SpawnLocationVector;
            dead = false;
            ResetStats();
        }


        #endregion


        #region Conversations

        //public Dialog StartConversation()
        //{
            
        //}

        #endregion


        #region IMindset Members

        public void Standing(Player player)
        {
            CheckAggroRange(player);
        }

        /*may not need 'traveling' mindset*/
        public void Traveling(Player player)
        {
            //CurrentAction = Actions.Traveling;
            //continue moving to new location
            //if new location is met, check previous mindset for new condition
                //ie; guarding / standing = playanimation("standing", Direction.Down)
                //roaming; generate new vector2 and start walking there
                //patrolling, increase point by 1 and walk to new point
        }

        public void Guarding(Player player)
        {
            //check any Hostile closing in on current location
                //if enemy is within attack range, attack enemy, chasing if needed
                //if pulled X distance away from Guarding location, walk back to spawn
        }

        public void Roaming(Player player)
        {
        }

        public void Patroling(Player player)
        {
        }

        public void Aggressive(Player player)
        {

            //if can attack, and player is within range, 
                //determine combo and attack
        }

        public void Fleeing(Player player)
        {
        }



        #endregion


        #region Movement

        public void WalkTowardsLocation(Vector2 location)
        {

        }

        #endregion


        #region Aggro


        private void CheckAggroRange(Player player)
        {
            if (IsPlayerInAgroRange(player))
            {
                ai.ChangeMindset(AI.Mindset.Aggressive);
                ai.BattleTarget = player;
            }
        }

        private bool IsPlayerInAgroRange(Player player)
        {
            Vector2 d = player.FootLocation - this.FootLocation;

            return (d.Length() < AggroRadius);
        }


        #endregion

    }
}
