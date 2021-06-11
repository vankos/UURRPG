﻿using Engine.Models.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models.Items
{
    public class Scheme
    {
        public int ID { get; }
        public string Name { get; }

        public List<ItemQuantity> RequiredItems { get; } = new List<ItemQuantity>();
        public List<ItemQuantity> QutputItems { get; } = new List<ItemQuantity>();

        public Scheme(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public void AddIngredient(int itemID, int quantity)
        {
            if (!RequiredItems.Any(x => x.ItemId == itemID))
                RequiredItems.Add(new ItemQuantity(itemID, quantity));
        }

        public void AddOutputItem(int itemID, int quantity)
        {
            if (!QutputItems.Any(x => x.ItemId == itemID))
                QutputItems.Add(new ItemQuantity(itemID, quantity));
        }
    }
}