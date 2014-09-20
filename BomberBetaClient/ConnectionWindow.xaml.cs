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
using System.Windows.Shapes;
using System.ServiceModel;
using BomberContracts_WCF;

namespace BomberBetaClient
{
    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        private MainWindow mainWindow;

        public ConnectionWindow()
        {
            InitializeComponent();
        }

        public ConnectionWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            mainWindow.IsEnabled = false;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.IsEnabled = true;
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            try
            {
                mainWindow.ChannelFactory = new DuplexChannelFactory<IBomberService>(mainWindow, "BomberServiceEndpoint");
                mainWindow.Server = mainWindow.ChannelFactory.CreateChannel();
                mainWindow.Server.StartSession(new AccountDC()
                {
                    Login = _login.Text,
                    Password = _password.Text
                });
                mainWindow.ToggleClientState();
                Close();
            }
            catch
            {
                Close();
                MessageBox.Show("Conexion failure... Please try again later.");
            }
        }
    }
}
