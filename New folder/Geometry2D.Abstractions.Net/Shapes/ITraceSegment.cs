namespace Altium.Geometry2D.Shapes
{
    public interface ITraceSegment : IClosedShape
    {
        IPathSegment Guide { get; }
        double Width { get; }
        new ITraceSegment Clone();
    }
}
