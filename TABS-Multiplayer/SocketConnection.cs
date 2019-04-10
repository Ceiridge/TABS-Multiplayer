using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TABS_Multiplayer
{
    // The class for handling the socket between the players and the ui locally
    public class SocketConnection
    {
        private static TcpListener tcpServer, uiServer;
        private static TcpClient tcpClient;
        private static Thread uiTcpThread;

        public static void Init()
        {
            tcpServer = new TcpListener(IPAddress.Any, 8042); // TODO: Change the port if you want
            uiServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 8043); // Listen for the UI client locally (Port: 8043)
            tcpClient = new TcpClient();

            uiTcpThread = new Thread(() => ListenUI());
            uiTcpThread.Start();
        }

        private static void ListenUI()
        {
            TcpClient uiClient = uiServer.AcceptTcpClient(); // Wait and accept ui client
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