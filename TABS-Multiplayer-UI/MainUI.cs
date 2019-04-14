using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
        static bool IsElevated => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator); // Used to detect if it's started with admin rights (for modders in order to start two instances)
        public static bool DEBUG = false; // Debug Mode
        public static TcpClient tcp;
        public static UdpClient screenClient; public static IPEndPoint screenPartner;
        public static BinaryWriter uiWriter;
        public static Thread tcpThread;
        public static MainUI instance;

        public static bool isIngame = true, isHost = false;

        public static ScreenshareForm screenshareForm;

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

            tcp = new TcpClient("localhost", IsElevated ? 8046 : 8044); // Connect to a custom TABS socket with the hardcoded port 8044/8046: DEBUG PORT START WITH ADMIN!!!
            tcpThread = new Thread(() => TCPReceiver());
            tcpThread.Start();

            try
            {
                screenClient = new UdpClient(8042);
            }
            catch (Exception)
            {
                screenClient = new UdpClient(); // Make a normal udp client if it's already listening in another instance (for dev purposes)
            }
        }

        private void hostBtn_Click(object sender, EventArgs e) // Host Button
        {
            if(isIngame)
            {
                MessageBox.Show("Please stay in the main menu before starting a session!", "Error"); // Prevent the user from hosting/connecting while being ingame
                return;
            }
            WriteToTABS("HOSTNOW"); // Send the host command
            button1.Enabled = false; // Disable the host btn
            button2.Enabled = false; // Disable the connection btn
            isHost = true;
            new Thread(() => ScreenshotHandler.UdpThread()).Start(); // Start the udp listener
        }

        private void connectBtn_Click(object sender, EventArgs e) // Connect Button
        {
            if (isIngame)
            {
                MessageBox.Show("Please stay in/enter the main menu before starting a session!", "Error");
                return;
            }
            try
            {
                WriteToTABS("CONNECT|" + IPAddress.Parse(textBox1.Text).ToString()); // Send connect cmd + ip with parse checking
                button1.Enabled = false; // Disable the host btn
                button2.Enabled = false; // Disable the connection btn

                screenPartner = new IPEndPoint(IPAddress.Parse(textBox1.Text), 8042); // Connect to the other screenClient
                screenClient.Connect(screenPartner);
                new Thread(() => ScreenshotHandler.UdpThread()).Start(); // Start the udp listener
                ScreenshotHandler.WriteToUdp(StrToByte("HELLO")); // Send hello
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
                        else if (newData.StartsWith("DEBUG"))
                        {
                            Console.WriteLine(newData.Split('|')[1]); // Print out debug text
                        }
                        else if (newData.StartsWith("WINH"))
                        {
                            ScreenshotHandler.unityWindow = new IntPtr(long.Parse(newData.Split('|')[1])); // Set the handle
                            new Thread(() => ScreenshotHandler.FramingThread()).Start(); // Start the framing thread
                        }
                        else if (newData.StartsWith("GSTARTED"))
                        {
                            bool started = bool.Parse(newData.Split('|')[1]); // Is game started?
                            if (!isIngame) continue; // Continue if the player's in the main menu
                            if (!isHost)
                            {
                                if (started)
                                {
                                    screenshareForm.ArrangeWindow();
                                    screenshareForm.Visible = true;
                                    WinAPIs.SetForegroundWindow(screenshareForm.Handle);
                                    // Arrange and make the screenshare window ready for receiving
                                } else
                                {
                                    screenshareForm.Visible = false;
                                    WinAPIs.SetForegroundWindow(ScreenshotHandler.unityWindow); // Hide the screenshare form
                                    // and set the game as the active one again
                                }
                            }
                            ScreenshotHandler.streaming = started;
                        }
                    }
                }
            }
        }

        

        public static void WriteToTABS(string content)
        {
            if (tcp.Connected)
            {
                uiWriter.Write(content);
                uiWriter.Flush();
            }
        }

        public static byte[] StrToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string ByteToStr(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public void Invoke(Action action)
        {
            this.Invoke((Delegate)action);
        }

        private void budgetBtn_Click(object sender, EventArgs e)
        {
            if(isHost)
            {
                WriteToTABS("BUDGET|" + (int)budgetVal.Value);
            } else
            {
                MessageBox.Show("You are not the host!", "Error");
            }
        }

        public static void SetCulture() // Set the cultureinfo to one that uses dots instead of commas
        {
            CultureInfo ci = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
        }

    }
}
