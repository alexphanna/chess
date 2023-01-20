using System;
using System.Diagnostics;

namespace Chess
{
    internal class Chess
    {
        public static Piece[] board = new Piece[32];
        public static int turn = 0;
        public static bool check = false;
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Title = "Chess";
            Console.SetWindowSize(16, 8);
            Console.SetBufferSize(16, 8);
            
            Point cursor = new Point(1, 1);
            Piece piece = null;
            Point[] moves = new Point[0];
            int index = 0;

            Board.Fill(board);
            Board.Write(board);

            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow when moves.Length == 0:
                        if (cursor.Y < 8) cursor.Y++;
                        else cursor.Y = 1;
                        break;
                    case ConsoleKey.DownArrow when moves.Length == 0:
                        if (cursor.Y > 1) cursor.Y--;
                        else cursor.Y = 8;
                        break;
                    case ConsoleKey.RightArrow when moves.Length == 0:
                        if (cursor.X < 8) cursor.X++;
                        else cursor.X = 1;
                        break;
                    case ConsoleKey.LeftArrow when moves.Length == 0:
                        if (cursor.X > 1) cursor.X--;
                        else cursor.X = 8;
                        break;
                    case ConsoleKey.DownArrow or ConsoleKey.LeftArrow when moves.Length > 0:
                        if (index > 0) index--;
                        else index = moves.Length - 1;
                        break;
                    case ConsoleKey.UpArrow or ConsoleKey.RightArrow when moves.Length > 0:
                        if (index < moves.Length - 1) index++;
                        else index = 0;
                        break;
                    case ConsoleKey.Enter when moves.Length == 0 && Board.Exists(board, cursor) && Board.Find(board, cursor).Color.Item1 == (turn % 2 == 0):
                        piece = Board.Find(board, cursor); 
                        moves = Board.Find(board, cursor).CurrentMoves;
                        index = 0;
                        break;
                    case ConsoleKey.Enter when moves.Length > 0:
                        Board.Move(ref board, piece, cursor);
                        goto case ConsoleKey.Escape;
                    case ConsoleKey.Escape:
                        moves = new Point[0];
                        Board.Write(board);
                        break;
                }

                Board.Write(board);

                if (moves.Length > 0)
                {
                    Board.Write(board, moves);
                    cursor = moves[index];
                }

                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Point.SetCursorPosition(cursor);
                if (moves.Length == 0 && Board.Exists(board, cursor))
                {
                    Console.ForegroundColor = Board.Find(board, cursor).Color.Item2;
                    Console.Write(Board.Find(board, cursor));
                    if (Board.Find(board, cursor).Color.Item1 == (turn % 2 == 0)) Board.Write(board, Board.Find(board, cursor).CurrentMoves);
                }
                else if (moves.Length > 0 && Board.Exists(board, cursor)) Console.Write(Board.Find(board, cursor));
                else if (moves.Length > 0) Console.Write("●");
                else Console.Write("  ");

                if (Board.Find(board, color: Chess.turn % 2 == 0, type: "King").IsUnderAttack()) Chess.check = true;
            }
        }
    }
}