namespace Engine.Models
{
    public class Enemy : LivingEntity
    {
        public string ImageName { get; }

        public int MinDamage { get; }
        public int MaxDamage { get; }

        public int RewardExp { get; }

        public Enemy(string name, string imageName, int maxHealth, int health, int rewardExp, int rewardCredits, int minDamage, int maxDamage) :
            base(name, maxHealth, health, rewardCredits)
        {
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            RewardExp = rewardExp;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}
