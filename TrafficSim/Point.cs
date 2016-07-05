
namespace TrafficSim_API.SimSrc
{
    public class Point
    {
        private byte X { get; set; }
        private byte Y { get; set; }

        public Point(byte _x, byte _y)
        {
            X = _x;
            Y = _y;
        }
        
        public byte GetX()
        {
            return X;
        }

        public byte GetY()
        {
            return Y;
        }
    }
}
