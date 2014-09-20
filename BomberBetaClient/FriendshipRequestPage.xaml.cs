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
    /// Interaction logic for FriendshipRequestPage.xaml
    /// </summary>
    public partial class FriendshipRequestPage : Page
    {
        private MainWindow mainWindow;
        private BomberContracts_WCF.PlayerDC playerDC;

        public FriendshipRequestPage()
        {
            InitializeComponent();
        }

        public FriendshipRequestPage(MainWindow mainWindow, BomberContracts_WCF.PlayerDC playerDC)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.playerDC = playerDC;
            var playerDCViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerDCViewSource")));
            playerDCViewSource.Source = new List<PlayerDC>() { playerDC };
        }

        private void OnAcceptFriendshipRequest(object sender, RoutedEventArgs e)
        {
            mainWindow.Server.AcceptFriendshipRequest(playerDC);
        }

        private void OnRefuseFriendshipRequest(object sender, RoutedEventArgs e)
        {
            mainWindow.RemoveFriendshipRequestFromList(playerDC);
            //multiple clicks very quickly = risk of bug...
            if (mainWindow.playerDCListView1.Items.Count == 0)
                mainWindow._contentFrame.Content = null;
            else
                mainWindow._contentFrame.Content = new FriendshipRequestPage(mainWindow, mainWindow.playerDCListView1.SelectedItem as PlayerDC);
            try
            {
                mainWindow.Server.RefuseFriendshipRequest(playerDC);
            }
            catch { }
        }
    }
}
