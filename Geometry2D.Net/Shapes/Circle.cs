﻿namespace Svd.Geometry2D.Shapes
{
    internal class Circle : ClosedShape, ICircle
    {
        public Circle(Point2DD center, double radius)
            : base()
        {
            Center = center;
            Radius = radius;
        }

        protected Circle(Circle other)
            : base(other)
        {
            Center = other.Center;
            Radius = other.Radius;
        }

        public Point2DD Center { get; }
        public double Radius { get; }
        public override double Perimeter() => Math2D.DPI * Radius;
        public override double Area() => Math2D.PI * Radius * Radius;

        public override bool Equal(IShape obj)
        {
            if (ReferenceEquals(obj, this)) return true;
            if (ReferenceEquals(obj, null) || !(obj is Circle other)) return false;
            return Center == other.Center && Radius == other.Radius;
        }

        public Circle Clone() => new Circle(this);
        protected override Shape CloneImpl() => Clone();
        ICircle ICircle.Clone() => Clone();

        protected override AARect2DD CalcBoundingRect()
        {
            return new AARect2DD(new Point2DD(Center.X - Radius, Center.Y - Radius), new Point2DD(Center.X + Radius, Center.Y + Radius));
        }

        protected override void BuildRegion(IGeometry2DEngine geometry2DEngine, IRegionBuilder regionBuilder, IPathBuilder pathBuilder, bool invert)
        {
            pathBuilder.BeginPath(invert);
            var startPoint = new Point2DD(Center.X + Radius, Center.Y);
            pathBuilder.Add(new Arc(startPoint, startPoint, Radius));
            regionBuilder.Add(pathBuilder.EndPath(true));
        }

        public override string ToString()
        {
            return $"Circle({Center},{Radius})";
        }
    }
}