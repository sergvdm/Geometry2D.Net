namespace Svd.Geometry2D.Shapes
{
    public interface IArc : IPathSegment, ICircularShape
    {
        double StartAngle { get; }
        double EndAngle { get; }
        double SweepAngle { get; }
        new IArc Clone();
        new IArc Reverse();
    }
}
