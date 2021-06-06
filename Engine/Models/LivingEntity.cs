using Engine.Models.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        private string _name;
        private int _health;
        private int _maxHealth;
        private int _credits;
        private int _level;
        private Item _currentWeapon;
        private Item _currentConsumable;

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int Health
        {
            get { return _health; }
            private set
            {
                _health = value;
                OnPropertyChanged();
            }
        }

        public int MaxHealth
        {
            get { return _maxHealth; }
            protected set
            {
                _maxHealth = value;
                OnPropertyChanged();
            }
        }

        public int Credits
        {
            get { return _credits; }
            private set
            {
                _credits = value;
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        public Item CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                if (_currentWeapon != null)
                    _currentWeapon.Action.OnActionPerformed -= RaiseOnActionPerformed;

                _currentWeapon = value;

                if (_currentWeapon != null)
                    _currentWeapon.Action.OnActionPerformed += RaiseOnActionPerformed;

                OnPropertyChanged();
            }
        }

        public Item CurrentConsumable
        {
            get { return _currentConsumable; }
            set
            {
                if (_currentConsumable != null)
                    _currentConsumable.Action.OnActionPerformed -= RaiseOnActionPerformed;

                _currentConsumable = value;

                if (_currentConsumable != null)
                    _currentConsumable.Action.OnActionPerformed += RaiseOnActionPerformed;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Item> Inventory { get; }

        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }

#pragma warning disable S2365 // Properties should not make collection or array copies
        public List<Item> Weapons => Inventory.Where(i => i.Category == Item.ItemCategory.Weapon).ToList();
#pragma warning restore S2365 // Properties should not make collection or array copies

#pragma warning disable S2365 // Properties should not make collection or array copies
        public List<Item> Consumables => Inventory.Where(i => i.Category == Item.ItemCategory.Consumable).ToList();
#pragma warning restore S2365 // Properties should not make collection or array copies

        public bool IsDead => Health <= 0;

        public bool HasConsumables => Consumables.Count>0;

        public event EventHandler OnKilled;
        public event EventHandler<string> OnActionPerformed;

        protected LivingEntity(string name, int maxHealth, int health, int credits, int level = 1)
        {
            Inventory = new ObservableCollection<Item>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();

            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            Credits = credits;
            Level = level;
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
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumables));
        }

        public void RemoveItemFromInventory(Item item)
        {
            Inventory.Remove(item);

            GroupedInventoryItem groupedInventoryItemToRemove = item.IsUnique ?
                GroupedInventory.FirstOrDefault(gi => gi.Item == item) :
                GroupedInventory.FirstOrDefault(gi => gi.Item.Id == item.Id);
            if (groupedInventoryItemToRemove != null)
            {
                if (groupedInventoryItemToRemove.Quantity == 1)
                    GroupedInventory.Remove(groupedInventoryItemToRemove);
                else
                    groupedInventoryItemToRemove.Quantity--;
            }

            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumables));
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

        public void ReciveCredits(int credits) => Credits += credits;

        public void SpendCredits(int credits)
        {
            if (credits > Credits)
                throw new ArgumentOutOfRangeException($"{Name} has only {Credits} credits");
            Credits -= credits;
        }

        public void AttackWithCurrentWeapon(LivingEntity target) => CurrentWeapon.PerformAction(this, target);
        public void UseCurrentConsumable(LivingEntity target) => CurrentConsumable.PerformAction(this,target);

        private void RaiseOnKilledEvent() => OnKilled?.Invoke(this, System.EventArgs.Empty);
        private void RaiseOnActionPerformed(object sender, string result) => OnActionPerformed?.Invoke(this, result);
    }
}
