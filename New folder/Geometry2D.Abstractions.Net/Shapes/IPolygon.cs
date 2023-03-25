using System.Collections.Generic;

namespace Altium.Geometry2D.Shapes
{
    public interface IPolygon : IClosedShape
    {
        IReadOnlyList<Point2DD> Vertices { get; }
        new IPolygon Clone();
    }
}
