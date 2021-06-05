using Engine.Models;
using Engine.Models.Quests;
using System;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Enemy GetMonster(int monsterId)
        {
            switch (monsterId)
            {
                case 1:
                    Enemy snail = new Enemy("Snail", "snail.png", 4, 4, 5, 1)
                    {
                        CurrentWeapon = ItemFactory.CreateItem(5)
                    };
                    AddLootItem(snail, 3, 80);
                    AddLootItem(snail, 4, 60);
                    return snail;
                default:
                    throw new ArgumentException($"Monster Type with id {monsterId} does not exist");
            }
        }

        private static void AddLootItem(Enemy monster, int itemId, int dropChance)
        {
            if (RandomNumberGenerator.GetRandNumberBetween(1, 100) <= dropChance)
                monster.AddItemToInventory(ItemFactory.CreateItem(itemId));
        }
    }
}
