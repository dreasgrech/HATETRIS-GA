using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.GA.Operators.Selection
{
    interface ISelection
    {
        Chromosome Select(Population population);
    }
}
