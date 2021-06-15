using Engine.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Engine.Shared;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Traders.xml";
        private readonly static HashSet<Trader> _traders = new HashSet<Trader>();

        static TraderFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadTradersFromNodes(data.SelectNodes("/Traders/Trader"));
            }
            else
            {
#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods
                throw new FileNotFoundException("XML file with game data was no found", GAME_DATA_FILENAME);
#pragma warning restore S3877 // Exceptions should not be thrown from unexpected methods
            }
        }

        private static void LoadTradersFromNodes(XmlNodeList xmlNodeList)
        {
            if (xmlNodeList == null) return;
            foreach (XmlNode traderNode in xmlNodeList)
            {
                Trader newTrader = new Trader(
                    traderNode.GetXMLAttributeValue<int>("Id"),
                    traderNode.GetXMLAttributeValue<string>("Name")
                    );

                foreach (XmlNode item in traderNode.SelectNodes("./Inventory/Item"))
                {
                    int quantity = item.GetXMLAttributeValue<int>("Quantity");
                    for (int i = 0; i < quantity; i++)
                    {
                        newTrader.AddItemToInventory(ItemFactory.CreateItem(item.GetXMLAttributeValue<int>("Id")));
                    }
                }
                _traders.Add(newTrader);
            }
        }

        public static Trader GetTraderByName(int id) => _traders.FirstOrDefault(t => t.Id == id);
    }
}
