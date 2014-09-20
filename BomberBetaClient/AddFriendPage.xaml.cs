using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BomberContracts_WCF;

namespace BomberBetaClient
{
    /// <summary>
    /// Interaction logic for AddFriendPage.xaml
    /// </summary>
    public partial class AddFriendPage : Page
    {
        private MainWindow mainWindow;
        private List<PlayerDC> potentialFriends;

        public System.Windows.Data.CollectionViewSource playerDCViewSource { get; set; }
        
        public AddFriendPage()
        {
            InitializeComponent();
            playerDCViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerDCViewSource")));
            playerDCViewSource.Source = new List<PlayerDC>();
        }

        public AddFriendPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        public AddFriendPage(MainWindow mainWindow, List<PlayerDC> potentialFriends)
        {
            InitializeComponent();
            playerDCViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerDCViewSource")));
            playerDCViewSource.Source = potentialFriends;
            this.mainWindow = mainWindow;
            this.potentialFriends = potentialFriends;
        }

        private void OnSearchFriends(object sender, RoutedEventArgs e)
        {
            PlayerDC refPlayer = new PlayerDC()
            {
                Pseudonym = _pseudo.Text,
                Login = _login.Text
            };
            mainWindow.Server.SearchFriends(refPlayer);
        }

        private void OnSendFriendshipRequest(object sender, RoutedEventArgs e)
        {
            var players = playerDCListView.SelectedItems.Cast<PlayerDC>();
            mainWindow.Server.SendFriendshipRequests(players);
        }
    }
}
