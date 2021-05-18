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
        private Monster _currentMonster;

        public event EventHandler<GameLogsEventArgs> OnMessageRaised;

        public Player CurrentPlayer { get; set; }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));

                GetLocationQuests();
                GetLocationMonster();
            }
        }

        public World CurrentWorld { get; set; }

        public Monster CurrentEnemy
        {
            get { return _currentMonster; }
            set

            {
                _currentMonster = value;
                OnPropertyChanged(nameof(CurrentEnemy));
                OnPropertyChanged(nameof(HasMonster));

                if (CurrentEnemy != null)
                    RaiseMessage($"\nYou see a {CurrentEnemy.Name}!");
            }
        }

        public Weapon CurrentWeapon { get; set; }

        public bool HasLocationToNorth
        {
            get => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        }

        public bool HasLocationToEast
        {
            get => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        }

        public bool HasLocationToWest
        {
            get => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        }

        public bool HasLocationToSouth
        {
            get => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        }

        public bool HasMonster => CurrentEnemy != null;

        public GameSession()
        {
            CurrentPlayer = new Player()
            {
                Name = "John Doe",
                CharacterClass = "Scientist",
                Health = 1,
                Credits = 100,
                Experience = 0,
                Level = 1
            };
            if (CurrentPlayer.Weapons.Count==0)
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1));

            CurrentWorld =  WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }

        public void MoveNorth()
        {   if(HasLocationToNorth)
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
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
            }
        }

        private void GetLocationMonster()=>  CurrentEnemy= CurrentLocation.GetMonster();

        private void RaiseMessage(string message) => OnMessageRaised?.Invoke(this, new GameLogsEventArgs(message));

        public void AttackEnemy()
        {
            if (CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon!");
                return;
            }

            int damageToEnemy = RandomNumberGenerator.GetRandNumberBetween(CurrentWeapon.MinDamage, CurrentWeapon.MaxDamage);
            CurrentEnemy.Health -= damageToEnemy;
            RaiseMessage($"You deal to {CurrentEnemy.Name} {damageToEnemy} hp damage");

            if (CurrentEnemy.Health <= 0)
            {
                RaiseMessage($"{CurrentEnemy.Name} is dead");
                CurrentPlayer.Experience += CurrentEnemy.RewardExp;
                RaiseMessage($"You get {CurrentEnemy.RewardExp} exp");
                CurrentPlayer.Credits += CurrentEnemy.RewardCredits;
                RaiseMessage($"You get {CurrentEnemy.RewardCredits} credits");
                foreach (ItemQuantity lootItem in CurrentEnemy.Inventory)
                {
                    for (int i = 0; i < lootItem.Quantity; i++)
                    {
                        Item item = ItemFactory.CreateItem(lootItem.ItemId);
                        CurrentPlayer.AddItemToInventory(item);
                    }
                    RaiseMessage($"You get {lootItem.Quantity} x {ItemFactory.GetItemNameById(lootItem.ItemId)} from corpse");
                }

                GetLocationMonster();
            }
            else
            {
                int enemyDamage = RandomNumberGenerator.GetRandNumberBetween(CurrentEnemy.MinDamage, CurrentEnemy.MaxDamage);
                CurrentPlayer.Health -= enemyDamage;
                RaiseMessage($"{CurrentEnemy.Name} deal to you {enemyDamage} hp damage");

                if (CurrentPlayer.Health <= 0)
                {
                    RaiseMessage("YOU DIED");
                }

                CurrentLocation = CurrentWorld.LocationAt(0, 0);
                CurrentPlayer.Health = CurrentPlayer.Level;
            }
        }
    }
}
