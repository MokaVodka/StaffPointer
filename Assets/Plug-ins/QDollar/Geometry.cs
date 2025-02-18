using System;

namespace QDollar
{
    public class Geometry
    {
        // Computes the Squared Euclidean Distance between two points in 2D
        public static float SqrEuclideanDistance(Point a, Point b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        // Computes the Euclidean Distance between two points in 2D
        public static float EuclideanDistance(Point a, Point b)
        {
            return (float)Math.Sqrt(SqrEuclideanDistance(a, b));
        }
    }
}
