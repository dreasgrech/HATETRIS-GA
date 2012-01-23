using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HatetrisGA.GA.FitnessCalculators;
using HatetrisGA.Simulation;

namespace HatetrisGA.GA.Operators.Crossover
{
    class OnePointCrossover:ICrossover
    {
        private readonly Random random;
        private readonly FitnessCalculator fitnessCalculator;

        public OnePointCrossover(FitnessCalculator fitnessCalculator)
        {
            this.fitnessCalculator = fitnessCalculator;
            random = new Random();
        }

        public Tuple<Chromosome, Chromosome> Crossover(Chromosome c1, Chromosome c2)
        {
            string replay1 = c1.Replay.ToString(), replay2 = c2.Replay.ToString();

            var locus = random.Next(0, replay1.Length + 1); // Locus: http://en.wikipedia.org/wiki/Locus_(genetics)
            string ch1 = replay1.Substring(0, locus) + replay2.Substring(locus),
                   ch2 = replay2.Substring(0, locus) + replay1.Substring(locus);

            return new Tuple<Chromosome, Chromosome>(new Chromosome(new Replay(ch1), fitnessCalculator), new Chromosome(new Replay(ch2),fitnessCalculator));
        }

        public override string ToString()
        {
            return "One Point";
        }
    }
}
