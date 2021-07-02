using Engine.Models.Items;
using Engine.Models.Quests;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        private string _characterClass;
        private int _experience;

        public string CharacterClass
        {
            get => _characterClass;
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }

        public int Experience
        {
            get => _experience;
            private set
            {
                _experience = value;
                OnPropertyChanged();
                CheckForLevelUp();
            }
        }

        public ObservableCollection<QuestStatus> Quests { get; }
        public ObservableCollection<Scheme> Schemes { get; }

        public event EventHandler OnLevelUp;

        public Player(string name, string charClass, int expirience, int maxHealth, int health, int dexterity, int credits) : base(name, maxHealth, health, dexterity, credits)
        {
            CharacterClass = charClass;
            Experience = expirience;
            Quests = new ObservableCollection<QuestStatus>();
            Schemes = new ObservableCollection<Scheme>();
        }

        public void AddExp(int exp) => Experience += exp;

        private void CheckForLevelUp()
        {
            int currentLvl = Level;
            Level = (Experience / 100) + 1;
            if (Level != currentLvl)
            {
                MaxHealth = Level * 10;
                FullHeal();
                OnLevelUp?.Invoke(this, System.EventArgs.Empty);
            }
        }

        public void LearnScheme(Scheme scheme)
        {
            if (!Schemes.Any(s => s.ID == scheme.ID))
                Schemes.Add(scheme);
        }
    }
}
