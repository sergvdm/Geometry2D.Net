using System;

namespace Altium.Geometry2D
{
    /// <summary>
    /// Axis-aligned rectangle (float)
    /// </summary>
    [Serializable]
    public struct AARect2DF
    {
        public static readonly AARect2DF Zero = new AARect2DF();
        public static readonly AARect2DF EmptyAABB = new AARect2DF()
        {
            LB = new Point2DF(float.PositiveInfinity, float.PositiveInfinity),
            RT = new Point2DF(float.NegativeInfinity, float.NegativeInfinity)
        };

        public static implicit operator AARect2DF(AARect2DI rect)
        {
            return new AARect2DF() { LB = rect.LB, RT = rect.RT };
        }

        public static explicit operator AARect2DF(AARect2DD rect)
        {
            return new AARect2DF() { LB = (Point2DF)rect.LB, RT = (Point2DF)rect.RT };
        }

        public static bool operator ==(AARect2DF r1, AARect2DF r2)
        {
            return r1.LB == r2.LB && r1.RT == r2.RT;
        }

        public static bool operator !=(AARect2DF r1, AARect2DF r2)
        {
            return !(r1 == r2);
        }

        public AARect2DF(float x1, float y1, float x2, float y2)
        {
            LB = new Point2DF(Math.Min(x1, x2), Math.Min(y1, y2));
            RT = new Point2DF(Math.Max(x1, x2), Math.Max(y1, y2));
        }

        public AARect2DF(Point2DF lb, Point2DF rt)
        {
            LB = new Point2DF(Math.Min(lb.X, rt.X), Math.Min(lb.Y, rt.Y));
            RT = new Point2DF(Math.Max(lb.X, rt.X), Math.Max(lb.Y, rt.Y));
        }

        public AARect2DF(Point2DF center, float width, float height)
        {
            var v = new Vector2DF(width / 2, height / 2);
            LB = center - v;
            RT = center + v;
        }

        public Point2DF LB;
        public Point2DF RT;

        public Point2DF LT => new Point2DF(LB.X, RT.Y);
        public Point2DF RB => new Point2DF(RT.X, LB.Y);
        public Point2DF Center => new Point2DF((LB.X + RT.X) / 2, (LB.Y + RT.Y) / 2);
        public float Width => RT.X - LB.X;
        public float Height => RT.Y - LB.Y;
        public float DiagonalLength => (float)Math2D.DistanceTo(LB, RT);
        public float Area => Width * Height;
        public float MinSize => Math.Min(Width, Height);
        public float MaxSize => Math.Max(Width, Height);

        public bool Contains(Point2DF pt)
        {
            return LB.X <= pt.X && pt.X <= RT.X && LB.Y <= pt.Y && pt.Y <= RT.Y;
        }

        public bool Contains(AARect2DF rect)
        {
            return LB.X <= rect.LB.X && rect.RT.X <= RT.X && LB.Y <= rect.LB.Y && rect.RT.Y <= RT.Y;
        }

        public bool Intersect(AARect2DF rect)
        {
            var x1 = LB.X >= rect.LB.X ? LB.X : rect.LB.X;
            var x2 = RT.X <= rect.RT.X ? RT.X : rect.RT.X;
            if (x2 < x1) return false;
            var y1 = LB.Y >= rect.LB.Y ? LB.Y : rect.LB.Y;
            var y2 = RT.Y <= rect.RT.Y ? RT.Y : rect.RT.Y;
            if (y2 < y1) return false;
            return true;
        }

        public AARect2DF? Intersection(AARect2DF rect)
        {
            var x1 = Math.Max(LB.X, rect.LB.X);
            var x2 = Math.Min(RT.X, rect.RT.X);
            var y1 = Math.Max(LB.Y, rect.LB.Y);
            var y2 = Math.Min(RT.Y, rect.RT.Y);
            if (x2 >= x1 && y2 >= y1) return new AARect2DF(new Point2DF(x1, y1), new Point2DF(x2, y2));
            return null;
        }

        public AARect2DF Union(AARect2DF rect)
        {
            var x1 = Math.Min(LB.X, rect.LB.X);
            var x2 = Math.Max(RT.X, rect.RT.X);
            var y1 = Math.Min(LB.Y, rect.LB.Y);
            var y2 = Math.Max(RT.Y, rect.RT.Y);
            return new AARect2DF(new Point2DF(x1, y1), new Point2DF(x2, y2));
        }

        public void UnionWith(AARect2DF rect)
        {
            var x1 = Math.Min(LB.X, rect.LB.X);
            var x2 = Math.Max(RT.X, rect.RT.X);
            var y1 = Math.Min(LB.Y, rect.LB.Y);
            var y2 = Math.Max(RT.Y, rect.RT.Y);
            LB = new Point2DF(x1, y1);
            RT = new Point2DF(x2, y2);
        }

        public AARect2DF Inflate(float x, float y)
        {
            return new AARect2DF(new Point2DF(LB.X - x, LB.Y - y), new Point2DF(RT.X + x, RT.Y + y));
        }

        public void InflateWith(float x, float y)
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
            if (!(obj is AARect2DF)) return false;
            AARect2DF other = (AARect2DF)obj;
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
