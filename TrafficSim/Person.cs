
using System.Collections.Generic;
using TrafficSim_API.SimSrc.Roads;

namespace TrafficSim_API.SimSrc
{
    public class Person
    {
        public Home Home { get; set; }
        public Office Work { get; set; }
        public Point CurrentLocation { get; set; }
        public byte StartTime { get; set; }
        public byte EndTime { get; set; }
        public List<Direction> PathToWork { get; set; }
        private static ITile[,] _map;
        


        public Person(Home home, Office work, byte startByte, byte endByte, ITile [,] mapTiles)
        {
            Home = home;
            Work = work;
            StartTime = startByte;
            EndTime = endByte;
            CurrentLocation = Home.Location;
            _map = mapTiles;
            CalculatePathToWork();
        }

        public void CalculatePathToWork()
        {
            ulong timeToWork = 0;
            bool isFinsihed = false;
            Point currentPos = Home.Location;
            /*while (!isFinsihed)
            {
                if()
            }*/
        }

        public void Update()
        {
            
        }

        private bool CrossIntersection(Intersection intersection)
        {
            return true;
        }

    }
}
