using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HatetrisGA.GA.FitnessCalculators;
using HatetrisGA.Simulation;

namespace HatetrisGA.GA.Operators.Crossover
{
    class TwoPointCrossover
    {
        private readonly Random random;
        private FitnessCalculator fitnessCalculator;

        public TwoPointCrossover(FitnessCalculator fitnessCalculator)
        {
            this.fitnessCalculator = fitnessCalculator;
            random = new Random();
        }

        public Tuple<Chromosome, Chromosome> Crossover(Chromosome c1, Chromosome c2)
        {
            string replay1 = c1.Replay.ToString(), replay2 = c2.Replay.ToString();

            int locus1 = random.Next(0, replay1.Length),
                locus2 = random.Next(locus1, replay1.Length),
                distance = locus2 - locus1;

            string ch1 = replay1.Substring(0, locus1) + replay2.Substring(locus1, distance) + replay1.Substring(locus2),
                   ch2 = replay2.Substring(0, locus1) + replay1.Substring(locus1, distance) + replay2.Substring(locus2);

            return new Tuple<Chromosome, Chromosome>(new Chromosome(new Replay(ch1), fitnessCalculator), new Chromosome(new Replay(ch2), fitnessCalculator));
        }

        public override string ToString()
        {
            return "Two Point";
        }
    }

}
