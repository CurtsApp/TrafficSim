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
        private int lastTimeInTraffic;
        private List<Intersection> allIntersections;
        public TrafficMutator(List<Intersection> intersections)
        {
            allIntersections = intersections;
        }

        public Mutation getNextMutation(int timeInTraffic)
        {
            if (timeInTraffic > lastTimeInTraffic)
            {
                
            }
            return  new Mutation(new Point(1,1), 1 );
        }
    }
}
