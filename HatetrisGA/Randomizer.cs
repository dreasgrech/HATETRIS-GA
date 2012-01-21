using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA
{
    class Randomizer<T>
    {
        private readonly Random random;

        public Randomizer()
        {
            random = new Random();
        }

        public T GetRandom(IEnumerable<T> elements)
        {
            return elements.ElementAt(random.Next(0, elements.Count() - 1));
        }
    }
}
