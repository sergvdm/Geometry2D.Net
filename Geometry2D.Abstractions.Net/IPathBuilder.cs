using System.Collections.Generic;

namespace Svd.Geometry2D
{
    public interface IPathBuilder
    {
        void BeginPath(bool reverse = false, bool allowReduce = true);
        void Add(Point2DD lineVertex);
        void Add(params Point2DD[] lineVertices);
        void Add(IEnumerable<Point2DD> lineVertices);
        void Add(IPathSegment segment);
        void Add(params IPathSegment[] segments);
        void Add(IEnumerable<IPathSegment> segments);
        bool IsClosed { get; }
        void ClosePath();
        IPath EndPath(bool close);
    }
}
