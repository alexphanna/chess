using System;
using System.Diagnostics;

namespace Chess
{
    abstract class Piece
    {
        public string Type { get; protected set; }
        public Point Point { get; set; }
        private bool color;
        public (bool, ConsoleColor) Color
        {
            get
            {
                if (color) return (true, ConsoleColor.White);
                else return (false, ConsoleColor.Black);
            }
        }
        public int PreviousMove { get; set; }
        public int TotalMoves { get; set; }
        public Point[] CurrentMoves
        {
            get
            {
                Point[] moves = new Point[0];

                for (int x = 1; x <= 8; x++)
                {
                    for (int y = 1; y <= 8; y++)
                    {
                        if (IsLegal(new Point(x, y)))
                        {
                            /*
                             * Only show move that will take the player out of check simply
                             */

                            Array.Resize(ref moves, moves.Length + 1);
                            moves[moves.Length - 1] = new Point(x, y);
                        }
                    }
                }

                return moves;
            }
        }
        public Piece(Point point, bool color)
        {
            Point = point;
            this.color = color;
            PreviousMove = 0;
        }
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
                    if (Board.Exists(Chess.board, new Point(x, y))) return true;
                }
            }
            return false;
        }
        public abstract bool IsLegal(Point point);
        public bool IsUnderAttack()
        {
            foreach (Piece piece in Chess.board)
            {
                foreach (Point move in piece.CurrentMoves)
                {
                    if (move.Equals(Point)) return true;
                }
            }
            return false;
        }
    }
    sealed class Pawn : Piece
    {
        public Pawn(Point point, bool color) : base(point, color) { }
        public override string ToString() => "♟";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Chess.board, point) && Board.Find(Chess.board, point).Color == Color) return false;

            int direction = 1;
            if (!Color.Item1) direction = -1;

            // small jump
            if (!Board.Exists(Chess.board, point) && Point.Y + direction == point.Y && Point.X == point.X) return true;
            // big jump
            if (!Board.Exists(Chess.board, point) && ((Color.Item1 && Point.Y == 2) || (!Color.Item1 && Point.Y == 7)) && Point.Y + direction * 2 == point.Y && Point.X == point.X) return true;
            // Taking opponent pieces
            if (Board.Exists(Chess.board, point) && Point.Y + direction == point.Y && (Point.X == point.X + 1 || Point.X == point.X - 1)) return true;
            // En passant
            if (Board.Find(Chess.board, new Point(point.X, point.Y - direction)) != null)
            {
                Piece enPassant = Board.Find(Chess.board, new Point(point.X, point.Y - direction));
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
        public Knight(Point point, bool color) : base(point, color) { }
        public override string ToString() => "♞ ";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Chess.board, point) && Board.Find(Chess.board, point).Color == Color) return false;

            if (Math.Abs(point.X - Point.X) == 2 && Math.Abs(point.Y - Point.Y) == 1) return true;
            else if (Math.Abs(point.X - Point.X) == 1 && Math.Abs(point.Y - Point.Y) == 2) return true;

            return false;
        }
    }
    sealed class Bishop : Piece
    {
        public Bishop(Point point, bool color) : base(point, color) { }
        public override string ToString() => "♝ ";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Chess.board, point) && Board.Find(Chess.board, point).Color == Color) return false;
            if ((Math.Abs(point.X - Point.X) == Math.Abs(point.Y - Point.Y)) && !IsBlocked(point)) return true;
            return false;
        }
    }
    sealed class Rook : Piece
    {
        public Rook(Point point, bool color) : base(point, color) { }
        public override string ToString() => "♜ ";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Chess.board, point) && Board.Find(Chess.board, point).Color == Color) return false;
            if ((point.X == Point.X || point.Y == Point.Y) && !IsBlocked(point)) return true;
            return false;
        }
    }
    sealed class Queen : Piece
    {
        public Queen(Point point, bool color) : base(point, color) { }
        public override string ToString() => "♛ ";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Chess.board, point) && Board.Find(Chess.board, point).Color == Color) return false;
            if ((point.X == Point.X || point.Y == Point.Y) && !IsBlocked(point)) return true;
            if (Math.Abs(point.X - Point.X) == Math.Abs(point.Y - Point.Y) && !IsBlocked(point)) return true;
            return false;
        }
    }
    sealed class King : Piece
    {
        public King(Point point, bool color) : base(point, color) { }
        public override string ToString() => "♚ ";
        public override bool IsLegal(Point point)
        {
            //if (Board.Exists(Chess.board, point) && Board.Find(Chess.board, point).Color == Color) return false;
            if (Math.Abs(point.X - Point.X) == 1 && Math.Abs(point.Y - Point.Y) == 1) return true;
            else if (Math.Abs(point.X - Point.X) == 0 && Math.Abs(point.Y - Point.Y) == 1) return true;
            else if (Math.Abs(point.X - Point.X) == 1 && Math.Abs(point.Y - Point.Y) == 0) return true;
            return false;
        }
    }
}