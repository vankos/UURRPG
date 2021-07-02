using Engine.Models;
using Engine.Models.Items;
using Engine.Services;
using System;

namespace Engine.Actions
{
    public class AttackWithWeapon : BaseAction, IAction
    {
        private readonly int _maxDamage;
        private readonly int _minDamage;

        public AttackWithWeapon(Item weapon, int minDamage, int maxDamage) : base(weapon)
        {
            if (weapon.Category != Item.ItemCategory.Weapon)
                throw new ArgumentException($"{weapon.Name} is not a weapon");
            if (minDamage < 0)
                throw new ArgumentException("min damage should be > 0");
            if (minDamage > maxDamage)
                throw new ArgumentException("min damage should be > than max damage");

            _maxDamage = maxDamage;
            _minDamage = minDamage;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorString = (actor is Player) ? "You" : actor.Name;
            string targetString = (target is Player) ? "you" : target.Name.ToLower();

            if (CombatService.DoesAttackSucceeded(actor, target))
            {
                int damage = RandomNumberGenerator.GetRandNumberBetween(_minDamage, _maxDamage);

                ReportResult($"\n{actorString} deal to {targetString} {damage} hp damage");
                target.TakeDamage(damage);
            }
            else
            {
                ReportResult($"{actorString} missed {targetString}");
            }
        }
    }
}
