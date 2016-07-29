
using System;
using TrafficSim_API.SimSrc;
using TrafficSim_API.SimSrc.Roads;

namespace TrafficSim.Roads
{
    public class TwoLaneRoadOneWay : IRoad
    {
        
        public int NumberOfLanes { get; set; }
        public int SpeedLimit { get; set; }
        public Point Start { get; set; }
        public Point End { get; set; }
public double Cost { get; set; }
        public ushort DirectionAOccupancy { get; set; }
        public ushort DirectionBOccupancy { get; set; }

        TwoLaneRoadOneWay(Point startPoint, Point endPoint) 
        {
            NumberOfLanes = 2;
            SpeedLimit = 40;
            Start = startPoint;
            End = endPoint;
            Cost = 3;
        }

        public void Update()
        {
            
        }

        public Point Location { get; set; }
    }

   
}
