using System;
using System.Globalization;

namespace Altium.Geometry2D
{
    /// <summary>
    /// 2D vector (double)
    /// </summary>
    [Serializable]
    public struct Vector2DD
    {
        public static readonly Vector2DD Zero = new Vector2DD();

        public static implicit operator Vector2DD(Vector2DF v)
        {
            return new Vector2DD(v.X, v.Y);
        }

        public static explicit operator Vector2DD(Point2DD pt)
        {
            return new Vector2DD(pt.X, pt.Y);
        }

        public static bool operator ==(Vector2DD v1, Vector2DD v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(Vector2DD v1, Vector2DD v2)
        {
            return !(v1 == v2);
        }

        public static Vector2DD operator -(Vector2DD v)
        {
            return new Vector2DD(-v.X, -v.Y);
        }

        public static Vector2DD operator +(Vector2DD v1, Vector2DD v2)
        {
            return new Vector2DD(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2DD operator -(Vector2DD v1, Vector2DD v2)
        {
            return new Vector2DD(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Point2DD operator +(Point2DD pt, Vector2DD v)
        {
            return new Point2DD(pt.X + v.X, pt.Y + v.Y);
        }

        public static Point2DD operator +(Vector2DD v, Point2DD pt)
        {
            return new Point2DD(v.X + pt.X, v.Y + pt.Y);
        }

        public static Point2DD operator -(Point2DD pt, Vector2DD v)
        {
            return new Point2DD(pt.X - v.X, pt.Y - v.Y);
        }

        public static double operator *(Vector2DD v1, Vector2DD v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static Vector2DD operator *(double c, Vector2DD v)
        {
            return new Vector2DD(c * v.X, c * v.Y);
        }

        public static Vector2DD operator *(Vector2DD v, double c)
        {
            return new Vector2DD(v.X * c, v.Y * c);
        }

        public static Vector2DD operator /(Vector2DD v, double c)
        {
            return new Vector2DD(v.X / c, v.Y / c);
        }

        public static double operator ^(Vector2DD v1, Vector2DD v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        public Vector2DD(Point2DD start, Point2DD end)
        {
            X = end.X - start.X;
            Y = end.Y - start.Y;
        }

        public Vector2DD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2DD(double angle)
        {
            angle = angle.NormalizeRadAngle();
            X = Math.Cos(angle);
            Y = Math.Sin(angle);
        }

        public double X;
        public double Y;

        public double Angle => Math.Atan2(Y, X);

        public double Norm()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public Vector2DD Unit()
        {
            if (X == 0 && Y == 0) return new Vector2DD(1, 0);
            var n = Norm();
            if (n < 1E-6) return new Vector2DD(Angle);
            else return this / n;
        }

        public Vector2DD Ortogonal()
        {
            return new Vector2DD(-Y, X);
        }

        public Vector2DD ProjectTo(Vector2DD s)
        {
            return (this * s) / (s * s) * s;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0:F2},{1:F2}]", X, Y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2DD)) return false;
            Vector2DD v = (Vector2DD)obj;
            return v == this;
        }

        public override int GetHashCode()
        {
            return (int)X ^ (int)Y;
        }
    }
}
