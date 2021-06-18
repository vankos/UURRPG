using Engine.Factories;
using Engine.Models.Items;
using Engine.Models.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Services
{
    public static class InventoryService
    {
        public static Inventory AddItems(this Inventory inventory, List<Item> items) => new Inventory(inventory.Items.Concat(items));
        public static Inventory AddItems(this Inventory inventory, IEnumerable<ItemQuantity> items)
        {
            List<Item> itemsToAdd = new List<Item>();
            foreach (ItemQuantity item in items)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    itemsToAdd.Add(ItemFactory.CreateItem(item.ItemId));
                }
            }
            return inventory.AddItems(itemsToAdd);
        }
        public static Inventory AddItem(this Inventory inventory, Item item) => inventory.AddItems(new List<Item> { item });

        public static Inventory AddItemFromFactory(this Inventory inventory, int itemId) => inventory.AddItem(ItemFactory.CreateItem(itemId));

        public static Inventory RemoveItems(this Inventory inventory, List<Item> items) => new Inventory(inventory.Items.Except(items));

        public static Inventory RemoveItems(this Inventory inventory, IEnumerable<ItemQuantity> items)
        {
            foreach (ItemQuantity item in items)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    inventory = inventory.RemoveItem(inventory.Items.First(it => it.Id == item.ItemId));
                }
            }
            return inventory;
        }

        public static Inventory RemoveItem(this Inventory inventory, Item item) => inventory.RemoveItems(new List<Item> { item });
    }
}
