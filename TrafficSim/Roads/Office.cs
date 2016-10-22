using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSim.Roads
{
    public class Office : ITile
    {
        public Point Location { get; set; }
        public string ClassName { get; set; }

        public Office(Point location)
        {
            Location = location;
            ClassName = "Office";
        }
        public void Update()
        {
            
        }
    }
}
