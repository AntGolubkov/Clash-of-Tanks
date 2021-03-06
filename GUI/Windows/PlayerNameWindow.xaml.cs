﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

using ClashOfTanks.Core.PlayerModels;
using ClashOfTanks.GUI.Utility;

namespace ClashOfTanks.GUI.Windows
{
    /// <summary>
    /// Interaction logic for PlayerNameWindow.xaml
    /// </summary>
    public partial class PlayerNameWindow : Window
    {
        private Window NextWindow { get; set; } = null;
        private TextBoxInputChecker InputChecker { get; set; }
        private bool IsLoadPlayerName { get; set; } = true;

        public PlayerNameWindow()
        {
            InitializeComponent();
            StringBuilder characterGroups = new StringBuilder();

            foreach (KeyValuePair<string, string> characterGroup in Player.Requirements.NameCharacters)
            {
                characterGroups.Append(characterGroup.Value);
                AcceptableCharactersTextBlock.Inlines.Add(new Run($" \u2022 {characterGroup.Key} ({characterGroup.Value})"));
                AcceptableCharactersTextBlock.Inlines.Add(new LineBreak());
            }

            InputChecker = new TextBoxInputChecker($@"\A[{characterGroups.ToString()}]+\z", Player.Requirements.NameMaxLength);
            AcceptableLengthTextBlock.Inlines.Add(new Run($" \u2022 {Player.Requirements.NameMinLength}-{Player.Requirements.NameMaxLength} Characters"));

            DataObject.AddPastingHandler(PlayerNameTextBox, TextBox_Paste);
            PlayerNameTextBox.Focus();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                string playerName = await SettingsFileProcessor.ReadSettings("DefaultPlayerName") ?? string.Empty;

                if (IsLoadPlayerName && InputChecker.IsValidInput(PlayerNameTextBox, playerName))
                {
                    PlayerNameTextBox.Text = playerName;
                    PlayerNameTextBox.SelectAll();
                }
            }
            catch (Exception) { }
        }

        private void PlayerNameTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = !InputChecker.IsValidInput(sender as TextBox, " ");
            }
        }

        private void PlayerNameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void PlayerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OKButton.IsEnabled = (sender as TextBox).Text.Length >= Player.Requirements.NameMinLength;
            IsLoadPlayerName = false;
        }

        private void PlayerNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsLoadPlayerName = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Player.Current = new Player(PlayerNameTextBox.Text);
            Task.Run(() =>
                {
                    try
                    {
                        SettingsFileProcessor.WriteSettings("DefaultPlayerName", Player.Current.Name);
                    }
                    catch (Exception) { }
                });

            NextWindow = new GameListWindow();
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
                new MainWindow().Show();
            }
        }
    }
}
