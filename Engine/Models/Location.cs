using System.Collections.Generic;
using System.Linq;
using Engine.Factories;
using Engine.Models.Quests;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; }
        public int YCoordinate { get; }
        public string Name { get; }
        public string Description { get; }
        public string BackgroundImageName { get; }

        public List<Quest> AvailibleQuests { get; } = new List<Quest>();

        public List<MonsterEncounter> PossibleMonsters { get; } = new List<MonsterEncounter>();

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
                    return MonsterFactory.GetMonster(monster.MonsterID);
            }

            return MonsterFactory.GetMonster(PossibleMonsters[0].MonsterID);
        }
    }
}
