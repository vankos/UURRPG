using Engine.Models.Items;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Engine.Shared;

namespace Engine.Factories
{
    public static class SchemeFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Schemes.xml";
        private static readonly List<Scheme> _schemes = new List<Scheme>();

        static SchemeFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadSchemesFromNodes(data.SelectNodes("/Schemes/Scheme"));
            }
            else
            {
#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods
                throw new FileNotFoundException("XML file with game data was no found", GAME_DATA_FILENAME);
#pragma warning restore S3877 // Exceptions should not be thrown from unexpected methods
            }
        }

        private static void LoadSchemesFromNodes(XmlNodeList xmlNodeList)
        {
            if (xmlNodeList == null) return;

            foreach (XmlNode schemeNode in xmlNodeList)
            {
                Scheme newScheme = new Scheme(
                    schemeNode.GetXMLAttributeValue<int>("Id"),
                    schemeNode.GetXMLAttributeValue<string>("Name")
                    );

                foreach (XmlNode ingredientItem in schemeNode.SelectNodes("./Ingredients/Item"))
                {
                        newScheme.AddIngredient(
                            ingredientItem.GetXMLAttributeValue<int>("Id"),
                            ingredientItem.GetXMLAttributeValue<int>("Quantity"));
                }

                foreach (XmlNode outputItem in schemeNode.SelectNodes("./OutputItems/Item"))
                {
                    newScheme.AddOutputItem(
                        outputItem.GetXMLAttributeValue<int>("Id"),
                        outputItem.GetXMLAttributeValue<int>("Quantity"));
                }

                _schemes.Add(newScheme);
            }
        }

        public static Scheme GetSchemeById(int id) => _schemes.Find(x => x.ID == id);
    }
}
