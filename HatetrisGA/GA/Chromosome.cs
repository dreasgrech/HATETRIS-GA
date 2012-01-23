using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HatetrisGA.GA.FitnessCalculators;
using HatetrisGA.Simulation;

namespace HatetrisGA.GA
{
    [DebuggerDisplay("{Fitness}")]
    class Chromosome:IHasFitness
    {
        public Replay Replay { private set; get; }

        public Chromosome(Replay replay, FitnessCalculator fitnessCalculator)
        {
            Replay = replay;
            Fitness = fitnessCalculator.CalculateFitness(this);
        }

        public double Fitness{ get; private set;}
    }
}
