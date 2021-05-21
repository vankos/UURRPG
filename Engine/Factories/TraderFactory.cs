using Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private readonly static HashSet<Trader> _traders = new HashSet<Trader>();

        static TraderFactory()
        {
            Trader broker = new Trader("Broker");
            broker.AddItemToInventory(ItemFactory.CreateItem(2));
            _traders.Add(broker);
        }

        public static Trader GetTraderByName(string name) => _traders.FirstOrDefault(t => t.Name == name);
    }
}
