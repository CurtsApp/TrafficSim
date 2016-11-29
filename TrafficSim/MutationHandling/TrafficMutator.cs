using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.Roads;

namespace TrafficSim.MutationHandling
{
    class TrafficMutator
    {
        private Mutation lastChange;
        private ulong lastTimeInTraffic;
        private Random rand = new Random();
        public TrafficMutator()
        {
            
        }

        public Mutation GetNextMutation()
        {
            Intersection intersection = City._intersections[rand.Next(0, City._intersections.Count)];
            Mutation change = new Mutation(intersection, rand.Next(-10, 11));
            lastChange = change;
            return change;
        }

        public ulong GetLastTimeInTraffic()
        {
            return lastTimeInTraffic;
        }

        public void UpdateLastTimeInTraffic(ulong timeInTraffic)
        {
            lastTimeInTraffic = timeInTraffic;
        }

        public Mutation GetLastMutation()
        {
            return lastChange;
        }
    }
}
