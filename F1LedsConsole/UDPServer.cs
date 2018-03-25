using System;
using System.Net;
using System.Net.Sockets;

namespace F1LedsConsole
{
    class UDPServer
    {
        private int PORT;
        private string HOST;
        private IPEndPoint ipep;
        private UdpClient newsock;
        private ConvertMethod OnPacketReceived;

        public UDPServer(int _port, string _host, ConvertMethod _OnPacketReceived)
        {
            PORT = _port;
            HOST = _host;
            OnPacketReceived = _OnPacketReceived;
            ipep = new IPEndPoint(IPAddress.Parse(HOST), PORT);
            newsock = new UdpClient(ipep);
        }

        public void ListenLoop()
        {
            while (true)
            {
                OnPacketReceived(newsock.Receive(ref ipep));
            }
        }
    }
}
