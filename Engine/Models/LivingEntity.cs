using Engine.Models.Items;
using Engine.Models.Quests;
using Engine.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        private string _name;
        private int _health;
        private int _dexterity;
        private int _maxHealth;
        private int _credits;
        private int _level;
        private Item _currentWeapon;
        private Item _currentConsumable;
        private Inventory _inventory;

        public string Name
        {
            get => _name;
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int Health
        {
            get => _health;
            private set
            {
                _health = value;
                OnPropertyChanged();
            }
        }

        public int Dexterity
        {
            get => _dexterity;
            private set
            {
                _dexterity = value;
                OnPropertyChanged();
            }
        }

        public int MaxHealth
        {
            get => _maxHealth;
            protected set
            {
                _maxHealth = value;
                OnPropertyChanged();
            }
        }

        public int Credits
        {
            get => _credits;
            private set
            {
                _credits = value;
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get => _level;
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        public Inventory Inventory
        {
            get { return _inventory; }
            private set
            {
                _inventory = value;
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

        [JsonIgnore]
        public bool IsDead => Health <= 0;

        public event EventHandler OnKilled;
        public event EventHandler<string> OnActionPerformed;

        protected LivingEntity(string name, int maxHealth, int health, int dexterity, int credits, int level = 1)
        {
            Inventory = new Inventory();

            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            Credits = credits;
            Level = level;
            Dexterity = dexterity;
        }

        public void AddItemToInventory(Item item) => Inventory = Inventory.AddItem(item);

        public void RemoveItemFromInventory(Item item) => Inventory = Inventory.RemoveItem(item);

        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities) => Inventory = Inventory.RemoveItems(itemQuantities);

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
        public void UseCurrentConsumable(LivingEntity target) => CurrentConsumable.PerformAction(this, target);

        private void RaiseOnKilledEvent() => OnKilled?.Invoke(this, System.EventArgs.Empty);
        private void RaiseOnActionPerformed(object sender, string result) => OnActionPerformed?.Invoke(this, result);
    }
}
