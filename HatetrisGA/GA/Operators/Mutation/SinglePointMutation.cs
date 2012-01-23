using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HatetrisGA.CharacterSets;
using HatetrisGA.GA.FitnessCalculators;
using HatetrisGA.Simulation;

namespace HatetrisGA.GA.Operators.Mutation
{
    class SinglePointMutation:IMutation
    {
        private Random random;
        private FitnessCalculator fitnessCalculator;
        private CharacterSet characterSet;


        public SinglePointMutation(CharacterSet characterSet, FitnessCalculator fitnessCalculator)
        {
            this.fitnessCalculator = fitnessCalculator;
            random = new Random();
            this.characterSet = characterSet;
        }

        public Chromosome Mutate(Chromosome ch)
        {
            var replay = ch.Replay.ToString();
            var sb = new StringBuilder(replay);
            var randomPositon = random.Next(0, replay.Length);
            sb[randomPositon] = characterSet.GetRandomCharacter();

            return new Chromosome(new Replay(sb.ToString()), fitnessCalculator);
        }
    }
}
