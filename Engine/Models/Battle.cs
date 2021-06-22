using System;
using Engine.EventArgs;
using Engine.Models.Items;
using Engine.Services;

namespace Engine.Models
{
    public class Battle : IDisposable
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Player _player;
        private readonly Enemy _enemy;

        private enum Combatant
        {
            Player,
            Enemy
        }

        private static Combatant FirstAttacker => RandomNumberGenerator.GetRandNumberBetween(1, 2) == 1 ? Combatant.Player : Combatant.Enemy;

        public event EventHandler<CombatVictoryEventArgs> OnVictory;

        public Battle(Player player, Enemy enemy)
        {
            _player = player;
            _enemy = enemy;

            _player.OnActionPerformed += OnCombatActionPerformed;
            _enemy.OnActionPerformed += OnCombatActionPerformed;
            _enemy.OnKilled += OnEnemyKilled;

            _messageBroker.RaiseMessage($"\nYou see a {_enemy.Name}!");

            if (FirstAttacker == Combatant.Enemy)
                AttackPlayer();
        }

        public void AttackEnemy()
        {
            if (_player.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage("You must select a weapon!");
                return;
            }

            _player.AttackWithCurrentWeapon(_enemy);

            if (!_enemy.IsDead)
                AttackPlayer();
        }

        private void AttackPlayer()=> _enemy.AttackWithCurrentWeapon(_player);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _player.OnActionPerformed -= OnCombatActionPerformed;
            _enemy.OnActionPerformed -= OnCombatActionPerformed;
            _enemy.OnKilled -= OnEnemyKilled;
        }

        private void OnEnemyKilled(object sender, System.EventArgs e)
        {
            _messageBroker.RaiseMessage($"{_enemy.Name} is dead");
            _messageBroker.RaiseMessage($"You get {_enemy.RewardExp} exp");
            _messageBroker.RaiseMessage($"You get {_enemy.Credits} credits");
            _player.ReciveCredits(_enemy.Credits);
            foreach (Item lootItem in _enemy.Inventory.Items)
            {
                _player.AddItemToInventory(lootItem);
                _messageBroker.RaiseMessage($"You get {lootItem.Name} from corpse");
            }
            _player.AddExp(_enemy.RewardExp);

            OnVictory?.Invoke(this, new CombatVictoryEventArgs());
        }

        private void OnCombatActionPerformed(object sender, string result) => _messageBroker.RaiseMessage(result);
    }
}
