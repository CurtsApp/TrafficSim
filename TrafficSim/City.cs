
using System;
using System.Collections.Generic;
using TrafficSim_API.SimSrc.Roads;

namespace TrafficSim_API.SimSrc
{
    public class City
    {
        private readonly ZoneMap _zoning;
        private ulong _budget;
        private readonly byte _cityHeight;
        private readonly byte _cityWidth;
        private readonly ulong _population;
        private readonly ulong _traffictCycleTime;
        private readonly Random _rand = new Random();
        public static ITile [,] LiveMap;
        private List<Home> _homes = new List<Home>();
        private List<Person> _people = new List<Person>();
        private List<Office> _offices = new List<Office>();
        private List<Intersection> _intersections = new List<Intersection>();
        public ulong TicksSinceStartUp { get; set; }
        
        

         public City(StartingValues startValues)
        {
            
            _budget = startValues.Budget;
             _traffictCycleTime = startValues.TrafficLightCycleTimeDefault;
            _cityHeight = startValues.MapHeight;
            _cityWidth = startValues.MapWidth;
            _population = startValues.Population;
            _zoning = new ZoneMap(_cityWidth,_cityHeight);
            _zoning = GenerateZones(_zoning);
            LiveMap = new ITile[_cityWidth, _cityHeight];
            LiveMap = GenerateLiveMap(LiveMap);
        }

        public void Tick()
        {
            TicksSinceStartUp++;
            foreach (Person person in _people)
            {
                person.Update();
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
                            Intersection bufferIntersection = new Intersection(_traffictCycleTime, TicksSinceStartUp);
                            _intersections.Add(bufferIntersection);
                            
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
                    else
                    {
                        var rng = _rand.Next(101);
                        var xSquared = x ^ x;
                        var ySquared = y ^ y;
                        var maxXSquared = _cityWidth ^ _cityWidth;
                        var maxYSquared = _cityHeight ^ _cityHeight;
                        var distanceFromEdge = Math.Sqrt(xSquared + ySquared);
                        var percentFromEdge = distanceFromEdge/Math.Sqrt(maxXSquared + maxYSquared);
                        if (rng < percentFromEdge)
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

        private void GeneratePeople()
        {
            for (ulong i = 0; i < _population; i++)
            {
                var isNightShift = _rand.Next(101) < 10;
                var scheduleShift = _rand.Next(-2,3);
                int startTime;

                if (isNightShift)
                {
                    startTime = 22 + scheduleShift;
                }
                else
                {
                    startTime = 8 + scheduleShift;
                }
                var endTime = startTime + 8;
                if (endTime > 24)
                {
                    endTime = endTime - 24;
                }
                var personBuffer = new Person(_homes[_rand.Next(_homes.Count)], _offices[_rand.Next(_offices.Count)],
                    (byte) startTime, (byte) endTime, LiveMap);
                _people.Add(personBuffer);

            }
        }
    }
}
