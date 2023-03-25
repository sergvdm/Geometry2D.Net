namespace Altium.Geometry2D.Shapes
{
    public interface ILimitedShape : IShape
    {
        AARect2DD BoundingRect { get; }
        IRegion CreateRegion(IGeometry2DEngine geometry2DEngine, bool invert = false);
    }
}
