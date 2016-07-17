using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim_API.SimSrc;

namespace TrafficSim
{
    class StartUp
    {
        static void Main(string[] args)
        {
            StartingValues config = new StartingValues();
            config.Budget = 1000000; //1Million
            config.MapHeight = 25;
            config.MapWidth = 25;
            config.Population = 5000;
            config.TrafficLightCycleTimeDefault = 120; //Measured in ticks, 60mph = 60ticks
            City city = new City(config);

        }
    }

   
}
