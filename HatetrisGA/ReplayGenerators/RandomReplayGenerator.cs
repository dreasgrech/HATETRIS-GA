using System;
using System.Collections.Generic;
using HatetrisGA.Simulation;

namespace HatetrisGA.ReplayGenerators
{
    class RandomReplayGenerator:IReplayGenerator
    {
        public int Length { get; set; }

        private readonly char[] allowedCharacters = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
        private readonly Randomizer<char> randomizer = new Randomizer<char>();

        public RandomReplayGenerator(int length)
        {
            Length = length;
        }

        public Replay Generate()
        {
            var replay = new List<char>();
            for (int i = 0; i < Length; i++)
            {
                replay.Add(randomizer.GetRandom(allowedCharacters));
            }

            return new Replay(replay);
        }
    }
}
