using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficSim.PersonNavigation;
using TrafficSim_API.SimSrc;

namespace TrafficSim.PersonNavigation
{
    public class PathPossibilities
    {
        public List<Direction> Options { get; set; }
        public Point Location { get; set; }
        public PathPossibilities(Point startPoint)
        {
            Options = new List<Direction>();
            Location = startPoint;
        }
    }
}
