﻿namespace Altium.Geometry2D.Shapes
{
    internal class Ray : Shape, IRay
    {
        public Ray(Point2DD startPoint, Point2DD pointOnLine)
        {
            Point1 = startPoint;
            Point2 = pointOnLine;
        }

        public Ray(Point2DD startPoint, Vector2DD vector)
            : this(startPoint, startPoint + vector)
        {
        }

        protected Ray(Ray other)
            : base(other)
        {
            Point1 = other.Point1;
            Point2 = other.Point2;
        }

        public Point2DD Point1 { get; }
        public Point2DD Point2 { get; }
        public Vector2DD Vector => (Point2 - Point1).Unit();

        public override bool Equal(IShape obj)
        {
            if (ReferenceEquals(obj, this)) return true;
            if (ReferenceEquals(obj, null) || !(obj is Ray other)) return false;
            return Point1 == other.Point1 && Vector == other.Vector;
        }

        public Ray Clone() => new Ray(this);
        protected override Shape CloneImpl() => Clone();
        IRay IRay.Clone() => Clone();

        public override string ToString()
        {
            return $"Ray(P={Point1},V={Vector})";
        }
    }
}