using System;
using System.Windows;

using ClashOfTanks.GUI.Utility;

namespace ClashOfTanks.GUI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            string result = new AssemblyChecker().GetMissingAssemblies();

            if (result == null)
            {
                new PlayerNameWindow().Show();
                Close();
            }
            else
            {
                MessageBox.Show($"These files are missing:{Environment.NewLine}{result}", "Error! Missing Files", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
