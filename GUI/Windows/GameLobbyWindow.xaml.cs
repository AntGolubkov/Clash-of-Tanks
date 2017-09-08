using ClashOfTanks.Core.GameModels;
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
using ClashOfTanks.GUI.Controllers;
using ClashOfTanks.Core.NetworkModels;

namespace ClashOfTanks.GUI.Windows
{
    /// <summary>
    /// Interaction logic for GameLobbyWindow.xaml
    /// </summary>
    public partial class GameLobbyWindow : Window
    {
        public GameLobbyWindow()
        {
            InitializeComponent();

            Network.Current = new Network(Network.Sides.Server);
            Game.Current.AnnounceGame();
        }
    }
}
