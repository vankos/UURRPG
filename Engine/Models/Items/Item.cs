using System;
using Engine.Actions;

namespace Engine.Models.Items
{
    public class Item : ICloneable
    {
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon
        }

        public ItemCategory Category { get; }
        public int Id { get; }
        public string Name { get; }
        public int Price { get; }
        public bool IsUnique { get; }
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

        public void PerformAttack(LivingEntity actor, LivingEntity target) => Action?.Execute(actor, target);

        public object Clone() => new Item(Category, Id, Name, Price, IsUnique, Action);
    }
}
