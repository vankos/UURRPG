﻿using System.Collections.Generic;

namespace Engine.Models.Items
{
    public static class ItemFactory
    {
        private readonly static List<Item> _referenceItems = new List<Item>()
        {
            new Weapon(1, "Pen", 5, 0, 1),
            new Weapon(2, "Ruler", 30, 0.5, 1)
        };

        public static Item CreateItem(int itemId)
        {
           Item standartItem = _referenceItems.Find(i => i.Id == itemId);
           return standartItem.Clone() as Item;
        }
    }
}
