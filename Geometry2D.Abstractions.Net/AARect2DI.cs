using System;

namespace Svd.Geometry2D
{
    /// <summary>
    /// Axis-aligned rectangle (integer)
    /// </summary>
    [Serializable]
    public struct AARect2DI
    {
        public static readonly AARect2DI Zero = new AARect2DI();

        public static explicit operator AARect2DI(AARect2DF rect)
        {
            return new AARect2DI() { LB = (Point2DI)rect.LB, RT = (Point2DI)rect.RT };
        }

        public static explicit operator AARect2DI(AARect2DD rect)
        {
            return new AARect2DI() { LB = (Point2DI)rect.LB, RT = (Point2DI)rect.RT };
        }

        public static bool operator ==(AARect2DI r1, AARect2DI r2)
        {
            return r1.LB == r2.LB && r1.RT == r2.RT;
        }

        public static bool operator !=(AARect2DI r1, AARect2DI r2)
        {
            return !(r1 == r2);
        }

        public AARect2DI(int x1, int y1, int x2, int y2)
        {
            LB = new Point2DI(Math.Min(x1, x2), Math.Min(y1, y2));
            RT = new Point2DI(Math.Max(x1, x2), Math.Max(y1, y2));
        }

        public AARect2DI(Point2DI lb, Point2DI rt)
        {
            LB = new Point2DI(Math.Min(lb.X, rt.X), Math.Min(lb.Y, rt.Y));
            RT = new Point2DI(Math.Max(lb.X, rt.X), Math.Max(lb.Y, rt.Y));
        }

        public Point2DI LB;
        public Point2DI RT;

        public Point2DI LT => new Point2DI(LB.X, RT.Y);
        public Point2DI RB => new Point2DI(RT.X, LB.Y);
        public Point2DI Center => new Point2DI((LB.X + RT.X) / 2, (LB.Y + RT.Y) / 2);
        public int Width => RT.X - LB.X;
        public int Height => RT.Y - LB.Y;
        public int Area => Width * Height;
        public int MinSize => Math.Min(Width, Height);
        public int MaxSize => Math.Max(Width, Height);

        public bool Contains(Point2DI pt)
        {
            return LB.X <= pt.X && pt.X <= RT.X && LB.Y <= pt.Y && pt.Y <= RT.Y;
        }

        public bool Contains(AARect2DI rect)
        {
            return LB.X <= rect.LB.X && rect.RT.X <= RT.X && LB.Y <= rect.LB.Y && rect.RT.Y <= RT.Y;
        }

        public bool Intersect(AARect2DI rect)
        {
            var x1 = LB.X >= rect.LB.X ? LB.X : rect.LB.X;
            var x2 = RT.X <= rect.RT.X ? RT.X : rect.RT.X;
            if (x2 < x1) return false;
            var y1 = LB.Y >= rect.LB.Y ? LB.Y : rect.LB.Y;
            var y2 = RT.Y <= rect.RT.Y ? RT.Y : rect.RT.Y;
            if (y2 < y1) return false;
            return true;
        }

        public AARect2DI? Intersection(AARect2DI rect)
        {
            var x1 = Math.Max(LB.X, rect.LB.X);
            var x2 = Math.Min(RT.X, rect.RT.X);
            var y1 = Math.Max(LB.Y, rect.LB.Y);
            var y2 = Math.Min(RT.Y, rect.RT.Y);
            if (x2 >= x1 && y2 >= y1) return new AARect2DI(new Point2DI(x1, y1), new Point2DI(x2, y2));
            return null;
        }

        public AARect2DI Union(AARect2DI rect)
        {
            var x1 = Math.Min(LB.X, rect.LB.X);
            var x2 = Math.Max(RT.X, rect.RT.X);
            var y1 = Math.Min(LB.Y, rect.LB.Y);
            var y2 = Math.Max(RT.Y, rect.RT.Y);
            return new AARect2DI(new Point2DI(x1, y1), new Point2DI(x2, y2));
        }

        public void UnionWith(AARect2DI rect)
        {
            var x1 = Math.Min(LB.X, rect.LB.X);
            var x2 = Math.Max(RT.X, rect.RT.X);
            var y1 = Math.Min(LB.Y, rect.LB.Y);
            var y2 = Math.Max(RT.Y, rect.RT.Y);
            LB = new Point2DI(x1, y1);
            RT = new Point2DI(x2, y2);
        }

        public AARect2DI Inflate(int x, int y)
        {
            return new AARect2DI(new Point2DI(LB.X - x, LB.Y - y), new Point2DI(RT.X + x, RT.Y + y));
        }

        public void InflateWith(int x, int y)
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
            if (!(obj is AARect2DI)) return false;
            AARect2DI other = (AARect2DI)obj;
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
