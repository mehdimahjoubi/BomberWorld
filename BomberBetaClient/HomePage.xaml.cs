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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private MainWindow mainWindow;
        public CollectionViewSource playerDCViewSource { get; set; }
        PlayerDC profil;

        public HomePage()
        {
            InitializeComponent();
        }

        public HomePage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            playerDCViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerDCViewSource")));
            LoadProfil();
        }

        private void LoadProfil()
        {
            profil = new PlayerDC()
            {
                NumberOfVictories = mainWindow.Player.NumberOfVictories,
                Pseudonym = mainWindow.Player.Pseudonym,
                PlayerDescription = mainWindow.Player.PlayerDescription
            };
            playerDCViewSource.Source = new List<PlayerDC>() { profil };
            playerDCViewSource.View.Refresh();
        }

        private void OnSaveProfil(object sender, RoutedEventArgs e)
        {
            mainWindow.Server.SaveProfil(profil);
            mainWindow.Player.PlayerDescription = profil.PlayerDescription;
            mainWindow.Player.Pseudonym = profil.Pseudonym;
        }

        private void OnCancelChanges(object sender, RoutedEventArgs e)
        {
            LoadProfil();
        }
    }
}
