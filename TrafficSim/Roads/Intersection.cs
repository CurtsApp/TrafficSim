using System;
using TrafficSim.PersonNavigation;

namespace TrafficSim.Roads
{
    public class Intersection : ITile
    {
        public int TimeToSwitch;
        public bool VerticleGreen;
        Random rand = new Random();


        public Intersection(int timeToSwtich, bool startVerticleTraffic)
        {
            ClassName = "Intersection";
            TimeToSwitch = timeToSwtich;
            VerticleGreen = startVerticleTraffic;
        }

        public Point Location { get; set; }
        public string ClassName { get; set; }

        public void Update(int currentTick)
        {
            if (currentTick %TimeToSwitch == 0)
            {
                VerticleGreen = !VerticleGreen;
            }
        }

        public void ChangeCycleTime(int cycleTime)
        {
            //If cycle time becomes a negative number it will wrap around to the max extreme of ulong
            TimeToSwitch = TimeToSwitch + cycleTime;
            //Prevents Extreme Values
            if (TimeToSwitch < 1 || TimeToSwitch > 200)
            {
                TimeToSwitch = 100 + rand.Next(-20,20);
            } 
        }

        public int GetCycleTime()
        {
            return TimeToSwitch;
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