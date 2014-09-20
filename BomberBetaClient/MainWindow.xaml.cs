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
using System.ServiceModel;
using NAudio.Wave;
using System.Threading;
using Bomberman;
using System.Windows.Threading;

namespace BomberBetaClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IBomberCallback
    {
        public bool allowSelection { get; set; }
        public bool NotifyConnexion { get; set; }
        public bool MicEnabled { get; set; }
        public bool LoudSpeakerEnabled { get; set; }
        bool IsConnected = false;
        MediaPlayer mediaPlayer = new MediaPlayer();
        public PlayerDC Player { get; set; }
        public System.Windows.Data.CollectionViewSource playerDCViewSource { get; set; }
        public System.Windows.Data.CollectionViewSource FriendshipRequestsViewSource { get; set; }
        public System.Windows.Data.CollectionViewSource ConversationsListViewSource { get; set; }
        IEnumerable<ConversationDC> conversations;
        IEnumerable<PlayerDC> friends;
        public List<PlayerDC> friendshipRequests { get; set; }
        FriendPage friendPage;
        AddFriendPage addFriendsPage;
        FriendshipRequestPage friendshipRequestPage;
        HomePage homePage;
        ConversationPage partyPage;

        public DuplexChannelFactory<IBomberService> ChannelFactory { get; set; }
        public IBomberService Server { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            playerDCViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerDCViewSource")));
            FriendshipRequestsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerDCViewSource1")));
            ConversationsListViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerDCViewSource2")));
            _contentFrame.Content = new WellcomePage();
            friendPage = new FriendPage(this);
            addFriendsPage = new AddFriendPage(this);
            partyPage = new ConversationPage(this);
            allowSelection = true;
            NotifyConnexion = true;
            MicEnabled = true;
            LoudSpeakerEnabled = true;
            friendshipRequests = new List<PlayerDC>();
            conversations = new List<ConversationDC>();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (GameThread != null)
            {
                GameThread.Abort();
            }
            mediaPlayer.Close();
            if (partyPage != null)
            {
                if (partyPage.waveIn != null)
                    partyPage.waveIn.StopRecording();
                if (partyPage.waveOut != null)
                    partyPage.waveOut.Stop();
            }
            if (IsConnected)
            {
                try
                {
                    Server.TerminateSession();
                }
                catch { }
            }
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            new ConnectionWindow(this).Show();
        }

        private void OnDisonnect(object sender, RoutedEventArgs e)
        {
            ToggleClientState();
            playerDCViewSource.Source = new List<PlayerDC>();
            FriendshipRequestsViewSource.Source = new List<PlayerDC>();
            ConversationsListViewSource.Source = new List<ConversationDC>();
            if (addFriendsPage.playerDCViewSource != null)
            {
                addFriendsPage.playerDCViewSource.Source = new List<PlayerDC>();
                addFriendsPage.playerDCViewSource.View.Refresh();
            }
            _contentFrame.Content = new WellcomePage();
            try
            {
                Server.TerminateSession();
            }
            catch { }
        }

        public void ToggleClientState()
        {
            _disconnectMenuItem.IsEnabled = !_disconnectMenuItem.IsEnabled;
            _connectMenuItem.IsEnabled = !_connectMenuItem.IsEnabled;
            _statusMenu.IsEnabled = !_statusMenu.IsEnabled;
            IsConnected = !IsConnected;
            _addFriendBtn.IsEnabled = !_addFriendBtn.IsEnabled;
            _createAccount.IsEnabled = !_createAccount.IsEnabled;
            _homeBtn.IsEnabled = !_homeBtn.IsEnabled;
            _createPartyBtn.IsEnabled = !_createPartyBtn.IsEnabled;
            _options.IsEnabled = !_options.IsEnabled;
        }

        #region IBomberCallback Members

        public void ReceiveFriendsList(IEnumerable<PlayerDC> friends)
        {
            this.friends = friends;
            for (int i = 0; i < friends.Count(); i++)
            {
                friends.ElementAt(i).HasNotSeenMsg = false;
            }
            LoadFriends();
        }

        public void ReceiveSearchFriendsResult(IEnumerable<PlayerDC> players)
        {
            var potentialFriends = players.ToList<PlayerDC>();
            addFriendsPage = new AddFriendPage(this, potentialFriends);
            _contentFrame.Content = addFriendsPage;
        }

        public void ReceiveProfil(PlayerDC myProfil, IEnumerable<PlayerDC> friendShipRequests, IEnumerable<ConversationDC> playerConvDCs)
        {
            Player = myProfil;
            homePage = new HomePage(this);
            FriendshipRequestsViewSource.Source = friendShipRequests;
            this.friendshipRequests = friendShipRequests.ToList<PlayerDC>();
            foreach (var c in playerConvDCs)
            {
                InitializeConversation(c);
            }
            Server.RequestFriends();
        }

        public void RejectConnection()
        {
            ToggleClientState();
            MessageBox.Show("Access denied. Please check your login and password.");
            new ConnectionWindow(this).Show();
            Server.TerminateSession();
        }

        public void DisconnectPlayer()
        {
            ToggleClientState();
            playerDCViewSource.Source = new List<PlayerDC>();
            FriendshipRequestsViewSource.Source = new List<PlayerDC>();
            ConversationsListViewSource.Source = new List<ConversationDC>();
            MessageBox.Show("You have been disconnected...");
            _contentFrame.Content = new WellcomePage();
            try
            {
                Server.TerminateSession();
            }
            catch { }
        }

        public void ReceiveMessage(int senderPlayerID, string msg)
        {
            var q = from f in friends
                    where f.PlayerId == senderPlayerID
                    select f;
            if (q.Count() > 0)
            {
                var f = q.ToList().ElementAt(0);
                f.MessagesLog += String.Format("{0}\n", msg);
                friendPage._friendChatHistoryScroller.ScrollToEnd();
                if (_contentFrame.Content == friendPage && friendPage.friend.PlayerId == senderPlayerID && this.IsFocused)
                {
                    friendPage.LoadPlayerInfos(f);
                }
                else if (_contentFrame.Content == friendPage && friendPage.friend.PlayerId == senderPlayerID && !this.IsFocused)
                {
                    f.HasNotSeenMsg = true;
                    LoadFriends();
                    allowSelection = false;
                    playerDCListView.SelectedItem = f;
                    friendPage.LoadPlayerInfos(f);
                    mediaPlayer.Open(new Uri("msgNotif.wav", UriKind.Relative));
                    mediaPlayer.Play();
                }
                else
                {
                    var v = playerDCListView.SelectedItem;
                    allowSelection = false;
                    playerDCListView.SelectedItem = f;
                    f.HasNotSeenMsg = true;
                    LoadFriends();
                    allowSelection = false;
                    playerDCListView.SelectedItem = v;
                    friendPage.LoadPlayerInfos(v as PlayerDC);
                    mediaPlayer.Open(new Uri("msgNotif.wav", UriKind.Relative));
                    mediaPlayer.Play();
                }
            }
        }

        public void NotifyFriendshipRequest(PlayerDC playerDC)
        {
            friendshipRequests.Add(playerDC);
            FriendshipRequestsViewSource.Source = friendshipRequests;
            FriendshipRequestsViewSource.View.Refresh();
        }

        public void NotifyFriendshipRequestSuccess()
        {
            MessageBox.Show("Request successfully sent!");
        }

        public void RefuseSubscription()
        {
            this.IsEnabled = true;
            MessageBox.Show("Subscription refused! Please try another login.");
            try
            {
                Server.TerminateSession();
            }
            catch { }
        }

        public void NotifySubscriptionSuccess()
        {
            this.IsEnabled = true;
            _contentFrame.Content = null;
            MessageBox.Show("Subscription succeeded!");
            try
            {
                Server.TerminateSession();
            }
            catch { }
        }

        public void NotifyPlayerStatusChange(PlayerDC friend)
        {
            var q = from f in friends
                    where f.PlayerId == friend.PlayerId
                    select f;
            if (q.Count() > 0)
            {
                var f = q.ToList().ElementAt(0);
                var previousStatus = f.PlayerStatus;
                f.PlayerStatus = friend.PlayerStatus;
                LoadFriends();
                friendPage.LoadPlayerInfos(friendPage.friend);
                if (previousStatus == "Offline" && NotifyConnexion)
                    new NotificationWindow(f.Pseudonym + "\nhas connected!");
            }
        }

        public void NotifyFriendshipEstablishment(PlayerDC playerDC)
        {
            var q = (from r in friendshipRequests
                     where r.PlayerId == playerDC.PlayerId
                     select r).ToList();
            if (q.Count > 0)
            {
                RemoveFriendshipRequestFromList(q.ElementAt(0));
            }
            q = (from f in friends
                 where f.PlayerId == playerDC.PlayerId
                 select f).ToList();
            if (q.Count == 0)
            {
                addFriendsPage.playerDCViewSource.Source = new List<PlayerDC>();
                addFriendsPage.playerDCViewSource.View.Refresh();
                var newFriendsList = new List<PlayerDC>() { playerDC };
                friends = newFriendsList.Concat(friends);
                LoadFriends();
                playerDCViewSource.View.Refresh();
            }
        }

        public void NotifyFriendshipEnding(PlayerDC playerDC)
        {
            var q = (from f in friends
                     where f.PlayerId == playerDC.PlayerId
                     select f).ToList();
            if (q.Count > 0)
            {
                var newFriendsList = friends.ToList();
                newFriendsList.Remove(q.ElementAt(0));
                friends = newFriendsList;
                playerDCViewSource.Source = friends;
                playerDCViewSource.View.Refresh();
            }
        }

        public void NotifyFriendProfilChange(PlayerDC playerDC)
        {
            var q = (from f in friends
                     where f.PlayerId == playerDC.PlayerId
                     select f).ToList();
            if (q.Count > 0)
            {
                var f = q.ElementAt(0);
                f.Pseudonym = playerDC.Pseudonym;
                f.PlayerDescription = playerDC.PlayerDescription;
                var selectedFriend = playerDCListView.SelectedItem;
                LoadFriends();
                allowSelection = false;
                playerDCListView.SelectedItem = selectedFriend;
                friendPage.friend = selectedFriend as PlayerDC;
                if (_contentFrame.Content == friendPage)
                    friendPage.LoadPlayerInfos(friendPage.friend);
            }
        }

        public void InitializeConversation(ConversationDC conversationDC)
        {
            conversationDC.ParticipantsNames = " " + conversationDC.Host.Pseudonym + " (Host) \n ";
            foreach (var p in conversationDC.Participants)
            {
                conversationDC.ParticipantsNames += p.Pseudonym + ", ";
            }
            conversationDC.ParticipantsNames = conversationDC.ParticipantsNames.Remove(conversationDC.ParticipantsNames.Length - 2);
            conversationDC.NewMessage = "";
            var q = (from co in conversations
                     where co.ConversationId == conversationDC.ConversationId
                     select co).ToList();
            if (q.Count != 0)
            {
                foreach (var co in q)
                {
                    ((List<ConversationDC>)conversations).Remove(co);
                }
            }
            ((List<ConversationDC>)conversations).Add(conversationDC);
            ConversationsListViewSource.Source = conversations;
            ConversationsListViewSource.View.Refresh();
        }

        public void ReceiveConversationMessage(ConversationDC c)
        {
            if (c.NewMessageType == 1 && partyPage != null)
            {
                if (LoudSpeakerEnabled && c.MessageSenderPlayerId != Player.PlayerId)
                    partyPage.waveProvider.AddSamples(c.NewAudioMessageBuffer, 0, c.NewAudioMessageBytesRecorded);
                return;
            }
            var q = (from co in conversations
                     where co.ConversationId == c.ConversationId
                     select co).ToList();
            if (q.Count > 0)
            {
                var conv = q.ElementAt(0);
                conv.ConversationLog += c.NewMessage + " \n";
                partyPage.LoadConversation();
            }
        }

        public void DropConversation(ConversationDC conv)
        {
            var selectedConv = conversationDCListView.SelectedItem as ConversationDC;
            var q = (from c in conversations where c.ConversationId == conv.ConversationId select c).ToList();
            if (q.Count > 0)
            {
                var c = q.ElementAt(0);
                ((List<ConversationDC>)conversations).Remove(c);
                ConversationsListViewSource.Source = conversations;
                ConversationsListViewSource.View.Refresh();
                if (selectedConv != null && selectedConv != c)
                {
                    conversationDCListView.SelectedItem = selectedConv;
                    partyPage.LoadConversation(selectedConv);
                }
                if (_contentFrame.Content == partyPage && partyPage.Party == c)
                    _contentFrame.Content = null;
            }
        }

        public void RemovePlayerFromParty(PlayerDC player, ConversationDC conv)
        {
            if (player.PlayerId == Player.PlayerId)
            {
                DropConversation(conv);
                return;
            }
            var selectedConv = conversationDCListView.SelectedItem as ConversationDC;
            var q = (from c in conversations where c.ConversationId == conv.ConversationId select c).ToList();
            if (q.Count > 0)
            {
                var c = q.ElementAt(0);
                var q1 = (from p in c.Participants where p.PlayerId == player.PlayerId select p).ToList();
                if (q1.Count > 0)
                {
                    c.Participants = c.Participants.ToList();
                    ((List<PlayerDC>)c.Participants).Remove(q1.ElementAt(0));
                    c.ParticipantsNames = " " + c.Host.Pseudonym + " (Host) \n ";
                    foreach (var p in c.Participants)
                    {
                        c.ParticipantsNames += p.Pseudonym + ", ";
                    }
                    c.ParticipantsNames = c.ParticipantsNames.Remove(c.ParticipantsNames.Length - 2);
                    partyPage.LoadConversation();
                    ConversationsListViewSource.View.Refresh();
                }
            }
        }

        #endregion

        private void OnFriendsFilterChanged(object sender, TextChangedEventArgs e)
        {
            LoadFriends();
        }

        public void RemoveFriendshipRequestFromList(PlayerDC p)
        {
            friendshipRequests.Remove(p);
            FriendshipRequestsViewSource.Source = friendshipRequests;
            FriendshipRequestsViewSource.View.Refresh();
            if (playerDCListView1.Items.Count != 0 && playerDCListView1.SelectedItem as PlayerDC != null)
                friendshipRequestPage = new FriendshipRequestPage(this, playerDCListView1.SelectedItem as PlayerDC);
            else
                _contentFrame.Content = null;
        }

        public void LoadFriends()
        {
            var onlineFriends = from f in friends where f.PlayerStatus == "Online" && f.Pseudonym.Contains(_friendsFilter.Text) select f;
            var offlineFriends = from f in friends where f.PlayerStatus == "Offline" && f.Pseudonym.Contains(_friendsFilter.Text) select f;
            var absentFriends = from f in friends where f.PlayerStatus == "Absent" && f.Pseudonym.Contains(_friendsFilter.Text) select f;
            var inPartyFriends = from f in friends where f.PlayerStatus == "InParty" && f.Pseudonym.Contains(_friendsFilter.Text) select f;
            var orderedFriends = new List<PlayerDC>();
            playerDCViewSource.Source = orderedFriends.Concat(onlineFriends).Concat(absentFriends).Concat(inPartyFriends).Concat(offlineFriends);
            playerDCViewSource.View.Refresh();
        }

        private void OnSelectedFriendChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!allowSelection)
            {
                allowSelection = true;
                return;
            }

            PlayerDC p = playerDCListView.SelectedItem as PlayerDC;
            if (p != null && p.HasNotSeenMsg)
            {
                p.HasNotSeenMsg = false;
                LoadFriends();
                playerDCListView.SelectedItem = p;
            }
            friendPage.LoadPlayerInfos(p);
        }

        private void OnFriendsFocus(object sender, RoutedEventArgs e)
        {
            _contentFrame.Content = friendPage;
            PlayerDC f = playerDCListView.SelectedItem as PlayerDC;
            if (f != null && f.HasNotSeenMsg)
            {
                f.HasNotSeenMsg = false;
                LoadFriends();
                allowSelection = false;
                playerDCListView.SelectedItem = f;
                friendPage.LoadPlayerInfos(f);
            }
            else
            {
                allowSelection = true;
            }
        }

        private void OnCreateAccount(object sender, RoutedEventArgs e)
        {
            _contentFrame.Content = new AcountCreationPage(this);
        }

        private void OnAddFriends(object sender, RoutedEventArgs e)
        {
            _contentFrame.Content = addFriendsPage;
        }

        private void OnSelectedFriendshipRequestChanged(object sender, SelectionChangedEventArgs e)
        {
            friendshipRequestPage = new FriendshipRequestPage(this, playerDCListView1.SelectedItem as PlayerDC);
        }

        private void OnFriendshipRequestsForcus(object sender, RoutedEventArgs e)
        {
            _contentFrame.Content = friendshipRequestPage;
        }

        private void OnNotifEnablingChanged(object sender, RoutedEventArgs e)
        {
            NotifyConnexion = !NotifyConnexion;
        }

        private void OnHomeBtnClicked(object sender, RoutedEventArgs e)
        {
            if (homePage != null)
                _contentFrame.Content = homePage;
        }

        private void OnCreateParty(object sender, RoutedEventArgs e)
        {
            var q = (from f in friends
                     where f.PlayerStatus != "Offline"
                     select f).ToList<PlayerDC>();
            if (q.Count > 0)
            {
                _contentFrame.Content = new CreatePartyPage(this, q);
            }
            else
            {
                MessageBox.Show("no connected friend");
            }
        }

        private void OnSelectedPartyChanged(object sender, SelectionChangedEventArgs e)
        {
            var conv = conversationDCListView.SelectedItem as ConversationDC;
            if (conv != null && _contentFrame.Content == partyPage)
            {
                partyPage.LoadConversation(conv);
                _contentFrame.Content = partyPage;
            }
        }

        private void OnPartyFocus(object sender, RoutedEventArgs e)
        {
            var conv = conversationDCListView.SelectedItem as ConversationDC;
            partyPage.LoadConversation(conv);
            _contentFrame.Content = partyPage;
        }

        private void OnEnableMic(object sender, RoutedEventArgs e)
        {
            MicEnabled = !MicEnabled;
            if (MicEnabled && WaveInEvent.DeviceCount > 0)
                partyPage.RestartRecording();
            else
                partyPage.waveIn.StopRecording();
        }

        private void OnEnableLoudSpeaker(object sender, RoutedEventArgs e)
        {
            LoudSpeakerEnabled = !LoudSpeakerEnabled;
            if (LoudSpeakerEnabled)
                partyPage.waveOut.Play();
            else
                partyPage.waveOut.Stop();
        }

        #region IBomberCallback Members


        public void OpenGameWindow(ConversationDC conv)
        {
            List<string> allPlayers = new List<string>();
            allPlayers.Add(conv.Host.Login);
            var logins = (from p in conv.Participants select p.Login).ToList();
            allPlayers = allPlayers.Concat<string>(logins).ToList();
            GameLaunching(allPlayers, conv.ConversationId);
        }

        public void UpdatePlayerPosition(ConversationDC conv, PlayerDC player)
        {
            Game.GameContentManager.UpdatePlayerPosition(player.Login, player.PlayerPositionX, player.PlayerPositionY);
        }

        public void SpawnBomb(ConversationDC conv, int BombPositionX, int BombPositionY)
        {
            Game.GameContentManager.SpawnBomb(BombPositionX, BombPositionY);
        }

        public void KillPlayer(ConversationDC conv, PlayerDC player)
        {
            Game.GameContentManager.KillPlayer(player.Login);
        }

        #endregion

        //bomber game
        private Thread GameThread;
        public Bomberman.BomberGame Game { get; set; }

        private void GameLaunching(List<string> AllPlayers, int convID)
        {
            if (GameThread != null)
            {
                GameThread.Abort();
                GameThread = null;
            }
            GameThread = new Thread(new ParameterizedThreadStart(gameThreadStart));
            GameThread.Start(new Object[] { AllPlayers, convID });
        }

        private void gameThreadStart(Object o)
        {
            Object[] objs = (Object[])o;
            List<string> AllPlayers = (List<string>)objs[0];
            var convID = (int)objs[1];
            var game = new BomberGame();
            //using (var game = new BomberGame())
            //{
                //Dispatcher.Invoke(DispatcherPriority.Send, new Action(() =>
                //{
                    game.GameContentManager = new GameContentManager(Player.Login, AllPlayers, game);
                    game.GameContentManager.Server = Server;
                    game.GameID = convID;
                    this.Game = game;
                //}));
                game.Run();
            //}
        }
    }
}
