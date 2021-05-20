using Engine.Models.Items;
using System.Collections.Generic;

namespace Engine.Factories
{
    internal static class ItemFactory
    {
        private readonly static List<Item> _referenceItems = new List<Item>()
        {
            new Weapon(1, "Pen", 5, 1, 3),
            new Weapon(2, "Ruler", 30,2, 3),
            new Item(3, "Snail's sneeze", 5),
            new Item(4, "Slime", 2)
        };

        public static Item CreateItem(int itemId)
        {
            Item standartItem = _referenceItems.Find(i => i.Id == itemId);
            if (standartItem is Weapon)
                return (standartItem as Weapon)?.Clone() as Weapon;
            return standartItem.Clone() as Item;
        }

        public static string GetItemNameById(int itemId)
        {
            Item standartItem = _referenceItems.Find(i => i.Id == itemId);
            return standartItem?.Name;
        }
    }
}
