﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSim_API.SimSrc.Roads
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
        public int SpeedLimit { get; set; }

        Point IRoad.Location
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Point Start { get; set; }
        public Point End { get; set; }
        public double Cost { get; set; }
        public bool IsDirectionAFull { get; set; }
        public bool IsDirectionBFull { get; set; }
        public ushort DirectionAOccupancy { get; set; }
        public ushort DirectionBOccupancy { get; set; }

        
    }
}
