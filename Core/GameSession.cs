using ClashOfTanks.Core.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfTanks.Core
{
    public sealed class GameSession
    {
        public static GameSession Current { get; private set; }

        public static void StartGame()
        {
            Current = new GameSession()
            {
                Players = new List<Player>()
                {
                    new Player(0, null, null)
                }
            };
        }

        public List<Player> Players { get; private set; }
    }
}
