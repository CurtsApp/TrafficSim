using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSim_API.SimSrc
{
    public interface ITile
    {
        Point Location { get; set; }
        
    }
}
