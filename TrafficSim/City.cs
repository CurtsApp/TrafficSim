
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TrafficSim.MutationHandling;
using TrafficSim.Roads;


namespace TrafficSim
{
    public class City
    {
        private readonly ZoneMap _zoning;
        private ulong _budget;
        private readonly int _cityHeight;
        private readonly int _cityWidth;
        private readonly ulong _population;
        private readonly ulong _traffictCycleTime;
        private readonly Random _rand = new Random();
        public static ITile [,] LiveMap;
        public static List<Home> _homes = new List<Home>();
        private List<Person> _people = new List<Person>();
        public static List<Office> _offices = new List<Office>();
        public static List<Intersection> _intersections = new List<Intersection>();
        public ulong TicksSinceStartUp { get; set; }
        public int [,] TravelTimes { get; set; }
        private string storagePathPeople;
        private string storagePathMap;
        private bool allFinishedTraveling = false;
        private TrafficMutator mManager;
        private bool firstRun = true;
        
        

        public City(StartingValues startValues)
        {
            storagePathPeople = startValues.StoragePath;
            storagePathMap = startValues.StoragePathMap;

            mManager = new TrafficMutator();
            _budget = startValues.Budget;
             _traffictCycleTime = startValues.TrafficLightCycleTimeDefault;
            _cityHeight = startValues.MapHeight;
            _cityWidth = startValues.MapWidth;
            _population = startValues.Population;
            _zoning = new ZoneMap(_cityWidth,_cityHeight);
           
            LiveMap = new ITile[_cityWidth, _cityHeight];
            
            TravelTimes = new int[_cityWidth, _cityHeight];
            
            
            if (File.Exists(storagePathPeople))
            {
                //GeneratePeople();
                LoadPeopleFromFile();
                
            }
            else
            {
                _zoning = GenerateZones(_zoning);
                LiveMap = GenerateLiveMap(LiveMap);
                GenerateTravelTimeHelper();
                GeneratePeople();
            }
            
            PrintCity();
            //The larger the number the more accurate the final prediction
            while (mManager.GetNumberOfCyclesSinceLastKeptChange() < 5000)
             {
                 Tick();
             }

             Console.Out.WriteLine("The ideal intersection timing for this city is... [Measured in ticks]");
            PrintFinalOutput();

             
        }

        private void Tick()
        {
            TicksSinceStartUp++;
            allFinishedTraveling = true;

            
            foreach (var person in _people)
            {
                if (!person.IsFinishedTraveling())
                {
                    person.Update2();
                    allFinishedTraveling = false;

                }
                
            }
            
            if (allFinishedTraveling)
            {
                //if(Headed Home)
                if (!_people[0].IsHeadedToWork())
                {
                    ulong totalTimeInTraffic = 0;
                    foreach (var person in _people)
                    {
                        totalTimeInTraffic = totalTimeInTraffic + person.GetTimeInTraffic();
                        person.ResetTimeInTraffic();
                    }
                    if (firstRun)
                    {
                        mManager.UpdateLastTimeInTraffic(totalTimeInTraffic);
                        firstRun = false;
                    }
                    else
                    {
                        if (totalTimeInTraffic > mManager.GetLastTimeInTraffic())
                        {
                            //Revert last mutation
                           ApplyMutation(mManager.GetRevertLastChange());
                            Console.WriteLine("Reverting last mutation, time in traffic was: " + totalTimeInTraffic);
                        }
                        else
                        {
                            mManager.UpdateLastTimeInTraffic(totalTimeInTraffic);
                            Console.WriteLine("Kept change, new time in traffic is: " + totalTimeInTraffic);
                            
                        }

                        //Make next Mutation
                        ApplyMutation(mManager.GetNextMutation());
                        
                    }
                    
                }
                //If person is headed to work they are now headed home and vise versa
                foreach (var person in _people)
                {
                   person.ReverseDirection();
                }

            }

            //Update all traffic lights
            foreach (var intersection in _intersections)
            {
                
                intersection.Update(TicksSinceStartUp);
            }

        }

