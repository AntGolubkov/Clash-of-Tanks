using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

using ClashOfTanks.Core.GameModels;
using ClashOfTanks.GUI.Utility;

namespace ClashOfTanks.GUI.Windows
{
    /// <summary>
    /// Interaction logic for CreateGameWindow.xaml
    /// </summary>
    public partial class CreateGameWindow : Window
    {
        private Window NextWindow { get; set; } = null;
        private TextBoxInputChecker InputChecker { get; set; }
        private bool IsLoadGameName { get; set; } = true;

        public CreateGameWindow()
        {
            InitializeComponent();
            StringBuilder characterGroups = new StringBuilder();

            foreach (KeyValuePair<string, string> characterGroup in Game.Requirements.NameCharacters)
            {
                characterGroups.Append(characterGroup.Value);
                AcceptableCharactersTextBlock.Inlines.Add(new Run($" \u2022 {characterGroup.Key} ({characterGroup.Value})"));
                AcceptableCharactersTextBlock.Inlines.Add(new LineBreak());
            }

            InputChecker = new TextBoxInputChecker($@"\A[{characterGroups.ToString()}]+\z", Game.Requirements.NameMaxLength);
            AcceptableLengthTextBlock.Inlines.Add(new Run($" \u2022 {Game.Requirements.NameMinLength}-{Game.Requirements.NameMaxLength} Characters"));

            DataObject.AddPastingHandler(GameNameTextBox, TextBox_Paste);
            GameNameTextBox.Focus();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                string gameName = await SettingsFileProcessor.ReadSettings("DefaultGameName") ?? string.Empty;

                if (IsLoadGameName && InputChecker.IsValidInput(GameNameTextBox, gameName))
                {
                    GameNameTextBox.Text = gameName;
                    GameNameTextBox.SelectAll();
                }
            }
            catch (Exception) { }
        }

        private void GameNameTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = !InputChecker.IsValidInput(sender as TextBox, " ");
            }
        }

        private void GameNameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !InputChecker.IsValidInput(sender as TextBox, e.Text);
        }

        private void TextBox_Paste(object sender, DataObjectPastingEventArgs e)
        {
            if (!InputChecker.IsValidInput(sender as TextBox, e.DataObject.GetData(typeof(string)) as string))
            {
                e.CancelCommand();
            }
        }

        private void GameNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OKButton.IsEnabled = (sender as TextBox).Text.Length >= Game.Requirements.NameMinLength;
            IsLoadGameName = false;
        }

        private void GameNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsLoadGameName = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Game.Current = new Game(GameNameTextBox.Text);
            Task.Run(() =>
            {
                try
                {
                    SettingsFileProcessor.WriteSettings("DefaultGameName", Game.Current.Name);
                }
                catch (Exception) { }
            });

            NextWindow = new GameLobbyWindow();
            NextWindow.Show();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (NextWindow == null)
            {
                new GameListWindow().Show();
            }
        }
    }
}
