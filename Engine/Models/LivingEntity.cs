using Engine.Models.Items;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public abstract class LivingEntity:BaseNotificationClass
    {
        private string _name;
        private int _health;
        private int _maxHealth;
        private int _credits;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                OnPropertyChanged(nameof(Health));
            }
        }

        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                OnPropertyChanged(nameof(MaxHealth));
            }
        }

        public int Credits
        {
            get { return _credits; }
            set
            {
                _credits = value;
                OnPropertyChanged(nameof(Credits));
            }
        }

        public ObservableCollection<Item> Inventory { get; set; }

        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; set; }

#pragma warning disable S2365 // Properties should not make collection or array copies
        public List<Item> Weapons => Inventory.Where(i => i is Weapon).ToList();
#pragma warning restore S2365 // Properties should not make collection or array copies

        protected LivingEntity()
        {
            Inventory = new ObservableCollection<Item>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
        }

        public void AddItemToInventory(Item item)
        {
            Inventory.Add(item);

            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (!GroupedInventory.Any(gi => gi.Item.Id == item.Id))
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                GroupedInventory.First(gi => gi.Item.Id == item.Id).Quantity++;
            }

            OnPropertyChanged(nameof(Weapons));
        }

        public void RemoveItemFromInventory(Item item)
        {
            Inventory.Remove(item);

            GroupedInventoryItem groupedInventoryItemToRemove = GroupedInventory.FirstOrDefault(gi => gi.Item.Id == item.Id);
            if (groupedInventoryItemToRemove != null)
            {
                if (groupedInventoryItemToRemove.Quantity == 1)
                    GroupedInventory.Remove(groupedInventoryItemToRemove);
                else
                    groupedInventoryItemToRemove.Quantity--;
            }

            OnPropertyChanged(nameof(Weapons));
        }
    }
}
