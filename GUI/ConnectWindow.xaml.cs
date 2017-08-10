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

namespace ClashOfTanks.GUI
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        public ConnectWindow()
        {
            InitializeComponent();
            ServerIPTextBox.Focus();

            NetworkController.Client.CreateConnection();
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown -= Window_KeyDown;

            if (e.Key == Key.Enter)
            {
                string serverIP = ServerIPTextBox.Text;
                ServerIPTextBox.Text = "Waiting...";
                ServerIPTextBox.IsEnabled = false;

                try
                {
                    if (await NetworkController.Client.SendNewPlayerRequest(serverIP))
                    {
                        ServerIPTextBox.Text = "Connected!";

                        await NetworkController.Client.ListenStartGameCommand();
                        new GameWindow(true).Show();
                        Close();
                    }
                    else
                    {
                        ServerIPTextBox.Text = "Not Connected!";
                    }
                }
                catch (Exception)
                {
                    ServerIPTextBox.Text = "Bad IP!";
                }

                ServerIPTextBox.IsEnabled = true;
                ServerIPTextBox.Focus();
                ServerIPTextBox.SelectAll();
            }

            KeyDown += Window_KeyDown;
        }
    }
}
