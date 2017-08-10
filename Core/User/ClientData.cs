using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfTanks.Core.User
{
    [Serializable]
    public class ClientData
    {
        public object GameObjects { get; set; }
        public double Health { get; set; }

        public ClientData(Player player, object gameObjects)
        {
            GameObjects = gameObjects;
            Health = player.Tank.Health;
        }
    }
}
