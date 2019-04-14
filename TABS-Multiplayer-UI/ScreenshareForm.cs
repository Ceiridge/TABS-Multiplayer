using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TABS_Multiplayer_UI
{
    public partial class ScreenshareForm : Form
    {
        private bool initImg = false;

        public ScreenshareForm()
        {
            InitializeComponent();
        }

        protected override bool ShowWithoutActivation => true; // Don't steal the focus

        private void ScreenshareForm_Load(object sender, EventArgs e)
        {
            imageUpdater.Interval = 1000 / ScreenshotHandler.FPS;
        }

        private void ScreenshareForm_Shown(object sender, EventArgs e)
        {
            this.Visible = false; // Hide this window for now
        }

        private void ScreenshareForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // The UI can only be closed by the main window
        }

        private void imageUpdater_Tick(object sender, EventArgs e)
        {
            if(!initImg && ScreenshotHandler.winSize.Width > 0) {
                initImg = true;
                pictureBox1.Image = new Bitmap(ScreenshotHandler.winSize.Width, ScreenshotHandler.winSize.Height);
            }

            pictureBox1.Invalidate();
        }

        public void ArrangeWindow()
        {
            WinAPIs.RECT winSize = new WinAPIs.RECT();
            WinAPIs.GetWindowRect(ScreenshotHandler.unityWindow, ref winSize); // Get the size of the window

            this.Location = new Point(winSize.left, winSize.top);
            this.Size = new Size(winSize.right - winSize.left, winSize.bottom - winSize.top); // Set size and location
        }

        public void Invoke(Action action)
        {
            this.Invoke((Delegate)action);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (initImg && this.Visible)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                foreach (Point loc in ScreenshotHandler.imageBlocks.Keys)
                {
                    Bitmap img;
                    if(ScreenshotHandler.imageBlocks.TryGetValue(loc, out img))
                        g.DrawImage(img, loc);
                }
            }
        }
    }
}
