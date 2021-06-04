using Engine.Models;
using Engine.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Actions
{
    public class AttackWithWeapon:IAction
    {
        private readonly int _maxDamage;
        private readonly int _minDamage;

        public event EventHandler<string> OnActionPerformed;

        public AttackWithWeapon(Item weapon, int minDamage, int maxDamage)
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
            int damage = RandomNumberGenerator.GetRandNumberBetween(_minDamage, _maxDamage);
            ReportResult($"\nYou deal to {target.Name} {damage} hp damage");
            target.TakeDamage(damage);
        }

        private void ReportResult(string result) => OnActionPerformed?.Invoke(this, result);
    }
}
