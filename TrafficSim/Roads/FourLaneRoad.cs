
using System;

namespace TrafficSim_API.SimSrc.Roads
{
    public class FourLaneRoad : Road
    {
        

        public FourLaneRoad(Point startPoint, Point endPoint) 
        {
            NumberOfLanes = 4;
            SpeedLimit = 45;
            Start = startPoint;
            End = endPoint;
            Cost = 5;
        }


    }

   
}
