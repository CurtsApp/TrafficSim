
using System.Runtime.CompilerServices;

namespace TrafficSim
{
    public class Point
    {
        private int X { get; set; }
        private int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
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

        public bool IsEqual(Point point)
        {
            return this.GetX() == point.GetX() && this.GetY() == point.GetY();
        }
    }
}
