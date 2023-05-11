using System;

namespace Chess
{
    abstract class Player
    {
        public string Name { get; }
        public bool Color { get; }
        public Player(string name, bool color)
        {
            Name = name;
            Color = color;
        }
        public abstract void Move(Board board);
        public bool IsCheck(Board board)
        {
            return board.Find(type: "King", color: Color).IsUnderAttack();
        }
        public bool IsCheckmate(Board board)
        {
            if (IsCheck(board))
            {
                foreach (Piece item in board)
                {
                    if (item.Color == Color)
                    {
                        foreach (Point currentMove in item.CurrentMoves)
                        {
                            if (!item.IsCheck(currentMove)) return false;
                        }
                    }
                }
            }
            return true;
        }
        public bool IsStalemate(Board board)
        {
            if (!IsCheck(board))
            {
                foreach (Piece item in board)
                {
                    if (item.Color == Color && item.CurrentMoves.Count > 0) return false;
                }
            }
            return true;
        }
        /*public bool IsDeadPosition(Board board)
        {
            // Insufficient Material
            if (Count == 2) return true;
            else if (Count == 3 && (Exists(type: "Bishop") || Exists(type: "Knight"))) return true;
            else if (Count == 4
                     && Exists(color: true, type: "Bishop")
                     && Exists(color: false, type: "Bishop")
                     && Find(color: true, type: "Bishop").Point.ConsoleColor == Find(color: false, type: "Bishop").Point.ConsoleColor) return true;
            return false;
        }*/ 
    }
}
