using System;
using System.Windows;
using System.Windows.Documents;
using Engine.EventArgs;
using Engine.Models.Items;
using Engine.ViewModels;
using UURRPG;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameSession _gameSession;

        public MainWindow()
        {
            InitializeComponent();
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
    }
}
