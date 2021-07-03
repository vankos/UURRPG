using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Quests
{
    public class Quest
    {
        public int ID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public List<ItemQuantity> Requirements { get; }
        [JsonIgnore]
        public int RewardExperience { get; }
        [JsonIgnore]
        public int RewardCredits { get; }
        [JsonIgnore]
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

        [JsonIgnore]
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
