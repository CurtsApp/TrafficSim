using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSim_API.SimSrc.Roads
{
    public class Office : ITile
    {
        public Point Location { get; set; }

        public Office(Point location)
        {
            Location = location;
        }
        public void Update()
        {
            
        }
    }
}
