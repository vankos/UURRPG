namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        public string ImageName { get; }

        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }

        public int RewardExp { get; set; }

        public Monster(string name, string imageName, int maxHealth, int health, int rewardExp, int rewardCredits, int minDamage, int maxDamage)
        {
            Name = name;
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            MaxHealth = maxHealth;
            Health = health;
            RewardExp = rewardExp;
            Credits = rewardCredits;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}
