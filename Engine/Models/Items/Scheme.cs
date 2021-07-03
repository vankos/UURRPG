using Engine.Models.Quests;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Models.Items
{
    public class Scheme
    {
        public int ID { get; }
        [JsonIgnore]
        public string Name { get; }

        [JsonIgnore]
        public List<ItemQuantity> RequiredItems { get; } = new List<ItemQuantity>();
        [JsonIgnore]
        public List<ItemQuantity> QutputItems { get; } = new List<ItemQuantity>();

        public Scheme(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public void AddIngredient(int itemID, int quantity)
        {
            if (!RequiredItems.Any(x => x.ItemId == itemID))
                RequiredItems.Add(new ItemQuantity(itemID, quantity));
        }

        public void AddOutputItem(int itemID, int quantity)
        {
            if (!QutputItems.Any(x => x.ItemId == itemID))
                QutputItems.Add(new ItemQuantity(itemID, quantity));
        }

        public string TooltipMessage
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("You need:");
                foreach (var item in RequiredItems)
                {
                    stringBuilder.AppendLine(item.ToString());
                }
                return stringBuilder.ToString();
            }
        }
    }
}
