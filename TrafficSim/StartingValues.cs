using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSim_API.SimSrc
{
    public class StartingValues
    {
        public byte MapWidth;
        public byte MapHeight;
        public ulong Budget;
        public ulong Population;
        public ulong TrafficLightCycleTimeDefault;
    }
}
