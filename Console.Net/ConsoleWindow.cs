using System.Runtime.InteropServices;
using System;
using System.Drawing;
using System.Diagnostics;

namespace Console.Net
{
    public class ConsoleWindow : ConsoleCore
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;        
            public int Top;         
            public int Right;       
            public int Bottom;      
        }

        [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern int GetSystemMetrics(int nIndex);

        /// <summary> Получить размер указаного хэндла окна  </summary>
        /// <param name="window">хэндл окна</param>
        private static Size GetWindowSize(IntPtr window)
        {
            if (!GetWindowRect(new HandleRef(null, window), out Rect rect))
                throw new Exception("Unable to get window rect!");

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            return new Size(width, height);
        }

        /// <summary> Установить размер окна </summary>
        /// <param name="width"> Ширина </param>
        /// <param name="height"> Высота </param>
        public static void WindowSize(int width, int height)
        {
            System.Console.SetWindowSize(width,height);
            //System.Console.SetBufferSize(width-1,height-1);
        }

        private static Size GetScreenSize() => new Size(GetSystemMetrics(0), GetSystemMetrics(1));
    
        /// <summary>
        /// Расположить окно по центру экрана
        /// </summary>
        public static void WindowCenter()
        {
            IntPtr window = Process.GetCurrentProcess().MainWindowHandle;
            if (window == IntPtr.Zero)
                throw new Exception("Couldn't find a window to center!");
            Size screenSize = GetScreenSize();
            Size windowSize = GetWindowSize(window);
            int x = (screenSize.Width - windowSize.Width) / 2;
            int y = (screenSize.Height - windowSize.Height) / 2;
            SetWindowPos(window, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
        }
    }
}
