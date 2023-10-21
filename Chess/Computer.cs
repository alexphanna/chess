using System;

namespace Chess
{
    class Computer : Player
    {
        private static Random random = new Random();
        public Computer(string name, bool color) : base(name, color) { }
        public override void Move(Board board)
        {

        }
    }
}
