using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using Engine.EventArgs;
using Engine.Models.Items;
using Engine.ViewModels;
using UURRPG;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameSession _gameSession;
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeUserInputAction();
            _gameSession = new GameSession();
            _gameSession.OnMessageRaised += OnMessageRaised;
            _gameSession.StartTheGame();
            DataContext = _gameSession;
        }

        private void OnClickNorth(object sender, RoutedEventArgs e) => _gameSession.MoveNorth();

        private void OnClickWest(object sender, RoutedEventArgs e) => _gameSession.MoveWest();

        private void OnClickEast(object sender, RoutedEventArgs e) => _gameSession.MoveEast();

        private void OnClickSouth(object sender, RoutedEventArgs e) => _gameSession.MoveSouth();

        private void OnMessageRaised(object sender, GameLogsEventArgs e)
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
    }
}
