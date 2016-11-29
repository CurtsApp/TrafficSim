using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TrafficSim.PersonNavigation;
using TrafficSim.Roads;

namespace TrafficSim
{
    public class Person
    {
        public static int PersonTracker;
        public static int[,] PathingHelper;
        private bool HeadedToWork = true;
        private bool FinishedTraveling;
        private int PathStep;
        public PathPossiblity PathToHome;
        public PathPossiblity PathToWork;
        private ulong TimeInTraffic;


        public Person(Home home, Office work, int[,] pathingHelper)
        {
            PersonTracker++;
            Home = home;
            Work = work;
            CurrentLocation = Home.Location;
            PathingHelper = pathingHelper;

            TimeInTraffic = 0;

            PathToWork = new PathPossiblity();
            PathToHome = new PathPossiblity();
            //From Home to Work
            CalculatePath(Home.Location, Work.Location, ref PathToWork);
            //From Work to Home
            CalculatePath(Work.Location, Home.Location, ref PathToHome);
        }

        [JsonConstructor]
        public Person(Home home, Office work, PathPossiblity pathToWork, PathPossiblity pathToHome)
        {
            PersonTracker++;
            Home = home;
            Work = work;
            CurrentLocation = Home.Location;


            TimeInTraffic = 0;

            PathToWork = pathToWork;
            PathToHome = pathToHome;
        }


        public Home Home { get; set; }
        public Office Work { get; set; }
        public Point CurrentLocation { get; set; }


        private void CalculatePath(Point startPoint, Point endPoint, ref PathPossiblity pathToDestination)
        {
            var unfinsihedPathPossiblity = new List<PathPossiblity>();


            //Avoiding reference error
            var currentLocation = new Point(startPoint.GetX(), startPoint.GetY());

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
                StepBranchedSearch(startPoint, endPoint, unfinsihedPathPossiblity, ref pathToDestination);
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
                if (pointToBeChecked.GetX().Equals(currentLocation.GetX()) &&
                    pointToBeChecked.GetY().Equals(currentLocation.GetY()))
                {
                    return true;
                }
            }

            return false;
        }

