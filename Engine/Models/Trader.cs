using Engine.Models.Items;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Trader : LivingEntity
    {
        public int Id { get; }
        public Trader(int id, string name) : base(name, 9999, 9999, 9999)
        {
            Id = id;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public override bool Equals(object obj)
        {
            return obj is Trader trader &&
                   Name == trader.Name &&
                   EqualityComparer<ObservableCollection<Item>>.Default.Equals(Inventory, trader.Inventory);
        }
    }
}