        private void ApplyMutation(Mutation mutation)
        {
           
            mutation.Intersection.ChangeCycleTime(mutation.ChangeAmount);
        }
        private void PrintRoadOccupancy()
        {
            for (int i = 0; i < _cityHeight; i++)
            {
                for (int j = 0; j < _cityWidth; j++)
                {
                    var live = LiveMap[j, i] as Road;
                    if (live != null)
                    {
                        Road road = live;
                        if (road.DirectionBOccupancy > 9 || road.DirectionBOccupancy < 0)
                        {
                            Console.Write(road.DirectionBOccupancy + " ");
                        }
                        else
                        {
                            Console.Write(road.DirectionBOccupancy + "  ");
                        }
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                }
                Console.WriteLine();
            }
        }
        private void PrintPeople()
        {
            int[,] peopleLocation = new int[_cityWidth,_cityHeight];
            for (int i = 0; i < _cityHeight; i++)
            {
                for (int j = 0; j < _cityWidth; j++)
                {
                    peopleLocation[j, i] = 0;
                }
            }
            foreach (var person in _people)
            {
                
                    peopleLocation[person.CurrentLocation.GetX(), person.CurrentLocation.GetY()]++;
                
            }
            for (int i = 0; i < _cityHeight; i++)
            {
                for (int j = 0; j < _cityWidth; j++)
                {
                    if (peopleLocation[j, i] > 9)
                    {
                        Console.Write(peopleLocation[j, i] + " ");
                    }
                    else
                    {
                        Console.Write(peopleLocation[j, i] + "  ");
                    }
                    
                }
                Console.WriteLine();
            }
        }
        private void PrintCity()
        {
            
            for (var y = 0; y < _cityHeight; y++)
            {
                for (var x = 0; x < _cityWidth; x++)
                {

                    if (x == _people[0].CurrentLocation.GetX() && y == _people[0].CurrentLocation.GetY())
                    {
                        Console.Write("R ");
                    }
                    else if (LiveMap[x, y] is TwoLaneRoad)
                    {
                        Console.Write("  ");
                    }
                    else if (LiveMap[x, y] is Home)
                    {
                        Console.Write("H ");
                    }
                    else if (LiveMap[x, y] is Office)
                    {
                        Console.Write("O ");
                    }
                    else if (LiveMap[x, y] is Intersection)
                    {
                        Console.Write("  ");
                    }
                    else if (LiveMap[x, y] is Vacant)
                    {
                        Console.Write("  ");
                    }


                }
                Console.WriteLine();
            }
        }
        private void GenerateTravelTimeHelper()
        {
            
            for (var x = 0; x < LiveMap.GetLength(0); x++)
            {
                for (var y = 0; y < LiveMap.GetLength(1); y++)
                {
                    if (LiveMap[x, y] is IRoad)
                    {
                        IRoad buffer = (IRoad) LiveMap[x, y];
                        TravelTimes[x, y] = buffer.TimeToTraverse;
                    } else if (LiveMap[x, y] is Intersection)
                    {
                        TravelTimes[x, y] = -1;
                    }
                   
                }   
            }
        }
        private ITile[,] GenerateLiveMap(ITile[,] liveMap)
        {
            for (byte x = 0; x < _cityWidth; x++)
            {
                for (byte y = 0; y < _cityHeight; y++)
                {
                    
                    switch ((int)_zoning.Map[x,y])
                    {
                            
                        //Residential
                        case 0:
                            Home bufferHome = new Home(new Point(x,y));
                            _homes.Add(bufferHome);
                            LiveMap[x, y] = bufferHome;
                            break;
                            //Work
                        case 1:
                            Office bufferOffice = new Office(new Point(x,y));
                            _offices.Add(bufferOffice);
                            LiveMap[x, y] = bufferOffice;
                            break;
                            //Road
                        case 2:
                            LiveMap[x,y] = new TwoLaneRoad(new Point(x, y));
                            break;
                            //Intersection
                        case 3:
                            Intersection bufferIntersection = new Intersection(_traffictCycleTime, true);
                            _intersections.Add(bufferIntersection);
                            LiveMap[x, y] = bufferIntersection;
                            break;
                        case 4:
                            LiveMap[x,y] = new Vacant(new Point(x,y));
                            break;
                            default:
                            throw new Exception("Zoning Map: Enum exceeds expected values");
                            
                    } 
                }
            }


           

            return liveMap;
        } 
        private ZoneMap GenerateZones(ZoneMap zones)
        {
            for (int x = 0; x < _cityWidth; x++)
            {
                for (int y = 0; y < _cityHeight; y++)
                {
                    int shiftedX = x + 2;
                    int shiftedY = y + 2;
                    if (x%4 == 0|| y%4 == 0)
                    {

                        if (x%4 == 0 && y%4 == 0)
                        {
                            zones.Map[x, y] = Zone.Intersection;
                        }
                        else
                        {
                            zones.Map[x, y] = Zone.Road;
                        }
                        
                        
                        
                    }
                    //Leave center of blocks Vacant
                    else if (shiftedX%4 == 0 && shiftedY%4 == 0)
                    {
                        zones.Map[x, y] = Zone.Vacant;
                    }
                        else
                    {

                        //If the percent from center is greater than 75% it will always be a house
                        double rng = _rand.Next(75);
                        rng = rng/100;
                        //Calculate maximum distance from center of map
                        var maxXSquared = _cityWidth/2;
                        var maxYSquared = _cityHeight/2;
                        //Calculated current position relative to center of map
                        var xSquared = x - maxXSquared;
                        var ySquared = y - maxYSquared;
                        //Square max value
                        maxXSquared = maxXSquared * maxXSquared;
                        maxYSquared = maxYSquared * maxYSquared;
                        //Square current positon
                        xSquared = xSquared * xSquared;
                        ySquared = ySquared * ySquared;
                        //Calculate percent from center of the map
                        var distanceFromCenter = Math.Sqrt(xSquared + ySquared);
                        var percentFromCenter = distanceFromCenter/Math.Sqrt(maxXSquared + maxYSquared);
                   
                        if (rng < percentFromCenter)
                        {
                            zones.Map[x, y] = Zone.Residential;
                        }
                        else
                        {
                            zones.Map[x, y] = Zone.Work;
                        }
                    }
                    

                }
                
            }
            return zones;
        }

        private void PrintFinalOutput()
        {
            for (var y = 0; y < _cityHeight; y++)
            {
                for (var x = 0; x < _cityWidth; x++)
                {

                    if (LiveMap[x, y] is TwoLaneRoad)
                    {
                        Console.Write("   ");
                    }
                    else if (LiveMap[x, y] is Home)
                    {
                        Console.Write("H  ");
                    }
                    else if (LiveMap[x, y] is Office)
                    {
                        Console.Write("O  ");
                    }
                    else if (LiveMap[x, y] is Intersection)
                    {
                        Intersection intersection = (Intersection) LiveMap[x, y];
                        if (intersection.GetCycleTime() > 99)
                        {
                            Console.Write(intersection.GetCycleTime());
                        }
                        else if (intersection.GetCycleTime() > 9)
                        {
                            Console.Write(intersection.GetCycleTime() + " ");
                        }
                        else
                        {
                            Console.Write(intersection.GetCycleTime() + "  ");
                        }
                    }
                    else if (LiveMap[x, y] is Vacant)
                    {
                        Console.Write("   ");
                    }


                }
                    Console.WriteLine();
                }
            
        }
        private void LoadPeopleFromFile()
        {
           
            JsonConverter[] converters = {new TileConverter()};
            LiveMap = JsonConvert.DeserializeObject<ITile[,]>(File.ReadAllText(storagePathMap),
                new JsonSerializerSettings() {Converters = converters});
            for (int x = 0; x < _cityWidth; x++)
            {
                for (int y = 0; y < _cityHeight; y++)
                {
                    if (LiveMap[x, y] is Home)
                    {
                         Home buffer = (Home)LiveMap[x, y];
                         _homes.Add(buffer);
                         LiveMap[x, y] = buffer;
                    } else if (LiveMap[x, y] is Office)
                    {
                        Office buffer = (Office)LiveMap[x, y];
                        _offices.Add(buffer);
                        LiveMap[x, y] = buffer;
                    }
                    else if (LiveMap[x, y] is Intersection)
                    {
                        Intersection buffer = (Intersection)LiveMap[x, y];
                        _intersections.Add(buffer);
                        LiveMap[x, y] = buffer;
                    }
                }
            }

            _people = JsonConvert.DeserializeObject<List<Person>>(File.ReadAllText(storagePathPeople));

        }

        
        private void WritePeopleToFile()
        {
            File.WriteAllText(@storagePathPeople, JsonConvert.SerializeObject(_people));
            File.WriteAllText(@storagePathMap, JsonConvert.SerializeObject(LiveMap));
        }
        private void GeneratePeople()
        {
            for (ulong i = 0; i < _population; i++)
            {
               
                var personBuffer = new Person(_homes[_rand.Next(_homes.Count)], _offices[_rand.Next(_offices.Count)],
                   TravelTimes);
                _people.Add(personBuffer);
                Console.Clear();
                var percentComplete = i / (double)_population;
                Console.Write($"People Created {i + 1} {Math.Ceiling(percentComplete * 100)}%");

            }
            WritePeopleToFile();
        }
    }
}
