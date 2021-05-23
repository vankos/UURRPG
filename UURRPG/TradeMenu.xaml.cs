using Engine.Models.Items;
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            if (((FrameworkElement)sender).DataContext is Item item)
            {
                Session.CurrentPlayer.Credits += item.Price;
                Session.CurrentTrader.AddItemToInventory(item);
                Session.CurrentPlayer.RemoveItemFromInventory(item);
            }
        }

        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is Item item)
            {
                if (Session.CurrentPlayer.Credits >= item.Price)
                {
                    Session.CurrentPlayer.Credits -= item.Price;
                    Session.CurrentPlayer.AddItemToInventory(item);
                    Session.CurrentTrader.RemoveItemFromInventory(item);
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
