using Engine.Models;
using Engine.Shared;
using System.IO;
using System.Xml;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Locations.xml";
        internal static World CreateWorld()
        {
            World newWorld = new World();
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath = data.SelectSingleNode("/Locations").GetXMLAttributeValue<string>("RootImagePath");

                LoadWorldFromNodes(newWorld, rootImagePath, data.SelectNodes("/Locations/Location"));
            }
            else
            {
                throw new FileNotFoundException("XML file with game data was no found", GAME_DATA_FILENAME);
            }
            return newWorld;
        }

        private static void LoadWorldFromNodes(World world, string rootImagePath, XmlNodeList xmlNodeList)
        {
            if (xmlNodeList == null) return;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                Location newLocation = new Location(
                    xmlNode.GetXMLAttributeValue<int>("X"),
                    xmlNode.GetXMLAttributeValue<int>("Y"),
                    xmlNode.GetXMLAttributeValue<string>("Name"),
                    xmlNode.SelectSingleNode("./Description")?.InnerText,
                    $".{rootImagePath}{xmlNode.GetXMLAttributeValue<string>("ImageName")}");

                AddEnemies(xmlNode.SelectNodes("./Enemies/Enemy"), newLocation);
                AddQuests(xmlNode.SelectNodes("./Quests/Quest"), newLocation);
                AddTraders(xmlNode.SelectSingleNode("Trader"), newLocation);

                world.AddLocation(newLocation);
            }
        }

        private static void AddTraders(XmlNode traderNode, Location newLocation)
        {
            if (traderNode != null)
            {
                newLocation.LocalTrader = TraderFactory.GetTraderByName(traderNode.GetXMLAttributeValue<string>("Name"));
            }
        }

        private static void AddEnemies(XmlNodeList enemiesNode, Location newLocation)
        {
            if (enemiesNode != null)
            {
                foreach (XmlNode enemyNode in enemiesNode)
                {
                    newLocation.AddMonster(
                        enemyNode.GetXMLAttributeValue<int>("Id"),
                        enemyNode.GetXMLAttributeValue<int>("EncounteringChance")
                    );
                }
            }
        }

        private static void AddQuests(XmlNodeList questsNode, Location newLocation)
        {
            if (questsNode != null)
            {
                foreach (XmlNode questNode in questsNode)
                {
                    newLocation.AvailibleQuests.Add(QuestFactory.GetQuestByID(questNode.GetXMLAttributeValue<int>("Id")));
                }
            }
        }
    }
}
