using System.Collections.Generic;

namespace Svd.Geometry2D.Shapes
{
    public interface IPolygon : IClosedShape
    {
        IReadOnlyList<Point2DD> Vertices { get; }
        new IPolygon Clone();
    }
}
