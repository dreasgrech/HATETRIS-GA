using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.Simulation
{
    class Orientation
    {
        public int? XMin { get; set; }
        public int? YMin { get; set; }
        public int? XDim { get; set; }
        public int? YDim { get; set; }

        public List<int> Rows { get; private set; }

        public Orientation()
        {
            Rows = new List<int>();
            for (int row = 0; row < 4; row++)
            {
                Rows.Add(0);
            }
        }
    }
}
