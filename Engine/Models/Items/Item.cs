using System;

namespace Engine.Models.Items
{
    public class Item : ICloneable
    {
        public int Id { get; }
        public string Name { get; }
        public int Price { get; }
        public bool IsUnique { get; }

        public Item(int id, string name, int price, bool isUnique = false)
        {
            Id = id;
            Name = name;
            Price = price;
            IsUnique = isUnique;
        }

        public object Clone() => new Item(Id, Name, Price, IsUnique);
    }
}
