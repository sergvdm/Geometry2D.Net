﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Svd.Geometry2D.Shapes
{
    internal class RoundedPolygon : Polygon, IRoundedPolygon
    {
        public RoundedPolygon(double rounding, ShapeRoundingMode roundingMode, params Point2DD[] vertices)
            : this(new RoundedPolyInitData(rounding, roundingMode, vertices))
        {
        }

        internal RoundedPolygon(RoundedPolyInitData roundedPolyInitData)
            : base(roundedPolyInitData.Vertices)
        {
            Rounding = roundedPolyInitData.Rounding;
            RoundingMode = roundedPolyInitData.RoundingMode;
            RoundingArcs = roundedPolyInitData.RoundingArcs.ToList();
        }

        protected RoundedPolygon(RoundedPolygon other)
            : base(other)
        {
            Rounding = other.Rounding;
            RoundingMode = other.RoundingMode;
            RoundingArcs = other.RoundingArcs.Select(x => x.Clone()).ToList();
        }

        public double Rounding { get; }
        public ShapeRoundingMode RoundingMode { get; }
        public IReadOnlyList<IArc> RoundingArcs { get; }

        public override bool Equal(IShape obj)
        {
            if (ReferenceEquals(obj, this)) return true;
            if (ReferenceEquals(obj, null) || !(obj is RoundedPolygon other)) return false;
            return Rounding == other.Rounding && Vertices.SequenceEqual(other.Vertices);
        }

        public new RoundedPolygon Clone() => new RoundedPolygon(this);
        protected override Shape CloneImpl() => Clone();
        IRoundedPolygon IRoundedPolygon.Clone() => Clone();

        protected override AARect2DD CalcBoundingRect()
        {
            double left = 0, top = 0, right = 0, bottom = 0;
            foreach (var arc in RoundingArcs)
            {
                var xmin = arc.Center.X - arc.Radius;
                var xmax = arc.Center.X + arc.Radius;
                var ymin = arc.Center.Y - arc.Radius;
                var ymax = arc.Center.Y + arc.Radius;
                if (left > xmin) left = xmin;
                if (top < ymax) top = ymax;
                if (right < xmax) right = xmax;
                if (bottom > ymin) bottom = ymin;
            }

            return new AARect2DD(new Point2DD(left, bottom), new Point2DD(right, top));
        }

        protected override void BuildRegion(IGeometry2DEngine geometry2DEngine, IRegionBuilder regionBuilder, IPathBuilder pathBuilder, bool invert)
        {
            pathBuilder.BeginPath(invert);
            for (int i = 0; i < RoundingArcs.Count; i++)
            {
                var inext = i + 1;
                if (inext == RoundingArcs.Count) inext = 0;
                pathBuilder.Add(RoundingArcs[i]);
                pathBuilder.Add(new Line(RoundingArcs[i].EndPoint, RoundingArcs[inext].StartPoint));
            }

            regionBuilder.Add(pathBuilder.EndPath(true));
        }
    }

    internal class RoundedPolyInitData
    {
        public RoundedPolyInitData(double rounding, ShapeRoundingMode roundingMode, params Point2DD[] vertices)
        {
            RoundingMode = roundingMode;
            Rounding = RoundingMode == ShapeRoundingMode.Percent ? rounding.LimitRange(0, 100) : Math.Abs(rounding);
            Vertices = vertices;
            RoundingArcs = new IArc[Vertices.Length];
            for (int i = 0; i < Vertices.Length; i++)
            {
                var prevVertex = i == 0 ? Vertices[Vertices.Length - 1] : Vertices[i - 1];
                var nextVertex = i == (Vertices.Length - 1) ? Vertices[0] : Vertices[i + 1];
                var pv = new Vector2DD(Vertices[i], prevVertex);
                var nv = new Vector2DD(Vertices[i], nextVertex);
                var arcMaxRadius = Math.Min(pv.Norm(), nv.Norm()) / 2;
                var arcRadius = RoundingMode == ShapeRoundingMode.Percent ? arcMaxRadius * Rounding / 100 : Math.Min(Rounding, arcMaxRadius);
                var pvunit = pv.Unit();
                var nvunit = nv.Unit();
                var arcStartPoint = Vertices[i] + pvunit * arcRadius;
                var arcEndPoint = Vertices[i] + nvunit * arcRadius;
                var ir = Math2D.Intersection(new InfiniteLine(arcStartPoint, pvunit.Ortogonal()), new InfiniteLine(arcEndPoint, nvunit.Ortogonal()));
                if (ir.AreIntersected)
                {
                    var center = ir.Point1;
                    //var arcMidPoint = center - (pvunit + nvunit).Unit() * arcRadius;
                    RoundingArcs[i] = new Arc(arcStartPoint, arcEndPoint, arcRadius, center);
                }
                else
                {
                    RoundingArcs[i] = new Arc(vertices[i], 0, 0, 0);
                }
            }
        }

        public double Rounding { get; }
        public ShapeRoundingMode RoundingMode { get; }
        public Point2DD[] Vertices { get; }
        public IArc[] RoundingArcs { get; }
    }
}
