using TrafficSim.PersonNavigation;

namespace TrafficSim.Roads
{
    public class Intersection : ITile
    {
        private readonly ulong _timeToSwitch;
        private bool _verticleGreen;


        public Intersection(ulong timeToSwtich, bool startVerticleTraffic)
        {
            
            _timeToSwitch = timeToSwtich;
            _verticleGreen = startVerticleTraffic;
        }

        public static ulong CurrentTick { get; set; }
        public Point Location { get; set; }

        public void Update(ulong currentTick)
        {
            if (_timeToSwitch%currentTick == 0)
            {
                _verticleGreen = !_verticleGreen;
            }
        }

        public bool CanCross(Direction travelingDirection)
        {
            //When verticle is Green allow N/S travel
            if (_verticleGreen)
            {
                return travelingDirection == Direction.North || travelingDirection == Direction.South;
            }
            //When horizontal is Green allow E/W travel
            return travelingDirection == Direction.East || travelingDirection == Direction.West;
        }
    }
}