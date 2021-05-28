namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        public string ImageName { get; }

        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }

        public int RewardExp { get; set; }

        public Monster(string name, string imageName, int maxHealth, int health, int rewardExp, int rewardCredits, int minDamage, int maxDamage):
            base(name, maxHealth, health, rewardCredits)
        {
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            RewardExp = rewardExp;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}
