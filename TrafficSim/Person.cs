

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
        public PathPossiblity PathToWork { get; set; }
        private static ITile[,] _map;
        private int[,] PathingHelper { get; }
        public static int PersonTracker = 0;


        public Person(Home home, Office work, byte startByte, byte endByte, ITile[,] mapTiles, int[,] travelTimes)
        {
            PersonTracker++;
            Home = home;
            Work = work;
            StartTime = startByte;
            EndTime = endByte;
            CurrentLocation = Home.Location;
            _map = mapTiles;

            PathingHelper = (int[,]) travelTimes.Clone();
            CalculatePathToWork();
        }

        private void CalculatePathToWork()
        {
            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 25; x++)
                {


                    if (x == Home.Location.GetX() && y == Home.Location.GetY())
                    {
                        Console.Write("X  ");
                    }
                    else if (x == Work.Location.GetX() && y == Work.Location.GetY())
                    {
                        Console.Write("Y  ");
                    }
                    else
                    {
                        if (PathingHelper[x, y] == 0f)
                        {
                            Console.Write("{0}  ", PathingHelper[x, y]);
                        }
                        else
                        {
                            Console.Write("{0} ", PathingHelper[x, y]);
                        }
                        
                    }
                    /*else if (_map[x, y] is TwoLaneRoad)
                    {
                        Console.Write("  ");
                    }
                    else if (_map[x, y] is Home)
                    {
                        Console.Write("H ");
                    }
                    else if (_map[x, y] is Office)
                    {
                        Console.Write("O ");
                    }
                    else if (_map[x, y] is Intersection)
                    {
                        Console.Write("  ");
                    }
                    else if (_map[x, y] is Vacant)
                    {
                        Console.Write("V ");
                    }*/


                }
                Console.WriteLine();
            }

            var unfinsihedPathPossiblity = new List<PathPossiblity>();
            PathPossiblity finishedPathPossibility = new PathPossiblity();

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
                    unfinsihedPathPossiblity.Add(directionAPossiblity);
                    var directionBPosiblity = new PathPossiblity();
                    directionBPosiblity.Directions.Add(Direction.West);
                    directionBPosiblity.NextDirection = Direction.South;
                    unfinsihedPathPossiblity.Add(directionBPosiblity);
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
                    directionAPossiblity.Directions.Add(Direction.East);
                    directionAPossiblity.NextDirection = Direction.North;
                    unfinsihedPathPossiblity.Add(directionAPossiblity);
                    var directionBPosiblity = new PathPossiblity();
                    directionBPosiblity.Directions.Add(Direction.East);
                    directionBPosiblity.NextDirection = Direction.South;
                    unfinsihedPathPossiblity.Add(directionBPosiblity);
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
                    directionAPossiblity.Directions.Add(Direction.North);
                    directionAPossiblity.NextDirection = Direction.East;
                    unfinsihedPathPossiblity.Add(directionAPossiblity);
                    var directionBPosiblity = new PathPossiblity();
                    directionBPosiblity.Directions.Add(Direction.North);
                    directionBPosiblity.NextDirection = Direction.West;
                    unfinsihedPathPossiblity.Add(directionBPosiblity);
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
                    directionAPossiblity.Directions.Add(Direction.South);
                    directionAPossiblity.NextDirection = Direction.East;
                    unfinsihedPathPossiblity.Add(directionAPossiblity);
                    var directionBPosiblity = new PathPossiblity();
                    directionBPosiblity.Directions.Add(Direction.South);
                    directionBPosiblity.NextDirection = Direction.West;
                    unfinsihedPathPossiblity.Add(directionBPosiblity);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            //Main Path Finding Loop

            while (unfinsihedPathPossiblity.Count > 0)
            {
                StepBranchedSearch(unfinsihedPathPossiblity);
                
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
                if (pointToBeChecked.GetX().Equals(currentLocation.GetX()) && pointToBeChecked.GetY().Equals(currentLocation.GetY()))
                {
                    return true;
                }
            }

            return false;
        }

        private void StepBranchedSearch(List<PathPossiblity> unfinishedPathPossiblities)
        {
            
            for (var i = 0; i < unfinishedPathPossiblities.Count; i++)
            {
                
                    
                    var currentPos = new Point(Home.Location.GetX(), Home.Location.GetY());
                    //Find where we are at along the path
                    foreach (var direction in unfinishedPathPossiblities[i].Directions)
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
                    switch (unfinishedPathPossiblities[i].NextDirection)
                    {
                        case Direction.East:
                            currentPos.SetX(currentPos.GetX() + 1);
                            unfinishedPathPossiblities[i].PathLength = unfinishedPathPossiblities[i].PathLength +
                                                                       PathingHelper[
                                                                           currentPos.GetX(), currentPos.GetY()];
                            break;
                        case Direction.North:
                            currentPos.SetY(currentPos.GetY() + 1);
                            unfinishedPathPossiblities[i].PathLength = unfinishedPathPossiblities[i].PathLength +
                                                                       PathingHelper[
                                                                           currentPos.GetX(), currentPos.GetY()];
                        break;
                        case Direction.South:
                            currentPos.SetY(currentPos.GetY() - 1);
                            unfinishedPathPossiblities[i].PathLength = unfinishedPathPossiblities[i].PathLength +
                                                                       PathingHelper[
                                                                           currentPos.GetX(), currentPos.GetY()];
                        break;
                        case Direction.West:
                            currentPos.SetX(currentPos.GetX() - 1);
                            unfinishedPathPossiblities[i].PathLength = unfinishedPathPossiblities[i].PathLength +
                                                                       PathingHelper[
                                                                           currentPos.GetX(), currentPos.GetY()];
                        break;
                    }
                    //After position calculation add the NextDirection to List<Directions> for future copying
                    unfinishedPathPossiblities[i].Directions.Add(unfinishedPathPossiblities[i].NextDirection);

                //Remove paths longer than the shortest path found
                if (unfinishedPathPossiblities[i].PathLength > PathToWork?.PathLength)
                {
                    unfinishedPathPossiblities.RemoveAt(i);
                    i--;
                }
                //See if the path has arrived at the office
                    else if (currentPos.GetX() + 1 == Work.Location.GetX() && currentPos.GetY() == Work.Location.GetY())
                    {
                        unfinishedPathPossiblities[i].Directions.Add(Direction.East);
                        PathToWork = unfinishedPathPossiblities[i];
                        unfinishedPathPossiblities.RemoveAt(i);
                        i--;
                }
                    else if (currentPos.GetX() - 1 == Work.Location.GetX() && currentPos.GetY() == Work.Location.GetY())
                    {
                        unfinishedPathPossiblities[i].Directions.Add(Direction.West);
                    PathToWork = unfinishedPathPossiblities[i];
                    unfinishedPathPossiblities.RemoveAt(i);
                        i--;
                }
                    else if (currentPos.GetX() == Work.Location.GetX() && currentPos.GetY() == Work.Location.GetY() + 1)
                    {
                        unfinishedPathPossiblities[i].Directions.Add(Direction.North);
                    PathToWork = unfinishedPathPossiblities[i];
                    unfinishedPathPossiblities.RemoveAt(i);
                        i--;
                }
                    else if (currentPos.GetX() == Work.Location.GetX() && currentPos.GetY() == Work.Location.GetY() - 1)
                    {
                        unfinishedPathPossiblities[i].Directions.Add(Direction.South);
                        PathToWork = unfinishedPathPossiblities[i];
                        unfinishedPathPossiblities.RemoveAt(i);
                        i--;
                }
                    //If the office hasn't been found add another round of possiblies
                    else
                    {


                        //TODO Out of Index error, out of bounds + and - for x and y. Randomly steps 1 West and then turns South in some instances. Sometimes South and then west. Then east to north. East to South
                        //Diagnosis: The current location gets off the road and then runs off the edge. Edge calculation is only done at intersections.
                        //Cure: Prevent car from leaving roadway...Start at seeing if on top of intersection

                        //Means you are currently on top of an intersection
                        if (PathingHelper[currentPos.GetX(), currentPos.GetY()] < 0)
                        {
                            //No Uturns
                            //Scrubbing Out of index
                            if (currentPos.GetX() != PathingHelper.GetLength(0) - 1)
                            {
                                if (PathingHelper[currentPos.GetX() + 1, currentPos.GetY()] > 0 &&
                                    unfinishedPathPossiblities[i].NextDirection != Direction.West) //South
                                {
                                    var buffer = new Point(currentPos.GetX() + 1, currentPos.GetY());
                                    if (!CheckForBackTrack(buffer, unfinishedPathPossiblities[i].Directions, Home.Location))
                                    {
                                        var newPossiblity = new PathPossiblity();
                                        //Copy the list by value
                                        foreach (var direction in unfinishedPathPossiblities[i].Directions)
                                        {
                                            newPossiblity.Directions.Add(direction);
                                        }
                                        newPossiblity.NextDirection = Direction.East;
                                        unfinishedPathPossiblities.Add(newPossiblity);
                                    }
                                }
                            }
                            //Scrubbing for out of index
                            if (currentPos.GetX() != 0)
                            {
                                if (PathingHelper[currentPos.GetX() - 1, currentPos.GetY()] > 0 &&
                                    unfinishedPathPossiblities[i].NextDirection != Direction.East) //North
                                {
                                    var buffer = new Point(currentPos.GetX() - 1, currentPos.GetY());
                                    if (!CheckForBackTrack(buffer, unfinishedPathPossiblities[i].Directions, Home.Location))
                                    {
                                        var newPossiblity = new PathPossiblity();
                                        //Copy the list by value
                                        foreach (var direction in unfinishedPathPossiblities[i].Directions)
                                        {
                                            newPossiblity.Directions.Add(direction);
                                        }
                                        newPossiblity.NextDirection = Direction.West;
                                        unfinishedPathPossiblities.Add(newPossiblity);
                                    }
                                }
                            }
                            if (currentPos.GetY() != PathingHelper.GetLength(1) - 1)
                            {
                                if (PathingHelper[currentPos.GetX(), currentPos.GetY() + 1] > 0 &&
                                    unfinishedPathPossiblities[i].NextDirection != Direction.South) //West
                                {
                                    var buffer = new Point(currentPos.GetX(), currentPos.GetY() + 1);
                                    if (!CheckForBackTrack(buffer, unfinishedPathPossiblities[i].Directions, Home.Location))
                                    {
                                        var newPossiblity = new PathPossiblity();
                                        //Copy the list by value
                                        foreach (var direction in unfinishedPathPossiblities[i].Directions)
                                        {
                                            newPossiblity.Directions.Add(direction);
                                        }
                                        newPossiblity.NextDirection = Direction.North;
                                        unfinishedPathPossiblities.Add(newPossiblity);
                                    }
                                }
                            }
                            if (currentPos.GetY() != 0)
                            {
                                if (PathingHelper[currentPos.GetX(), currentPos.GetY() - 1] > 0 &&
                                    unfinishedPathPossiblities[i].NextDirection != Direction.North) //East
                                {
                                    var buffer = new Point(currentPos.GetX(), currentPos.GetY() - 1);
                                    if (!CheckForBackTrack(buffer, unfinishedPathPossiblities[i].Directions, Home.Location))
                                    {
                                        var newPossiblity = new PathPossiblity();
                                        //Copy the list by value
                                        foreach (var direction in unfinishedPathPossiblities[i].Directions)
                                        {
                                            newPossiblity.Directions.Add(direction);
                                        }
                                        newPossiblity.NextDirection = Direction.South;
                                        unfinishedPathPossiblities.Add(newPossiblity);
                                    }
                                }
                            }
                            //Remove the path that ends at the intersection
                            unfinishedPathPossiblities.RemoveAt(i);
                            i--;


                        }

                    }
                
            }
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
