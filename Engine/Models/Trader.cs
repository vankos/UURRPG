using Engine.Models.Items;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Trader : LivingEntity
    {
        public Trader(string name) : base(name, 9999, 9999, 9999) { }

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
