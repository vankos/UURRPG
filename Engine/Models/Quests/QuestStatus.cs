namespace Engine.Models.Quests
{
    public class QuestStatus : BaseNotificationClass
    {
        private bool _isComplete;

        public Quest PlayerQuest { get; }
        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                _isComplete = value;
                OnPropertyChanged();
            }
        }

        public QuestStatus(Quest playerQuest)
        {
            PlayerQuest = playerQuest;
            IsComplete = false;
        }
    }
}
