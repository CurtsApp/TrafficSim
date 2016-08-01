

using System;
using System.Collections.Generic;
using TrafficSim.PersonNavigation;
using TrafficSim.Roads;

namespace TrafficSim
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
        private float[,] PathingHelper { get; }
        public static int PersonTracker = 0;


        public Person(Home home, Office work, byte startByte, byte endByte, ITile[,] mapTiles, float[,] travelTimes)
        {
            PersonTracker++;
            Home = home;
            Work = work;
            StartTime = startByte;
            EndTime = endByte;
            CurrentLocation = Home.Location;
            _map = mapTiles;

            PathingHelper = (float[,]) travelTimes.Clone();
            CalculatePathToWork();
        }

        private void CalculatePathToWork()
        {

            var allPathPossiblity = new List<PathPossiblity>();
            var bestCantidate = 0;
            //Avoiding reference error
            var currentLocation = new Point(Home.Location.GetX(), Home.Location.GetY());

            //Left One
            try
            {
                if (PathingHelper[currentLocation.GetX() - 1, currentLocation.GetY()] > 0)
                {
                    var directionAPossiblity = new PathPossiblity();
                    directionAPossiblity.Directions.Add(Direction.West);
                    directionAPossiblity.NextDirection = Direction.North;
                    allPathPossiblity.Add(directionAPossiblity);
                    var directionBPosiblity = new PathPossiblity();
                    directionBPosiblity.Directions.Add(Direction.West);
                    directionBPosiblity.NextDirection = Direction.South;
                    allPathPossiblity.Add(directionBPosiblity);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            //Right One
            try
            {
                if (PathingHelper[currentLocation.GetX() + 1, currentLocation.GetY()] > 0)
                {
                    var directionAPossiblity = new PathPossiblity();
                    directionAPossiblity.Directions.Add(Direction.West);
                    directionAPossiblity.NextDirection = Direction.North;
                    allPathPossiblity.Add(directionAPossiblity);
                    var directionBPosiblity = new PathPossiblity();
                    directionBPosiblity.Directions.Add(Direction.West);
                    directionBPosiblity.NextDirection = Direction.South;
                    allPathPossiblity.Add(directionBPosiblity);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            } //Up One
            try
            {
                if (PathingHelper[currentLocation.GetX(), currentLocation.GetY() + 1] > 0)
                {
                    var directionAPossiblity = new PathPossiblity();
                    directionAPossiblity.Directions.Add(Direction.West);
                    directionAPossiblity.NextDirection = Direction.North;
                    allPathPossiblity.Add(directionAPossiblity);
                    var directionBPosiblity = new PathPossiblity();
                    directionBPosiblity.Directions.Add(Direction.West);
                    directionBPosiblity.NextDirection = Direction.South;
                    allPathPossiblity.Add(directionBPosiblity);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            } //Down One
            try
            {
                if (PathingHelper[currentLocation.GetX(), currentLocation.GetY() - 1] > 0)
                {
                    var directionAPossiblity = new PathPossiblity();
                    directionAPossiblity.Directions.Add(Direction.West);
                    directionAPossiblity.NextDirection = Direction.North;
                    allPathPossiblity.Add(directionAPossiblity);
                    var directionBPosiblity = new PathPossiblity();
                    directionBPosiblity.Directions.Add(Direction.West);
                    directionBPosiblity.NextDirection = Direction.South;
                    allPathPossiblity.Add(directionBPosiblity);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            for (int i = 0; i < allPathPossiblity.Count; i++)
            {
                {
                    var currentPos = new Point(Home.Location.GetX(), Home.Location.GetY());
                    //Find where we are at along the pathh
                    foreach (var direction in allPathPossiblity[i].Directions)
                    {
                        switch (direction)
                        {
                            case Direction.East:
                                currentPos.SetX(currentPos.GetX() + 1);
                                break;
                            case Direction.North:

                                currentPos.SetY(currentPos.GetY() + 1);
                                break;
                            case Direction.South:
                                currentPos.SetY(currentPos.GetY() - 1);
                                break;
                            case Direction.West:
                                currentPos.SetX(currentPos.GetX() - 1);
                                break;
                        }
                    }
                    switch (allPathPossiblity[i].NextDirection)
                    {
                        case Direction.East:
                            currentPos.SetX(currentPos.GetX() + 1);
                            break;
                        case Direction.North:

                            currentPos.SetY(currentPos.GetY() + 1);
                            break;
                        case Direction.South:
                            currentPos.SetY(currentPos.GetY() - 1);
                            break;
                        case Direction.West:
                            currentPos.SetX(currentPos.GetX() - 1);
                            break;
                    }
                    //After position calculation add the NextDirection to List<Directions> for future copying
                    allPathPossiblity[i].Directions.Add(allPathPossiblity[i].NextDirection);

                    //See if the path has arrived at the office
                    if (currentPos.GetX() + 1 == Work.Location.GetX() && currentPos.GetY() == Work.Location.GetY())
                    {
                        allPathPossiblity[i].Directions.Add(Direction.East);
                    }
                    else if (currentPos.GetX() - 1 == Work.Location.GetX() && currentPos.GetY() == Work.Location.GetY())
                    {
                        allPathPossiblity[i].Directions.Add(Direction.West);
                    }
                    else if (currentPos.GetX() == Work.Location.GetX() && currentPos.GetY() == Work.Location.GetY() + 1)
                    {
                        allPathPossiblity[i].Directions.Add(Direction.North);
                    }
                    else if (currentPos.GetX() == Work.Location.GetX() && currentPos.GetY() == Work.Location.GetY() - 1)
                    {
                        allPathPossiblity[i].Directions.Add(Direction.South);
                    }
                    //If the office hasn't been found add another round of possiblies
                    else
                    {
                        //Means you are currently on top of an intersection
                        if (PathingHelper[currentPos.GetX(), currentPos.GetY()] < 0)
                        {
                            //No Uturns
                            //Scrubbing Out of index
                            if (currentPos.GetX() != PathingHelper.GetLength(0) - 1)
                            {
                                if (PathingHelper[currentPos.GetX() + 1, currentPos.GetY()] > 0 &&
                                    allPathPossiblity[i].NextDirection != Direction.South)
                                {
                                    var buffer = new Point(currentPos.GetX() + 1, currentPos.GetY());
                                    if (!CheckForBackTrack(buffer, allPathPossiblity[i].Directions, Home.Location))
                                    {
                                        var newPossiblity = new PathPossiblity();
                                        //Copy the list by value
                                        foreach (var direction in allPathPossiblity[i].Directions)
                                        {
                                            newPossiblity.Directions.Add(direction);
                                        }
                                        newPossiblity.NextDirection = Direction.East;
                                        allPathPossiblity.Add(newPossiblity);
                                    }
                                }
                            }
                            //Scrubbing for out of index
                            if (currentPos.GetX() != 0)
                            {
                                if (PathingHelper[currentPos.GetX() - 1, currentPos.GetY()] > 0 &&
                                    allPathPossiblity[i].NextDirection != Direction.North)
                                {
                                    var buffer = new Point(currentPos.GetX() - 1, currentPos.GetY());
                                    if (!CheckForBackTrack(buffer, allPathPossiblity[i].Directions, Home.Location))
                                    {
                                        var newPossiblity = new PathPossiblity();
                                        //Copy the list by value
                                        foreach (var direction in allPathPossiblity[i].Directions)
                                        {
                                            newPossiblity.Directions.Add(direction);
                                        }
                                        newPossiblity.NextDirection = Direction.West;
                                        allPathPossiblity.Add(newPossiblity);
                                    }
                                }
                            }
                            if (currentPos.GetY() != PathingHelper.GetLength(1) - 1)
                            {
                                if (PathingHelper[currentPos.GetX(), currentPos.GetY() + 1] > 0 &&
                                    allPathPossiblity[i].NextDirection != Direction.West)
                                {
                                    var buffer = new Point(currentPos.GetX(), currentPos.GetY() + 1);
                                    if (!CheckForBackTrack(buffer, allPathPossiblity[i].Directions, Home.Location))
                                    {
                                        var newPossiblity = new PathPossiblity();
                                        //Copy the list by value
                                        foreach (var direction in allPathPossiblity[i].Directions)
                                        {
                                            newPossiblity.Directions.Add(direction);
                                        }
                                        newPossiblity.NextDirection = Direction.North;
                                        allPathPossiblity.Add(newPossiblity);
                                    }
                                }
                            }
                            if (currentPos.GetY() != 0)
                            {
                                if (PathingHelper[currentPos.GetX(), currentPos.GetY() - 1] > 0 &&
                                    allPathPossiblity[i].NextDirection != Direction.East)
                                {
                                    var buffer = new Point(currentPos.GetX(), currentPos.GetY() - 1);
                                    if (!CheckForBackTrack(buffer, allPathPossiblity[i].Directions, Home.Location))
                                    {
                                        var newPossiblity = new PathPossiblity();
                                        //Copy the list by value
                                        foreach (var direction in allPathPossiblity[i].Directions)
                                        {
                                            newPossiblity.Directions.Add(direction);
                                        }
                                        newPossiblity.NextDirection = Direction.South;
                                        allPathPossiblity.Add(newPossiblity);
                                    }
                                }
                            }



                        }

                    }
                }
            }
        }

        private static bool CheckForBackTrack(Point pointToBeChecked, List<Direction> listOfDirections, Point startPoint)
        {
            var currentLocation = new Point(startPoint.GetX(), startPoint.GetY());

            foreach (var direction in listOfDirections)
            {
                switch (direction)
                {
                    case Direction.East:
                        currentLocation.SetX(currentLocation.GetX() + 1);
                        break;
                    case Direction.North:

                        currentLocation.SetY(currentLocation.GetY() + 1);
                        break;
                    case Direction.South:
                        currentLocation.SetY(currentLocation.GetY() - 1);
                        break;
                    case Direction.West:
                        currentLocation.SetX(currentLocation.GetX() - 1);
                        break;
                }
                if (pointToBeChecked.Equals(currentLocation))
                {
                    return true;
                }
            }

            return false;
        }

        private void StepBranchedSearch(List<PathPossiblity> allPathPossiblities)
        {
        }

        /*
         * Path Planning
         * 
         *Generate TimeToTraverse
         Begin at home
         calculate all possible paths to office using no backtrack rule //prevents infinite loops
         run through all paths, find fastest path
         return fastest path 
         * 
         * 
         * 
         */


        public void Update()
        {
        }

        private bool CrossIntersection(Intersection intersection)
        {
            return true;
        }
    }
}
