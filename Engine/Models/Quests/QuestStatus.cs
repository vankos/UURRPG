namespace Engine.Models.Quests
{
    public class QuestStatus
    {
        public Quest PlayerQuest { get; set; }
        public bool IsComplete{ get; set; }

        public QuestStatus(Quest playerQuest)
        {
            PlayerQuest = playerQuest;
            IsComplete = false;
        }
    }
}
