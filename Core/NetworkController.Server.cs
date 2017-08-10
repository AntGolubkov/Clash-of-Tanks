using ClashOfTanks.Core.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfTanks.Core
{
    public static partial class NetworkController
    {
        public static class Server
        {
            public static Player Host { get; set; } = null;

            public static void CreateConnection()
            {
                NetworkConnection.Setup();
            }

            public static async Task<bool> ListenNewPlayerRequest()
            {
                NetworkConnection networkConnection = NetworkConnection.Current;
                UdpReceiveResult result;

                using (networkConnection.GetUdp(NetworkConnection.Ports.ServerListener))
                {
                    result = await networkConnection.Receive();
                }

                if (NetworkUtility.Decode(result.Buffer) == "NewPlayerRequest")
                {
                    GameSession.Current.Players.Add(new Player(GameSession.Current.Players.Count, result.RemoteEndPoint, new IPEndPoint(result.RemoteEndPoint.Address, (int)NetworkConnection.Ports.ClientListener)));
                    await Task.Delay(TimeSpan.FromMilliseconds(50));

                    using (networkConnection.GetUdp(NetworkConnection.Ports.ServerTalker))
                    {
                        await networkConnection.Send("NewPlayerReply", GameSession.Current.Players[GameSession.Current.Players.Count - 1]);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static async Task SendStartGameCommand()
            {
                NetworkConnection networkConnection = NetworkConnection.Current;

                using (networkConnection.GetUdp(NetworkConnection.Ports.ServerTalker))
                {
                    await networkConnection.Send("StartGameCommand", GameSession.Current.Players);
                }
            }

            public static void ListenUserInput()
            {
                NetworkConnection networkConnection = NetworkConnection.Current;

                networkConnection.UdpReceiver.Close();

                using (networkConnection.GetUdp(NetworkConnection.Ports.ServerListener))
                {
                    while (true)
                    {
                        UdpReceiveResult result = networkConnection.Receive().Result;
                        Task.Run(() => UpdateUserInput(result));
                    }
                }
            }

            private static void UpdateUserInput(UdpReceiveResult result)
            {
                Player player = GameSession.Current.Players.Find(p => p.IPFrom != null && p.IPFrom.Port == result.RemoteEndPoint.Port);
                object actions = NetworkUtility.Deserialize(result.Buffer);

                if (actions is UserActions)
                {
                    player.Actions = NetworkUtility.Deserialize(result.Buffer) as UserActions;
                }
            }

            public static void SendClientData(List<Player> players, object gameplayElements)
            {
                NetworkConnection networkConnection = NetworkConnection.Current;
                using (networkConnection.GetUdp(NetworkConnection.Ports.ServerTalker))
                {
                    foreach (Player player in players)
                    {
                        networkConnection.Send(new ClientData(player, gameplayElements), player);
                    }
                }
            }
        }
    }
}
