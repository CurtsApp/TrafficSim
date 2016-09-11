namespace TrafficSim
{
    class StartUp
    {
        static void Main(string[] args)
        {
            StartingValues config = new StartingValues
            {
                Budget = 1000000,//1Million
                MapHeight = 25,
                MapWidth = 25,
                Population = 5000,
                TrafficLightCycleTimeDefault = 120//Measured in ticks, 60mph = 60ticks
            };
            
            
            City city = new City(config);

        }
    }

   
}
