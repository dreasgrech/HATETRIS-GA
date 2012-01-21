using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.Simulation
{
    class Piece
    {
        public PieceType ID { get; private set; }
        public List<Point> Bits { get; private set; }
        public Point Position { get; set; }
        public int O { get; set; }

        Piece()
        {
            Position = new Point();
        }

        public Piece(PieceType type, Point bit1, Point bit2, Point bit3, Point bit4):this()
        {
            ID = type;
            Bits = new List<Point> { bit1, bit2, bit3, bit4};
        }

        public Piece(PieceType type, Point position, int o)
        {
            ID = type;
            Position = position;
            O = o;
        }
    }
}
