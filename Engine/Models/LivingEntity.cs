using Engine.Models.Items;
using System;
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
            private set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Health
        {
            get { return _health; }
            private set
            {
                _health = value;
                OnPropertyChanged(nameof(Health));
            }
        }

        public int MaxHealth
        {
            get { return _maxHealth; }
            private set
            {
                _maxHealth = value;
                OnPropertyChanged(nameof(MaxHealth));
            }
        }

        public int Credits
        {
            get { return _credits; }
            private set
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

        public bool IsDead => Health <= 0;

        public event EventHandler OnKilled;

        protected LivingEntity(string name, int maxHealth, int health, int credits)
        {
            Inventory = new ObservableCollection<Item>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();

            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            Credits = credits;
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

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (IsDead)
            {
                Health = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int hp)
        {
            Health += hp;
            if (Health > MaxHealth)
                Health = MaxHealth;
        }

        public void FullHeal() => Health = MaxHealth;

        public void ReciveCredits(int credits)=> Credits += credits;

        public void SpendCredits(int credits)
        {
            if (credits > Credits)
                throw new ArgumentOutOfRangeException($"{Name} has only {Credits} credits");
            Credits -= credits;
        }

        private void RaiseOnKilledEvent()=> OnKilled?.Invoke(this, System.EventArgs.Empty);
    }
}
