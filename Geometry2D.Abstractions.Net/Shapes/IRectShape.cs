namespace Svd.Geometry2D.Shapes
{
    public interface IRectShape
    {
        Point2DD Center { get; }
        double Width { get; }
        double Height { get; }
        double Rotation { get; }
        double DiagonalLength { get; }
    }
}
