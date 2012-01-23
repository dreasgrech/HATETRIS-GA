using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.GA.Operators.Mutation
{
    interface IMutation
    {
        Chromosome Mutate(Chromosome ch);
    }
}
