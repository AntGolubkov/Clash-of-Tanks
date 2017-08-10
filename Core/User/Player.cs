using ClashOfTanks.Core.Gameplay.Models;
using System;
using System.Net;

namespace ClashOfTanks.Core.User
{
    [Serializable]
    public sealed class Player
    {
        public enum Colors { Red, Blue, White }
        
        public Colors Color { get; private set; }
        public UserActions Actions { get; set; }
        public UserInput Input { get; private set; }

        public bool IsLoser { get; internal set; }

        public IPEndPoint IPFrom { get; set; }
        public IPEndPoint IPTo { get; set; }

        public Tank Tank { get; set; }

        public Player(int id)
        {
            Color = id < Enum.GetNames(typeof(Colors)).Length - 1 ? (Colors)id : Colors.White;
            Actions = new UserActions();
            Input = new UserInput(this);

            IsLoser = false;

            IPFrom = null;
            IPTo = null;

            Tank = null;
        }

        public Player(int id, IPEndPoint ipFrom, IPEndPoint ipTo) : this(id)
        {
            IPFrom = ipFrom;
            IPTo = ipTo;
        }
    }
}
