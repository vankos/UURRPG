using System;
using System.IO;
using Engine.Factories;
using Engine.Models;
using Engine.Models.Items;
using Engine.Models.Quests;
using Engine.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Engine.Services
{
    public static class SaveGameService
    {
        public static void Save(GameSession gameSession, string fileName) => File.WriteAllText(fileName, JsonConvert.SerializeObject(gameSession, Formatting.Indented));

        public static GameSession LoadSavedOrCreateNewSession(string fileName)
        {
            if (!File.Exists(fileName))
                return new GameSession();

            try
            {
                JObject data = JObject.Parse(File.ReadAllText(fileName));
                Player player = CreatePlayer(data);

                int x = (int)data[nameof(GameSession.CurrentLocation)][nameof(Location.XCoordinate)];
                int y = (int)data[nameof(GameSession.CurrentLocation)][nameof(Location.YCoordinate)];

                GameSession gameSession = new GameSession();
                gameSession.StartTheGameRef = () => gameSession.StartTheGame(player, x, y);
                return gameSession;
            }
            catch (Exception)
            {
                return new GameSession();
            }
        }

        private static Player CreatePlayer(JObject data)
        {
            string fileversion = ParserVersion(data);
            Player player;
            switch (fileversion)
            {
                case "1.0":
                    player = new Player(
                        (string)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Name)],
                        (string)data[nameof(GameSession.CurrentPlayer)][nameof(Player.CharacterClass)],
                        (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Experience)],
                        (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.MaxHealth)],
                        (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Health)],
                        (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Dexterity)],
                        (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Credits)]
                        );
                    break;
                default:
                    throw new InvalidDataException($"File version {fileversion} is not supported");
            }
            PopulatePlayerInventory(data, player);
            PopulatePlayerQuests(data, player);
            PopulatePlayerSchemes(data, player);

            return player;
        }

        private static void PopulatePlayerSchemes(JObject data, Player player)
        {
            string fileversion = ParserVersion(data);

            switch (fileversion)
            {
                case "1.0":
                    foreach (JToken schemeToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Schemes)])
                    {
                        int schemeId = (int)schemeToken[nameof(Scheme.ID)];
                        Scheme scheme = SchemeFactory.GetSchemeById(schemeId);

                        player.LearnScheme(scheme);
                    }
                    break;
                default:
                    throw new InvalidDataException($"File version {fileversion} is not supported");
            }
        }

        private static void PopulatePlayerQuests(JObject data, Player player)
        {
            string fileversion = ParserVersion(data);

            switch (fileversion)
            {
                case "1.0":
                    foreach (JToken questToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Quests)])
                    {
                        int questId = (int)questToken[nameof(QuestStatus.PlayerQuest)][nameof(QuestStatus.PlayerQuest.ID)];
                        Quest quest = QuestFactory.GetQuestByID(questId);
                        QuestStatus questStatus = new QuestStatus(quest)
                        {
                            IsComplete = (bool)questToken[nameof(QuestStatus.IsComplete)]
                        };

                        player.Quests.Add(questStatus);
                    }
                    break;
                default:
                    throw new InvalidDataException($"File version {fileversion} is not supported");
            }
        }

        private static void PopulatePlayerInventory(JObject data, Player player)
        {
            string fileversion = ParserVersion(data);

            switch (fileversion)
            {
                case "1.0":
                    foreach (JToken itemToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Inventory)][nameof(Inventory.Items)])
                    {
                        int itemId = (int)itemToken[nameof(Item.Id)];
                        player.AddItemToInventory(ItemFactory.CreateItem(itemId));
                    }
                    break;
                default:
                    throw new InvalidDataException($"File version {fileversion} is not supported");
            }
        }

        private static string ParserVersion(JObject data) => (string)data[nameof(GameSession.ParserVersion)];
    }
}
