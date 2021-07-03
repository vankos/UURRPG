using System.Collections.Generic;
using System.Linq;
using Engine.Factories;
using Engine.Models.Quests;
using Newtonsoft.Json;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; }
        public int YCoordinate { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public string BackgroundImageName { get; }
        [JsonIgnore]
        public List<Quest> AvailibleQuests { get; } = new List<Quest>();
        [JsonIgnore]
        public List<MonsterEncounter> PossibleMonsters { get; } = new List<MonsterEncounter>();
        [JsonIgnore]
        public Trader LocalTrader { get; set; }

        public Location(int xCoordinate, int yCoordinate, string name, string description, string backgroundImageName)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Name = name;
            Description = description;
            BackgroundImageName = backgroundImageName;
        }

        public void AddMonster(int monsterId, int chanceOfEncountering)
        {
            if (PossibleMonsters.Exists(m => m.MonsterID == monsterId))
                PossibleMonsters.Find(m => m.MonsterID == monsterId).ChanceofEncountering = chanceOfEncountering;
            else
                PossibleMonsters.Add(new MonsterEncounter(monsterId, chanceOfEncountering));
        }

        public Enemy GetMonster()
        {
            if (PossibleMonsters.Count == 0)
                return null;

            int totalChances = PossibleMonsters.Sum(m => m.ChanceofEncountering);
            int randNumber = RandomNumberGenerator.GetRandNumberBetween(1, totalChances);
            int runningTotal = 0;
            foreach (var monster in PossibleMonsters)
            {
                runningTotal += monster.ChanceofEncountering;
                if (randNumber <= runningTotal)
                    return EnemyFactory.GetMonster(monster.MonsterID);
            }

            return EnemyFactory.GetMonster(PossibleMonsters[0].MonsterID);
        }
    }
}
