using System;

namespace Chess
{
    abstract class Piece
    {
        public Piece[] Parent { get; set; }
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
        public Point[] LegalMoves
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
                            Array.Resize(ref moves, moves.Length + 1);
                            moves[moves.Length - 1] = new Point(x, y);
                        }
                    }
                }
                return moves;
            }
        }
        public Point[] CurrentMoves
        {
            get
            {
                Point[] moves = new Point[0];

                for (int x = 1; x <= 8; x++)
                {
                    for (int y = 1; y <= 8; y++)
                    {
                        if (IsLegal(new Point(x, y)) && (!GetType().Name.Equals("King") || !new Point(x, y).IsUnderAttack()))
                        {
                            Array.Resize(ref moves, moves.Length + 1);
                            moves[moves.Length - 1] = new Point(x, y);
                        }
                    }
                }
                if (Chess.check)
                {
                    Point[] newMoves = new Point[0];
                    foreach (Point move in moves)
                    {
                        if (StopsCheck(move))
                        {
                            Array.Resize(ref newMoves, newMoves.Length + 1);
                            newMoves[newMoves.Length - 1] = move;
                        }
                    }
                    moves = newMoves;
                }

                return moves;
            }
        }
        public Piece(Piece[] board, Point point, bool color)
        {
            Parent = board;
            Point = point;
            Color = color;
            PreviousMove = 0;
        }
        public void Move(Point point)
        {
            Point.X = point.X;
            Point.Y = point.Y;
        }
        abstract public bool IsLegal(Point point);
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
                    if (Board.Exists(Parent, new Point(x, y))) return true;
                }
            }
            return false;
        }
        public bool IsUnderAttack()
        {
            foreach (Piece piece in Parent)
            {
                if (piece.Color != Color)
                {
                    foreach (Point move in piece.LegalMoves)
                    {
                        if (move.Equals(Point)) return true;
                    }
                }
            }
            return false;
        }
        public bool StopsCheck(Point point)
        {
            Piece[] board = Board.Copy(Parent);
            Piece piece = Board.Find(board, Point);
            piece.Move(point);
            Piece king = Board.Find(board, type: "King", color: Color);
            if (king.IsUnderAttack())  return false;
            return true;
        }
        public static Piece Copy(Piece[] board, Piece piece)
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
        public Pawn(Piece[] board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♟";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Parent, point) && Board.Find(Parent, point).Color == Color) return false;

            int direction = 1;
            if (!Color) direction = -1;

            // small jump
            if (!Board.Exists(Parent, point) && Point.Y + direction == point.Y && Point.X == point.X) return true;
            // big jump
            if (!Board.Exists(Parent, point) && ((Color && Point.Y == 2) || (!Color && Point.Y == 7)) && Point.Y + direction * 2 == point.Y && Point.X == point.X) return true;
            // Taking opponent pieces
            if (Board.Exists(Parent, point) && Point.Y + direction == point.Y && (Point.X == point.X + 1 || Point.X == point.X - 1)) return true;
            // En passant
            if (Board.Find(Parent, new Point(point.X, point.Y - direction)) != null)
            {
                Piece enPassant = Board.Find(Parent, new Point(point.X, point.Y - direction));
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
        public Knight(Piece[] board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♞ ";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Parent, point) && Board.Find(Parent, point).Color == Color) return false;

            if (Math.Abs(point.X - Point.X) == 2 && Math.Abs(point.Y - Point.Y) == 1) return true;
            else if (Math.Abs(point.X - Point.X) == 1 && Math.Abs(point.Y - Point.Y) == 2) return true;

            return false;
        }
    }
    sealed class Bishop : Piece
    {
        public Bishop(Piece[] board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♝ ";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Parent, point) && Board.Find(Parent, point).Color == Color) return false;
            if ((Math.Abs(point.X - Point.X) == Math.Abs(point.Y - Point.Y)) && !IsBlocked(point)) return true;
            return false;
        }
    }
    sealed class Rook : Piece
    {
        public Rook(Piece[] board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♜ ";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Parent, point) && Board.Find(Parent, point).Color == Color) return false;
            if ((point.X == Point.X || point.Y == Point.Y) && !IsBlocked(point)) return true;
            return false;
        }
    }
    sealed class Queen : Piece
    {
        public Queen(Piece[] board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♛ ";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Parent, point) && Board.Find(Parent, point).Color == Color) return false;
            if ((point.X == Point.X || point.Y == Point.Y) && !IsBlocked(point)) return true;
            if (Math.Abs(point.X - Point.X) == Math.Abs(point.Y - Point.Y) && !IsBlocked(point)) return true;
            return false;
        }
    }
    sealed class King : Piece
    {
        public King(Piece[] board, Point point, bool color) : base(board, point, color) { }
        public override string ToString() => "♚ ";
        public override bool IsLegal(Point point)
        {
            if (Board.Exists(Parent, point) && Board.Find(Parent, point).Color == Color) return false;
            if (Math.Abs(point.X - Point.X) == 1 && Math.Abs(point.Y - Point.Y) == 1) return true;
            else if (Math.Abs(point.X - Point.X) == 0 && Math.Abs(point.Y - Point.Y) == 1) return true;
            else if (Math.Abs(point.X - Point.X) == 1 && Math.Abs(point.Y - Point.Y) == 0) return true;
            return false;
        }
    }
}