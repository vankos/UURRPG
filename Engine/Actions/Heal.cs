using Engine.Models;
using Engine.Models.Items;
using System;

namespace Engine.Actions
{
    public class Heal : IAction
    {
        private readonly Item _item;
        private readonly int _hp;

        public event EventHandler<string> OnActionPerformed;

        public Heal(Item item, int hp)
        {
            if (item.Category != Item.ItemCategory.Consumable)
                throw new ArgumentException($"{item.Name} is not consumable");
            _item = item;
            _hp = hp;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorString = (actor is Player) ? "You" : actor.Name;
            string targetString = (target is Player) ? "yourself" : target.Name.ToLower();

            ReportResult($"\n{actorString} heal {targetString} {_hp} by {_item.Name.ToLower()}");
            target.Heal(_hp);
            actor.RemoveItemFromInventory(_item);
        }

        private void ReportResult(string result) => OnActionPerformed?.Invoke(this, result);
    }
}
