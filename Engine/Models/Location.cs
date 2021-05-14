using System.Collections.Generic;
using System.Linq;
using Engine.Factories;
using Engine.Models.Quests;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundImageName { get; set; }

        public List<Quest> AvailibleQuests { get; set; } = new List<Quest>();

        public List<MonsterEncounter> PossibleMonsters { get; set; } = new List<MonsterEncounter>();

        public void AddMonster(int monsterId, int chanceOfEncountering)
        {
            if (PossibleMonsters.Exists(m => m.MonsterID == monsterId))
                PossibleMonsters.Find(m => m.MonsterID == monsterId).ChanceofEncountering = chanceOfEncountering;
            else
                PossibleMonsters.Add(new MonsterEncounter(monsterId, chanceOfEncountering));
        }

        public Monster GetMonster()
        {
            if (!PossibleMonsters.Any())
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

            return MonsterFactory.GetMonster(PossibleMonsters.First().MonsterID);
        }
    }
}
