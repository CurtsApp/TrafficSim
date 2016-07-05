using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSim_API.SimSrc.Roads
{ 
    public class Intersection : ITile
    {
        private ulong _timeToSwitch;
        private bool _verticleGreen = true;
        public Point Location { get; set; }
        public static ulong CurrentTick { get; set; }


        public Intersection(ulong timeToSwtich, ulong currentTick)
        {
            CurrentTick = currentTick;
            _timeToSwitch = timeToSwtich;
        }
    }
}

        
        
    

