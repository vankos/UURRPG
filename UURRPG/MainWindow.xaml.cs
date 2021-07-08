using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using Engine.EventArgs;
using Engine.Models.Items;
using Engine.Services;
using Engine.ViewModels;
using UURRPG;
using System.Windows.Controls;
using Microsoft.Win32;
using UURRPG.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private GameSession _gameSession;
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeUserInputAction();
            SetActiveGameSessionTo(new GameSession());
        }

        private void SetActiveGameSessionTo(GameSession gameSession)
        {
            _messageBroker.OnMessageRaised -= OnMessageRaised;

            _gameSession = gameSession;

            GameLog.Document.Blocks.Clear();
            _messageBroker.OnMessageRaised += OnMessageRaised;
            _gameSession.StartTheGameRef.Invoke();
            DataContext = _gameSession;
        }

        private void OnClickNorth(object sender, RoutedEventArgs e) => _gameSession.MoveNorth();

        private void OnClickWest(object sender, RoutedEventArgs e) => _gameSession.MoveWest();

        private void OnClickEast(object sender, RoutedEventArgs e) => _gameSession.MoveEast();

        private void OnClickSouth(object sender, RoutedEventArgs e) => _gameSession.MoveSouth();

        private void OnMessageRaised(object sender, GameMessageEventArgs e)
        {
            GameLog.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameLog.ScrollToEnd();
        }

        private void OnClick_Attack(object sender, RoutedEventArgs e) => _gameSession.AttackEnemy();
        private void OnClick_UseConsumable(object sender, RoutedEventArgs e) => _gameSession.UseCurrentConsumable();

        private void OnClick_DisplayTradeMenu(object sender, RoutedEventArgs e)
        {
            if (!_gameSession.HasTrader) return;

            TradeMenu tradeMenu = new TradeMenu()
            {
                Owner = this,
                DataContext = _gameSession
            };

            tradeMenu.ShowDialog();
        }

        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
            Scheme scheme = ((FrameworkElement)sender).DataContext as Scheme;
            _gameSession.CraftItemUsing(scheme);
        }

        private void InitializeUserInputAction()
        {
            _userInputActions.Add(Key.W, () => _gameSession.MoveNorth());
            _userInputActions.Add(Key.A, () => _gameSession.MoveWest());
            _userInputActions.Add(Key.S, () => _gameSession.MoveSouth());
            _userInputActions.Add(Key.D, () => _gameSession.MoveEast());
            _userInputActions.Add(Key.X, () => _gameSession.AttackEnemy());
            _userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());
            _userInputActions.Add(Key.I, () => SetActiveTab("InventoryTab"));
            _userInputActions.Add(Key.J, () => SetActiveTab("JournalTab"));
            _userInputActions.Add(Key.R, () => SetActiveTab("SchemesTab"));
            _userInputActions.Add(Key.T, () => OnClick_DisplayTradeMenu(null, null));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_userInputActions.ContainsKey(e.Key))
                _userInputActions[e.Key].Invoke();
        }

        private void SetActiveTab(string tabName)
        {
            foreach (var tab in PlayerTabControl.Items)
            {
                if (tab is TabItem tabItem && tabItem.Name == tabName)
                {
                    tabItem.IsSelected = true;
                    return;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            YesNoWindow message = new YesNoWindow("Save game", "Do you want to save your game?")
            {
                Owner = GetWindow(this)
            };
            message.ShowDialog();

            if (message.ClickedYes)
                SaveGame();
        }
        private void StartNewGame_Click(object sender, RoutedEventArgs e) => SetActiveGameSessionTo(new GameSession());

        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "Saved games (*.json)|*.josn"
            };

            if (openFileDialog.ShowDialog() == true)
                SetActiveGameSessionTo(SaveGameService.LoadSavedOrCreateNewSession(openFileDialog.FileName));
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e) => SaveGame();
        private void SaveGame()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "Saved games (*.json)|*.josn"
            };

            if (saveFileDialog.ShowDialog() == true)
                SaveGameService.Save(_gameSession, saveFileDialog.FileName);
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e) => Close();
    }
}
