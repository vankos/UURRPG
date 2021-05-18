using Engine.Models;
using Engine.Models.Quests;
using System;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static  Monster GetMonster(int monsterId)
        {
            switch (monsterId)
            {
                case 1:
                    Monster snail = new Monster("Snail", "snail.png", 4, 4, 5, 1,1,2);
                    AddLootItem(snail,3,25);
                    AddLootItem(snail,4,60);
                    return snail;
                default:
                    throw new ArgumentException($"Monster Type with id {monsterId} does not exist");
             }
        }

        private static void  AddLootItem(Monster monster, int itemId, int dropChance)
        {
            if (RandomNumberGenerator.GetRandNumberBetween(1, 100) <= dropChance)
                monster.Inventory.Add(new ItemQuantity(itemId,1));
        }
    }
}
