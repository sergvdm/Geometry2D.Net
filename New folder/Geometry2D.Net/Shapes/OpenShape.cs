namespace Altium.Geometry2D.Shapes
{
    internal abstract class OpenShape : LimitedShape, IOpenShape
    {
        protected OpenShape()
        {
        }

        protected OpenShape(OpenShape other)
            : base(other)
        {
            StartPoint = other.StartPoint;
            EndPoint = other.EndPoint;
        }

        public Point2DD StartPoint { get; protected set; }
        public Point2DD EndPoint { get; protected set; }

        public abstract Point2DD Midpoint();

        public abstract bool IsSinglePoint();
        public abstract double Length();
        public abstract double LengthFromStart(Point2DD point);
        public abstract Vector2DD Tangent(Point2DD point);
        public abstract double Curvature(Point2DD point);
    }
}
