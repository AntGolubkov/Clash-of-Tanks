using System.Collections.Generic;

using ClashOfTanks.Core.NetworkModels;

namespace ClashOfTanks.Core.PlayerModels
{
    public sealed class Player
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

        public static Player Current { get; set; } = null;

        public string Name { get; private set; }
        private Node NetworkNode { get; set; } = null;

        public Player(string name)
        {
            Name = name;
        }

        internal Player(string name, Node networkNode) : this(name)
        {
            NetworkNode = networkNode;
        }
    }
}
