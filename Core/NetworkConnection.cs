using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


using ClashOfTanks.Core.User;

namespace ClashOfTanks.Core
{
    public sealed class NetworkConnection
    {
        public enum Ports { ServerListener = 1000, ServerTalker = 1002, ClientListener = 1010, ClientTalker = 1012 }

        internal static NetworkConnection Current { get; private set; } = null;

        internal static NetworkConnection Setup()
        {
            return Current = new NetworkConnection();
        }

        internal UdpClient GetUdp(Ports port)
        {
            UdpClient client = new UdpClient((int)port);

            if (port == Ports.ServerListener || port == Ports.ClientListener)
            {
                UdpReceiver = client;
            }
            else
            {
                UdpSender = client;
            }

            return client;
        }

        internal UdpClient UdpSender { get; set; } = null;
        internal UdpClient UdpReceiver { get; set; } = null;

        internal async Task Send(string data, Player player)
        {
            await Send(NetworkUtility.Encode(data), player);
        }

        internal async Task Send(string data, List<Player> players)
        {
            await Send(NetworkUtility.Encode(data), players);
        }

        internal async Task Send(object data, Player player)
        {
            await Send(NetworkUtility.Serialize(data), player);
        }

        internal async Task Send(object data, List<Player> players)
        {
            await Send(NetworkUtility.Serialize(data), players);
        }

        private async Task Send(byte[] data, Player player)
        {
            if (player.IPTo != null)
            {
                await UdpSender.SendAsync(data, data.Length, player.IPTo);
            }
        }

        private async Task Send(byte[] data, List<Player> players)
        {
            foreach (Player player in players)
            {
                await Send(data, player);
            }
        }

        internal async Task<UdpReceiveResult> Receive()
        {
            return await UdpReceiver.ReceiveAsync();
        }
    }
}
