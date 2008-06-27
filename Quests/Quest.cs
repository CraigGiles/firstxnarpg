using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;


namespace ActionRPG
{
    /// <summary>
    /// A quest that the party can embark on, with goals and rewards.
    /// </summary>
    public class Quest 
    {
        #region Quest Stage


        /// <summary>
        /// The possible stages of a quest.
        /// </summary>
        public enum QuestStage
        {
            NotStarted,
            InProgress,
            RequirementsMet,
            Completed
        };


        /// <summary>
        /// The current stage of this quest.
        /// </summary>
        public QuestStage Stage
        {
            get { return stage; }
            set { stage = value; }
        }
        QuestStage stage = QuestStage.NotStarted;

        
        #endregion


        #region Description

        /// <summary>
        /// The name of the quest.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string name;


        /// <summary>
        /// A description of the quest.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        string description;


        /// <summary>
        /// A message describing the objective of the quest, 
        /// presented when the player receives the quest.
        /// </summary>
        public string ObjectiveMessage
        {
            get { return objectiveMessage; }
            set { objectiveMessage = value; }
        }
        string objectiveMessage;


        /// <summary>
        /// A message announcing the completion of the quest, 
        /// presented when the player reaches the goals of the quest.
        /// </summary>
        public string CompletionMessage
        {
            get { return completionMessage; }
            set { completionMessage = value; }
        }
        string completionMessage;


        #endregion

        //todo: impliment
        #region Quest Requirements


        /// <summary>
        /// List of gear requirements for finishing the quest
        /// </summary>
        public List<QuestRequirement<Gear>> GearRequirements
        {
            get { return gearRequirements; }
            set { gearRequirements = value; }
        }
        List<QuestRequirement<Gear>> gearRequirements = new List<QuestRequirement<Gear>>();


        /// <summary>
        /// List of items needing to be obtained for
        /// completing the quest
        /// </summary>
        public List<QuestRequirement<Item>> ItemRequirements
        {
            get { return itemRequirements; }
            set { itemRequirements = value; }
        }
        List<QuestRequirement<Item>> itemRequirements = new List<QuestRequirement<Item>>();


        /// <summary>
        /// List of NPCs needing to be slain for 
        /// completing the quest
        /// </summary>
        public List<QuestRequirement<NPC>> NpcRequirements
        {
            get { return npcRequirements; }
            set { npcRequirements = value; }
        }
        List<QuestRequirement<NPC>> npcRequirements = new List<QuestRequirement<NPC>>();


        /// <summary>
        /// Checks to see if all requirements are met
        /// </summary>
        public bool AreRequirementsMet
        {
            get
            {
                bool complete = true;
                foreach (QuestRequirement<Gear> req in gearRequirements)
                {
                    if (req.CompletedCount < gearRequirements.Count)
                        complete = false;
                }

                foreach (QuestRequirement<Item> req in itemRequirements)
                {
                    if (req.CompletedCount < itemRequirements.Count)
                        complete = false;
                }

                foreach (QuestRequirement<NPC> req in npcRequirements)
                {
                    if (req.CompletedCount < npcRequirements.Count)
                        complete = false;
                }


                return complete;
            }
        }


        public void UpdateQuestRequirements(NPC npcSlain)
        {

        }

        public void UpdateQuestRequirements(Item itemObtained)
        {
        }

        public void UpdateQuestRequirements(Gear gearObtained)
        {
        }



        #endregion


        #region Reward


        /// <summary>
        /// The amount of gold given to the party as a reward.
        /// </summary>
        public int GoldReward
        {
            get { return goldReward; }
            set { goldReward = value; }
        }
        int goldReward;


        /// <summary>
        /// The content names of the gear given to the party as a reward.
        /// </summary>
        public List<string> GearRewardContentNames
        {
            get { return gearRewardContentNames; }
            set { gearRewardContentNames = value; }
        }
        List<string> gearRewardContentNames = new List<string>();


        /// <summary>
        /// The gear given to the party as a reward.
        /// </summary>
        public List<Gear> GearRewards
        {
            get { return gearRewards; }
            set { gearRewards = value; }
        }
        List<Gear> gearRewards = new List<Gear>();


        #endregion

    }
}
