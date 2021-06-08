﻿using Engine.Models.Items;
using Engine.Models.Quests;
using System;
using System.Collections.Generic;
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
            get { return _characterClass; }
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }

        public int Experience
        {
            get { return _experience; }
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

        public Player(string name, string charClass, int expirience, int maxHealth, int health, int credits) : base(name, maxHealth, health, credits)
        {
            CharacterClass = charClass;
            Experience = expirience;
            Quests = new ObservableCollection<QuestStatus>();
            Schemes = new ObservableCollection<Scheme>();
        }

        public bool HasAllThisItems(List<ItemQuantity> items)
        {
            foreach (var item in items)
            {
                if (Inventory.Count(i => i.Id == item.ItemId) < item.Quantity)
                    return false;
            }
            return true;
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
