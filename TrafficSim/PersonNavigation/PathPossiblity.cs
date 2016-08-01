
using System.Collections.Generic;


namespace TrafficSim.PersonNavigation
{
    public class PathPossiblity
    {
        public List<Direction> Directions { get; set; }
        public Direction NextDirection { get; set; }
        public PathPossiblity()
        {
            Directions = new List<Direction>();
        }
    }
}
