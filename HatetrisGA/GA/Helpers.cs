using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.GA
{
    static class Helpers
    {
        public static double NextDouble(this Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
        }
    }
}
