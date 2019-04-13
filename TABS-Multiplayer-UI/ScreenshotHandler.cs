using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace TABS_Multiplayer_UI
{
    class ScreenshotHandler
    {
        public static IntPtr unityWindow = IntPtr.Zero;
        public static bool streaming = true; // Debug

        private const int FPS = 24;
        private List<Image> imageBlocks = new List<Image>();

        public static Image GetImageFromWindow()
        {
            IntPtr wDC = WinAPIs.GetWindowDC(unityWindow); // Device Context 

            WinAPIs.RECT winSize = new WinAPIs.RECT();
            WinAPIs.GetWindowRect(unityWindow, ref winSize); // Get the size of the window
            WinAPIs.RECT clientSize = new WinAPIs.RECT();
            WinAPIs.GetClientRect(unityWindow, out clientSize); // Get the client bounds for cropping later

            int width = winSize.right - winSize.left;
            int height = winSize.bottom - winSize.top;
            int clientWidth = clientSize.right - clientSize.left;
            int clientHeight = clientSize.bottom - clientSize.top;

            int winBorderSize = (width - clientWidth) / 2;
            int titleBorderSize = (height - clientHeight) - winBorderSize;

            IntPtr newDest = WinAPIs.CreateCompatibleDC(wDC); // New device context
            IntPtr newBitmap = WinAPIs.CreateCompatibleBitmap(wDC, width, height); // New bitmap for the image

            IntPtr bitmapObj = WinAPIs.SelectObject(newDest, newBitmap);
            WinAPIs.BitBlt(newDest, 0, 0, width, height, wDC, 0, 0, WinAPIs.SRC_COPY); // Copy the exact image from these bounds
            WinAPIs.SelectObject(newDest, newBitmap); // Reset the selection

            WinAPIs.DeleteDC(newDest);
            WinAPIs.ReleaseDC(unityWindow, wDC); // Clean up memeory

            Bitmap cropImage = Image.FromHbitmap(newBitmap); // Convert into a bitmap
            WinAPIs.DeleteObject(newBitmap); // More cleaning

            Bitmap cropped = cropImage.Clone(new Rectangle(winBorderSize, titleBorderSize, cropImage.Width - 2 * winBorderSize,
                cropImage.Height - titleBorderSize - winBorderSize), PixelFormat.Format24bppRgb); // Remove the window borders

            cropImage.Dispose(); // Prevent a memleak
            return cropped;
        }

        public static void FramingThread()
        {
            while(true) // Make screenshots forever >:)
            {
                if(streaming & unityWindow != IntPtr.Zero)
                {
                    Image gameScreen = GetImageFromWindow();
                    gameScreen = JpegCompression(gameScreen, 50);
                    //gameScreen.Save("screenshot.jpg"); // Debug
                }
                Thread.Sleep(1000 / FPS);
            }
        }

        private static Image JpegCompression(Image img, int quality)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == "image/jpeg")
                {
                    ici = codec;
                    break;
                }
            }

            EncoderParameters ep = new EncoderParameters();
            ep.Param[0] = new EncoderParameter(Encoder.Quality, (long)quality);
            MemoryStream memStream = new MemoryStream();
            img.Save(memStream, ici, ep);
            img.Dispose();

            Image newImg = Image.FromStream(memStream);
            //memStream.Dispose();
            return newImg;
        }
    }

    class WinAPIs // A class for the access of some Windows APIs
    {
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        } // Copied from pinvoke

        public const int SRC_COPY = 0x00CC0020; // A parameter for BitBlt
    }
}