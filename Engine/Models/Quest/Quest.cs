using System.Collections.Generic;

namespace Engine.Models.Quest
{
    public class Quest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ItemQuantity> Requirements { get; set; }

        public int RewardExperience { get; set; }
        public int RewardCredits { get; set; }
        public List<ItemQuantity> RewardItems { get; set; }

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
