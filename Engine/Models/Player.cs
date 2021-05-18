using Engine.Models.Items;
using Engine.Models.Quests;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public class Player : BaseNotificationClass
    {
        private string _name;
        private string _characterClass;
        private int _health;
        private int _experience;
        private int _level;
        private int _credits;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string CharacterClass
        {
            get { return _characterClass; }
            set
            {
                _characterClass = value;
                OnPropertyChanged(nameof(CharacterClass));
            }
        }
        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                OnPropertyChanged(nameof(Health));
            }
        }
        public int Experience
        {
            get { return _experience; }
            set
            {
                _experience = value;
                OnPropertyChanged(nameof(Experience));
            }
        }
        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }
        public int Credits
        {
            get { return _credits; }
            set
            {
                _credits = value;
                OnPropertyChanged(nameof(Credits));
            }
        }

        public ObservableCollection<Item> Inventory { get; set; }

        public List<Item> Weapons => Inventory.Where(i => i is Weapon).ToList();

        public ObservableCollection<QuestStatus> Quests { get; set; }

        public Player()
        {
            Inventory = new ObservableCollection<Item>();
            Quests = new ObservableCollection<QuestStatus>();
        }

        public void AddItemToInventory(Item item)
        {
            Inventory.Add(item);
            OnPropertyChanged(nameof(Weapons));
        }
    }
}
