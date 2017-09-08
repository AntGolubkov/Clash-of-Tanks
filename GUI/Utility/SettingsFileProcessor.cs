using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfTanks.GUI.Utility
{
    static class SettingsFileProcessor
    {
        private static string SettingsFilePath { get; } = "Settings.ini";

        public static async Task<string> ReadSettings(string settingsName)
        {
            using (StreamReader reader = new StreamReader(File.Open(SettingsFilePath, FileMode.Open, FileAccess.Read)))
            {
                string line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    int indexOfEquals;

                    if (line.Contains("=") && line.Remove(indexOfEquals = line.IndexOf('=')).ToLower().Trim() == settingsName.ToLower())
                    {
                        return line.Substring(indexOfEquals + 1).Trim();
                    }
                }

                return null;
            }
        }

        public static void WriteSettings(string settingsName, string settingsValue)
        {
            List<string> lines;

            if (File.Exists(SettingsFilePath))
            {
                using (StreamReader reader = new StreamReader(File.Open(SettingsFilePath, FileMode.Open, FileAccess.Read)))
                {
                    lines = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                }

                bool hasSettingsName = false;

                for (int i = 0; i < lines.Count; i++)
                {
                    string lineBeforeEquals;
                    int indexOfEquals;

                    if (lines[i].Contains("=") && (lineBeforeEquals = lines[i].Remove(indexOfEquals = lines[i].IndexOf('='))).ToLower().Trim() == settingsName.ToLower())
                    {
                        string lineAfterEquals;
                        string trimmedLineAfterEquals;

                        if ((trimmedLineAfterEquals = (lineAfterEquals = lines[i].Substring(indexOfEquals + 1)).Trim()) != settingsValue)
                        {
                            if (trimmedLineAfterEquals != string.Empty)
                            {
                                lines[i] = $"{lineBeforeEquals}={lineAfterEquals.Replace(trimmedLineAfterEquals, settingsValue)}";
                            }
                            else
                            {
                                lines[i] = $"{lines[i]}{settingsValue}";
                            }
                            
                            hasSettingsName = true;
                            break;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                if (!hasSettingsName)
                {
                    if (lines[lines.Count - 1] == string.Empty)
                    {
                        lines[lines.Count - 1] = $"{settingsName}={settingsValue}";
                        lines.Add(string.Empty);
                    }
                    else
                    {
                        lines.Add($"{settingsName}={settingsValue}");
                    }
                }
            }
            else
            {
                lines = new List<string>()
                {
                    $"{settingsName}={settingsValue}",
                    string.Empty
                };
            }

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < lines.Count; i++)
            {
                if (i != lines.Count - 1)
                {
                    output.AppendLine(lines[i]);
                }
                else
                {
                    output.Append(lines[i]);
                }
            }

            using (StreamWriter writer = new StreamWriter(File.Open(SettingsFilePath, FileMode.Create, FileAccess.Write)))
            {
                writer.Write(output.ToString());
            }
        }
    }
}
