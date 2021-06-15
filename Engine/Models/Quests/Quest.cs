using System.Collections.Generic;
using System.Text;

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

        public Quest(int iD, string name, string description, List<ItemQuantity> requirements, List<ItemQuantity> rewardItems, int rewardExperience, int rewardCredits)
        {
            ID = iD;
            Name = name;
            Description = description;
            Requirements = requirements;
            RewardItems = rewardItems;
            RewardExperience = rewardExperience;
            RewardCredits = rewardCredits;
        }

        public string TooltipMessage
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append('\'').Append(Name).Append("':\n").AppendLine(Description);
                stringBuilder.AppendLine("You need:");
                foreach (var item in Requirements)
                    stringBuilder.AppendLine(item.ToString());
                stringBuilder.Append("Reward is ").Append(RewardCredits).Append(" credits and ").Append(RewardExperience).AppendLine(" exp");

                if (RewardItems != null)
                {
                    stringBuilder.AppendLine("Also you will get:");
                    foreach (var item in RewardItems)
                        stringBuilder.AppendLine(item.ToString());
                }
                return stringBuilder.ToString();
            }
        }
    }
}
