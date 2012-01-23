using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HatetrisGA.Simulation;

namespace HatetrisGA.GA.FitnessCalculators
{
    class HighestScoreFitnessCalculator:FitnessCalculator
    {
        private readonly Game game;
        public HighestScoreFitnessCalculator(Game game)
        {
            this.game = game;
        }

        public override double CalculateFitness(Chromosome ch)
        {
            var score = game.Start(ch.Replay);
            //var filledSpaces = game.FilledSpaces();

            // TODO: find a way to calculate a fitness based on the score and filledSpaces
            return score;// (score + 1) * filledSpaces;
        }

        public override string ToString()
        {
            return "HighScore";
        }
    }
}
