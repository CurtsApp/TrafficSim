

using System;

namespace TrafficSim.Roads
{
    public abstract class Road : IRoad
    {


       
        public Road MergeToRoad(string direction, Road currentRoad)
        {
            if (direction.Equals("A"))
            {
                if (DirectionAOccupancy > NumberOfLanes*OccupancyPerLane) return currentRoad;
                DirectionAOccupancy++;
                return this;
            }
            else
            {
                if (DirectionBOccupancy >= NumberOfLanes*OccupancyPerLane) return currentRoad;
                DirectionBOccupancy++;
                return this;
            }
        }

        
        private const ushort OccupancyPerLane = 4;
        public int NumberOfLanes { get; set; }
        public float TimeToTraverse { get; set; }

        
        public Point Start { get; set; }
        public Point End { get; set; }
        public double Cost { get; set; }
        public bool IsDirectionAFull { get; set; }
        public bool IsDirectionBFull { get; set; }
        public ushort DirectionAOccupancy { get; set; }
        public ushort DirectionBOccupancy { get; set; }


        public Point Location { get; set; }

       
    }
}
