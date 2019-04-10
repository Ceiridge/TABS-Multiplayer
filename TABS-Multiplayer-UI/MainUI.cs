using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TABS_Multiplayer_UI
{
    public partial class MainUI : Form
    {
        public static bool DEBUG = true; // Debug Mode
        public static TcpClient tcp;
        public static BinaryWriter uiWriter;

        ScreenshareForm screenshareForm;

        public MainUI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!DEBUG)
            {
                waitPanel.Dock = DockStyle.Fill; // Make waitPanel visible on start
                waitPanel.BringToFront();
            }

            screenshareForm = new ScreenshareForm();
            screenshareForm.Show();

            Thread.Sleep(1500); // Sleep to make sure the socket's started
            tcp = new TcpClient("localhost", 8044); // Connect to a custom TABS socket with the hardcoded port 8044
        }

        private void hostBtn_Click(object sender, EventArgs e) // Host Button
        {
            WriteToTABS(StrToByte("HOSTNOW")); // Send the host command
            button1.Enabled = false; // Disable the host btn
            button2.Enabled = false; // Disable the connection btn
        }

        private void connectBtn_Click(object sender, EventArgs e) // Connect Button
        {
            try
            {
                WriteToTABS(StrToByte("CONNECT|" + IPAddress.Parse(textBox1.Text).ToString())); // Send connect cmd + ip with parse checking
                button1.Enabled = false; // Disable the host btn
                button2.Enabled = false; // Disable the connection btn
            } catch(Exception)
            {
                MessageBox.Show("Invalid IP!", "Error");
            }
        }

        private void tcpWaiter_Tick(object sender, EventArgs e)
        {
            if(tcp.Connected) // If connected to TABS
            {
                waitPanel.Visible = false;
            } else if(!waitPanel.Visible)
            {
                this.Close(); // Exit this UI after a disconnect of TABS
            }
        }

        private void TCPReceiver()
        {
            using (NetworkStream nStream = tcp.GetStream()) // Get the stream
            {
                using (BinaryReader reader = new BinaryReader(nStream)) // Read it
                {
                    uiWriter = new BinaryWriter(nStream); // Set the writer

                    while (true) // Permanently try to read
                    {
                        string newData = reader.ReadString();

                        if(newData.Equals("SHOWSAND"))
                        {
                            Invoke(() => { MessageBox.Show("You can now start the Sandbox", "Connected!"); });
                        }
                    }
                }
            }
        }

        private void WriteToTABS(byte[] content)
        {
            if (tcp.Connected)
            {
                uiWriter.Write(content);
            }
        }

        private static byte[] StrToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        private static string ByteToStr(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public void Invoke(Action action)
        {
            this.Invoke((Delegate)action);
        }
    }
}
