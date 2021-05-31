using Engine.Models;
using Engine.Factories;
using System;
using Engine.Models.Quests;
using System.Linq;
using Engine.EventArgs;
using Engine.Models.Items;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        private Location _currentLocation;
        private Enemy _currentEnemy;
        private Trader _currentTrader;
        private Player _currentPlayer;

        public event EventHandler<GameLogsEventArgs> OnMessageRaised;

        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                    CurrentPlayer.OnLevelUp -= OnPlayerLevelUp;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                    CurrentPlayer.OnLevelUp += OnPlayerLevelUp;
                }
            }
        }

        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));

                CompleteQuestsAtLocation();
                GetLocationQuests();
                GetLocationMonster();
                CurrentTrader = CurrentLocation.LocalTrader;
            }
        }

        public World CurrentWorld { get; set; }

        public Enemy CurrentEnemy
        {
            get { return _currentEnemy; }
            set
            {
                if (_currentEnemy != null)
                    _currentEnemy.OnKilled -= OnCurrentEnemyKilled;

                _currentEnemy = value;

                if (_currentEnemy != null)
                {
                    RaiseMessage($"\nYou see a {CurrentEnemy.Name}!");
                    _currentEnemy.OnKilled += OnCurrentEnemyKilled;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }

        public Weapon CurrentWeapon { get; set; }

        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;

        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;

        public bool HasMonster => CurrentEnemy != null;

        public bool HasTrader => CurrentTrader != null;

        public void StartTheGame()
        {
            CurrentPlayer = new Player("John Doe", "Scientist", 0, 10, 10, 100);

            if (CurrentPlayer.Weapons.Count == 0)
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1));

            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }

        public void MoveNorth()
        {
            if (HasLocationToNorth)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
        }

        public void MoveEast()
        {
            if (HasLocationToEast)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
        }

        public void MoveWest()
        {
            if (HasLocationToWest)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
        }

        public void MoveSouth()
        {
            if (HasLocationToSouth)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
        }

        private void GetLocationQuests()
        {
            foreach (Quest quest in CurrentLocation.AvailibleQuests)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    RaiseMessage($"\nYou get new Quest! '{quest.Name}':\n{quest.Description}");
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    RaiseMessage("You need:");
                    foreach (var item in quest.Requirements)
                        RaiseMessage($"{item.Quantity} x {ItemFactory.GetItemNameById(item.ItemId)}");
                    RaiseMessage($"Reward is {quest.RewardCredits} credits and {quest.RewardExperience} exp");

                    if (quest.RewardItems != null)
                    {
                        RaiseMessage("Also you will get:");
                        foreach (var item in quest.RewardItems)
                            RaiseMessage($"{item.Quantity} x {ItemFactory.GetItemNameById(item.ItemId)}");
                    }
                }
            }
        }

        private void GetLocationMonster() => CurrentEnemy = CurrentLocation.GetMonster();

        private void RaiseMessage(string message) => OnMessageRaised?.Invoke(this, new GameLogsEventArgs(message));

        public void AttackEnemy()
        {
            if (CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon!");
                return;
            }

            int damageToEnemy = RandomNumberGenerator.GetRandNumberBetween(CurrentWeapon.MinDamage, CurrentWeapon.MaxDamage);
            RaiseMessage($"\nYou deal to {CurrentEnemy.Name} {damageToEnemy} hp damage");
            CurrentEnemy.TakeDamage(damageToEnemy);

            if (CurrentEnemy.IsDead)
            {
                GetLocationMonster();
            }
            else
            {
                int enemyDamage = RandomNumberGenerator.GetRandNumberBetween(CurrentEnemy.MinDamage, CurrentEnemy.MaxDamage);
                RaiseMessage($"{CurrentEnemy.Name} deal to you {enemyDamage} hp damage");
                CurrentPlayer.TakeDamage(enemyDamage);
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (var quest in CurrentLocation.AvailibleQuests)
            {
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsComplete);
                if (questToComplete != null && CurrentPlayer.HasAllThisItems(quest.Requirements))
                {
                    foreach (ItemQuantity iq in quest.Requirements)
                    {
                        for (int i = 0; i < iq.Quantity; i++)
                        {
                            CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(it => it.Id == iq.ItemId));
                        }
                    }
                    RaiseMessage($"\n You completed '{quest.Name}' quest!");
                    RaiseMessage($"You got {quest.RewardCredits} credits");
                    CurrentPlayer.ReciveCredits(quest.RewardCredits);

                    if (quest.RewardItems != null)
                    {
                        RaiseMessage("Also you  got:");
                        foreach (var item in quest.RewardItems)
                        {
                            RaiseMessage($"{item.Quantity} x {ItemFactory.GetItemNameById(item.ItemId)}");
                            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(item.ItemId));
                        }
                    }
                    RaiseMessage($"You got {quest.RewardExperience} exp");
                    CurrentPlayer.AddExp(quest.RewardExperience);

                    questToComplete.IsComplete = true;
                }
            }
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs e)
        {
            RaiseMessage("YOU DIED");
            CurrentLocation = CurrentWorld.LocationAt(0, 0);
            CurrentPlayer.FullHeal();
        }

        private void OnCurrentEnemyKilled(object sender, System.EventArgs e)
        {
            RaiseMessage($"{CurrentEnemy.Name} is dead");
            RaiseMessage($"You get {CurrentEnemy.RewardExp} exp");
            RaiseMessage($"You get {CurrentEnemy.Credits} credits");
            CurrentPlayer.ReciveCredits(CurrentEnemy.Credits);
            foreach (Item lootItem in CurrentEnemy.Inventory)
            {
                CurrentPlayer.AddItemToInventory(lootItem);
                RaiseMessage($"You get {lootItem.Name} from corpse");
            }
            CurrentPlayer.AddExp(CurrentEnemy.RewardExp);
            CompleteQuestsAtLocation();
        }

        private void OnPlayerLevelUp(object sender, System.EventArgs e) => RaiseMessage($"\nYou got level {CurrentPlayer.Level}!");
    }
}
