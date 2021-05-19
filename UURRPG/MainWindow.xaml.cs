﻿using System;
using System.Windows;
using System.Windows.Documents;
using Engine.EventArgs;
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
    }
}
