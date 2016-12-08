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
        private ulong totalChangesMade = 0;
        private ulong toatlChangesKept = 0;
        private ulong cyclesSinceLastKeptChange = 0;
        private Random rand = new Random();

        public Mutation GetNextMutation()
        {
            Intersection intersection = City._intersections[rand.Next(0, City._intersections.Count)];
            Mutation change = new Mutation(intersection, rand.Next(-100, 101));
            lastChange = change;
            toatlChangesKept++;
            totalChangesMade++;
            return change;
        }

        public Mutation GetRevertLastChange()
        {
            Mutation previousChange = GetLastMutation();
            previousChange.ChangeAmount = previousChange.ChangeAmount * -1;
            toatlChangesKept--;
            cyclesSinceLastKeptChange++;
            return previousChange;
        }
        public ulong GetLastTimeInTraffic()
        {
            return lastTimeInTraffic;
        }

        
        public void UpdateLastTimeInTraffic(ulong timeInTraffic)
        {
            lastTimeInTraffic = timeInTraffic;
            cyclesSinceLastKeptChange = 0;
        }

        public Mutation GetLastMutation()
        {
            return lastChange;
        }

        public ulong GetTotalChangesMade()
        {
            return totalChangesMade;
        }

        public ulong GetNumberOfCyclesSinceLastKeptChange()
        {
            return cyclesSinceLastKeptChange;
        }
    }
}
