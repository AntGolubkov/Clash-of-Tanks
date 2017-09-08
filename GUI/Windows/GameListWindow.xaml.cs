using ClashOfTanks.Core.NetworkModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using ClashOfTanks.GUI.Controllers;
using System.Windows.Controls;
using ClashOfTanks.Core.GameModels;

namespace ClashOfTanks.GUI.Windows
{
    /// <summary>
    /// Interaction logic for GameListWindow.xaml
    /// </summary>
    public partial class GameListWindow : Window
    {
        private Window NextWindow { get; set; } = null;

        public GameListWindow()
        {
            InitializeComponent();
            Network.Current = new Network(Network.Sides.Client);

            Task.Run(() =>
            {
                try
                {
                    new Game(null).ScanGames(ShowGame);
                }
                catch (Exception) { }
            });
        }

        private void ShowGame(string gameName, string ipAddress, string players)
        {
            Dispatcher.Invoke(() =>
            {
                GameListDataGrid.Items.Add(new { Id = GameListDataGrid.Items.Count + 1, GameName = gameName, IPAddress = ipAddress, Players = players });
            });
        }

        private void CreateGameButton_Click(object sender, RoutedEventArgs e)
        {
            NextWindow = new CreateGameWindow();
            NextWindow.Show();
            Close();
        }

        private void JoinGameButton_Click(object sender, RoutedEventArgs e)
        {
            NextWindow = new GameLobbyWindow();
            NextWindow.Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (NextWindow == null)
            {
                new MainWindow().Show();
            }
        }
    }
}
