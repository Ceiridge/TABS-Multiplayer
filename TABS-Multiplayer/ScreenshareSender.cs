using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace TABS_Multiplayer
{
    class ScreenshareSender
    {
        public static IntPtr unityWindow; // Handle to the window

        public static Image GetImageFromWindow()
        {
            IntPtr wDC = WinAPIs.GetWindowDC(unityWindow); // Device Context 

            WinAPIs.RECT winSize = new WinAPIs.RECT();
            WinAPIs.GetWindowRect(unityWindow, ref winSize); // Get the size of the window
            int width = winSize.right - winSize.left;
            int height = winSize.bottom - winSize.top;

            IntPtr newDest = WinAPIs.CreateCompatibleDC(wDC); // New device context
            IntPtr newBitmap = WinAPIs.CreateCompatibleBitmap(wDC, width, height); // New bitmap for the image

            IntPtr bitmapObj = WinAPIs.SelectObject(newDest, newBitmap);
            WinAPIs.BitBlt(newDest, 0, 0, width, height, wDC, 0, 0, WinAPIs.SRC_COPY); // Copy the exact image from these bounds
            WinAPIs.SelectObject(newDest, newBitmap); // Reset the selection

            WinAPIs.DeleteDC(newDest);
            WinAPIs.ReleaseDC(unityWindow, wDC); // Clean up memeory

            Image image = Image.FromHbitmap(newBitmap); // Convert into System.Drawing.Image
            WinAPIs.DeleteObject(newBitmap); // More cleaning
            return image;
        }

        public static bool SetWinHandle() // Set the unity window handle
        {
            if (unityWindow != null) return false; // Cancel if it's already set
            const int maxChars = 35; // TABS' name is about 33 chars long
            StringBuilder winTextBuilder = new StringBuilder(maxChars);

            IntPtr winPtr = WinAPIs.GetActiveWindow();
            WinAPIs.GetWindowText(winPtr, winTextBuilder, maxChars); // Get the title of the active window

            if (winTextBuilder.ToString() == "Totally Accurate Battle Simulator")
            {
                unityWindow = winPtr; // Set the handle
                return true;
            }
            else return false;
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
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

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
