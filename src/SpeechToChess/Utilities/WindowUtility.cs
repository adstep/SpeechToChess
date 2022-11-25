using System.Runtime.InteropServices;

namespace SpeechToChess.Utilities
{
    public static class WindowUtility
    {
        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static IntPtr FindWindow(string windowName, bool wait)
        {
            IntPtr hWnd = FindWindowByCaption(IntPtr.Zero, windowName);
            while (wait && hWnd == 0)
            {
                System.Threading.Thread.Sleep(500);
                hWnd = FindWindowByCaption(IntPtr.Zero, windowName);
            }

            return hWnd;
        }

        public static bool Maximize(string windowName)
        {
            IntPtr hWnd = FindWindow(windowName, false);

            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, SW_MAXIMIZE);
                return true;
            }

            return false;
        }

        
    }
}
