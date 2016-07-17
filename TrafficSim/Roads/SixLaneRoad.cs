
using System;

namespace TrafficSim_API.SimSrc.Roads
{
    public class SixLaneRoad : IRoad
    {
        
        public int NumberOfLanes { get; set; }
        public int SpeedLimit { get; set; }
        public Point Start { get; set; }
        public Point End { get; set; }
        public double Cost { get; set; }
        public ushort DirectionAOccupancy { get; set; }
        public ushort DirectionBOccupancy { get; set; }

        SixLaneRoad(Point startPoint, Point endPoint) 
        {
            NumberOfLanes = 6;
            SpeedLimit = 50;
            Cost = 7;
            Start = startPoint;
            End = endPoint;
        }

        public void Update()
        {
            
        }

        public Point Location { get; set; }
    }

   
}
