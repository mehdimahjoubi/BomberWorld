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
    /// Interaction logic for FriendPage.xaml
    /// </summary>
    public partial class FriendPage : Page
    {
        private MainWindow mainWindow;
        System.Windows.Data.CollectionViewSource playerDCViewSource;
        public PlayerDC friend { get; set; }

        public FriendPage()
        {
            InitializeComponent();
        }

        public FriendPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            playerDCViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerDCViewSource")));
        }

        public void LoadPlayerInfos(PlayerDC p)
        {
            if (p != null)
            {
                if (p.PlayerStatus == "Offline")
                    p.IsConnected = false;
                else
                    p.IsConnected = true;
            }
            playerDCViewSource.Source = new List<PlayerDC>() { p };
            friend = p;
        }

        private void OnMessageTextBoxAreaKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && Keyboard.Modifiers == ModifierKeys.Shift)
            {
                _messageToFriend.Text += " \n";
                _messageToFriend.CaretIndex = _messageToFriend.Text.Length;
            }
            else if (e.Key == Key.Return)
            {
                DateTime utcTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                string msg = utcTime.ToString();
                msg += String.Format(" [{0}] : ", mainWindow.Player.Pseudonym);
                msg += _messageToFriend.Text;
                mainWindow.Server.SendMessage(friend.PlayerId, msg);
                _messageToFriend.Text = "";
                friend.NextMessage = "";
                friend.MessagesLog += String.Format("{0}\n", msg);
                LoadPlayerInfos(friend);
                _friendChatHistoryScroller.ScrollToEnd();
            }
        }

        private void OnRemoveFriend(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmResult = MessageBox.Show("Are you sure you want to remove this player from your friends' list?", "delete friend", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmResult == MessageBoxResult.No)
                return;
            mainWindow.Server.RemoveFriend(friend);
        }
    }
}
