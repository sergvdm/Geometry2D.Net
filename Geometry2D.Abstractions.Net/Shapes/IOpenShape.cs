namespace Svd.Geometry2D.Shapes
{
    public interface IOpenShape : ILimitedShape
    {
        Point2DD StartPoint { get; }
        Point2DD EndPoint { get; }
        Point2DD Midpoint();
        bool IsSinglePoint();
        double Length();
        double LengthFromStart(Point2DD point);
        Vector2DD Tangent(Point2DD point);
        double Curvature(Point2DD point);
    }
}
