using System;
using System.Collections.Generic;

namespace Chess
{
    class Board : List<Piece>
    {
        public Board(bool pieces = false)
        {
            if (pieces)
            {
                for (int y = 1; y <= 8; y += 7)
                {
                    for (int x = 1; x <= 8; x++)
                    {
                        if (x == 1 || x == 8) Add(new Rook(this, new Point(x, y), y == 1));
                        else if (x == 2 || x == 7) Add(new Knight(this, new Point(x, y), y == 1));
                        else if (x == 3 || x == 6) Add(new Bishop(this, new Point(x, y), y == 1));
                        else if (x == 4) Add(new Queen(this, new Point(x, y), y == 1));
                        else if (x == 5) Add(new King(this, new Point(x, y), y == 1));

                        if (y == 1) Add(new Pawn(this, new Point(x, y + 1), y == 1));
                        else if (y == 8) Add(new Pawn(this, new Point(x, y - 1), y == 1));
                    }
                }
            }
        }
        public Piece Find(Point point = null, bool? color = null, string type = null)
        {
            foreach (Piece piece in this)
            {
                if (color == null || piece.Color == color)
                {
                    if (point == null || piece.Point.Equals(point))
                    {
                        if (type == null || piece.GetType().Name.Equals(type)) return piece;
                    }
                }
            }
            return null;
        }
        public bool Exists(Point point = null, bool? color = null, string type = null)
        {
            foreach (Piece piece in this)
            {
                if (color == null || piece.Color == color)
                {
                    if (point == null || piece.Point.Equals(point))
                    {
                        if (type == null || piece.GetType().Name.Equals(type)) return true;
                    }
                }
            }
            return false;
        }
        public void CopyTo(Board board)
        {
            foreach (Piece piece in this)
            {
                board.Add(Piece.Copy(board, piece));
            }
        }
        public static void Write(Board board)
        {
            Console.SetCursorPosition(0, 0);
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    Console.BackgroundColor = new Point(x, y).ConsoleColor;
                    Point.SetCursorPosition(new Point(x, y));
                    Console.Write("  ");
                }
            }

            foreach (Piece piece in board)
            {
                Console.BackgroundColor = piece.Point.ConsoleColor;
                Console.ForegroundColor = piece.ConsoleColor;
                Point.SetCursorPosition(piece.Point);
                Console.Write(piece);
            }
        }
        public static void Write(Board board, List<Point> points)
        {
            if (points == null) return;
            foreach (Point point in points)
            {
                Console.BackgroundColor = point.ConsoleColor;
                Console.ForegroundColor = ConsoleColor.Gray;
                Point.SetCursorPosition(point);
                if (board.Find(point) == null) Console.Write("●");
                else Console.Write(board.Find(point));
            }
        }
    }
}