using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HatetrisGA.CharacterSets;
using HatetrisGA.CommandLineArgs;
using HatetrisGA.GA;
using HatetrisGA.GA.FitnessCalculators;
using HatetrisGA.GA.Operators.Crossover;
using HatetrisGA.GA.Operators.Mutation;
using HatetrisGA.GA.Operators.Selection;
using HatetrisGA.ReplayGenerators;
using HatetrisGA.Simulation;

namespace HatetrisGA
{
    class Program
    {
        private static double crossoverRate = 0.6, mutationRate = 0.25, elitismRate = 0.1, truncationRate = 0.8;
        private static int chromosomeCount = 2000;
        private static OptionSet options;
        private static FitnessCalculator fitness;
        private static ICrossover crossover;
        private static IMutation mutation;
        private static ISelection selection;

        private static Dictionary<string, Type> fitnessTypes = new Dictionary<string, Type>
                            {
                                {"highestscore", typeof (HighestScoreFitnessCalculator)}
                            };

        private static Dictionary<string, Type> crossoverTypes = new Dictionary<string, Type>
                                                              {
                                                                  {"one", typeof (OnePointCrossover)},
                                                                  {"two", typeof (TwoPointCrossover)}
                                                              };

        static void Main(string[] args)
        {
            const int replayLength = 750;
            //IReplayGenerator replayGenerator = new FromStringReplay("AAAA AAAA AAAA AAAA AAAA AAAA AAAA AAAA AAAA AAAA AAAA A");
            IReplayGenerator replayGenerator = new RandomReplayGenerator(new ReplayCharacterSet(), replayLength);

            var game = new Game();
            if (!HandleCommandLineArgs(game, args))
            {
                ShowHelp(options);
                return;
            }

            Console.WriteLine("Mutation Rate: {0}%", mutationRate * 100);
            Console.WriteLine("Crossover Rate: {0}%", crossoverRate * 100);
            Console.WriteLine("Truncation Rate: {0}%", truncationRate * 100);
            Console.WriteLine("Chromosomes / population: {0}", chromosomeCount);
            Console.WriteLine("Elitism / population: {0} ({1}%)", elitismRate * chromosomeCount, elitismRate);
            Console.WriteLine("Fitness Calculator: {0}", fitness);
            Console.WriteLine("Crossover Type: {0}", crossover);
            Console.WriteLine();

            //game.Start(new Replay("AAAA A00A AAAA AAAA AAA2 8000 AAAA AAAA AAAA AAAA AAAA AAAA AAAA AAAA AAAA AAAA AAAA"));
            var fitnessCalculator = new HighestScoreFitnessCalculator(game);

            var ga = new GeneticAlgorithm(0.6, 0.25, 0.1, 0.8, chromosomeCount, 
                new RouletteWheelSelection(), 
                new OnePointCrossover(fitnessCalculator), 
                new SinglePointMutation(new ReplayCharacterSet(),fitnessCalculator), 
                fitnessCalculator);

            Console.WriteLine(DateTime.Now);
            Population population = new Population(GenerateRandomReplays(replayGenerator, chromosomeCount), fitnessCalculator);
            int generation = 1;
            while(true)
            {
                var bestOne = population.ElementAt(0);

                Console.WriteLine("Generation {0}", generation++);
                Console.WriteLine("Top fitness: {0}", bestOne.Fitness);
                Console.WriteLine("Replay: {0}\n", bestOne.Replay);

                population = ga.AdvancePopulation(population);
            }
        }

        static List<Replay> GenerateRandomReplays(IReplayGenerator replayGenerator, int count)
        {
            var replies = new List<Replay>();
            for (int i = 0; i < count; i++)
            {
                replies.Add(replayGenerator.Generate());
            }
            return replies;
        }

        static bool HandleCommandLineArgs(Game game, IEnumerable<string> args)
        {
            string fitnessType = "", crossoverType = "";
            bool status = true;
            options = new OptionSet
                          {
                              {"m|mutation=", "The mutation rate (0-1)", (double v) => mutationRate = v},
                              {"s|crossover=", "The crossover rate (0-1)", (double v) => crossoverRate = v},
                              {"e|elitism=", "The elitism rate (0-1)", (double v) => elitismRate = v},
                              {"c|crcount=", "The number of chromosomes per population (>1)", (int v) => chromosomeCount = v},
                              {"fitness=", "The fitness calculator [sum | levenshtein | hamming]", v => fitnessType = v},
                              {"ctype=", "The crossover type [one | two ]", v => crossoverType = v},
                              {"t|truncate=", "The rate of the chromosomes to keep from a population before advancing (0 < t <= 1)", (double v) => truncationRate = v},
                              {"?|h|help", "Show help", v => { status = false; }},
                              //{"<>", v => target = v} // this can be used for a seed
                          };
            try
            {
                options.Parse(args);
            }
            catch (OptionException ex)
            {
                status = false;
            }

            fitness = (FitnessCalculator)Activator.CreateInstance(GetOperationType(fitnessTypes, fitnessType), new object[] { game });
            crossover = (ICrossover)Activator.CreateInstance(GetOperationType(crossoverTypes, crossoverType), new object[] { fitness });
            mutation = new SinglePointMutation(new ReplayCharacterSet(), fitness);
            selection = new RouletteWheelSelection();

            if (mutationRate > 1 || crossoverRate > 1 || elitismRate > 1 || chromosomeCount <= 1 || truncationRate <= 0 || truncationRate > 1)
            {
                status = false;
            }

            return status;
        }

        static Type GetOperationType(Dictionary<string, Type> types, string argument)
        {
            argument = argument.ToLower();
            return types.ContainsKey(argument) ? types[argument] : types.First().Value;
        }

        static void ShowHelp(OptionSet options)
        {
            Console.WriteLine("Usage: {0} [options] <destination>\n", AppDomain.CurrentDomain.FriendlyName);
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}
