using Engine.Factories;
using Engine.Models.Items;
using System.Collections.Generic;

namespace Engine.Models
{
    public class Enemy : LivingEntity
    {
        private readonly List<ItemPercentage> _lootTable = new List<ItemPercentage>();

        public int ID { get; }
        public string ImageName { get; }

        public int RewardExp { get; }

        public Enemy(int id, string name, string imageName, int maxHealth, Item weapon , int rewardExp, int rewardCredits) :
            base(name, maxHealth, maxHealth, rewardCredits)
        {
            ID = id;
            ImageName =imageName;
            RewardExp = rewardExp;
            CurrentWeapon = weapon;
        }

        public void AddItemToLootTable(int id, int percentage)
        {
            _lootTable.RemoveAll(ip => ip.ID==id);
            _lootTable.Add(new ItemPercentage(id, percentage));
        }

        public Enemy GetNewInstance()
        {
            Enemy newEnemy = new Enemy(ID, Name, ImageName, MaxHealth, CurrentWeapon.Clone() as Item, RewardExp, Credits);

            foreach (var itemPercentage in _lootTable)
            {
                newEnemy.AddItemToLootTable(itemPercentage.ID, itemPercentage.Pecentage);
                if (RandomNumberGenerator.GetRandNumberBetween(1, 100) <= itemPercentage.Pecentage)
                    newEnemy.AddItemToInventory(ItemFactory.CreateItem(itemPercentage.ID));
            }

            return newEnemy;
        }
    }
}
