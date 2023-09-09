using System;
using System.Collections.Generic;
using System.Numerics;

namespace Chess
{
    class Human : Player
    {
        public Human(string name, bool color) : base(name, color) { }
        public override void Move(Board board)
        {
            Point cursor = new Point(4, 4);
            Piece piece = null;
            List<Point> moves = new List<Point>();
            int index = 0;

            while (true)
            {
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
                    if (cursorPiece.Color == Color) Board.Write(board, cursorPiece.CurrentMoves);
                }
                else if (moves.Count > 0 && board.Exists(cursor)) Console.Write(board.Find(cursor));
                else if (moves.Count > 0) Console.Write("● ");
                else Console.Write("  ");

                ConsoleKey key = Console.ReadKey().Key;

                if (moves.Count == 0)
                {
                    if (key == ConsoleKey.UpArrow)
                    {
                        if (cursor.Y < 8) cursor.Y++;
                        else cursor.Y = 1;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        if (cursor.Y > 1) cursor.Y--;
                        else cursor.Y = 8;
                    }
                    else if (key == ConsoleKey.RightArrow)
                    {
                        if (cursor.X < 8) cursor.X++;
                        else cursor.X = 1;
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        if (cursor.X > 1) cursor.X--;
                        else cursor.X = 8;
                    }
                    else if (key == ConsoleKey.Enter && board.Exists(cursor) && board.Find(cursor).Color == Color)
                    {
                        piece = board.Find(cursor);
                        moves = piece.CurrentMoves;
                        if (piece.Color) index = 0;
                        else index = moves.Count - 1;
                    }
                }
                else
                {
                    if (key == ConsoleKey.UpArrow || key == ConsoleKey.RightArrow)
                    {
                        if (index < moves.Count - 1) index++;
                        else index = 0;
                    }
                    else if (key == ConsoleKey.DownArrow || key == ConsoleKey.LeftArrow)
                    {
                        if (index > 0) index--;
                        else index = moves.Count - 1;
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        board.Remove(board.Find(cursor));
                        piece.Move(cursor);

                        // Promotion
                        if (piece.GetType().Name.Equals("Pawn") && (cursor.Y == 1 || cursor.Y == 8))
                        {
                            Piece[] options = new Piece[] 
                            { 
                                new Queen(board, Point.Copy(cursor), Color), 
                                new Rook(board, Point.Copy(cursor), Color), 
                                new Bishop(board, Point.Copy(cursor), Color), 
                                new Knight(board, Point.Copy(cursor), Color) 
                            };
                            int i = 0;
                            board.Remove(piece);
                            board.Add(options[i]);
                            Board.Write(board);
                            ConsoleKey selection = 0;
                            while (selection != ConsoleKey.Enter)
                            {
                                Point.SetCursorPosition(cursor);
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = options[i].ConsoleColor;
                                Console.Write(options[i]);
                                selection = Console.ReadKey().Key;
                                if (selection == ConsoleKey.RightArrow || selection == ConsoleKey.LeftArrow)
                                {
                                    board.Remove(options[i]);
                                    if (selection == ConsoleKey.RightArrow)
                                    {
                                        if (i == 3) i = 0;
                                        else i++;
                                    }
                                    else if (selection == ConsoleKey.LeftArrow)
                                    {
                                        if (i == 0) i = 3;
                                        else i--;
                                    }
                                    board.Add(options[i]);
                                }
                            }
                        }

                        moves.Clear();
                        Board.Write(board);
                        return;
                    }
                }
                if (key == ConsoleKey.Escape) moves.Clear();
            }
        }
    }
}
