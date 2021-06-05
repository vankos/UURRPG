namespace Engine.Models
{
    public class Enemy : LivingEntity
    {
        public string ImageName { get; }

        public int RewardExp { get; }

        public Enemy(string name, string imageName, int maxHealth, int health, int rewardExp, int rewardCredits) :
            base(name, maxHealth, health, rewardCredits)
        {
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            RewardExp = rewardExp;
        }
    }
}
