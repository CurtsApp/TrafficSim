
namespace TrafficSim.Roads
{
    public class FourLaneRoad : Road
    {
        

        public FourLaneRoad(Point startPoint, Point endPoint) 
        {
            NumberOfLanes = 4;
            TimeToTraverse = 35;
            Start = startPoint;
            End = endPoint;
            Cost = 5;
        }


    }

   
}
