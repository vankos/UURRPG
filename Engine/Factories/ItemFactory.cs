using Engine.Actions;
using Engine.Models.Items;
using System.Collections.Generic;

namespace Engine.Factories
{
    internal static class ItemFactory
    {
        private readonly static List<Item> _referenceItems = new List<Item>();

        static ItemFactory()
        {
            //Player Weapons
            BuildWeapon(1, "Pen", 5, 1, 3);
            BuildWeapon(2, "Ruler", 30, 2, 3);

            //Ene,y weapons
            BuildWeapon(5, "Snail teeth", 0, 1, 2);

            //Consumable
            BuildHealingItem(6, "Small red syringe", 5, 5);

            //Miscellaneous
            BuildMiscellaneousItem(3, "Snail's sneeze", 5);
            BuildMiscellaneousItem(4, "Slime", 2);
        }

        public static Item CreateItem(int itemId) => _referenceItems.Find(i => i.Id == itemId).Clone() as Item;

        private static void BuildMiscellaneousItem(int id, string name, int price) => _referenceItems.Add(new Item(Item.ItemCategory.Miscellaneous, id, name, price));

        private static void BuildWeapon(int id, string name, int price, int minDamage, int maxDamage)
        {
            Item newItem = (new Item(Item.ItemCategory.Weapon, id, name, price, true));
            newItem.Action = new AttackWithWeapon(newItem, minDamage, maxDamage);
            _referenceItems.Add(newItem);
        }

        private static void BuildHealingItem(int id, string name, int price, int hp)
        {
            Item newItem = (new Item(Item.ItemCategory.Consumable, id, name, price));
            newItem.Action = new Heal(newItem,hp);
            _referenceItems.Add(newItem);
        }

        public static string GetItemNameById(int itemId)=> _referenceItems.Find(i => i.Id == itemId)?.Name ?? "*";
    }
}
