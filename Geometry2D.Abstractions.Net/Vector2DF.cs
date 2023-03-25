using System;
using System.Globalization;

namespace Svd.Geometry2D
{
    /// <summary>
    /// 2D vector (float)
    /// </summary>
    [Serializable]
    public struct Vector2DF
    {
        public static readonly Vector2DF Zero = new Vector2DF();

        public static explicit operator Vector2DF(Vector2DD v)
        {
            return new Vector2DF((float)v.X, (float)v.Y);
        }

        public static explicit operator Vector2DF(Point2DF pt)
        {
            return new Vector2DF(pt.X, pt.Y);
        }

        public static bool operator ==(Vector2DF v1, Vector2DF v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(Vector2DF v1, Vector2DF v2)
        {
            return !(v1 == v2);
        }

        public static Vector2DF operator -(Vector2DF v)
        {
            return new Vector2DF(-v.X, -v.Y);
        }

        public static Vector2DF operator +(Vector2DF v1, Vector2DF v2)
        {
            return new Vector2DF(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2DF operator -(Vector2DF v1, Vector2DF v2)
        {
            return new Vector2DF(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Point2DF operator +(Point2DF pt, Vector2DF v)
        {
            return new Point2DF(pt.X + v.X, pt.Y + v.Y);
        }

        public static Point2DF operator +(Vector2DF v, Point2DF pt)
        {
            return new Point2DF(v.X + pt.X, v.Y + pt.Y);
        }

        public static Point2DF operator -(Point2DF pt, Vector2DF v)
        {
            return new Point2DF(pt.X - v.X, pt.Y - v.Y);
        }

        public static float operator *(Vector2DF v1, Vector2DF v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static Vector2DF operator *(float c, Vector2DF v)
        {
            return new Vector2DF(c * v.X, c * v.Y);
        }

        public static Vector2DF operator *(Vector2DF v, float c)
        {
            return new Vector2DF(v.X * c, v.Y * c);
        }

        public static Vector2DF operator /(Vector2DF v, float c)
        {
            return new Vector2DF(v.X / c, v.Y / c);
        }

        public static float operator ^(Vector2DF v1, Vector2DF v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        public Vector2DF(Point2DF start, Point2DF end)
        {
            X = end.X - start.X;
            Y = end.Y - start.Y;
        }

        public Vector2DF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2DF(double angle)
        {
            angle = angle.NormalizeRadAngle();
            X = (float)Math.Cos(angle);
            Y = (float)Math.Sin(angle);
        }

        public float X;
        public float Y;

        public float Angle => (float)Math.Atan2(Y, X);

        public float Norm()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public Vector2DF Unit()
        {
            if (X == 0 && Y == 0) return new Vector2DF(1, 0);
            var n = Norm();
            if (n < 1E-6) return new Vector2DF(Angle);
            else return this / n;
        }

        public Vector2DF Ortogonal()
        {
            return new Vector2DF(-Y, X);
        }

        public Vector2DF ProjectTo(Vector2DF s)
        {
            return (this * s) / (s * s) * s;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0:F2},{1:F2}]", X, Y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2DF)) return false;
            Vector2DF v = (Vector2DF)obj;
            return v == this;
        }

        public override int GetHashCode()
        {
            return (int)X ^ (int)Y;
        }
    }
}
