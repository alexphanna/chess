using System;
using System.Collections.Generic;

namespace Chess
{
    abstract class Piece
    {
        public Board Board { get; set; }
        public Point Point { get; set; }
        public bool Color { get; set; }
        public ConsoleColor ConsoleColor
        {
            get
            {
                if (Color) return ConsoleColor.White;
                else return ConsoleColor.Black;
            }
        }
        public int PreviousMove { get; set; }
        public int TotalMoves { get; set; }
        public List<Point> OffensiveMoves
        {
            get
            {
                List<Point> moves = new List<Point>();

                for (int x = 1; x <= 8; x++)
                {
                    for (int y = 1; y <= 8; y++)
                    {
                        if (IsLegal(new Point(x, y), false)) moves.Add(new Point(x, y));
                    }
                }

                return moves;
            }
        }
        public List<Point> CurrentMoves
        {
            get
            {
                List<Point> moves = new List<Point>();

                for (int x = 1; x <= 8; x++)
                {
                    for (int y = 1; y <= 8; y++)
                    {
                        if (IsLegal(new Point(x, y)) && !StartsCheck(new Point(x, y))) moves.Add(new Point(x, y)); 
                    }
                }

                return moves;
            }
        }
        public Piece(Board board, Point point, bool color)
        {
            Board = board;
            Point = point;
            Color = color;
            PreviousMove = 0;
        }
        public void Move(Point point)
        {
            if (GetType().Name.Equals("King") && Point.X == 5)
            {
                if (point.X == 3) Board.Find(new Point(1, Point.Y)).Move(new Point(4, Point.Y));
                else if (point.X == 7) Board.Find(new Point(8, Point.Y)).Move(new Point(6, Point.Y));
            }

            Point.X = point.X;
            Point.Y = point.Y;
        }
        abstract public bool IsLegal(Point point, bool defensive = true);
        protected bool IsBlocked(Point point)
        {
            int x = Point.X, y = Point.Y;
            while (point.X != x || point.Y != y)
            {
                if (Point.X < point.X) x++;
                else if (Point.X > point.X) x--;
                if (Point.Y < point.Y) y++;
                else if (Point.Y > point.Y) y--;

                if (point.X != x || point.Y != y)
                {
                    if (Board.Exists(new Point(x, y))) return true;
                }
            }
            return false;
        }
        public bool IsUnderAttack()
        {
            foreach (Piece piece in Board)
            {
                if (piece.Color != Color)
                {
                    foreach (Point move in piece.OffensiveMoves)
                    {
                        if (move.Equals(Point)) return true;
                    }
                }
            }
            return false;
        }
        public bool StartsCheck(Point point)
        {
            Board board = new Board();
            Board.CopyTo(board);
            if (board.Exists(point)) board.Remove(board.Find(point));
            Piece piece = board.Find(Point);
            piece.Move(point);
            if (board.Find(type: "King", color: Color).IsUnderAttack()) return true;
            return false;
        }
        public bool StopsCheck(Point point)
        {
            Board board = new Board();
            Board.CopyTo(board);
            if (board.Exists(point)) board.Remove(board.Find(point));
            Piece piece = board.Find(Point);
            piece.Move(point);
            if (board.Find(type: "King", color: Color).IsUnderAttack())  return false;
            return true;
        }
        public static Piece Copy(Board board, Piece piece)
        {
            switch (piece.GetType().Name)
            {
                case "Knight": 
                    return new Knight(board, Point.Copy(piece.Point), piece.Color);
                case "Bishop": 
                    return new Bishop(board, Point.Copy(piece.Point), piece.Color);
                case "Rook": 
                    return new Rook(board, Point.Copy(piece.Point), piece.Color);
                case "Queen": 
                    return new Queen(board, Point.Copy(piece.Point), piece.Color);
                case "King":
                    return new King(board, Point.Copy(piece.Point), piece.Color);
            }
            return new Pawn(board, Point.Copy(piece.Point), piece.Color);
        }
    }
    sealed class Pawn : Piece
    {
        public Pawn(Board board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♟";
        public override bool IsLegal(Point point, bool defensive = true)
        {
            if (Board.Exists(point) && Board.Find(point).Color == Color) return false;

            int direction = 1;
            if (!Color) direction = -1;

            // small jump
            if (!Board.Exists(point) && Point.Y + direction == point.Y && Point.X == point.X) return true;
            // big jump
            if (!Board.Exists(point) && !Board.Exists(new Point(point.X, point.Y - direction)) && ((Color && Point.Y == 2) || (!Color && Point.Y == 7)) && Point.Y + direction * 2 == point.Y && Point.X == point.X) return true;
            // Taking opponent pieces
            if (Board.Exists(point) && Point.Y + direction == point.Y && (Point.X == point.X + 1 || Point.X == point.X - 1)) return true;
            // En passant
            if (Board.Exists(new Point(point.X, point.Y - direction)))
            {
                Piece enPassant = Board.Find(new Point(point.X, point.Y - direction));
                if (enPassant.Color != Color
                    && enPassant.PreviousMove == Chess.turn - 1
                    && enPassant.TotalMoves == 1
                    && Point.Y + direction == point.Y
                    && (Point.X == point.X + 1 || Point.X == point.X - 1)) return true;
            }
            return false;
        }
    }
    sealed class Knight : Piece
    {
        public Knight(Board board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♞ ";
        public override bool IsLegal(Point point, bool defensive = true)
        {
            if (Board.Exists(point) && Board.Find(point).Color == Color) return false;

            if (Math.Abs(point.X - Point.X) == 2 && Math.Abs(point.Y - Point.Y) == 1) return true;
            else if (Math.Abs(point.X - Point.X) == 1 && Math.Abs(point.Y - Point.Y) == 2) return true;

            return false;
        }
    }
    sealed class Bishop : Piece
    {
        public Bishop(Board board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♝ ";
        public override bool IsLegal(Point point, bool defensive = true)
        {
            if (Board.Exists(point) && Board.Find(point).Color == Color) return false;
            if ((Math.Abs(point.X - Point.X) == Math.Abs(point.Y - Point.Y)) && !IsBlocked(point)) return true;
            return false;
        }
    }
    sealed class Rook : Piece
    {
        public Rook(Board board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♜ ";
        public override bool IsLegal(Point point, bool defensive = true)
        {
            if (Board.Exists(point) && Board.Find(point).Color == Color) return false;
            if ((point.X == Point.X || point.Y == Point.Y) && !IsBlocked(point)) return true;
            return false;
        }
    }
    sealed class Queen : Piece
    {
        public Queen(Board board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♛ ";
        public override bool IsLegal(Point point, bool defensive = true)
        {
            if (Board.Exists(point) && Board.Find(point).Color == Color) return false;
            if ((point.X == Point.X || point.Y == Point.Y) && !IsBlocked(point)) return true;
            if (Math.Abs(point.X - Point.X) == Math.Abs(point.Y - Point.Y) && !IsBlocked(point)) return true;
            return false;
        }
    }
    sealed class King : Piece
    {
        public King(Board board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♚ ";
        public override bool IsLegal(Point point, bool defensive = true)
        {
            if (defensive && TotalMoves == 0 && !IsUnderAttack())
            {
                Piece rook = null;
                if (Board.Exists(new Point(point.X - 2, point.Y), Color, "Rook")) rook = Board.Find(new Point(point.X - 2, point.Y), Color, "Rook");
                else if (Board.Exists(new Point(point.X + 1, point.Y), Color, "Rook")) rook = Board.Find(new Point(point.X + 1, point.Y), Color, "Rook");
                if (rook != null
                    && rook.TotalMoves == 0
                    && !IsBlocked(rook.Point)
                    && ((rook.Point.X == 1 
                        && !new Point(3, Point.Y).IsUnderAttack() 
                        && !new Point(4, Point.Y).IsUnderAttack())
                    || (rook.Point.X == 8 
                        && !new Point(6, Point.Y).IsUnderAttack() 
                        && !new Point(7, Point.Y).IsUnderAttack()))) return true;
            }
            if (Board.Exists(point) && Board.Find(point).Color == Color) return false;
            if (Math.Abs(point.X - Point.X) == 1 && Math.Abs(point.Y - Point.Y) == 1) return true;
            else if (Math.Abs(point.X - Point.X) == 0 && Math.Abs(point.Y - Point.Y) == 1) return true;
            else if (Math.Abs(point.X - Point.X) == 1 && Math.Abs(point.Y - Point.Y) == 0) return true;
            return false;
        }
    }
}