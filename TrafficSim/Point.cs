
using System.Runtime.CompilerServices;

namespace TrafficSim
{
    public class Point
    {
        private int X { get; set; }
        private int Y { get; set; }

        public Point(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
        
        public int GetX()
        {
            return X;
        }

        public int GetY()
        {
            return Y;
        }

        public void SetX(int newX)
        {
            X = newX;
        }

        public void SetY(int newY)
        {
            Y = newY;
        }
    }
}
