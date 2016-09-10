

namespace TrafficSim.Roads
{
    public class TwoLaneRoadOneWay : Road
    {
   

        TwoLaneRoadOneWay(Point startPoint, Point endPoint) 
        {
            NumberOfLanes = 2;
            TimeToTraverse = 40;
            Cost = 3;
        }

       
    }

   
}
