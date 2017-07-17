using System.Windows;

using ClashOfTanks.GUI.Service;

namespace ClashOfTanks.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 1; i <= InputProcessor.PatternCount; i++)
            {
                PlayersComboBox.Items.Add(i);
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            InputProcessor.SetupPlayers((int)PlayersComboBox.SelectedValue);

            new GameWindow().Show();
            Close();
        }
    }
}
