using System;
using System.Runtime.InteropServices;

namespace Chess
{
    internal class Font
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern Int32 SetCurrentConsoleFontEx(IntPtr ConsoleOutput, bool MaximumWindow, ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx);
        private enum StdHandle { OutputHandle = -11 }

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(StdHandle index);

        public static void SetFont(string name, short size)
        {
            CONSOLE_FONT_INFO_EX ConsoleFontInfo = new CONSOLE_FONT_INFO_EX();
            ConsoleFontInfo.cbSize = (uint)Marshal.SizeOf(ConsoleFontInfo);
            ConsoleFontInfo.FaceName = name;
            ConsoleFontInfo.dwFontSize.X = (short)(size / 2);
            ConsoleFontInfo.dwFontSize.Y = size;
            SetCurrentConsoleFontEx(GetStdHandle(StdHandle.OutputHandle), false, ref ConsoleFontInfo);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
            public COORD(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CONSOLE_FONT_INFO_EX
        {
            public uint cbSize;
            public uint nFont;
            public COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }
    }
}
