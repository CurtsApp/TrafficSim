

namespace TrafficSim.Roads
{
    public class ThreeLaneRoadOneWay : Road
    {
        
     

        ThreeLaneRoadOneWay(Point startPoint, Point endPoint) 
        {
            NumberOfLanes = 3;
            TimeToTraverse = 30;
            Start = startPoint;
            End = endPoint;
        }

        
    }

   
}
