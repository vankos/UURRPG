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
        public AttackWithWeapon Attack { get; set; }

        public Item(ItemCategory category, int id, string name, int price, bool isUnique = false, AttackWithWeapon action = null)
        {
            Category = category;
            Id = id;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            Attack = action;
        }

        public void PerformAttack(LivingEntity target) => Attack?.Execute(target);

        public object Clone() => new Item(Category, Id, Name, Price, IsUnique, Attack);
    }
}
