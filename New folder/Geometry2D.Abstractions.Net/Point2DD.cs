using System;
using System.Globalization;

namespace Altium.Geometry2D
{
    /// <summary>
    /// 2D point (double)
    /// </summary>
    [Serializable]
    public struct Point2DD
    {
        public static readonly Point2DD Zero = new Point2DD();

        public static implicit operator Point2DD(Point2DI pt)
        {
            return new Point2DD(pt.X, pt.Y);
        }

        public static implicit operator Point2DD(Point2DF pt)
        {
            return new Point2DD(pt.X, pt.Y);
        }

        public static bool operator ==(Point2DD pt1, Point2DD pt2)
        {
            return pt1.X == pt2.X && pt1.Y == pt2.Y;
        }

        public static bool operator !=(Point2DD pt1, Point2DD pt2)
        {
            return !(pt1 == pt2);
        }

        public static Point2DD operator +(Point2DD pt1, Point2DD pt2)
        {
            return new Point2DD(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static Vector2DD operator -(Point2DD pt1, Point2DD pt2)
        {
            return new Vector2DD(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public static Point2DD operator *(double c, Point2DD pt)
        {
            return new Point2DD(c * pt.X, c * pt.Y);
        }

        public static Point2DD operator *(Point2DD pt, double c)
        {
            return new Point2DD(pt.X * c, pt.Y * c);
        }

        public static Point2DD operator /(Point2DD pt, double c)
        {
            return new Point2DD(pt.X / c, pt.Y / c);
        }

        public Point2DD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X;
        public double Y;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0:F2},{1:F2}]", X, Y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point2DD)) return false;
            Point2DD other = (Point2DD)obj;
            return other == this;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
