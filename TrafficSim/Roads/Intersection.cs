using TrafficSim.PersonNavigation;

namespace TrafficSim.Roads
{
    public class Intersection : ITile
    {
        private ulong _timeToSwitch;
        private bool _verticleGreen;


        public Intersection(ulong timeToSwtich, bool startVerticleTraffic)
        {
            ClassName = "Intersection";
            _timeToSwitch = timeToSwtich;
            _verticleGreen = startVerticleTraffic;
        }

        public Point Location { get; set; }
        public string ClassName { get; set; }

        public void Update(ulong currentTick)
        {
            if (currentTick %_timeToSwitch == 0)
            {
                _verticleGreen = !_verticleGreen;
            }
        }

        public void ChangeCycleTime(int cycleTime)
        {
            _timeToSwitch = _timeToSwitch + (ulong)cycleTime;
        }
        public bool CanCross(Direction travelingDirection)
        {
            
            //When verticle is Green allow N/S travel
            if (_verticleGreen)
            {
                if (travelingDirection == Direction.North || travelingDirection == Direction.South)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //return (travelingDirection == Direction.North || travelingDirection == Direction.South);
            }
            //When horizontal is Green allow E/W travel
            if (travelingDirection == Direction.East || travelingDirection == Direction.West)
            {
                return true;
            }
            else
            {
                return false;
            }
            //return (travelingDirection == Direction.East || travelingDirection == Direction.West);
        }
    }
}