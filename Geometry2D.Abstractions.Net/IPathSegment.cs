using Svd.Geometry2D.Shapes;
using System;

namespace Svd.Geometry2D
{
    public interface IPathSegment : IOpenShape
    {
        IPathSegment SnapToGrid(IGeometry2DEngine geometry2DEngine);
        PathVertex Split(Point2DD point);
        IPathSegment TryJoin(IGeometry2DEngine geometry2DEngine, IPathSegment otherSegment, PathSegmentJoinMode otherSegmentJoinMode);
        IPathSegment Reverse();
        int WindingNumber(IGeometry2DEngine geometry2DEngine, Point2DD point);
        new IPathSegment Clone();
    }

    [Flags]
    public enum PathSegmentJoinMode
    {
        Start = 1,
        End = 2,
        Any = Start | End
    }
}
