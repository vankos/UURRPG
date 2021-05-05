using System.Windows;
using Engine.ViewModels;

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

            DataContext = _gameSession;
        }

        private void OnClickNorth(object sender, RoutedEventArgs e) => _gameSession.MoveNorth();

        private void OnClickWest(object sender, RoutedEventArgs e) => _gameSession.MoveWest();

        private void OnClickEast(object sender, RoutedEventArgs e) => _gameSession.MoveEast();

        private void OnClickSouth(object sender, RoutedEventArgs e) => _gameSession.MoveSouth();
    }
}
