
using System;
using TrafficSim_API.SimSrc;
using TrafficSim_API.SimSrc.Roads;

namespace TrafficSim.Roads
{
    public class ThreeLaneRoadOneWay : IRoad
    {
        
        public int NumberOfLanes { get; set; }
        public int SpeedLimit { get; set; }
        public Point Start { get; set; }
        public Point End { get; set; }
        public double Cost { get; set; }
        public ushort DirectionAOccupancy { get; set; }
        public ushort DirectionBOccupancy { get; set; }

        ThreeLaneRoadOneWay(Point startPoint, Point endPoint) 
        {
            NumberOfLanes = 3;
            SpeedLimit = 50;
            Start = startPoint;
            End = endPoint;
        }

        public void Update()
        {
            
        }

        public Point Location { get; set; }
    }

   
}
