using System;
using System.Diagnostics;

namespace Chess
{
    internal class Board
    {
        public static void Fill(Piece[] board)
        {
            int index = 0;
            for (int y = 1; y <= 8; y += 7)
            {
                for (int x = 1; x <= 8; x++)
                {
                    if (x == 1 || x == 8) board[index++] = new Rook(new Point(x, y), y == 1);
                    else if (x == 2 || x == 7) board[index++] = new Knight(new Point(x, y), y == 1);
                    else if (x == 3 || x == 6) board[index++] = new Bishop(new Point(x, y), y == 1);
                    else if (x == 4) board[index++] = new Queen(new Point(x, y), y == 1);
                    else if (x == 5) board[index++] = new King(new Point(x, y), y == 1);

                    if (y == 1) board[index++] = new Pawn(new Point(x, y + 1), y == 1);
                    else if (y == 8) board[index++] = new Pawn(new Point(x, y - 1), y == 1);
                }
            }
        }
        public static void Write(Piece[] board)
        {
            Console.SetCursorPosition(0, 0);
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    Console.BackgroundColor = new Point(x, y).Color;
                    Point.SetCursorPosition(new Point(x, y));
                    Console.Write("  ");
                }
            }

            foreach (Piece piece in board)
            {
                Console.BackgroundColor = piece.Point.Color;
                Console.ForegroundColor = piece.Color.Item2;
                Point.SetCursorPosition(piece.Point);
                Console.Write(piece);
            }
        }
        public static void Write(Piece[] board, Point[] points)
        {
            if (points == null) return;
            foreach (Point point in points)
            {
                Console.BackgroundColor = point.Color;
                Console.ForegroundColor = ConsoleColor.Gray;
                Point.SetCursorPosition(point);
                if (Find(board, point) == null) Console.Write("●");
                else Console.Write(Find(board, point));
            }
        }
        public static void Remove(ref Piece[] board, Point point)
        {
            Piece[] temp = new Piece[board.Length - 1];
            int index = 0;
            foreach (Piece piece in board)
            {
                if (!point.Equals(piece.Point)) temp[index++] = piece;
            }
            board = temp;
        }
        public static void Move(ref Piece[] board, Piece piece, Point point)
        {
            if (Exists(Chess.board, point)) Remove(ref Chess.board, point);
            piece.Point.X = point.X;
            piece.Point.Y = point.Y;
            piece.PreviousMove = Chess.turn;
            piece.TotalMoves++;
            Chess.turn++;
        }
        public static Piece Find(Piece[] board, Point point = null, bool? color = null, string type = null)
        {
            foreach (Piece piece in board)
            {
                if (color == null || piece.Color.Item1 == color)
                {
                    if (point == null || piece.Point.Equals(point))
                    {
                        if (type == null || piece.GetType().Name.Equals(type)) return piece;
                    }
                }
            }
            return null;
        }
        public static bool Exists(Piece[] board, Point point = null, bool? color = null, string type = null)
        {
            foreach (Piece piece in board)
            {
                if (color == null || piece.Color.Item1 == color)
                {
                    if (point == null || piece.Point.Equals(point))
                    {
                        if (type == null || piece.GetType().Name.Equals(type)) return true;
                    }
                }
            }
            return false;
        }
    }
}