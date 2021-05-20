using Engine.Models.Quests;
using System.Collections.Generic;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> rewardItems = new List<ItemQuantity>();

            itemsToComplete.Add(new ItemQuantity(3, 5));
            rewardItems.Add(new ItemQuantity(2, 1));

            _quests.Add(new Quest(1, "Sneeze collector", "Poke snails around with your Pen and collect their sneeze ", itemsToComplete, 10, 5));
        }

        internal static Quest GetQuestByID(int id) => _quests.Find(q => q.ID == id);
    }
}
