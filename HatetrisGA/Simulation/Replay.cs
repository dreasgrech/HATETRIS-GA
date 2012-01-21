using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.Simulation
{
    class Replay : IEnumerable<Transform>
    {
        private readonly IEnumerable<char> replay;
        private readonly List<Transform> transforms;

        public Replay(IEnumerable<char> replay)
        {
            this.replay = replay;
            transforms = Convert(replay);
        }

        private static List<Transform> Convert(IEnumerable<char> replay)
        {
            var moves = new List<Transform>();
            foreach (var replayMove in replay)
            {
                switch (replayMove)
                {
                    case '0': moves.Add(Transform.Left); moves.Add(Transform.Left); break;
                    case '1': moves.Add(Transform.Left); moves.Add(Transform.Right); break;
                    case '2': moves.Add(Transform.Left); moves.Add(Transform.Down); break;
                    case '3': moves.Add(Transform.Left); moves.Add(Transform.Up); break;
                    case '4': moves.Add(Transform.Right); moves.Add(Transform.Left); break;
                    case '5': moves.Add(Transform.Right); moves.Add(Transform.Right); break;
                    case '6': moves.Add(Transform.Right); moves.Add(Transform.Down); break;
                    case '7': moves.Add(Transform.Right); moves.Add(Transform.Up); break;
                    case '8': moves.Add(Transform.Down); moves.Add(Transform.Left); break;
                    case '9': moves.Add(Transform.Down); moves.Add(Transform.Right); break;
                    case 'A': moves.Add(Transform.Down); moves.Add(Transform.Down); break;
                    case 'B': moves.Add(Transform.Down); moves.Add(Transform.Up); break;
                    case 'C': moves.Add(Transform.Up); moves.Add(Transform.Left); break;
                    case 'D': moves.Add(Transform.Up); moves.Add(Transform.Right); break;
                    case 'E': moves.Add(Transform.Up); moves.Add(Transform.Down); break;
                    case 'F': moves.Add(Transform.Up); moves.Add(Transform.Up); break;
                }
            }

            return moves;
        }

        public IEnumerator<Transform> GetEnumerator()
        {
            return transforms.GetEnumerator();
        }

        public override string ToString()
        {
            return String.Concat(replay);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Transform Shift()
        {
            var t = transforms[0];
            transforms.RemoveAt(0);
            return t;
        }
    }
}
