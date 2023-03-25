using System;

namespace Svd.Geometry2D
{
    /// <summary>
    /// Axis-aligned rectangle (double)
    /// </summary>
    [Serializable]
    public struct AARect2DD
    {
        public static readonly AARect2DD Zero = new AARect2DD();
        public static readonly AARect2DD EmptyAABB = new AARect2DD()
        {
            LB = new Point2DD(double.PositiveInfinity, double.PositiveInfinity),
            RT = new Point2DD(double.NegativeInfinity, double.NegativeInfinity)
        };

        public static implicit operator AARect2DD(AARect2DI rect)
        {
            return new AARect2DD() { LB = rect.LB, RT = rect.RT };
        }

        public static implicit operator AARect2DD(AARect2DF rect)
        {
            return new AARect2DD() { LB = rect.LB, RT = rect.RT };
        }

        public static bool operator ==(AARect2DD r1, AARect2DD r2)
        {
            return r1.LB == r2.LB && r1.RT == r2.RT;
        }

        public static bool operator !=(AARect2DD r1, AARect2DD r2)
        {
            return !(r1 == r2);
        }

        public AARect2DD(double x1, double y1, double x2, double y2)
        {
            LB = new Point2DD(Math.Min(x1, x2), Math.Min(y1, y2));
            RT = new Point2DD(Math.Max(x1, x2), Math.Max(y1, y2));
        }

        public AARect2DD(Point2DD lb, Point2DD rt)
        {
            LB = new Point2DD(Math.Min(lb.X, rt.X), Math.Min(lb.Y, rt.Y));
            RT = new Point2DD(Math.Max(lb.X, rt.X), Math.Max(lb.Y, rt.Y));
        }

        public AARect2DD(Point2DD center, double width, double height)
        {
            var v = new Vector2DD(width / 2, height / 2);
            LB = center - v;
            RT = center + v;
        }

        public Point2DD LB;
        public Point2DD RT;

        public Point2DD LT => new Point2DD(LB.X, RT.Y);
        public Point2DD RB => new Point2DD(RT.X, LB.Y);
        public Point2DD Center => new Point2DD((LB.X + RT.X) / 2, (LB.Y + RT.Y) / 2);
        public double Width => RT.X - LB.X;
        public double Height => RT.Y - LB.Y;
        public double DiagonalLength => LB.DistanceTo(RT);
        public double Area => Width * Height;
        public double MinSize => Math.Min(Width, Height);
        public double MaxSize => Math.Max(Width, Height);

        public bool Contains(Point2DD pt)
        {
            return LB.X <= pt.X && pt.X <= RT.X && LB.Y <= pt.Y && pt.Y <= RT.Y;
        }

        public bool Contains(AARect2DD rect)
        {
            return LB.X <= rect.LB.X && rect.RT.X <= RT.X && LB.Y <= rect.LB.Y && rect.RT.Y <= RT.Y;
        }

        public bool Intersect(AARect2DD rect)
        {
            var x1 = LB.X >= rect.LB.X ? LB.X : rect.LB.X;
            var x2 = RT.X <= rect.RT.X ? RT.X : rect.RT.X;
            if (x2 < x1) return false;
            var y1 = LB.Y >= rect.LB.Y ? LB.Y : rect.LB.Y;
            var y2 = RT.Y <= rect.RT.Y ? RT.Y : rect.RT.Y;
            if (y2 < y1) return false;
            return true;
        }

        public AARect2DD? Intersection(AARect2DD rect)
        {
            var x1 = Math.Max(LB.X, rect.LB.X);
            var x2 = Math.Min(RT.X, rect.RT.X);
            var y1 = Math.Max(LB.Y, rect.LB.Y);
            var y2 = Math.Min(RT.Y, rect.RT.Y);
            if (x2 >= x1 && y2 >= y1) return new AARect2DD(new Point2DD(x1, y1), new Point2DD(x2, y2));
            return null;
        }

        public AARect2DD Union(AARect2DD rect)
        {
            var x1 = Math.Min(LB.X, rect.LB.X);
            var x2 = Math.Max(RT.X, rect.RT.X);
            var y1 = Math.Min(LB.Y, rect.LB.Y);
            var y2 = Math.Max(RT.Y, rect.RT.Y);
            return new AARect2DD(new Point2DD(x1, y1), new Point2DD(x2, y2));
        }

        public void UnionWith(AARect2DD rect)
        {
            var x1 = Math.Min(LB.X, rect.LB.X);
            var x2 = Math.Max(RT.X, rect.RT.X);
            var y1 = Math.Min(LB.Y, rect.LB.Y);
            var y2 = Math.Max(RT.Y, rect.RT.Y);
            LB = new Point2DD(x1, y1);
            RT = new Point2DD(x2, y2);
        }

        public AARect2DD Inflate(double x, double y)
        {
            return new AARect2DD(new Point2DD(LB.X - x, LB.Y - y), new Point2DD(RT.X + x, RT.Y + y));
        }

        public void InflateWith(double x, double y)
        {
            LB.X -= x;
            LB.Y -= y;
            RT.X += x;
            RT.Y += y;
        }

        public bool IsSinglePoint()
        {
            return Width == 0 && Height == 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AARect2DD)) return false;
            AARect2DD other = (AARect2DD)obj;
            return other == this;
        }

        public override int GetHashCode()
        {
            return LB.GetHashCode() ^ RT.GetHashCode();
        }

        public override string ToString()
        {
            return $"AARect({LB}-{RT})";
        }
    }
}
