using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TABS_Multiplayer_UI
{
    public partial class MainUI : Form
    {
        static bool IsElevated => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        public static bool DEBUG = true; // Debug Mode
        public static TcpClient tcp;
        public static BinaryWriter uiWriter;
        public static Thread tcpThread;
        public static MainUI instance;

        private static bool isIngame = false;

        ScreenshareForm screenshareForm;

        public MainUI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            instance = this;
            if (!DEBUG)
            {
                waitPanel.Dock = DockStyle.Fill; // Make waitPanel visible on start
                waitPanel.BringToFront();
            }

            screenshareForm = new ScreenshareForm();
            screenshareForm.Show();

            Thread.Sleep(1500); // Sleep to make sure the socket's started
            tcp = new TcpClient("localhost", IsElevated ? 8046 : 8044); // Connect to a custom TABS socket with the hardcoded port 8044/8046: DEBUG PORT START WITH ADMIN!!!
            tcpThread = new Thread(() => TCPReceiver());
            tcpThread.Start();
        }

        private void hostBtn_Click(object sender, EventArgs e) // Host Button
        {
            WriteToTABS("HOSTNOW"); // Send the host command
            button1.Enabled = false; // Disable the host btn
            button2.Enabled = false; // Disable the connection btn
        }

        private void connectBtn_Click(object sender, EventArgs e) // Connect Button
        {
            try
            {
                WriteToTABS("CONNECT|" + IPAddress.Parse(textBox1.Text).ToString()); // Send connect cmd + ip with parse checking
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
                Environment.Exit(0); // Exit this UI after a disconnect of TABS
            }
        }

        private static void TCPReceiver()
        {
            using (NetworkStream nStream = tcp.GetStream()) // Get the stream
            {
                using (BinaryReader reader = new BinaryReader(nStream)) // Read it
                {
                    uiWriter = new BinaryWriter(nStream); // Set the writer

                    while (true) // Permanently try to read
                    {
                        string newData = reader.ReadString(); // Unbelievably messy code for receiving commands (somebody else can improve it :)) )

                        if (newData.Equals("SHOWSAND"))
                        {
                            instance.Invoke(() => { MessageBox.Show("You can now start the Sandbox", "Connected!"); });
                        } else if (newData.StartsWith("SHOWMSG"))
                        {
                            instance.Invoke(() => { MessageBox.Show(newData.Split('|')[1], "Message from TABS"); });
                        }
                        else if (newData.StartsWith("INGAME"))
                        {
                            isIngame = bool.Parse(newData.Split('|')[1]); // Change the ingame status
                        }
                    }
                }
            }
        }

        private void WriteToTABS(string content)
        {
            if (tcp.Connected)
            {
                uiWriter.Write(content);
                uiWriter.Flush();
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
