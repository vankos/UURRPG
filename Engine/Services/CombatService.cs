using Engine.Models;

namespace Engine.Services
{
    public static class CombatService
    {
        public enum Combatant
        {
            Player,
            Enemy
        }

        public static Combatant FirstAttacker(Player player, Enemy enemy)
        {
            int playerDexterity = player.Dexterity * player.Dexterity;
            int enemyDexterity = enemy.Dexterity * enemy.Dexterity;
            decimal dexterityOffcet = (playerDexterity - enemyDexterity) / 10m;
            int randomOffset = RandomNumberGenerator.GetRandNumberBetween(-10, 10);
            decimal totalOffcet = dexterityOffcet + randomOffset;

            return RandomNumberGenerator.GetRandNumberBetween(0, 100) <= 50 + totalOffcet ? Combatant.Player : Combatant.Enemy;
        }

        public static bool DoesAttackSucceeded(LivingEntity attacker, LivingEntity target)
        {
            int attackerDexterity = attacker.Dexterity * attacker.Dexterity;
            int targetDexterity = target.Dexterity * target.Dexterity;
            decimal dexterityOffcet = (attackerDexterity - targetDexterity) / 10m;
            int randomOffset = RandomNumberGenerator.GetRandNumberBetween(-10, 10);
            decimal totalOffcet = dexterityOffcet + randomOffset;

            return RandomNumberGenerator.GetRandNumberBetween(0, 100) <= 50 + totalOffcet;
        }
    }
}
