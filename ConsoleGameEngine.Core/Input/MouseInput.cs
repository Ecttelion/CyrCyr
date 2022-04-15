using ConsoleGameEngine.Core.Math;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ConsoleGameEngine.Core.Input
{
    public class MouseInput
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        public static Vector GetMousePosition()
        {
            IntPtr consoleHwnd = NativeConsole.GetConsoleWindow();
            var consoleLoca = DwmApi.GetExtendedFrameBounds(consoleHwnd);
            Point defPnt = new Point();
            GetCursorPos(ref defPnt);
            defPnt = new Point(defPnt.X - consoleLoca.Left, defPnt.Y - consoleLoca.Top);
            return new Vector(defPnt.X / 5 - 10, defPnt.Y / 8 - 10);
        }

    }
}
