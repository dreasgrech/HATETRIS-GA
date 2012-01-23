using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.CharacterSets
{
    class ReplayCharacterSet:CharacterSet
    {
        private readonly IEnumerable<char> allowed;

        public ReplayCharacterSet()
        {
            allowed = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        }
        public override IEnumerable<char> GetCharacters()
        {
            return allowed;
        }
    }
}
