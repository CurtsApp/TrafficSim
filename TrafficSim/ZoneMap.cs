


namespace TrafficSim
{
    public class ZoneMap
    {
        public Zone [,] Map;

        public ZoneMap(byte widthByte, byte heightByte)
        {
            Map = new Zone[widthByte, heightByte];
        }
    }
}
