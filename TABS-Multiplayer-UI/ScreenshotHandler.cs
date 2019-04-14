using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace TABS_Multiplayer_UI
{
    class ScreenshotHandler
    {
        public static IntPtr unityWindow = IntPtr.Zero;
        public static bool streaming = false;
        public const int FPS = 24;

        public static ConcurrentBag<byte[]> datas = new ConcurrentBag<byte[]>();
        public static Dictionary<Point, Bitmap> imageBlocks = new Dictionary<Point, Bitmap>();
        private static Dictionary<Point, Bitmap> hostimageBlocks = new Dictionary<Point, Bitmap>();
        public static Size winSize;

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
            MainUI.SetCulture();
            while(true) // Make screenshots forever >:)
            {
                long millis = DateTimeOffset.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if (streaming & unityWindow != IntPtr.Zero && MainUI.isHost && MainUI.screenPartner != null)
                {
                    Image gameScreen = GetImageFromWindow();
                    UpdateChanges(gameScreen);
                }
                long dMillis = (DateTimeOffset.Now.Ticks / TimeSpan.TicksPerMillisecond) - millis;
                Thread.Sleep(Math.Max(1, (1000 / (FPS * 2)) - (int)dMillis));
            }
        }

        private static void UpdateChanges(Image gameScreen)
        {
            int width = gameScreen.Width;
            int height = gameScreen.Height; // Get the bound
            Bitmap gameScreenBitMap = new Bitmap(gameScreen);

            for (int x = 0; x <= (width / 50); x++)
            {
                for(int y = 0; y <= (height / 50); y++) // 50x50 block image
                {
                    Point loc = new Point(x * 50, y * 50);

                    int dW = (loc.X + 50) - gameScreenBitMap.Width; // Prevent cropping out of bounds
                    int dH = (loc.Y + 50) - gameScreenBitMap.Height;
                    int toRemW = dW > 0 ? dW : 0;
                    int toRemH = dH > 0 ? dH : 0;

                    Bitmap part = gameScreenBitMap.Clone(new Rectangle(loc.X, loc.Y, 50 - toRemW, 50 - toRemH), gameScreenBitMap.PixelFormat);
                    bool hasChanged = false;

                    if(!hostimageBlocks.ContainsKey(loc))
                    {
                        hostimageBlocks.Add(loc, part); // Add the image if it's not there yet
                        hasChanged = true;
                    } else
                    {
                        int diffPixs = GetDifferentPixels(hostimageBlocks[loc], part);
                        if(diffPixs > 3) // If 3 pixels have changed of the block
                        {
                            hostimageBlocks[loc] = part;
                            hasChanged = true;
                        }
                    }

                    if (hasChanged)
                    {
                        MemoryStream buffer = new MemoryStream();
                        StreamWriter bufferWriter = new StreamWriter(buffer);
                        bufferWriter.Write("IMG|" + loc.ToString() +
                            "|" + width + "," + height + "|$|"); // Init the image first and send screen bounds

                        MemoryStream imgBuffer = new MemoryStream();
                        object[] parameters = JpegCompression(part, 50); // 50% Jpeg Quality
                        part.Save(imgBuffer, (ImageCodecInfo) parameters[0], (EncoderParameters) parameters[1]); // Write to the image buffer with jpeg compression

                        byte[] imgData = Compress(imgBuffer.ToArray());
                        bufferWriter.Flush();
                        buffer.Write(imgData, 0, imgData.Length); // Write image bytes to buffer
                        bufferWriter.Close();

                        WriteToUdp(buffer.ToArray());
                        buffer.Dispose();
                        imgBuffer.Dispose();
                    }
                }
            }
        }

        public static void UdpThread()
        {
            MainUI.SetCulture();
            while (true)
            {
                var listenStuff = !MainUI.isHost ? MainUI.screenPartner : new IPEndPoint(IPAddress.Any, 8042);
                byte[] data = MainUI.screenClient.Receive(ref listenStuff);
                if(MainUI.screenPartner == null)
                    MainUI.screenPartner = listenStuff;

                if(data.Length > 4 && !MainUI.isHost)
                {
                    datas.Add(data);
                }
            }
        }

        public static void WriteToUdp(byte[] data)
        {
            if (MainUI.screenPartner != null)
                if(MainUI.isHost)
                    MainUI.screenClient.Send(data, data.Length, MainUI.screenPartner); // Send the data to the partner
                else
                    MainUI.screenClient.Send(data, data.Length); // Send the data to the partner
        }

        public static byte[] GetImageData(byte[] data) // Split the byte array at |$|
        {
            int biglines = 0, dollars = 0;

            byte bigLine = MainUI.StrToByte("|")[0];
            byte dollar = MainUI.StrToByte("$")[0];
            bool allowWriting = false;

            MemoryStream memStream = new MemoryStream();

            for(int i = 0; i < data.Length; i++)
            {
                if (allowWriting)
                {
                    memStream.WriteByte(data[i]);
                    continue;
                }

                if (data[i] == bigLine)
                {
                    biglines++;
                    if(dollars == 1 && biglines > 1)
                    {
                        allowWriting = true;
                    }
                } else if(data[i] == dollar)
                {
                    dollars++;
                }
            }
            return memStream.ToArray();
        }
        public static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    zipStream.Write(data, 0, data.Length);
                    zipStream.Close();
                    return compressedStream.ToArray();
                }
            }
        }
        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }

        public static int GetDifferentPixels(Bitmap orig, Bitmap img)
        {
            if (orig.Width != img.Width || orig.Height != img.Height) // Only compare if the bounds are the same
                return -1;

            int differentPixels = 0;
            for(int x = 0; x < orig.Width; x++)
            {
                for(int y = 0; y < orig.Height; y++) // Iterate through every pixel
                {
                    Color origC = orig.GetPixel(x, y);
                    Color imgC = img.GetPixel(x, y);

                    int dR = Math.Abs(origC.R - imgC.R);
                    int dG = Math.Abs(origC.G - imgC.G);
                    int dB = Math.Abs(origC.B - imgC.B);

                    if ((dR + dG + dB) > 15) // If the pixel has a different color (15 rgb values away)
                    {
                        differentPixels++;
                    }
                }
            }

            return differentPixels;
        }
        private static object[] JpegCompression(Image img, int quality)
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
            return new object[] { ici, ep };
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
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


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