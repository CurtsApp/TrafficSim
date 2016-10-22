
namespace TrafficSim.Roads
{
    public class Home : ITile
    {
        public Point Location { get; set; }
        public string ClassName { get; set; }
        


        public Home(Point location)
        {
            Location = location;
            ClassName = "Home";
        }
        public void Update()
        {
            
        }
    }
}
