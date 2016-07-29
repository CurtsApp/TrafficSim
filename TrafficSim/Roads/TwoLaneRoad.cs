
using System;
using TrafficSim_API.SimSrc;
using TrafficSim_API.SimSrc.Roads;

namespace TrafficSim.Roads
{
    public class TwoLaneRoad : IRoad
    {
        
        public int NumberOfLanes { get; set; }
        public int SpeedLimit { get; set; }
        public double Cost { get; set; }
        public ushort DirectionAOccupancy { get; set; }
        public ushort DirectionBOccupancy { get; set; }

        public TwoLaneRoad(Point location) 
        {
            NumberOfLanes = 2;
            SpeedLimit = 25;
            Location = location;
            Cost = 3;
        }

        public void Update()
        {
            
        }

        public Point Location { get; set; }
    }

   
}
