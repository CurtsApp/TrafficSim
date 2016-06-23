using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSim_API.SimSrc.Roads
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
