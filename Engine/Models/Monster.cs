using Engine.Models.Quests;
using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Monster:BaseNotificationClass
    {
        private int _health;

        public string Name { get; }
        public string ImageName { get; }
        public int MaxHealth { get; }
        public int Health
        { get
            { return _health; }
            set
            {
                _health = value;
                OnPropertyChanged(nameof(Health));
            }
        }

        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }

        public int RewardExp { get; set; }
        public int RewardCredits { get; set; }

        public ObservableCollection<ItemQuantity> Inventory { get; set; }

        public Monster(string name, string imageName, int maxHealth, int health, int rewardExp, int rewardCredits, int minDamage, int maxDamage)
        {
            Name = name;
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            MaxHealth = maxHealth;
            Health = health;
            RewardExp = rewardExp;
            RewardCredits = rewardCredits;
            MinDamage = minDamage;
            MaxDamage = maxDamage;

            Inventory = new ObservableCollection<ItemQuantity>();
        }
    }
}
