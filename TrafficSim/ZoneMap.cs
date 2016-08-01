


namespace TrafficSim
{
    public class ZoneMap
    {
        public Zone [,] Map;

        public ZoneMap(int widthByte, int heightByte)
        {
            Map = new Zone[widthByte, heightByte];
        }
    }
}
