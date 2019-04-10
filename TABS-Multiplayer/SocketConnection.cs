using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TABS_Multiplayer
{
    // The class for handling the socket between the players and the ui locally
    public class SocketConnection
    {
        private static TcpListener tcpServer, uiServer;
        private static TcpClient tcpClient, uiClient;
        private static Thread uiTcpThread, tcpThread;
        private static BinaryWriter tcpWriter, uiWriter;
        private static bool isServer = true; // Used to identify the status

        public static void Init()
        {
            tcpServer = new TcpListener(IPAddress.Any, 8042); // TODO: Change the port if you want
            uiServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 8044); // Listen for the UI client locally (Port: 8044)
            tcpClient = new TcpClient();

            uiTcpThread = new Thread(() => ListenUI());
            uiTcpThread.Start();
        }

        private static void ListenUI()
        {
            uiServer.Start();
            uiClient = uiServer.AcceptTcpClient(); // Wait and accept ui client

            using(NetworkStream nStream = uiClient.GetStream()) // Get the stream
            {
                using (BinaryReader reader = new BinaryReader(nStream)) // Read it
                {
                    uiWriter = new BinaryWriter(nStream); // Set the writer

                    while(true) // Permanently try to read
                    {
                        string newData = reader.ReadString();

                        if(newData.StartsWith("HOSTNOW")) // Unbelievably messy code for receiving commands (somebody else can improve it :)) )
                        {
                            tcpThread = new Thread(() => ListenServer());
                            tcpThread.Start();
                        } else if(newData.StartsWith("CONNECT|"))
                        {
                            tcpClient.Connect(newData.Split('|')[1], 8042); // Connect to ip with hardcoded port
                            tcpThread = new Thread(() => ConnectClient());
                            tcpThread.Start();
                        }
                    }
                }
            }
        }

        private void WriteToUI(byte[] content)
        {
            if (uiClient.Connected)
            {
                uiWriter.Write(content);
            }
        }

        private void ListenServer()
        {
            tcpServer.Start();
            tcpClient = tcpServer.AcceptTcpClient(); // Wait for an opponent

            WriteToUI(StrToByte("SHOWSAND"));
            using (NetworkStream nStream = uiClient.GetStream()) // Get the stream
            {
                using (BinaryReader reader = new BinaryReader(nStream)) // Read it
                {
                    tcpWriter = new BinaryWriter(nStream); // Set the writer

                    while (true) // Permanently try to read
                    {
                        string newData = reader.ReadString();

                        if(isServer)
                        {

                        } else
                        {

                        }
                    }
                }
            }
        }

        private void ConnectClient()
        {
            ListenServer();
            tcpServer.Stop();
            isServer = false;
        }

        private void WriteToOpponent(byte[] content)
        {
            if (tcpClient.Connected)
            {
                tcpWriter.Write(content);
            }
        }

        public static TcpClient getTcpClient()
        {
            return tcpClient;
        }

        public static TcpListener getTcpServer()
        {
            return tcpServer;
        }

        private static byte[] StrToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        private static string ByteToStr(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}