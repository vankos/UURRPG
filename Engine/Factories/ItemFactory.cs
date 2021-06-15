using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Engine.Actions;
using Engine.Models.Items;
using Engine.Shared;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\GameItems.xml";

        private readonly static List<Item> _referenceItems = new List<Item>();

        static ItemFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadItemsFromNodes(data.SelectNodes("/GameItems/Weapons/Weapon"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/ConsumableItems/Consumable"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/MiscellaneousItems/Miscellaneous"));
            }
            else
            {
#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods
                throw new FileNotFoundException("XML file with game data was no found", GAME_DATA_FILENAME);
#pragma warning restore S3877 // Exceptions should not be thrown from unexpected methods
            }
        }

        private static void LoadItemsFromNodes(XmlNodeList xmlNodeList)
        {
            if (xmlNodeList == null) return;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (Enum.TryParse(xmlNode.Name, out Item.ItemCategory itemCategory))
                {
                    Item newItem = new Item(itemCategory,
                        xmlNode.GetXMLAttributeValue<int>( "ID"),
                        xmlNode.GetXMLAttributeValue<string>( "Name"),
                        xmlNode.GetXMLAttributeValue<int>( "Price"),
                        itemCategory == Item.ItemCategory.Weapon);

                    if (itemCategory == Item.ItemCategory.Weapon)
                    {
                        newItem.Action = new AttackWithWeapon(newItem,
                            xmlNode.GetXMLAttributeValue<int>( "MinDamage"),
                            xmlNode.GetXMLAttributeValue<int>( "MaxDamage"));
                    }
                    else if (itemCategory == Item.ItemCategory.Consumable)
                    {
                        newItem.Action = new Heal(newItem,
                            xmlNode.GetXMLAttributeValue<int>("Hp"));
                    }

                    _referenceItems.Add(newItem);
                }
                else
                {
                    throw new XmlException($"Wrong data tag {xmlNode.Name}");
                }
            }
        }

        public static Item CreateItem(int itemId) => _referenceItems.Find(i => i.Id == itemId).Clone() as Item;

        public static string GetItemNameById(int itemId) => _referenceItems.Find(i => i.Id == itemId)?.Name ?? "*";
    }
}
