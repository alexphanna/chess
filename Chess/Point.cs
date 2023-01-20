using System;

namespace Chess
{
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public ConsoleColor Color
        {
            get
            {
                if (X % 2 == 0)
                {
                    if (Y % 2 == 0) return ConsoleColor.DarkBlue;
                    else return ConsoleColor.Blue;
                }
                else
                {
                    if (Y % 2 == 0) return ConsoleColor.Blue;
                    else return ConsoleColor.DarkBlue;
                }
            }
        }
        public bool Equals(Point point)
        {
            return point.X == X && point.Y == Y;
        }
        public static void SetCursorPosition(Point point)
        {
            Console.SetCursorPosition((point.X - 1) * 2, 8 - point.Y);
        }
    }
}
