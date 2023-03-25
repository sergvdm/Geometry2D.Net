using Svd.Geometry2D.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Svd.Geometry2D
{
    internal class Path : LimitedShape, IPath
    {
        private const double DefaultWindingTestPointOffset = 0.01;

        private readonly IGeometry2DEngine geometry2DEngine;

        internal Path(IGeometry2DEngine geometry2DEngine, IEnumerable<IPathSegment> segments)
        {
            this.geometry2DEngine = geometry2DEngine;
            Segments = segments.ToList();
        }

        protected Path(IGeometry2DEngine geometry2DEngine, Path other)
            : base(other)
        {
            this.geometry2DEngine = geometry2DEngine;
            Segments = other.Segments.Select(x => x.Clone()).ToList();
        }

        public IReadOnlyList<IPathSegment> Segments { get; private set; }
        public bool IsClosed => Segments.Count > 0 && Segments[0].StartPoint == Segments[Segments.Count - 1].EndPoint;

        public override bool Equal(IShape obj)
        {
            if (ReferenceEquals(obj, this)) return true;
            if (ReferenceEquals(obj, null) || !(obj is Path other)) return false;
            if (Segments.Count != other.Segments.Count) return false;
            for (int i = 0; i < Segments.Count; i++)
            {
                if (!Segments[i].Equal(other.Segments[i])) return false;
            }

            return true;
        }

        public Path Clone() => new Path(geometry2DEngine, this);
        protected override Shape CloneImpl() => Clone();
        IPath IPath.Clone() => Clone();

        private int? winding;
        public int Winding
        {
            get
            {
                if (!winding.HasValue)
                    winding = CalcWinding();
                return winding.Value;
            }
        }

        private Point2DD? innerPoint = null;
        public Point2DD? GetInnerPoint()
        {
            if (!innerPoint.HasValue && IsClosed)
                innerPoint = FindInnerPoint(out _);
            return innerPoint;
        }

        public WindingInfo CalcWindingInfo(Point2DD point)
        {
            var result = new WindingInfo()
            {
                Coincident = geometry2DEngine.ShortestLineBetween(this, point).Length() < (geometry2DEngine.Epsilon * 1E-8)
            };
            if (!result.Coincident && IsClosed)
                result.Winding = CalcWinding(point);
            return result;
        }

        public bool IsInnerPoint(Point2DD point, bool includeCoincident)
        {
            var wi = CalcWindingInfo(point);
            return (!wi.Coincident && wi.Winding != 0) || (wi.Coincident && includeCoincident);
        }

        public IPath Reverse()
        {
            return new Path(geometry2DEngine, Segments.Select(x => x.Reverse()).Reverse());
        }

        protected void InvalidateWinding()
        {
            winding = null;
        }

        protected int CalcWinding(Point2DD point)
        {
            int wn = 0;
            foreach (var segment in Segments)
                wn += segment.WindingNumber(geometry2DEngine, point);
            return Math.Sign(wn);
        }

        protected Point2DD? FindInnerPoint(out int winding)
        {
            if (IsClosed && Segments.Count > 0)
            {
                double maxDistanceFromContour = double.MinValue;
                Point2DD innerPointResult = Point2DD.Zero;
                int windingResult = 0;
                for (int i = 0; i < Segments.Count; i++)
                {
                    var segment = Segments[i];
                    var midpoint = segment.Midpoint();
                    var normalLineViaMidpoint = new InfiniteLine(midpoint, segment.Tangent(midpoint).Ortogonal());
                    for (int j = 0; j < Segments.Count; j++)
                    {
                        var k = i + 1 + j;
                        if (k >= Segments.Count) k -= Segments.Count;
                        var testSegment = Segments[k];
                        var intersectionPoints = geometry2DEngine.Intersections(normalLineViaMidpoint, Segments[k]);
                        if (intersectionPoints.Any())
                        {
                            var closestIntersection = k != i ? intersectionPoints.OrderBy(x => x.DistanceTo(midpoint)).First() :
                                intersectionPoints.OrderByDescending(x => x.DistanceTo(midpoint)).First();
                            var testPoint = new Point2DD((midpoint.X + closestIntersection.X) / 2, (midpoint.Y + closestIntersection.Y) / 2);
                            var testPointWinding = CalcWinding(testPoint);
                            if (testPointWinding != 0)
                            {
                                var distanceFromContour = geometry2DEngine.ShortestLineBetween(testPoint, this).Length();
                                if (distanceFromContour > maxDistanceFromContour)
                                {
                                    maxDistanceFromContour = distanceFromContour;
                                    innerPointResult = testPoint;
                                    windingResult = testPointWinding;
                                }
                            }
                        }
                    }
                }

                if (windingResult != 0)
                {
                    winding = windingResult;
                    return innerPointResult;
                }
            }

            winding = 0;
            return null;
        }

        protected virtual int CalcWinding() => FindInnerPoint(out var winding).HasValue ? winding : 0;

        protected override AARect2DD CalcBoundingRect()
        {
            var result = AARect2DD.EmptyAABB;
            foreach (var segment in Segments)
                result.UnionWith(segment.BoundingRect);
            return result;
        }

        protected override void BuildRegion(IGeometry2DEngine geometry2DEngine, IRegionBuilder regionBuilder, IPathBuilder pathBuilder, bool invert)
        {
            regionBuilder.Add(this);
        }
    }
}
