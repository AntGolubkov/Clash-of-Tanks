using System;

namespace ClashOfTanks.Core.User
{
    public sealed class Player
    {
        public enum Colors { Red, Blue, White }
        
        public Colors Color { get; private set; }
        internal UserActions Actions { get; private set; }

        public Player(int id)
        {
            Color = id < Enum.GetNames(typeof(Colors)).Length - 1 ? (Colors)id : Colors.White;
            Actions = new UserActions();
        }
    }
}
