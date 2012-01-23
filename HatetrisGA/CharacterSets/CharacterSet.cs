using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.CharacterSets
{
    abstract class CharacterSet
    {
        private readonly Random random;

        protected CharacterSet()
        {
            random = new Random();   
        }

        public abstract IEnumerable<char> GetCharacters();

        public char GetRandomCharacter()
        {
            var characters = GetCharacters();
            return characters.ElementAt(random.Next(0, characters.Count()));
        }
    }
}
