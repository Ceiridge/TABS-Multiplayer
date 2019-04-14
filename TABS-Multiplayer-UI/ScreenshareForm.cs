using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            byte[] data;
            while (ScreenshotHandler.datas.TryTake(out data))
            {
                string strData = MainUI.ByteToStr(data);
                if (strData.StartsWith("IMG")) // If it's an image
                {
                    strData = strData.Split(new string[] { "|$|" }, StringSplitOptions.None)[0]; // Only get our header

                    if (ScreenshotHandler.winSize.Width == 0)
                    {
                        string[] sizeData = strData.Split('|')[2].Split(',');
                        ScreenshotHandler.winSize = new Size(int.Parse(sizeData[0]), int.Parse(sizeData[1]));
                    }
                    /*if(totalImage == null) 
                    {
                        string[] sizeData = strData.Split('|')[2].Split(',');
                        totalImage = new Bitmap(int.Parse(sizeData[0]), int.Parse(sizeData[1])); // Make a total image if it's missing
                    }*/

                    string[] pointData = strData.Replace("{X=", "").Replace("}", "").Replace("Y=", "").Split('|')[1].Split(',');

                    byte[] imgData = ScreenshotHandler.Decompress(ScreenshotHandler.GetImageData(data)); // Get the image bytes
                    Point loc = new Point(int.Parse(pointData[0]), int.Parse(pointData[1])); // Get the box location

                    MemoryStream imgMem = new MemoryStream(imgData);
                    Image boxImg = Image.FromStream(imgMem); // Get image from bytes

                    Bitmap boxBit = new Bitmap(boxImg);
                    if (!ScreenshotHandler.imageBlocks.ContainsKey(loc))
                        ScreenshotHandler.imageBlocks.Add(loc, boxBit);
                    else
                        ScreenshotHandler.imageBlocks[loc] = boxBit;
                    /*MainUI.screenshareForm.Invoke(() => {
                        /*int dW = (loc.X + boxImg.Width + 1) - totalImage.Width;
                        int dH = (loc.Y + boxImg.Height + 1) - totalImage.Height;

                        if (dW > 0) // Crop width if it's too big
                        {
                            Bitmap toCrop = new Bitmap(boxImg);
                            boxImg = toCrop.Clone(new Rectangle(0, 0, toCrop.Width - dW, toCrop.Height), toCrop.PixelFormat);
                            toCrop.Dispose();
                        }
                        if (dH > 0) // Crop height if it's too big
                        {
                            Bitmap toCrop = new Bitmap(boxImg);
                            boxImg = toCrop.Clone(new Rectangle(0, 0, toCrop.Width, toCrop.Height - dH), toCrop.PixelFormat);
                            toCrop.Dispose();
                        } // Old client-side cropping (useless)

                        using (Graphics g = Graphics.FromImage(totalImage)) // Draw the image on the totalImage
                        {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                            g.DrawImage(boxImg, loc);
                        }
                        boxImg.Dispose();
                    });*/

                    //imgMem.Dispose(); // Clear memory
                }
            }


            pictureBox1.Invalidate();
            pictureBox1.Refresh();
            pictureBox1.Update();
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
                    Bitmap img = ScreenshotHandler.imageBlocks[loc];
                    g.DrawImage(img, loc);
                }
                this.Text = "Imgboxes: " + ScreenshotHandler.imageBlocks.Count;
            }
        }
    }
}
