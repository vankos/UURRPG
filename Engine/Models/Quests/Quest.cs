using System.Collections.Generic;

namespace Engine.Models.Quests
{
    public class Quest
    {
        public int ID { get; }
        public string Name { get; }
        public string Description { get; }

        public List<ItemQuantity> Requirements { get; }

        public int RewardExperience { get; }
        public int RewardCredits { get; }
        public List<ItemQuantity> RewardItems { get; }

        public Quest(int iD, string name, string description, List<ItemQuantity> requirements, int rewardExperience, int rewardCredits)
        {
            ID = iD;
            Name = name;
            Description = description;
            Requirements = requirements;
            RewardExperience = rewardExperience;
            RewardCredits = rewardCredits;
        }
    }
}
