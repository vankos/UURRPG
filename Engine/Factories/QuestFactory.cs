using Engine.Models.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Engine.Shared;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Quests.xml";

        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadQuestsFromNodes(data.SelectNodes("/Quests/Quest"));
            }
            else
            {
#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods
                throw new FileNotFoundException("XML file with game data was no found", GAME_DATA_FILENAME);
#pragma warning restore S3877 // Exceptions should not be thrown from unexpected methods
            }
        }

        private static void LoadQuestsFromNodes(XmlNodeList xmlNodeList)
        {
            if (xmlNodeList == null) return;
            foreach (XmlNode questNode in xmlNodeList)
            {
                List<ItemQuantity> requirements = new List<ItemQuantity>();
                List<ItemQuantity> rewardItems = new List<ItemQuantity>();

                foreach (XmlNode reqItemNode in questNode.SelectNodes("./Requirements/Item"))
                {
                    requirements.Add(new ItemQuantity(
                        reqItemNode.GetXMLAttributeValue<int>("Id"),
                        reqItemNode.GetXMLAttributeValue<int>("Quantity")
                        ));
                }

                foreach (XmlNode rewardItemNode in questNode.SelectNodes("./RewardItems/Item"))
                {
                    rewardItems.Add(new ItemQuantity(
                        rewardItemNode.GetXMLAttributeValue<int>("Id"),
                        rewardItemNode.GetXMLAttributeValue<int>("Quantity")
                        ));
                }

                Quest newQuest = new Quest
                (
                questNode.GetXMLAttributeValue<int>("Id"),
                questNode.GetXMLAttributeValue<string>("Name"),
                questNode.SelectSingleNode("./Description").InnerText,
                requirements,
                rewardItems,
                questNode.GetXMLAttributeValue<int>("RewardExperience"),
                questNode.GetXMLAttributeValue<int>("rewardCredits")
                );

                _quests.Add(newQuest);
            }
        }

        internal static Quest GetQuestByID(int id) => _quests.Find(q => q.ID == id);
    }
}
