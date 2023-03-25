using Svd.Geometry2D.Shapes;
using System.Collections.Generic;

namespace Svd.Geometry2D
{
    public interface IPath : ILimitedShape
    {
        IReadOnlyList<IPathSegment> Segments { get; }
        bool IsClosed { get; }
        Point2DD? GetInnerPoint();
        int Winding { get; }
        WindingInfo CalcWindingInfo(Point2DD point);
        bool IsInnerPoint(Point2DD point, bool includeCoincident);
        new IPath Clone();
        IPath Reverse();
    }
}
