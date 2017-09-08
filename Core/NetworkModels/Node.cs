using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfTanks.Core.NetworkModels
{
    sealed class Node
    {
        public UdpClient ServerUdpTalker { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        public Node(UdpClient serverUdpTalker, IPEndPoint endPoint)
        {
            ServerUdpTalker = serverUdpTalker;
            EndPoint = endPoint;
        }
    }
}
