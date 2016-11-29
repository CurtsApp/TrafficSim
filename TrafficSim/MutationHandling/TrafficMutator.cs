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
        private List<Intersection> allIntersections;
        private Random rand = new Random();
        public TrafficMutator(List<Intersection> intersections)
        {
            allIntersections = intersections;
        }

        public Mutation GetNextMutation()
        {
            Intersection intersection = allIntersections[rand.Next(0, allIntersections.Count)];
            return new Mutation(intersection.Location, rand.Next(-10,11));
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
