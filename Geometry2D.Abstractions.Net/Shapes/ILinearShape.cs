namespace Svd.Geometry2D.Shapes
{
    public interface ILinearShape
    {
        Point2DD Point1 { get; }
        Point2DD Point2 { get; }
        Vector2DD Vector { get; }
    }
}
