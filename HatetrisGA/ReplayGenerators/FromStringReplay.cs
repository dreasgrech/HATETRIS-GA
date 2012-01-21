using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HatetrisGA.Simulation;

namespace HatetrisGA.ReplayGenerators
{
    class FromStringReplay:IReplayGenerator
    {
        private readonly string replay;

        public FromStringReplay(string replay)
        {
            this.replay = replay;
        }

        public Replay Generate()
        {
            return new Replay(replay);
        }
    }
}
