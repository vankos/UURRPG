using Engine.Models.Items;
using Engine.ViewModels;
using System.Windows;

namespace UURRPG
{
    /// <summary>
    /// Interaction logic for TradeMenu.xaml
    /// </summary>
    public partial class TradeMenu : Window
    {
        public GameSession Session => DataContext as GameSession;
        public TradeMenu()
        {
            InitializeComponent();
        }

        private void OnClick_Sell(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is GroupedInventoryItem itemGroup)
            {
                Session.CurrentPlayer.ReciveCredits(itemGroup.Item.Price);
                Session.CurrentTrader.AddItemToInventory(itemGroup.Item);
                Session.CurrentPlayer.RemoveItemFromInventory(itemGroup.Item);
            }
        }

        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is GroupedInventoryItem itemGroup)
            {
                if (Session.CurrentPlayer.Credits >= itemGroup.Item.Price)
                {
                    Session.CurrentPlayer.SpendCredits(itemGroup.Item.Price);
                    Session.CurrentPlayer.AddItemToInventory(itemGroup.Item);
                    Session.CurrentTrader.RemoveItemFromInventory(itemGroup.Item);
                }
                else
                {
                    MessageBox.Show("You do not have enough credits");
                }
            }
        }

        private void OnClick_Close(object sender, RoutedEventArgs e) => Close();
    }
}
