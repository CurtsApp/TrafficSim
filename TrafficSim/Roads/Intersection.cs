using TrafficSim.PersonNavigation;

namespace TrafficSim.Roads
{
    public class Intersection : ITile
    {
        public ulong TimeToSwitch;
        public bool VerticleGreen;


        public Intersection(ulong timeToSwtich, bool startVerticleTraffic)
        {
            ClassName = "Intersection";
            TimeToSwitch = timeToSwtich;
            VerticleGreen = startVerticleTraffic;
        }

        public Point Location { get; set; }
        public string ClassName { get; set; }

        public void Update(ulong currentTick)
        {
            if (currentTick %TimeToSwitch == 0)
            {
                VerticleGreen = !VerticleGreen;
            }
        }

        public void ChangeCycleTime(int cycleTime)
        {
            TimeToSwitch = TimeToSwitch + (ulong)cycleTime;
        }
        public bool CanCross(Direction travelingDirection)
        {
            
            //When verticle is Green allow N/S travel
            if (VerticleGreen)
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