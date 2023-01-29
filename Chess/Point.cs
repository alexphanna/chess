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
        public Point() { }
        public ConsoleColor ConsoleColor
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
        public override string ToString()
        {
            return + X + ", " + Y;
        }
        public bool Equals(Point point)
        {
            return point.X == X && point.Y == Y;
        }
        public static Point Copy(Point point)
        {
            return new Point(point.X, point.Y);
        }
        public static void SetCursorPosition(Point point)
        {
            Console.SetCursorPosition((point.X - 1) * 2, 8 - point.Y);
        }
        public bool IsUnderAttack()
        {
            foreach (Piece piece in Chess.board)
            {
                if (piece.Color != (Chess.turn % 2 == 0))
                {
                    foreach (Point move in piece.OffensiveMoves)
                    {
                        if (move.Equals(this)) return true;
                    }
                }
            }
            return false;
        }
    }
}