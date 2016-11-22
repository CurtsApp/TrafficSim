using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.MutationHandling
{
    class Mutation
    {
        public Point Location;
        public int ChangeAmount;

        public Mutation(Point location, int changeAmount)
        {
            Location = new Point(location.GetX(), location.GetY());
            ChangeAmount = changeAmount;
        }
    }
}
