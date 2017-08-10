using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ClashOfTanks.GUI.Utility
{
    sealed class TextBoxInputChecker
    {
        private string Pattern { get; set; }
        private int AcceptableLength { get; set; }

        public TextBoxInputChecker(string pattern, int acceptableLength)
        {
            Pattern = pattern;
            AcceptableLength = acceptableLength;
        }

        public bool IsValidInput(TextBox textBox, string input)
        {
            int actualLength = textBox.Text.Length - textBox.SelectedText.Length + input.Length;
            return IsValidCharacters(input) && IsValidLength(actualLength);
        }

        private bool IsValidCharacters(string input)
        {
            return Regex.IsMatch(input, Pattern, RegexOptions.IgnoreCase);
        }

        private bool IsValidLength(int actualLength)
        {
            return actualLength <= AcceptableLength;
        }
    }
}
