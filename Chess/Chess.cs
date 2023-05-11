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
            Font.SetFont("MS Gothic", 72);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Title = "Chess";
            Console.SetWindowSize(16, 8);
            Console.SetBufferSize(16, 8);

            new Chess(new Human("Alex", true), new Human("Ed", false)).Start();
        }
    }
}