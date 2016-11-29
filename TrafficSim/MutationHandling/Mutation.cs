using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.Roads;

namespace TrafficSim.MutationHandling
{
    class Mutation
    {
        public Intersection Intersection;
        public int ChangeAmount;

        public Mutation(Intersection intersection, int changeAmount)
        {
            Intersection = intersection;
            ChangeAmount = changeAmount;
        }
    }
}
