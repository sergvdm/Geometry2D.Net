using System;
using System.Globalization;

namespace Altium.Geometry2D
{
    /// <summary>
    /// 2D point (float)
    /// </summary>
    [Serializable]
    public struct Point2DF
    {
        public static readonly Point2DF Zero = new Point2DF();

        public static implicit operator Point2DF(Point2DI pt)
        {
            return new Point2DF(pt.X, pt.Y);
        }

        public static explicit operator Point2DF(Point2DD pt)
        {
            return new Point2DF((float)pt.X, (float)pt.Y);
        }

        public static bool operator ==(Point2DF pt1, Point2DF pt2)
        {
            return pt1.X == pt2.X && pt1.Y == pt2.Y;
        }

        public static bool operator !=(Point2DF pt1, Point2DF pt2)
        {
            return !(pt1 == pt2);
        }

        public static Point2DF operator +(Point2DF pt1, Point2DF pt2)
        {
            return new Point2DF(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static Vector2DF operator -(Point2DF pt1, Point2DF pt2)
        {
            return new Vector2DF(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public static Point2DF operator *(float c, Point2DF pt)
        {
            return new Point2DF(c * pt.X, c * pt.Y);
        }

        public static Point2DF operator *(Point2DF pt, float c)
        {
            return new Point2DF(pt.X * c, pt.Y * c);
        }

        public static Point2DF operator /(Point2DF pt, float c)
        {
            return new Point2DF(pt.X / c, pt.Y / c);
        }

        public Point2DF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X;
        public float Y;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0:F2},{1:F2}]", X, Y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point2DF)) return false;
            Point2DF other = (Point2DF)obj;
            return other == this;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
