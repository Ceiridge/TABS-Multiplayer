using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TABS_Multiplayer_UI
{
    public partial class MainUI : Form
    {
        public static bool DEBUG = true; // Debug Mode
        public static TcpClient tcp;

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

            tcp = new TcpClient("localhost", 8043); // Connect to a custom TABS socket with the hardcoded port 8043
        }

        private void hostBtn_Click(object sender, EventArgs e) // Host Button
        {

        }

        private void connectBtn_Click(object sender, EventArgs e) // Connect Button
        {

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
    }
}
