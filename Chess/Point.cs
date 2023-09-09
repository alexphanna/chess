using System;

namespace Chess
{
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public ConsoleColor ConsoleColor
        {
            get
            {
                if (X % 2 == Y % 2) return Board.Color;
                else return Board.Color - 8;
            }
        }
        public override string ToString() => $"({X},{Y})";
        public bool Equals(Point point) => point.X == X && point.Y == Y;
        public static Point Copy(Point point) => new Point(point.X, point.Y);
    }
}