        private void StepBranchedSearch(Point startPoint, Point endPoint,
            List<PathPossiblity> unfinishedPathPossiblities, ref PathPossiblity finishedDirections)
        {
            for (var i = 0; i < unfinishedPathPossiblities.Count; i++)
            {
                var currentPos = new Point(startPoint.GetX(), startPoint.GetY());
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
                if (unfinishedPathPossiblities[i].PathLength > finishedDirections.PathLength &&
                    finishedDirections.PathLength != 0)
                {
                    unfinishedPathPossiblities.RemoveAt(i);
                    i--;
                }
                //See if the path has arrived at the office
                else if (currentPos.GetX() + 1 == endPoint.GetX() && currentPos.GetY() == endPoint.GetY())
                {
                    unfinishedPathPossiblities[i].Directions.Add(Direction.East);
                    finishedDirections = unfinishedPathPossiblities[i];
                    unfinishedPathPossiblities.RemoveAt(i);
                    i--;
                }
                else if (currentPos.GetX() - 1 == endPoint.GetX() && currentPos.GetY() == endPoint.GetY())
                {
                    unfinishedPathPossiblities[i].Directions.Add(Direction.West);
                    finishedDirections = unfinishedPathPossiblities[i];
                    unfinishedPathPossiblities.RemoveAt(i);
                    i--;
                }
                else if (currentPos.GetX() == endPoint.GetX() && currentPos.GetY() + 1 == endPoint.GetY())
                {
                    unfinishedPathPossiblities[i].Directions.Add(Direction.North);
                    finishedDirections = unfinishedPathPossiblities[i];
                    unfinishedPathPossiblities.RemoveAt(i);
                    i--;
                }
                else if (currentPos.GetX() == endPoint.GetX() && currentPos.GetY() - 1 == endPoint.GetY())
                {
                    unfinishedPathPossiblities[i].Directions.Add(Direction.South);
                    finishedDirections = unfinishedPathPossiblities[i];
                    unfinishedPathPossiblities.RemoveAt(i);
                    i--;
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
                                unfinishedPathPossiblities[i].NextDirection != Direction.West)
                            {
                                var buffer = new Point(currentPos.GetX() + 1, currentPos.GetY());
                                if (!CheckForBackTrack(buffer, unfinishedPathPossiblities[i].Directions, startPoint))
                                {
                                    var newPossiblity = new PathPossiblity
                                    {
                                        PathLength = unfinishedPathPossiblities[i].PathLength
                                    };
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
                                unfinishedPathPossiblities[i].NextDirection != Direction.East)
                            {
                                var buffer = new Point(currentPos.GetX() - 1, currentPos.GetY());
                                if (!CheckForBackTrack(buffer, unfinishedPathPossiblities[i].Directions, startPoint))
                                {
                                    var newPossiblity = new PathPossiblity();
                                    newPossiblity.PathLength = unfinishedPathPossiblities[i].PathLength;
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
                                unfinishedPathPossiblities[i].NextDirection != Direction.South)
                            {
                                var buffer = new Point(currentPos.GetX(), currentPos.GetY() + 1);
                                if (!CheckForBackTrack(buffer, unfinishedPathPossiblities[i].Directions, startPoint))
                                {
                                    var newPossiblity = new PathPossiblity();
                                    newPossiblity.PathLength = unfinishedPathPossiblities[i].PathLength;
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
                                unfinishedPathPossiblities[i].NextDirection != Direction.North)
                            {
                                var buffer = new Point(currentPos.GetX(), currentPos.GetY() - 1);
                                if (!CheckForBackTrack(buffer, unfinishedPathPossiblities[i].Directions, startPoint))
                                {
                                    var newPossiblity = new PathPossiblity();
                                    newPossiblity.PathLength = unfinishedPathPossiblities[i].PathLength;
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

        public ulong GetTimeInTraffic()
        {
            return TimeInTraffic;
        }

        public void ResetTimeInTraffic()
        {
            TimeInTraffic = 0;
        }
        //Indicate to the person to head back the opposite direction along their path
        public void ReverseDirection()
        {
            HeadedToWork = !HeadedToWork;
            PathStep = 0;
            FinishedTraveling = false;
        }
        public bool IsFinishedTraveling()
        {
            return FinishedTraveling;
        }

        public bool IsHeadedToWork()
        {
            return HeadedToWork;
        }
        //Moves the CurrentLocation varibale relative to the passed in direction
        private void Move(Direction directon)
        {
            switch (directon)
            {
                case Direction.East:
                    Move(1, 0);
                    break;
                case Direction.North:
                    Move(0, 1);
                    break;
                case Direction.South:
                    Move(0, -1);
                    break;
                case Direction.West:
                    Move(-1, 0);
                    break;
                default:
                    Move();
                    break;
            }
        }

        //Moves the CurrentLocation variable relative to the passed in integers
        private void Move(int xOffset = 0, int yOffset = 0)
        {
            CurrentLocation.SetX(CurrentLocation.GetX() + xOffset);
            CurrentLocation.SetY(CurrentLocation.GetY() + yOffset);
        }

        //Gets the tile relative to current location using direction
        private ITile GetTile(Direction directon)
        {
            switch (directon)
            {
                case Direction.East:
                    return GetTile(1, 0);

                case Direction.North:
                    return GetTile(0, 1);

                case Direction.South:
                    return GetTile(0, -1);

                case Direction.West:
                    return GetTile(-1, 0);
                default:
                    return GetTile(0, 0);
            }
        }

        //Gets the tile relative to current location using 2 directions, this is for intersection pathing
        private ITile GetTile(Direction directon, Direction secondDirection)
        {
            var xOffset = 0;
            var yOffset = 0;

            switch (directon)
            {
                case Direction.East:
                    xOffset++;
                    break;
                case Direction.North:
                    yOffset++;
                    break;

                case Direction.South:
                    yOffset--;
                    break;

                case Direction.West:
                    xOffset--;
                    break;
            }
            switch (secondDirection)
            {
                case Direction.East:
                    xOffset++;
                    break;
                case Direction.North:
                    yOffset++;
                    break;

                case Direction.South:
                    yOffset--;
                    break;

                case Direction.West:
                    xOffset--;
                    break;
            }

            return GetTile(xOffset, yOffset);
        }

        //If no parameters are given GetTile(); will return currentLocation Tile
        private ITile GetTile(int xOffset = 0, int yOffset = 0)
        {
            return City.LiveMap[CurrentLocation.GetX() + xOffset, CurrentLocation.GetY() + yOffset];
        }

        
        public void Update()
        {
            //If Headed to work path = path to work if headed home path = path to home
            var path = HeadedToWork ? PathToWork.Directions : PathToHome.Directions;


            if (GetTile(path[PathStep]) is Road)
            {
                var road = (Road) GetTile(path[PathStep]);
                //Uses the direction you will be traveling once on the road. Not the direction to join the road
                if (road.MergeToRoad(path[PathStep + 1]))
                {
                    Move(path[PathStep]);
                    if (PathStep != 0)
                    {
                        road.LeaveRoad(path[PathStep]);
                    }
                    PathStep++;
                }
                else
                {
                    TimeInTraffic++;
                }
            }
            else if (GetTile(path[PathStep]) is Home)
            {
                Move(path[PathStep]);
                
                PathStep++;
            }
            else if (GetTile(path[PathStep]) is Office)
            {
                Move(path[PathStep]);
                PathStep++;
            }
            else if (GetTile(path[PathStep]) is Intersection)
            {
                var road = (Road) GetTile(path[PathStep], path[PathStep + 1]);
                var intersection = (Intersection) GetTile(path[PathStep]);
                if (road.MergeToRoad(path[PathStep + 1]) && intersection.CanCross(path[PathStep]))
                {
                    //Move to intersection

                    Move(path[PathStep]);
                    PathStep++;
                    //Move onto  road

                    Move(path[PathStep]);
                    PathStep++;
                }
                else
                {
                    TimeInTraffic++;
                }
            }
            //Allow City to decide if this person should continue to be updated
            FinishedTraveling = PathStep > path.Count - 1;
        }
        public void Update2()
        {
            //If Headed to work path = path to work if headed home path = path to home
            var path = HeadedToWork ? PathToWork.Directions : PathToHome.Directions;

            var currentTile = GetTile();
            var nextTile = GetTile(path[PathStep]);
            //Must be initlized with a value but once person approaches end of path will cause out of index
            var twoTilesAway = GetTile();
            if (PathStep + 1 < path.Count)
            {
                twoTilesAway = GetTile(path[PathStep], path[PathStep + 1]);
            }


            if (nextTile is Road)
            {
                var nextRoad = (Road)nextTile;
                //Uses the direction you will be traveling once on the road. Not the direction to join the road
                int PathStepToUse;
                if (twoTilesAway is Office || twoTilesAway is Home)
                {
                    PathStepToUse = PathStep;
                }
                else
                {
                    PathStepToUse = PathStep + 1;
                }
                if (nextRoad.MergeToRoad(path[PathStepToUse]))
                {
                    
                    if (currentTile is Road)
                    {
                        var currentRoad = (Road) currentTile;
                        currentRoad.LeaveRoad(path[PathStep]);
                    }
                    Move(path[PathStep]);
                    PathStep++;
                }
                else
                {
                    TimeInTraffic++;
                }
            }
            else if (nextTile is Home)
            {
                
                //Leave Current Road
                if (currentTile is Road)
                {
                    var currentRoad = (Road)currentTile;

                    if (PathStep == path.Count - 1)
                    {
                        currentRoad.LeaveRoad(path[PathStep - 1]);
                    }
                    else
                    {
                        currentRoad.LeaveRoad(path[PathStep]);
                    }

                }
                Move(path[PathStep]);
                PathStep++;
            }
            else if (nextTile is Office)
            {
                
                //Leave Current Road
                if (currentTile is Road)
                {
                    var currentRoad = (Road)currentTile;
                    if (PathStep == path.Count - 1)
                    {
                        currentRoad.LeaveRoad(path[PathStep - 1]);
                    }
                    else
                    {
                        currentRoad.LeaveRoad(path[PathStep]);
                    }
                }

                Move(path[PathStep]);
                PathStep++;
            }
            else if (nextTile is Intersection)
            {
                var roadAfterIntersection = (Road)twoTilesAway;
                var intersection = (Intersection)nextTile;
                //Important that intersection check is first so that Road Occupancy does not become messed up
                if (intersection.CanCross(path[PathStep]) && roadAfterIntersection.MergeToRoad(path[PathStep + 1]))
                {
                    //Leave Current Road
                    if (currentTile is Road)
                    {
                        var currentRoad = (Road)currentTile;
                        currentRoad.LeaveRoad(path[PathStep]);
                    }
                    
                    //Move to intersection

                    Move(path[PathStep]);
                    PathStep++;
                    //Move onto  road

                    Move(path[PathStep]);
                    PathStep++;
                }
                else
                {
                    TimeInTraffic++;
                }
            }
            //Allow City to decide if this person should continue to be updated
            FinishedTraveling = PathStep > path.Count - 1;
        }
    }



}