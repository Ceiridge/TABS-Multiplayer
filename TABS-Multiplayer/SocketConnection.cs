using System;
using System.Net;
using System.Net.Sockets;

namespace TABS_Multiplayer
{
    // The class for handling the socket between the players
    public class SocketConnection
    {
        private static TcpListener tcpServer;
        private static TcpClient tcpClient;

        private static bool inited = false; // Don't execute the init twice


        public static void Init()
        {
            if (inited) return;
            inited = true;

            tcpServer = new TcpListener(IPAddress.Any, 8042); // TODO: Change the port if you want
            tcpClient = new TcpClient();

            
        }

        public static TcpClient getTcpClient()
        {
            return tcpClient;
        }

        public static TcpListener getTcpServer()
        {
            return tcpServer;
        }
    }
}