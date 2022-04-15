using System;
using System.Runtime.InteropServices;

namespace ConsoleGameEngine.Core
{
    [StructLayout(LayoutKind.Sequential)]
    struct RectStruct
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public int Width => Right - Left;
        public int Height => Bottom - Top;
    }

    class NativeConsole
    {
        [DllImport("kernel32")]
        public static extern IntPtr GetConsoleWindow();
    }

    class Winuser
    {
        [DllImport(@"user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RectStruct lpRect);

        public static RectStruct GetWindowRect(IntPtr handle)
        {
            if (!GetWindowRect(handle, out RectStruct rect))
            {
                throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());
            }

            return rect;
        }
    }

    class DwmApi
    {
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;

        [DllImport(@"dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RectStruct pvAttribute, int cbAttribute);

        public static RectStruct GetExtendedFrameBounds(IntPtr hwnd)
        {
            int hresult = DwmGetWindowAttribute(hwnd, DWMWA_EXTENDED_FRAME_BOUNDS, out RectStruct rect, Marshal.SizeOf(typeof(RectStruct)));

            if (hresult != 0)
            {
                throw Marshal.GetExceptionForHR(hresult);
            }

            return rect;
        }
    }

}
