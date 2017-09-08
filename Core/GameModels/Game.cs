using System.Collections.Generic;

using ClashOfTanks.Core.PlayerModels;
using ClashOfTanks.Core.NetworkModels;

namespace ClashOfTanks.Core.GameModels
{
    public sealed class Game
    {
        public static class Requirements
        {
            public static Dictionary<string, string> NameCharacters { get; } = new Dictionary<string, string>()
            {
                { "Alphabetic", "A-Z" },
                { "Numeric", "0-9" },
                { "Underscore", "_" }
            };

            public static int NameMinLength { get; } = 2;
            public static int NameMaxLength { get; } = 12;
        }

        public static Game Current { get; set; } = null;

        public string Name { get; private set; }
        public List<Player> Players { get; private set; }

        public Game(string name)
        {
            Name = name;

            Players = new List<Player>()
            {
                Player.Current
            };
        }
    }
}
