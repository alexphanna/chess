using System;
using System.Collections.Generic;

namespace Chess
{
    class Chess
    {
        public const string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        public static Board board = new Board(DefaultFen);
        public static int turn = 0;
        public static bool check = false;
        public static void Main(string[] args)
        {
            Font.SetFont("MS Gothic", 72);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Title = "Chess";
            Console.SetWindowSize(16, 8);
            Console.SetBufferSize(16, 8);
            
            Point cursor = new Point(1, 1);
            Piece piece = null;
            List<Point> moves = new List<Point>();
            int index = 0;

            Board.Write(board);

            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow when moves.Count == 0:
                        if (cursor.Y < 8) cursor.Y++;
                        else cursor.Y = 1;
                        break;
                    case ConsoleKey.DownArrow when moves.Count == 0:
                        if (cursor.Y > 1) cursor.Y--;
                        else cursor.Y = 8;
                        break;
                    case ConsoleKey.RightArrow when moves.Count == 0:
                        if (cursor.X < 8) cursor.X++;
                        else cursor.X = 1;
                        break;
                    case ConsoleKey.LeftArrow when moves.Count == 0:
                        if (cursor.X > 1) cursor.X--;
                        else cursor.X = 8;
                        break;
                    case ConsoleKey.DownArrow or ConsoleKey.LeftArrow when moves.Count > 0:
                        if (index > 0) index--;
                        else index = moves.Count - 1;
                        break;
                    case ConsoleKey.UpArrow or ConsoleKey.RightArrow when moves.Count > 0:
                        if (index < moves.Count - 1) index++;
                        else index = 0;
                        break;
                    case ConsoleKey.Enter when moves.Count == 0 && board.Exists(cursor) && board.Find(cursor).Color == (turn % 2 == 0):
                        piece = board.Find(cursor);
                        moves = piece.CurrentMoves;
                        if (piece.Color) index = 0;
                        else index = moves.Count - 1;
                        break;
                    case ConsoleKey.Enter when moves.Count > 0:
                        board.Remove(board.Find(cursor));
                        piece.Move(cursor);
                        if (piece.GetType().Name.Equals("Pawn") && (cursor.Y == 1 || cursor.Y == 8))
                        {
                            Piece[] options = new Piece[] { new Queen(board, Point.Copy(cursor), turn % 2 == 0), new Rook(board, Point.Copy(cursor), turn % 2 == 0), new Bishop(board, Point.Copy(cursor), turn % 2 == 0), new Knight(board, Point.Copy(cursor), turn % 2 == 0) };
                            int i = 0;
                            board.Remove(piece);
                            board.Add(options[i]);
                            Board.Write(board);
                            while (true)
                            {
                                Point.SetCursorPosition(cursor);
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = options[i].ConsoleColor;
                                Console.Write(options[i]);
                                ConsoleKey selection = Console.ReadKey().Key;
                                switch (selection)
                                {
                                    case ConsoleKey.RightArrow:
                                        board.Remove(options[i]);
                                        if (i == 3) i = 0;
                                        else i++;
                                        board.Add(options[i]);
                                        break;
                                    case ConsoleKey.LeftArrow:
                                        board.Remove(options[i]);
                                        if (i == 0) i = 3;
                                        else i--;
                                        board.Add(options[i]);
                                        break;
                                    case ConsoleKey.Enter:
                                        goto select;
                                }
                            }
                        }
                        select:
                        check = false;
                        piece.PreviousMove = Chess.turn;
                        piece.TotalMoves++;
                        Chess.turn++;

                        // Check
                        if (board.Find(type: "King", color: Chess.turn % 2 == 0).IsUnderAttack() && !check) check = true;

                        // Checkmate
                        if (check)
                        {
                            bool checkmate = true;
                            foreach (Piece item in board)
                            {
                                if (item.Color == (Chess.turn % 2 == 0))
                                {
                                    foreach (Point currentMove in item.CurrentMoves)
                                    {
                                        if (item.StopsCheck(currentMove)) checkmate = false;
                                    }
                                }
                            }
                            if (checkmate) Console.Title = "Checkmate";
                        }

                        // Stalemate
                        if (!check)
                        {
                            bool stalemate = true;
                            foreach (Piece item in board)
                            {
                                if (item.Color == (Chess.turn % 2 == 0) && item.CurrentMoves.Count > 0) stalemate = false;
                            }
                            if (stalemate) Console.Title = "Stalemate";
                        }

                        // Insufficient Material
                        bool insufficientMaterial = false;
                        if (board.Count == 2) insufficientMaterial = true; // king against king
                        else if (board.Count == 3 && (board.Exists(type: "Bishop") || board.Exists(type: "Knight"))) insufficientMaterial = true; // king against king and bishop or knight
                        else if (board.Count == 4 
                                 && board.Exists(color: true, type: "Bishop") 
                                 && board.Exists(color: false, type: "Bishop") 
                                 && board.Find(color: true, type: "Bishop").Point.ConsoleColor == board.Find(color: false, type: "Bishop").Point.ConsoleColor) insufficientMaterial = true; //king and bishop against king and bishop, with both bishops on squares of the same color
                        if (insufficientMaterial) Console.Title = "Insufficient Material";

                        goto case ConsoleKey.Escape;
                    case ConsoleKey.Escape:
                        moves.Clear();
                        Board.Write(board);
                        break;
                }

                Board.Write(board);

                if (moves.Count > 0)
                {
                    Board.Write(board, moves);
                    cursor = moves[index];
                }

                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Point.SetCursorPosition(cursor);
                if (moves.Count == 0 && board.Exists(cursor))
                {
                    Piece cursorPiece = board.Find(cursor);
                    Console.ForegroundColor = cursorPiece.ConsoleColor;
                    Console.Write(cursorPiece);
                    if (cursorPiece.Color == (turn % 2 == 0)) Board.Write(board, cursorPiece.CurrentMoves);
                }
                else if (moves.Count > 0 && board.Exists(cursor)) Console.Write(board.Find(cursor));
                else if (moves.Count > 0) Console.Write("●");
                else Console.Write("  ");
            }
        }
    }
}