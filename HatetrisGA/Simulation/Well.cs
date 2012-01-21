using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.Simulation
{
    class Well
    {
        public int Score { get; set; }
        public int HighestBlue { get; set; }

        public int Width { get { return 10; } } // min: 4
        public int Depth { get { return 20; } } //min: bar
        public int Bar { get { return 4; } }

        public int[] Content { get; private set; }

        public Well()
        {
            Content = new int[Depth];
            HighestBlue = Depth;
        }

        public Well(int score, int highestBlue):this()
        {
            Score = score;
            HighestBlue = highestBlue;
        }
    }
}
