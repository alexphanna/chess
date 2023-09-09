using System;
using System.Collections.Generic;
using System.Drawing;

namespace Chess
{
    class Chess
    {
        private const string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        public Board Board { get; set; }
        private Player[] players;
        public Chess(Player player1, Player player2)
        {
            Board = new Board(DefaultFen);
            players = new Player[2] { player1, player2 };
        }
        public void Start()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            while (true)
            {
                foreach (Player player in players)
                {
                    player.Move(Board);
                    if (player.IsCheckmate(Board) || player.IsCheckmate(Board)) Console.Title = "Checkmate";
                    Console.Title = Board.Evaluate() + "";
                    Board.Turn++;
                }
            }
        }
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Title = "Chess";

            new Chess(new Human("Alex", true), new Human("Ed", false)).Start();
        }
    }
}