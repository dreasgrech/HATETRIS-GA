using System;
using System.Collections.Generic;
using System.Linq;
using HatetrisGA.CharacterSets;
using HatetrisGA.Simulation;

namespace HatetrisGA.ReplayGenerators
{
    class RandomReplayGenerator:IReplayGenerator
    {
        public int Length { get; set; }

        private readonly Randomizer<char> randomizer = new Randomizer<char>();
        private readonly CharacterSet characterSet;

        public RandomReplayGenerator(CharacterSet characterSet, int length)
        {
            this.characterSet = characterSet;
            Length = length;
        }

        public Replay Generate()
        {
            var replayData = new List<char>();
            for (int i = 0; i < Length; i++)
            {
                replayData.Add(randomizer.GetRandom(characterSet.GetCharacters()));
            }

            return new Replay(replayData);
        }
    }
}
