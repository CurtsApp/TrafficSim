
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
        private List<Home> _homes = new List<Home>();
        private List<Person> _people = new List<Person>();
        private List<Office> _offices = new List<Office>();
        private List<Intersection> _intersections = new List<Intersection>();
        public ulong TicksSinceStartUp { get; set; }
        public int [,] TravelTimes { get; set; }
        private String storagePath;
        
        

        public City(StartingValues startValues)
        {
            storagePath = startValues.StoragePath;

            _budget = startValues.Budget;
             _traffictCycleTime = startValues.TrafficLightCycleTimeDefault;
            _cityHeight = startValues.MapHeight;
            _cityWidth = startValues.MapWidth;
            _population = startValues.Population;
            _zoning = new ZoneMap(_cityWidth,_cityHeight);
            _zoning = GenerateZones(_zoning);
            LiveMap = new ITile[_cityWidth, _cityHeight];
            LiveMap = GenerateLiveMap(LiveMap);
            TravelTimes = new int[_cityWidth, _cityHeight];
            GenerateTravelTimeHelper();
            PrintCity();
            if (File.Exists(storagePath))
            {
                LoadPeopleFromFile();
            }
            else
            {
                GeneratePeople();
            }
            

             while (true)
             {
                 Tick();
             }
             
        }

        private void Tick()
        {
            TicksSinceStartUp++;
            foreach (var person in _people)
            {
                if (!person.IsFinishedTraveling())
                {
                    person.Update();
                }
                
            }
            foreach (var intersection in _intersections)
            {
                intersection.Update(TicksSinceStartUp);
            }

        }

        private void PrintCity()
        {
            for (var y = 0; y < _cityHeight; y++)
            {
                for (var x = 0; x < _cityWidth; x++)
                {


                    if (LiveMap[x, y] is TwoLaneRoad)
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

        private void LoadPeopleFromFile()
        {
            _people = JsonConvert.DeserializeObject<List<Person>>(File.ReadAllText(storagePath));
        }

        private void WritePeopleToFile()
        {
            File.WriteAllText(@storagePath, JsonConvert.SerializeObject(_people));

        }
        private void GeneratePeople()
        {
            for (ulong i = 0; i < _population; i++)
            {
               
                var personBuffer = new Person(_homes[_rand.Next(_homes.Count)], _offices[_rand.Next(_offices.Count)],
                    LiveMap, TravelTimes);
                _people.Add(personBuffer);
                Console.Clear();
                var percentComplete = i / (double)_population;
                Console.Write($"People Created {i + 1} {Math.Ceiling(percentComplete * 100)}%");

            }
            WritePeopleToFile();
        }
    }
}
