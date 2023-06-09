﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svd.Geometry2D.Shapes
{
    internal class Polygon : ClosedShape, IPolygon
    {
        public Polygon(params Point2DD[] vertices)
            : this((IEnumerable<Point2DD>)vertices)
        {
        }

        public Polygon(IEnumerable<Point2DD> vertices)
        {
            Vertices = vertices.ToList();
        }

        protected Polygon(Polygon other)
            : base(other)
        {
            Vertices = other.Vertices.ToList();
        }

        public IReadOnlyList<Point2DD> Vertices { get; }

        public override bool Equal(IShape obj)
        {
            if (ReferenceEquals(obj, this)) return true;
            if (ReferenceEquals(obj, null) || !(obj is Polygon other)) return false;
            return Vertices.SequenceEqual(other.Vertices);
        }

        public Polygon Clone() => new Polygon(this);
        protected override Shape CloneImpl() => Clone();
        IPolygon IPolygon.Clone() => Clone();

        protected override AARect2DD CalcBoundingRect()
        {
            double left = double.MaxValue, top = double.MinValue, right = double.MinValue, bottom = double.MaxValue;
            foreach (var vertex in Vertices)
            {
                if (left > vertex.X) left = vertex.X;
                if (top < vertex.Y) top = vertex.Y;
                if (right < vertex.X) right = vertex.X;
                if (bottom > vertex.Y) bottom = vertex.Y;
            }

            if (left > right) return AARect2DD.Zero;
            return new AARect2DD(new Point2DD(left, bottom), new Point2DD(right, top));
        }

        public override double Perimeter()
        {
            double result = 0;
            if (Vertices.Count > 1)
            {
                var pv = Vertices[0];
                for (int i = 1; i < Vertices.Count; i++)
                {
                    var v = Vertices[i];
                    result += pv.DistanceTo(v);
                    pv = v;
                }

                result += pv.DistanceTo(Vertices[0]);
            }

            return result;
        }

        public override double Area()
        {
            throw new NotImplementedException();
        }

        protected override void BuildRegion(IGeometry2DEngine geometry2DEngine, IRegionBuilder regionBuilder, IPathBuilder pathBuilder, bool invert)
        {
            pathBuilder.BeginPath(invert);
            pathBuilder.Add(Vertices);
            regionBuilder.Add(pathBuilder.EndPath(true));
        }

        public override string ToString()
        {
            var sb = new StringBuilder($"Polygon(N={Vertices.Count}");
            int i = 0;
            foreach (var vertex in Vertices.Take(4))
            {
                sb.Append($",V{i++}={vertex}");
            }

            if (Vertices.Count > 4) sb.Append(",...");
            sb.Append(')');
            return sb.ToString();
        }
    }
}
