using System;
using System.Collections.Concurrent;
using System.Globalization;
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

        // Command related vars Start

        public static volatile bool switchScene = false, switchMap = false;
        public static volatile string newScene = "";
        public static volatile int newMap = 0;
        public static ConcurrentQueue<string> tickCommands = new ConcurrentQueue<string>();
        // Command related vars End

        public static void Init()
        {
            tcpServer = new TcpListener(IPAddress.Any, 8042); // TODO: Change the port if you want
            try
            {
                uiServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 8044); // Listen for the UI client locally (Port: 8044)
                uiServer.Start();
            } catch(Exception)
            {
                uiServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 8046); // Debug Port (Start with Admin!)
                uiServer.Start();
            }
            tcpClient = new TcpClient();

            uiTcpThread = new Thread(() => ListenUI());
            uiTcpThread.Start();
        }

        private static void ListenUI()
        {
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

        public static void WriteToUI(string content)
        {
            if (uiClient != null && uiClient.Connected)
            {
                uiWriter.Write(content);
                uiWriter.Flush();
            }
        }

        private static void ListenServer()
        {
            if (isServer)
            {
                tcpServer.Start();
                tcpClient = tcpServer.AcceptTcpClient(); // Wait for an opponent
                WriteToUI("SHOWSAND"); // Show the sandbox message box
            }

            using (NetworkStream nStream = tcpClient.GetStream()) // Get the stream
            {
                using (BinaryReader reader = new BinaryReader(nStream)) // Read it
                {
                    tcpWriter = new BinaryWriter(nStream); // Set the writer
                    WriteToOpponent("HELLO");

                    while (true) // Permanently try to read
                    {
                        string newData = reader.ReadString();
                        SetCulture();

                        if (isServer)
                        {

                        } else
                        {
                            if(newData.StartsWith("LOADSCENE") && false) // Disable loadscene for now
                            {
                                newScene = newData.Split('|')[1];
                                switchScene = true; // Tell the client to switch the scene
                            } else if (newData.StartsWith("LOADMAP"))
                            {
                                newMap = int.Parse(newData.Split('|')[1]);
                                switchMap = true; // Tell the client to switch the map
                            }
                        }
                        if (newData.StartsWith("SPAWNUNIT") || newData.StartsWith("REMOVEUNIT"))
                            tickCommands.Enqueue(newData);
                        // Add the command to be processed by the tick coroutine
                    }
                }
            }
        }

        private static void ConnectClient()
        {
            isServer = false;
            tcpServer.Stop();
            ListenServer();
        }

        public static void WriteToOpponent(string content)
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                tcpWriter.Write(content);
                tcpWriter.Flush();
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
        public static bool GetIsServer()
        {
            return isServer;
        }

        private static byte[] StrToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        private static string ByteToStr(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static void SetCulture() // Set the cultureinfo to one that uses dots instead of commas
        {
            CultureInfo ci = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
        }
    }
}