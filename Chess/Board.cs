using System;

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
                    if (x == 1 || x == 8) board[index++] = new Rook(board, new Point(x, y), y == 1);
                    else if (x == 2 || x == 7) board[index++] = new Knight(board, new Point(x, y), y == 1);
                    else if (x == 3 || x == 6) board[index++] = new Bishop(board, new Point(x, y), y == 1);
                    else if (x == 4) board[index++] = new Queen(board, new Point(x, y), y == 1);
                    else if (x == 5) board[index++] = new King(board, new Point(x, y), y == 1);

                    if (y == 1) board[index++] = new Pawn(board, new Point(x, y + 1), y == 1);
                    else if (y == 8) board[index++] = new Pawn(board, new Point(x, y - 1), y == 1);
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
        public static void Write(Piece[] board, Point[] points)
        {
            if (points == null) return;
            foreach (Point point in points)
            {
                Console.BackgroundColor = point.ConsoleColor;
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
        public static Piece Find(Piece[] board, Point point = null, bool? color = null, string type = null)
        {
            foreach (Piece piece in board)
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
        public static bool Exists(Piece[] board, Point point = null, bool? color = null, string type = null)
        {
            foreach (Piece piece in board)
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
        public static Piece[] Copy(Piece[] board)
        {
            Piece[] newBoard = new Piece[board.Length];
            for (int i = 0; i < board.Length; i++)
            {
                newBoard[i] = Piece.Copy(newBoard, board[i]);
            }
            return newBoard;
        }
    }
}