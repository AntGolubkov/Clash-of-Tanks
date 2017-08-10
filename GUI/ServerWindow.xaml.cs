using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ClashOfTanks.GUI.Service;
using ClashOfTanks.Core;
using ClashOfTanks.Core.User;
using System.Net;

namespace ClashOfTanks.GUI
{
    /// <summary>
    /// Interaction logic for ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window
    {
        private bool IsGameStarted { get; set; } = false;

        public ServerWindow()
        {
            InitializeComponent();

            NetworkController.Server.CreateConnection();
            GameSession.StartGame();

            AddPlayer();
            ScanClients();
        }

        private void AddPlayer()
        {
            TextBlock playerIDTextBlock = new TextBlock()
            {
                Text = GameSession.Current.Players.Count.ToString(),
                FontFamily = new FontFamily("Times New Roman"),
                FontSize = 20,
                FontWeight = FontWeights.Bold
            };

            TextBlock playerIPTextBlock = new TextBlock()
            {
                Text = GameSession.Current.Players[GameSession.Current.Players.Count - 1].IPFrom?.Address.ToString() ?? "Server",
                FontFamily = new FontFamily("Times New Roman"),
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Right
            };

            DockPanel playerDockPanel = new DockPanel()
            {
                Margin = new Thickness(20)
            };

            DockPanel.SetDock(playerIDTextBlock, Dock.Left);
            playerDockPanel.Children.Add(playerIDTextBlock);
            playerDockPanel.Children.Add(playerIPTextBlock);

            PlayersStackPanel.Children.Add(playerDockPanel);

            (PlayersStackPanel.Parent as ScrollViewer).ScrollToBottom();
        }

        private async void ScanClients()
        {
            while (!IsGameStarted)
            {
                try
                {
                    if (await NetworkController.Server.ListenNewPlayerRequest())
                    {
                        AddPlayer();
                    }
                }
                catch (ObjectDisposedException)
                {

                }
            }
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown -= Window_KeyDown;

            if (e.Key == Key.Enter)
            {
                await NetworkController.Server.SendStartGameCommand();
                IsGameStarted = true;

                new GameWindow(false).Show();
                Close();
            }

            KeyDown += Window_KeyDown;
        }
    }
}
