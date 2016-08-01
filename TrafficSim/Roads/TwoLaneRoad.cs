
namespace TrafficSim.Roads
{
    public class TwoLaneRoad : Road
    {
  

        public TwoLaneRoad(Point location) 
        {
            NumberOfLanes = 2;
            TimeToTraverse = 55;
            Location = location;
            Cost = 3;
        }

    }

   
}
