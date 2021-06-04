using Engine.Models.Items;
using System.Collections.Generic;

namespace Engine.Factories
{
    internal static class ItemFactory
    {
        private readonly static List<Item> _referenceItems = new List<Item>();

        static ItemFactory()
        {
            BuildWeapon(1, "Pen", 5, 1, 3);
            BuildWeapon(2, "Ruler", 30, 2, 3);
            BuildMiscellaneousItem(3, "Snail's sneeze", 5);
            BuildMiscellaneousItem(4, "Slime", 2);
        }

        public static Item CreateItem(int itemId) => _referenceItems.Find(i => i.Id == itemId).Clone() as Item;

        private static void BuildMiscellaneousItem(int id, string name, int price) => _referenceItems.Add(new Item(Item.ItemCategory.Miscellaneous, id, name, price));

        private static void BuildWeapon(int id, string name, int price, int minDamage, int maxDamage)
        {
            Item newItem = (new Item(Item.ItemCategory.Weapon, id, name, price, true));
            newItem.Action = new Actions.AttackWithWeapon(newItem, minDamage, maxDamage);
            _referenceItems.Add(newItem);
        }

        public static string GetItemNameById(int itemId)
        {
            Item standartItem = _referenceItems.Find(i => i.Id == itemId);
            return standartItem?.Name;
        }
    }
}
