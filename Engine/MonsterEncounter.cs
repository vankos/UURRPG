namespace Engine
{
    public class MonsterEncounter
    {
        public int MonsterID { get; set; }
        public int ChanceofEncountering { get; set; }

        public MonsterEncounter(int monsterID, int chamceofEncounter)
        {
            MonsterID = monsterID;
            ChanceofEncountering = chamceofEncounter;
        }
    }
}
