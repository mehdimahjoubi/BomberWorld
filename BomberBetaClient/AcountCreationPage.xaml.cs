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
using System.ServiceModel;
using BomberContracts_WCF;

namespace BomberBetaClient
{
    /// <summary>
    /// Interaction logic for AcountCreationPage.xaml
    /// </summary>
    public partial class AcountCreationPage : Page
    {
        private MainWindow mainWindow;

        public AcountCreationPage()
        {
            InitializeComponent();
        }

        public AcountCreationPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void OnSubmitSubscription(object sender, RoutedEventArgs e)
        {
            if (_login.Text.Trim() != "" && _password.Text.Trim() != "")
            {
                try
                {
                    mainWindow.ChannelFactory = new DuplexChannelFactory<IBomberService>(mainWindow, "BomberServiceEndpoint");
                    mainWindow.Server = mainWindow.ChannelFactory.CreateChannel();
                    var account = new AccountDC()
                    {
                        Login = _login.Text,
                        Password = _password.Text
                    };
                    var player = new PlayerDC()
                    {
                        Pseudonym = _pseudonym.Text,
                        PlayerDescription = _playerDescription.Text,
                    };
                    if (player.Pseudonym.Trim() == "")
                        player.Pseudonym = account.Login;
                    mainWindow.Server.CreateAccount(account, player);
                    mainWindow.IsEnabled = false;
                }
                catch
                {
                    MessageBox.Show("Conexion failure... Please try again later.");
                }
            }
            else
            {
                MessageBox.Show("Please choose a login and password before submitting.");
            }
        }
    }
}
