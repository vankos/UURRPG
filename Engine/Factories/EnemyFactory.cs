using Engine.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Engine.Shared;

namespace Engine.Factories
{
    public static class EnemyFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Enemies.xml";

        private readonly static List<Enemy> _referenceMonsters = new List<Enemy>();

        static EnemyFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath = data.SelectSingleNode("/Enemies").GetXMLAttributeValue<string>("RootImagePath");

                LoadEnemiesFromNodes(data.SelectNodes("/Enemies/Enemy"), rootImagePath);
            }
            else
            {
#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods
                throw new FileNotFoundException("XML file with game data was no found", GAME_DATA_FILENAME);
#pragma warning restore S3877 // Exceptions should not be thrown from unexpected methods
            }
        }

        private static void LoadEnemiesFromNodes(XmlNodeList xmlNodeList, string rootImagePath)
        {
            if (xmlNodeList == null) return;

            foreach (XmlNode enemyNode in xmlNodeList)
            {
                Enemy newEnemy = new Enemy(
                    enemyNode.GetXMLAttributeValue<int>("Id"),
                    enemyNode.GetXMLAttributeValue<string>("Name"),
                     $".{rootImagePath}{enemyNode.GetXMLAttributeValue<string>("ImageName")}",
                    enemyNode.GetXMLAttributeValue<int>("MaxHealth"),
                    ItemFactory.CreateItem(enemyNode.GetXMLAttributeValue<int>("WeaponId")),
                    enemyNode.GetXMLAttributeValue<int>("RewardXP"),
                    enemyNode.GetXMLAttributeValue<int>("Credits")
                    );

                XmlNodeList lootItemsNodes = enemyNode.SelectNodes("./LootItems/LootItem");
                if (lootItemsNodes != null)
                {
                    foreach (XmlNode lootNode in lootItemsNodes)
                    {
                        newEnemy.AddItemToLootTable(
                            lootNode.GetXMLAttributeValue<int>("Id"),
                            lootNode.GetXMLAttributeValue<int>("DropChance")
                            );
                    }
                }
                _referenceMonsters.Add(newEnemy);
            }
        }

        public static Enemy GetMonster(int monsterId)=>_referenceMonsters.Find(i => i.ID == monsterId).GetNewInstance();
    }
}
