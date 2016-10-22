using TrafficSim.PersonNavigation;

namespace TrafficSim.Roads
{
    public abstract class Road : IRoad
    {
        private const ushort OccupancyPerLane = 4;
        public int NumberOfLanes { get; set; }
        public int TimeToTraverse { get; set; }


        public double Cost { get; set; }
        public ushort DirectionAOccupancy { get; set; }
        public ushort DirectionBOccupancy { get; set; }


        public Point Location { get; set; }
        public string ClassName { get; set; }

        
        public bool MergeToRoad(Direction headedDirection)
        {
            if (headedDirection == Direction.East || headedDirection == Direction.North)
            {
                if (DirectionAOccupancy > NumberOfLanes*OccupancyPerLane) return false;
                DirectionAOccupancy++;
                return true;
            }
            if (DirectionBOccupancy >= NumberOfLanes*OccupancyPerLane) return false;
            DirectionBOccupancy++;
            return true;
        }

        public void LeaveRoad(Direction oldHeadedDirection)
        {
            if (oldHeadedDirection == Direction.East || oldHeadedDirection == Direction.North)
            {
                DirectionAOccupancy--;
            }
            else
            {
                DirectionBOccupancy--;
            }
        }
    }
}