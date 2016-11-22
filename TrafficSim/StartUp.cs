using System;
using System.Collections.Generic;

namespace TrafficSim
{
    class StartUp
    {
        private static void Main(string[] args)
        {
            var config = new StartingValues
            {
                Budget = 1000000,//1Million
                MapHeight = 25,
                MapWidth = 25,
                Population = 50,
                TrafficLightCycleTimeDefault = 120//Measured in ticks, 60mph = 60ticks

               
            };
            //TODO Make a storage path to store the tile map and pathing helper
            var ExecutablePath = System.Reflection.Assembly.GetEntryAssembly().CodeBase;
            String[] executableLocation = ExecutablePath.Split('/');
            for (int i = 3; i < executableLocation.Length - 1; i++)
            {
                config.StoragePath = config.StoragePath + executableLocation[i] + "/";
            }
            config.StoragePathMap = config.StoragePath + "mapInfo.json";
            config.StoragePath = config.StoragePath + "people.json";
            Console.WriteLine(config.StoragePath);
            var city = new City(config);

        }
    }

   
}
