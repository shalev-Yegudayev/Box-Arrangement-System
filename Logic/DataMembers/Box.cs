using System;

namespace Logic.DataMembers
{
    public class Box : IComparable<Box>
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Box(double x, double y)
        {
            X = x;
            Y = y;
        }

        public int CompareTo(Box other)
        {
            if (X == other.X) return Y.CompareTo(other.Y);
            return X.CompareTo(other.X);
        }

        public override string ToString() => $"X:{X}/Y: {Y},";
    }
}
