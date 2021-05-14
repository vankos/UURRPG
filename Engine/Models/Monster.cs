using Engine.Models.Quests;
using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Monster:BaseNotificationClass
    {
        private int _damage;

        public string Name { get; }
        public string ImageName { get; }
        public int MaxDamage { get; }
        public int Damage
        { get
            { return _damage; }
            private set
            {
                _damage = value;
                OnPropertyChanged(nameof(Damage));
            }
        }

        public int RewardExp { get; set; }
        public int RewardCredits { get; set; }

        public ObservableCollection<ItemQuantity> Inventory { get; set; }

        public Monster(string name, string imageName, int maxDamage, int damage, int rewardExp, int rewardCredits)
        {
            Name = name;
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            MaxDamage = maxDamage;
            Damage = damage;
            RewardExp = rewardExp;
            RewardCredits = rewardCredits;

            Inventory = new ObservableCollection<ItemQuantity>();
        }
    }
}
