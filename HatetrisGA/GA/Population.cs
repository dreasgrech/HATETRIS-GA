using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HatetrisGA.GA.FitnessCalculators;
using HatetrisGA.Simulation;

namespace HatetrisGA.GA
{
    class Population:IEnumerable<Chromosome>, IHasFitness
    {
        public double Fitness { get; private set; }
        private readonly IEnumerable<Chromosome> chromosomes;

        public Population(IEnumerable<Chromosome> chromosomes)
        {
            this.chromosomes = chromosomes;
            Fitness = chromosomes.Sum(c => c.Fitness);
        } 

        public Population(IEnumerable<Replay> replays, FitnessCalculator fitnessCalculator)
        {
            chromosomes = replays.Select(replay => new Chromosome(replay, fitnessCalculator)).OrderByDescending(c => c.Fitness).ToList();
        }

        public IEnumerator<Chromosome> GetEnumerator()
        {
            return chromosomes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
