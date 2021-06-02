using System;

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
        public int MinDamage { get; }
        public int MaxDamage { get; }

        public Item(ItemCategory category, int id, string name, int price, bool isUnique = false, int minDamage=0, int maxDamage=0)
        {
            Category = category;
            Id = id;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public object Clone() => new Item(Category,Id, Name, Price, IsUnique, MinDamage, MaxDamage);
    }
}
