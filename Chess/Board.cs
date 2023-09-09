﻿using System;
using System.Collections.Generic;

namespace Chess
{
    class Board : List<Piece>
    {
        public const ConsoleColor Color = ConsoleColor.Blue;
        public int Turn { get; set; }
        public Board(string fen = "")
        {
            Point point = new Point(1, 1);

            for (int i = 0; i < fen.Length; i++)
            {
                if (fen[i] == '/') point = new Point(1, point.Y + 1);
                else if ('1' <= fen[i] && fen[i] <= '8') point.X += fen[i] - '0';
                else
                {
                    switch (Char.ToLower(fen[i]))
                    {
                        case 'p':
                            Add(new Pawn(this, Point.Copy(point), Char.IsLower(fen[i])));
                            break;
                        case 'n':
                            Add(new Knight(this, Point.Copy(point), Char.IsLower(fen[i])));
                            break;
                        case 'b':
                            Add(new Bishop(this, Point.Copy(point), Char.IsLower(fen[i])));
                            break;
                        case 'r':
                            Add(new Rook(this, Point.Copy(point), Char.IsLower(fen[i])));
                            break;
                        case 'q':
                            Add(new Queen(this, Point.Copy(point), Char.IsLower(fen[i])));
                            break;
                        case 'k':
                            Add(new King(this, Point.Copy(point), Char.IsLower(fen[i])));
                            break;
                    }
                    point.X++;
                }
            }
        }
        public int Evaluate()
        {
            int eval = 0;
            foreach (Piece piece in this)
            {
                if (piece.Color) eval += piece.Value;
                else eval -= piece.Value;
            }
            return eval;
        }
        public Piece Find(Point point = null, bool? color = null, string type = null)
        {
            foreach (Piece piece in this)
            {
                if ((color == null || piece.Color == color) && (point == null || piece.Point.Equals(point)) && (type == null || piece.GetType().Name.Equals(type))) return piece;
            }
            return null;
        }
        public bool Exists(Point point = null, bool? color = null, string type = null)
        {
            return Find(point, color, type) != null;
        }
        public static Board Copy(Board oldBoard)
        {
            Board newBoard = new Board();
            oldBoard.ForEach(piece => newBoard.Add(Piece.Copy(newBoard, piece)));
            return newBoard;
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
                if (board.Find(point) == null) Console.Write("● ");
                else Console.Write(board.Find(point));
            }
        }
    }
}