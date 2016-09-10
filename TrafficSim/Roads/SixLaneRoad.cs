

namespace TrafficSim.Roads
{
    public class SixLaneRoad : Road
    {
 

        SixLaneRoad(Point startPoint, Point endPoint) 
        {
            NumberOfLanes = 6;
            TimeToTraverse = 40;
            Cost = 7;
        }
        
    }

   
}
