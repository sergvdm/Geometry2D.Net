using System;

namespace Altium.Geometry2D.Shapes
{
    internal class Line : PathSegment, ILine
    {
        public Line(Point2DD startPoint, Point2DD endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        private Line(Line other)
            : base(other)
        {
        }

        public override bool IsSinglePoint()
        {
            return StartPoint == EndPoint;
        }

        Point2DD ILinearShape.Point1 => StartPoint;
        Point2DD ILinearShape.Point2 => EndPoint;

        public override Point2DD Midpoint()
        {
            return new Point2DD((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2);
        }

        public Vector2DD Vector => (EndPoint - StartPoint).Unit();
        public override Vector2DD Tangent(Point2DD pt) => Vector;
        public override double Curvature(Point2DD pt) => 0;
        public override double Length() => StartPoint.DistanceTo(EndPoint);
        public override double LengthFromStart(Point2DD point) => StartPoint.DistanceTo(point);

        public override bool Equal(IShape obj)
        {
            if (ReferenceEquals(obj, this)) return true;
            if (ReferenceEquals(obj, null) || !(obj is Line other)) return false;
            return StartPoint == other.StartPoint && EndPoint == other.EndPoint;
        }

        public Ray AsRay(bool reverse = false) => reverse ? new Ray(EndPoint, StartPoint) : new Ray(StartPoint, EndPoint);
        IRay ILine.AsRay(bool reverse) => AsRay(reverse);

        public InfiniteLine AsInfinite() => new InfiniteLine(StartPoint, EndPoint);
        IInfiniteLine ILine.AsInfinite() => AsInfinite();

        public Line Clone() => new Line(this);
        protected override Shape CloneImpl() => Clone();
        ILine ILine.Clone() => Clone();

        public Line Reverse() => new Line(EndPoint, StartPoint);
        protected override PathSegment ReverseImpl() => Reverse();
        ILine ILine.Reverse() => Reverse();

        public override IPathSegment SnapToGrid(IGeometry2DEngine geometry2DEngine)
        {
            return new Line(geometry2DEngine.SnapToGrid(StartPoint), geometry2DEngine.SnapToGrid(EndPoint));
        }

        public override PathVertex Split(Point2DD pt)
        {
            //pt = pt.ProjectTo(this);
            //if ((pt - StartPoint) * Vector < 0) pt = StartPoint;
            //if ((pt - EndPoint) * Vector > 0) pt = EndPoint;
            var line1 = new Line(StartPoint, pt);
            var line2 = new Line(pt, EndPoint);
            return new PathVertex(line1, line2);
        }

        public override IPathSegment TryJoin(IGeometry2DEngine geometry2DEngine, IPathSegment otherSegment, PathSegmentJoinMode otherSegmentJoinMode)
        {
            var tryJoinStart = otherSegmentJoinMode.HasFlag(PathSegmentJoinMode.Start) && otherSegment.StartPoint == EndPoint;
            var tryJoinEnd = otherSegmentJoinMode.HasFlag(PathSegmentJoinMode.End) && otherSegment.EndPoint == StartPoint;
            if (tryJoinStart)
            {
                if (otherSegment.IsSinglePoint()) return this;
                if (this.IsSinglePoint()) return otherSegment;
                if (otherSegment is Line otherLine && (Vector * otherLine.Vector) >= 0)
                {
                    var otherLineNewStartPoint = otherSegment.StartPoint - otherLine.Vector.Unit() * this.Length();
                    var thisLineNewEndPoint = this.EndPoint + this.Vector.Unit() * otherLine.Length();
                    if (this.StartPoint.DistanceTo(otherLineNewStartPoint) < geometry2DEngine.Epsilon && otherLine.EndPoint.DistanceTo(thisLineNewEndPoint) < geometry2DEngine.Epsilon)
                        return new Line(StartPoint, otherLine.EndPoint);
                }
            }

            if (tryJoinEnd)
            {
                if (otherSegment.IsSinglePoint()) return this;
                if (this.IsSinglePoint()) return otherSegment;
                if (otherSegment is Line otherLine && (Vector * otherLine.Vector) >= 0)
                {
                    var thisLineNewStartPoint = this.StartPoint - this.Vector.Unit() * otherLine.Length();
                    var otherLineNewEndPoint = otherLine.EndPoint + otherLine.Vector.Unit() * this.Length();
                    if (otherLine.StartPoint.DistanceTo(thisLineNewStartPoint) < geometry2DEngine.Epsilon && this.EndPoint.DistanceTo(otherLineNewEndPoint) < geometry2DEngine.Epsilon)
                        return new Line(otherLine.StartPoint, EndPoint);
                }
            }

            return null;
        }

        public override int WindingNumber(IGeometry2DEngine geometry2DEngine, Point2DD pt)
        {
            var minY = Math.Min(StartPoint.Y, EndPoint.Y);
            var maxY = Math.Max(StartPoint.Y, EndPoint.Y);
            if (pt.Y < minY || pt.Y >= maxY || minY == maxY) return 0;
            return Math.Sign(Vector ^ (pt - StartPoint));
        }

        protected override AARect2DD CalcBoundingRect()
        {
            return new AARect2DD(StartPoint, EndPoint);
        }

        protected override void BuildRegion(IGeometry2DEngine geometry2DEngine, IRegionBuilder regionBuilder, IPathBuilder pathBuilder, bool invert)
        {
            pathBuilder.BeginPath(invert);
            pathBuilder.Add(this);
            regionBuilder.Add(pathBuilder.EndPath(false));
        }

        public override string ToString()
        {
            return $"Line({StartPoint},{EndPoint})";
        }
    }
}