using System;

namespace Engine.Models.Items
{
    public class Item : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsUnique { get; set; }

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
