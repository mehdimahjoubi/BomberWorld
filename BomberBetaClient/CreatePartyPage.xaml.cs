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
    /// Interaction logic for CreatePartyPage.xaml
    /// </summary>
    public partial class CreatePartyPage : Page
    {
        private MainWindow mainWindow;
        ConversationDC conv;
        private List<BomberContracts_WCF.PlayerDC> friendsList;
        public System.Windows.Data.CollectionViewSource playerDCViewSource { get; set; }

        public CreatePartyPage()
        {
            InitializeComponent();
        }

        public CreatePartyPage(MainWindow mainWindow, List<BomberContracts_WCF.PlayerDC> friends)
        {
            InitializeComponent();
            playerDCViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerDCViewSource")));
            this.mainWindow = mainWindow;
            this.friendsList = friends;
            playerDCViewSource.Source = friends;
        }

        private void OnInviteFriends(object sender, RoutedEventArgs e)
        {
            if (conv == null)
            {
                var friends = playerDCListView.SelectedItems.Cast<PlayerDC>();
                var c = new ConversationDC()
                {
                    Host = mainWindow.Player,
                    Participants = friends
                };
                mainWindow.Server.CreateConversation(c);
            }
            else
            {
                //conversation already existes, add friends to conv...
            }
        }
    }
}
