
namespace TrafficSim.Roads
{
    public class Home : ITile
    {
        public Point Location { get; set; }

        public Home(Point location)
        {
            Location = location;
        }
        public void Update()
        {
            
        }
    }
}
