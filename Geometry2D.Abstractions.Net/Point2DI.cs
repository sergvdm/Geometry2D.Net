using System;
using System.Globalization;

namespace Svd.Geometry2D
{
    /// <summary>
    /// 2D point (integer)
    /// </summary>
    [Serializable]
    public struct Point2DI
    {
        public static readonly Point2DI Zero = new Point2DI();

        public static explicit operator Point2DI(Point2DF pt)
        {
            return new Point2DI((int)pt.X, (int)pt.Y);
        }

        public static explicit operator Point2DI(Point2DD pt)
        {
            return new Point2DI((int)pt.X, (int)pt.Y);
        }

        public static bool operator ==(Point2DI pt1, Point2DI pt2)
        {
            return pt1.X == pt2.X && pt1.Y == pt2.Y;
        }

        public static bool operator !=(Point2DI pt1, Point2DI pt2)
        {
            return !(pt1 == pt2);
        }

        public static Point2DI operator +(Point2DI pt1, Point2DI pt2)
        {
            return new Point2DI(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static Vector2DD operator -(Point2DI pt1, Point2DI pt2)
        {
            return new Vector2DD(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public static Point2DI operator *(int c, Point2DI pt)
        {
            return new Point2DI(c * pt.X, c * pt.Y);
        }

        public static Point2DI operator *(Point2DI pt, int c)
        {
            return new Point2DI(pt.X * c, pt.Y * c);
        }

        public Point2DI(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0:F2},{1:F2}]", X, Y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point2DI)) return false;
            Point2DI other = (Point2DI)obj;
            return other == this;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
