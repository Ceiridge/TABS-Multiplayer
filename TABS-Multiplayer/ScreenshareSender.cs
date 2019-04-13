using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TABS_Multiplayer
{
    class ScreenshareSender
    {
        public static IntPtr unityWindow = IntPtr.Zero; // Handle to the window

        public static bool SetWinHandle() // Set the unity window handle
        {
            if (unityWindow != IntPtr.Zero) return false; // Cancel if it's already set
            const int maxChars = 35; // TABS' name is about 33 chars long
            StringBuilder winTextBuilder = new StringBuilder(maxChars);

            IntPtr winPtr = WinAPIs.GetForegroundWindow();
            WinAPIs.GetWindowText(winPtr, winTextBuilder, maxChars); // Get the title of the active window
            string winTitle = winTextBuilder.ToString();

            if (winTitle == "Totally Accurate Battle Simulator")
            {
                unityWindow = winPtr; // Set the handle
                SocketConnection.WriteToUI("WINH|" + unityWindow.ToInt64()); // Send the handle
                return true;
            }
            else return false;
        }
    }

    class WinAPIs // A class for the access of some Windows APIs
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount); // Copied from pinvoke
    }
}
