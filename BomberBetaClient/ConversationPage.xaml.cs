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
using NAudio.Wave;

namespace BomberBetaClient
{
    /// <summary>
    /// Interaction logic for ConversationPage.xaml
    /// </summary>
    public partial class ConversationPage : Page
    {
        public WaveOut waveOut { get; set; }
        public WaveIn waveIn { get; set; }
        public BufferedWaveProvider waveProvider { get; set; }
        bool recordingStarted = false;
        private MainWindow mainWindow;
        public ConversationDC Party { get; set; }
        public System.Windows.Data.CollectionViewSource PartyListViewSource { get; set; }

        public ConversationPage()
        {
            InitializeComponent();
        }

        public ConversationPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            PartyListViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("conversationDCViewSource")));
            waveOut = new WaveOut();
            waveIn = new WaveIn();
            waveProvider = new BufferedWaveProvider(new WaveIn().WaveFormat);
            waveOut.Init(waveProvider);
            waveOut.Play();
            int inputDeviceNumber = WaveInEvent.DeviceCount - 1;
            waveIn.DeviceNumber = inputDeviceNumber;
            waveIn.BufferMilliseconds = 10;
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
        }

        public void RestartRecording()
        {
            if (WaveInEvent.DeviceCount > 0)
            {
                waveIn = new WaveIn();
                int inputDeviceNumber = WaveInEvent.DeviceCount - 1;
                waveIn.DeviceNumber = inputDeviceNumber;
                waveIn.BufferMilliseconds = 10;
                waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
                waveIn.StartRecording();
            }
        }

        public void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (recordingStarted)
                {
                    mainWindow.Server.SendConversationMessage(new ConversationDC()
                    {
                        ConversationId = mainWindow.Player.MainRoomId,
                        NewAudioMessageBuffer = e.Buffer,
                        NewAudioMessageBytesRecorded = e.BytesRecorded,
                        NewMessageType = 1,
                        MessageSenderPlayerId = mainWindow.Player.PlayerId
                    });
                }
            }
            catch { }
        }

        private void OnPartyMsgAreaKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && Keyboard.Modifiers == ModifierKeys.Shift)
            {
                _convMsgArea.Text += " \n";
                _convMsgArea.CaretIndex = _convMsgArea.Text.Length;
            }
            else if (e.Key == Key.Return)
            {
                DateTime utcTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                string msg = utcTime.ToString();
                msg += String.Format(" [{0}] : ", mainWindow.Player.Pseudonym);
                msg += _convMsgArea.Text;
                var msgContainer = new ConversationDC()
                {
                    ConversationId = Party.ConversationId,
                    NewMessage = msg,
                    NewMessageType = 0
                };
                mainWindow.Server.SendConversationMessage(msgContainer);
                _convMsgArea.Text = "";
                _convLogScroller.ScrollToEnd();
            }
        }

        public void LoadConversation(ConversationDC conv = null)
        {
            if (conv != null)
                Party = conv;
            PartyListViewSource.Source = new List<ConversationDC>() { Party };
            PartyListViewSource.View.Refresh();
            if (Party.Host.PlayerId == mainWindow.Player.PlayerId)
            {
                _delConvBtn.Visibility = System.Windows.Visibility.Visible;
                _inviteBtn.Visibility = System.Windows.Visibility.Visible;
                _kickBtn.Visibility = System.Windows.Visibility.Visible;
                _leaveBtn.Visibility = System.Windows.Visibility.Hidden;
                _playBtn.IsEnabled = true;
            }
            else
            {
                _delConvBtn.Visibility = System.Windows.Visibility.Hidden;
                _inviteBtn.Visibility = System.Windows.Visibility.Hidden;
                _kickBtn.Visibility = System.Windows.Visibility.Hidden;
                _leaveBtn.Visibility = System.Windows.Visibility.Visible;
                _playBtn.IsEnabled = false;
            }
            if (mainWindow.Player.MainRoomId == Party.ConversationId)
                _mainRoomBtn.IsEnabled = false;
            else
                _mainRoomBtn.IsEnabled = true;
        }

        private void OnDeleteParty(object sender, RoutedEventArgs e)
        {
            mainWindow.Server.DeleteConversation(new ConversationDC() { ConversationId = Party.ConversationId });
        }

        private void OnInviteToParty(object sender, RoutedEventArgs e)
        {
            //TDODO
        }

        private void OnKickFromParty(object sender, RoutedEventArgs e)
        {
            //TDODO
        }

        private void OnLeaveParty(object sender, RoutedEventArgs e)
        {
            mainWindow.Server.LeaveConversation(new ConversationDC() { ConversationId = Party.ConversationId });
        }

        private void OnMainRoombtnClicked(object sender, RoutedEventArgs e)
        {
            mainWindow._micMenuItem.IsEnabled = true;
            mainWindow._loudSpeakerMenuItem.IsEnabled = true;
            if (!recordingStarted)
            {
                if (WaveInEvent.DeviceCount > 0)
                {
                    waveIn.StartRecording();
                }
                recordingStarted = true;
            }
            mainWindow.Player.MainRoomId = Party.ConversationId;
            mainWindow.Server.SwitchMainConversation(new ConversationDC() { ConversationId = Party.ConversationId });
            _mainRoomBtn.IsEnabled = false;
        }

        private void OnStartGameBtnClicked(object sender, RoutedEventArgs e)
        {
            if (Party.Participants.Count() <= 3)
            {
                mainWindow.Server.StartGame(Party.ConversationId);
            }
            else
            {
                MessageBox.Show("The maximum allowed number of players is 4!");
            }
        }
    }
}
