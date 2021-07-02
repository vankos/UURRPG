using Engine.Models.Items;
using Engine.Models.Quests;
using Engine.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    public class Inventory
    {
        #region Backing variables
        private readonly List<Item> _inventory = new List<Item>();
        private readonly List<GroupedInventoryItem> _groupedItems = new List<GroupedInventoryItem>();
        #endregion

        #region Properties
        public IReadOnlyList<Item> Items => _inventory.AsReadOnly();
        public IReadOnlyList<GroupedInventoryItem> GroupedInventory => _groupedItems.AsReadOnly();
        public IReadOnlyList<Item> Weapons => _inventory.GetItemsThatAre(Item.ItemCategory.Weapon).AsReadOnly();
        public IReadOnlyList<Item> Consumables => _inventory.GetItemsThatAre(Item.ItemCategory.Consumable).AsReadOnly();
        public bool HasConsumable => Consumables.Count > 0;
        #endregion

        public Inventory(IEnumerable<Item> items = null)
        {
            if (items == null) return;

            foreach (Item item in items)
            {
                _inventory.Add(item);
                AddItemToGroupedInventory(item);
            }
        }

        #region Public functions
        public bool HasAllThisItems(IEnumerable<ItemQuantity> items) => items.All(item => Items.Count(i => i.Id == item.ItemId) >= item.Quantity);
        #endregion

        #region Private functions
        private void AddItemToGroupedInventory(Item item)
        {
            if (item.IsUnique)
            {
                _groupedItems.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (!GroupedInventory.Any(gi => gi.Item.Id == item.Id))
                    _groupedItems.Add(new GroupedInventoryItem(item, 0));
                GroupedInventory.First(gi => gi.Item.Id == item.Id).Quantity++;
            }
        }
        #endregion
    }
}
