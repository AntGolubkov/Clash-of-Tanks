using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfTanks.Core.NetworkModels
{
    public sealed class Network
    {
        public enum Sides
        {
            Client,
            Server
        }

        public enum Ports
        {
            ClientTalker = 1000,
            ClientListener = 1001,
            ServerTalker = 1002,
            ServerListener = 1003
        }

        public static Network Current { get; set; } = null;

        private Sides Side { get; set; }

        internal static Node Server { get; set; } = null;

        private List<UdpClient> UdpTalkers { get; } = new List<UdpClient>();
        private List<UdpClient> UdpListeners { get; } = new List<UdpClient>();

        public Network(Sides side)
        {
            Side = side;

            foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()).Distinct())
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    switch (Side)
                    {
                        case Sides.Client:
                            {
                                UdpTalkers.Add(new UdpClient(new IPEndPoint(address, (int)Ports.ClientTalker)));
                                UdpListeners.Add(new UdpClient(new IPEndPoint(address, (int)Ports.ClientListener)));
                                break;
                            }
                        case Sides.Server:
                            {
                                UdpTalkers.Add(new UdpClient(new IPEndPoint(address, (int)Ports.ServerTalker)));
                                UdpListeners.Add(new UdpClient(new IPEndPoint(address, (int)Ports.ServerListener)));
                                break;
                            }
                    }
                }
            }
        }

        public void Send(byte[] data, UdpClient clientUdp, IPEndPoint clientEndPoint)
        {
            clientUdp.Send(data, data.Length, clientEndPoint);
        }

        public void SendBroadcast(byte[] data)
        {
            foreach (UdpClient udp in UdpTalkers)
            {
                int port = -1;

                switch (Side)
                {
                    case Sides.Client:
                        {
                            port = (int)Ports.ServerListener;
                            break;
                        }
                    case Sides.Server:
                        {
                            port = (int)Ports.ClientListener;
                            break;
                        }
                }

                Send(data, udp, new IPEndPoint(IPAddress.Broadcast, port));
            }
        }

        public byte[] Receive(UdpClient Udp, ref IPEndPoint clientEndPoint)
        {
            return Udp.Receive(ref clientEndPoint);
        }

        public byte[] ReceiveBroadcast(ref string ipAddress)
        {
            foreach (UdpClient udp in UdpListeners)
            {
                if (udp.Available > 0)
                {
                    int port = -1;

                    switch (Side)
                    {
                        case Sides.Client:
                            {
                                port = (int)Ports.ServerTalker;
                                break;
                            }
                        case Sides.Server:
                            {
                                port = (int)Ports.ClientTalker;
                                break;
                            }
                    }

                    byte[] data;
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                    data = Receive(udp, ref endPoint);
                    ipAddress = endPoint.Address.ToString();
                    return data;
                }
            }

            return null;
        }
    }
}
