using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HatetrisGA.Simulation;

namespace HatetrisGA.ReplayGenerators
{
    interface IReplayGenerator
    {
        Replay Generate();
    }
}
