using ClashOfTanks.Core.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClashOfTanks.Core
{
    public static partial class NetworkController
    {
        public static class Client
        {
            public static Player Player { get; set; } = null;
            public static ClientData Data { get; set; }

            public static void CreateConnection()
            {
                NetworkConnection.Setup();
            }

            public static async Task<bool> SendNewPlayerRequest(string serverIP)
            {
                Server.Host = new Player(0, null, new IPEndPoint(IPAddress.Parse(serverIP), (int)NetworkConnection.Ports.ServerListener));

                NetworkConnection networkConnection = NetworkConnection.Current;

                using (networkConnection.GetUdp(NetworkConnection.Ports.ClientTalker))
                {
                    await networkConnection.Send("NewPlayerRequest", Server.Host);
                }

                using (networkConnection.GetUdp(NetworkConnection.Ports.ClientListener))
                {
                    Task<UdpReceiveResult> receiveTask = networkConnection.Receive();
                    var resultTask = await Task.WhenAny(receiveTask, Task.Delay(5000));

                    if (resultTask is Task<UdpReceiveResult>)
                    {
                        if (NetworkUtility.Decode((resultTask as Task<UdpReceiveResult>).Result.Buffer) == "NewPlayerReply")
                        {
                            Player = new Player(0, null, null);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public static async Task<bool> ListenStartGameCommand()
            {
                NetworkConnection networkConnection = NetworkConnection.Current;
                UdpReceiveResult result;

                using (networkConnection.GetUdp(NetworkConnection.Ports.ClientListener))
                {
                    result = await networkConnection.Receive();
                }

                if (NetworkUtility.Decode(result.Buffer) == "StartGameCommand")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static async Task SendUserActions(UserActions actions)
            {
                NetworkConnection networkConnection = NetworkConnection.Current;

                using (networkConnection.GetUdp(NetworkConnection.Ports.ClientTalker))
                {
                    await networkConnection.Send(actions as object, Server.Host);
                }
            }

            public static void ListenClientData()
            {
                NetworkConnection networkConnection = NetworkConnection.Current;

                using (networkConnection.GetUdp(NetworkConnection.Ports.ClientListener))
                {
                    while (true)
                    {
                        UdpReceiveResult result = networkConnection.Receive().Result;
                        Task.Run(() => UpdateClientData(result));
                    }
                }
            }

            private static void UpdateClientData(UdpReceiveResult result)
            {
                object clientData = NetworkUtility.Deserialize(result.Buffer);
                if (clientData is ClientData)
                {
                    Data = clientData as ClientData;
                }
            }
        }
    }
}
