using System;
using System.Globalization;
using System.Threading;
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
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            string result;

            try
            {
                result = new AssemblyChecker().GetMissingAssemblies();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error! Failed to Get Missing Assemblies", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (result == null)
            {
                new PlayerNameWindow().Show();
                Close();
            }
            else
            {
                MessageBox.Show($"These files are missing:{Environment.NewLine}{result}", "Error! Failed to Find Files", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
