using Engine.Models;
using Engine.Factories;
using Engine.Models.Quests;
using System.Linq;
using Engine.Models.Items;
using Engine.Services;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        private Location _currentLocation;
        private Enemy _currentEnemy;
        private Trader _currentTrader;
        private Player _currentPlayer;

        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                    CurrentPlayer.OnLevelUp -= OnPlayerLevelUp;
                    _currentPlayer.OnActionPerformed -= OnCurrentPlayerPerformedAction;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                    CurrentPlayer.OnLevelUp += OnPlayerLevelUp;
                    _currentPlayer.OnActionPerformed += OnCurrentPlayerPerformedAction;
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
                {
                    _currentEnemy.OnKilled -= OnCurrentEnemyKilled;
                    _currentEnemy.OnActionPerformed -= OnCurrentEnemyPerformedAction;
                }

                _currentEnemy = value;

                if (_currentEnemy != null)
                {
                    _messageBroker.RaiseMessage($"\nYou see a {CurrentEnemy.Name}!");
                    _currentEnemy.OnKilled += OnCurrentEnemyKilled;
                    _currentEnemy.OnActionPerformed += OnCurrentEnemyPerformedAction;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }

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

            if (CurrentPlayer.Inventory.Weapons.Count == 0)
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(6));
            CurrentPlayer.LearnScheme(SchemeFactory.GetSchemeById(1));

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
                    _messageBroker.RaiseMessage($"\nYou get new Quest! '{quest.Name}':\n{quest.Description}");
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    _messageBroker.RaiseMessage("You need:");
                    foreach (var item in quest.Requirements)
                        _messageBroker.RaiseMessage($"{item.Quantity} x {ItemFactory.GetItemNameById(item.ItemId)}");
                    _messageBroker.RaiseMessage($"Reward is {quest.RewardCredits} credits and {quest.RewardExperience} exp");

                    if (quest.RewardItems != null)
                    {
                        _messageBroker.RaiseMessage("Also you will get:");
                        foreach (var item in quest.RewardItems)
                            _messageBroker.RaiseMessage($"{item.Quantity} x {ItemFactory.GetItemNameById(item.ItemId)}");
                    }
                }
            }
        }

        private void GetLocationMonster() => CurrentEnemy = CurrentLocation.GetMonster();

        public void AttackEnemy()
        {
            if (CurrentEnemy == null) return;

            if (CurrentPlayer.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage("You must select a weapon!");
                return;
            }

            CurrentPlayer.AttackWithCurrentWeapon(CurrentEnemy);

            if (CurrentEnemy.IsDead)
                GetLocationMonster();
            else
                CurrentEnemy.AttackWithCurrentWeapon(CurrentPlayer);
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (var quest in CurrentLocation.AvailibleQuests)
            {
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsComplete);
                if (questToComplete != null && CurrentPlayer.Inventory.HasAllThisItems(quest.Requirements))
                {
                    CurrentPlayer.RemoveItemsFromInventory(quest.Requirements);

                    _messageBroker.RaiseMessage($"\n You completed '{quest.Name}' quest!");
                    _messageBroker.RaiseMessage($"You got {quest.RewardCredits} credits");
                    CurrentPlayer.ReciveCredits(quest.RewardCredits);

                    if (quest.RewardItems != null)
                    {
                        _messageBroker.RaiseMessage("Also you  got:");
                        foreach (var item in quest.RewardItems)
                        {
                            _messageBroker.RaiseMessage($"{item.Quantity} x {ItemFactory.GetItemNameById(item.ItemId)}");
                            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(item.ItemId));
                        }
                    }
                    _messageBroker.RaiseMessage($"You got {quest.RewardExperience} exp");
                    CurrentPlayer.AddExp(quest.RewardExperience);

                    questToComplete.IsComplete = true;
                }
            }
        }

        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentConsumable == null) return;

            CurrentPlayer.UseCurrentConsumable(CurrentPlayer);
            CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.CurrentConsumable);
        }

        public void CraftItemUsing(Scheme scheme)
        {
            if (CurrentPlayer.Inventory.HasAllThisItems(scheme.RequiredItems))
            {
                CurrentPlayer.RemoveItemsFromInventory(scheme.RequiredItems);
                foreach (var item in scheme.QutputItems)
                {
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        Item outputItem = ItemFactory.CreateItem(item.ItemId);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        _messageBroker.RaiseMessage($"\nYou created {outputItem.Name}");
                    }
                }
            }
            else
            {
                _messageBroker.RaiseMessage("\nYou don't have required details for the scheme");
                _messageBroker.RaiseMessage("You need:");
                foreach (var item in scheme.RequiredItems)
                {
                    _messageBroker.RaiseMessage($" You need: {item.Quantity} x {ItemFactory.GetItemNameById(item.ItemId)}");
                }
            }
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs e)
        {
            _messageBroker.RaiseMessage("YOU DIED");
            CurrentLocation = CurrentWorld.LocationAt(0, 0);
            CurrentPlayer.FullHeal();
        }

        private void OnCurrentEnemyKilled(object sender, System.EventArgs e)
        {
            _messageBroker.RaiseMessage($"{CurrentEnemy.Name} is dead");
            _messageBroker.RaiseMessage($"You get {CurrentEnemy.RewardExp} exp");
            _messageBroker.RaiseMessage($"You get {CurrentEnemy.Credits} credits");
            CurrentPlayer.ReciveCredits(CurrentEnemy.Credits);
            foreach (Item lootItem in CurrentEnemy.Inventory.Items)
            {
                CurrentPlayer.AddItemToInventory(lootItem);
                _messageBroker.RaiseMessage($"You get {lootItem.Name} from corpse");
            }
            CurrentPlayer.AddExp(CurrentEnemy.RewardExp);
            CompleteQuestsAtLocation();
        }

        private void OnPlayerLevelUp(object sender, System.EventArgs e) => _messageBroker.RaiseMessage($"\nYou got level {CurrentPlayer.Level}!");
        private void OnCurrentPlayerPerformedAction(object sender, string result) => _messageBroker.RaiseMessage(result);
        private void OnCurrentEnemyPerformedAction(object sender, string result) => _messageBroker.RaiseMessage(result);
    }
}
