using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    class Vacant : ITile
    {
        public Point Location { get; set; }
        public string ClassName { get; set; }

        public Vacant(Point location)
        {
            Location = location;
            ClassName = "Vacant";
        }
    }
}
