using System;
using Engine.Actions;
using Newtonsoft.Json;

namespace Engine.Models.Items
{
    public class Item : ICloneable
    {
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon,
            Consumable
        }

        [JsonIgnore]
        public ItemCategory Category { get; }
        public int Id { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public int Price { get; }
        [JsonIgnore]
        public bool IsUnique { get; }
        [JsonIgnore]
        public IAction Action { get; set; }

        public Item(ItemCategory category, int id, string name, int price, bool isUnique = false, IAction action = null)
        {
            Category = category;
            Id = id;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            Action = action;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target) => Action?.Execute(actor, target);

        public object Clone() => new Item(Category, Id, Name, Price, IsUnique, Action);
    }
}
