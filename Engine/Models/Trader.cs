using Engine.Models.Items;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Trader : BaseNotificationClass
    {
        public string Name { get; set; }
        public ObservableCollection<Item> Inventory { get; set; }
        public Trader(string name)
        {
            Name = name;
            Inventory = new ObservableCollection<Item>();
        }

        public void AddItemToInventory(Item item) => Inventory.Add(item);
        public void RemoveItemFromInventory(Item item) => Inventory.Remove(item);

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
