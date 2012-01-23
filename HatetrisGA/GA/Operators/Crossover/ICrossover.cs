using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.GA.Operators.Crossover
{
    interface ICrossover
    {
        Tuple<Chromosome, Chromosome> Crossover(Chromosome c1, Chromosome c2);
    }
}
