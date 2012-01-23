using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HatetrisGA.GA.FitnessCalculators;
using HatetrisGA.GA.Operators.Crossover;
using HatetrisGA.GA.Operators.Mutation;
using HatetrisGA.GA.Operators.Selection;
using HatetrisGA.Simulation;

namespace HatetrisGA.GA
{
    class GeneticAlgorithm
    {
        public double CrossoverRate { get; set; }
        public double MutationRate { get; set; }
        public double ElitismRate { get; set; }
        public double TruncationRate { get; set; }
        public double ChromosomeCount { get; set; }
        public int TotalGenerations { get; private set; }

        private FitnessCalculator fitnessCalculator;
        private ISelection selection;
        private IMutation mutation;
        private ICrossover crossover;
        private Random random;

        public GeneticAlgorithm(double crossoverRate, double mutationRate, double elitism, double truncation, double chromosomeCount, ISelection selection, ICrossover crossover, IMutation mutation, FitnessCalculator fitnessCalculator)
        {
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            ElitismRate = elitism;
            TruncationRate = truncation;
            ChromosomeCount = chromosomeCount;

            this.selection = selection;
            this.mutation = mutation;
            this.crossover = crossover;

            this.fitnessCalculator = fitnessCalculator;

            random = new Random();
        }

        public Population AdvancePopulation(Population population)
        {
            var chromosomes = new List<Chromosome>();
            population = new Population(population.Take((int)(TruncationRate * population.Count()))); // TRUNCATION

            do
            {
                Chromosome chosen1 = selection.Select(population),
                           chosen2 = selection.Select(population);

                if (random.NextDouble() < CrossoverRate)
                {
                    var children = crossover.Crossover(chosen1, chosen2); // CROSSOVER
                    chosen1 = children.Item1;
                    chosen2 = children.Item2;
                }

                if (random.NextDouble() < MutationRate)
                {
                    chosen1 = mutation.Mutate(chosen1); // MUTATION
                }

                if (random.NextDouble() < MutationRate)
                {
                    chosen2 = mutation.Mutate(chosen2); // MUTATION
                }

                chromosomes.Add(chosen1);
                chromosomes.Add(chosen2);
            } while (chromosomes.Count < ChromosomeCount);

            return new Population(chromosomes);
        }
    }
}
