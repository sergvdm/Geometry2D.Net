using System.Collections.Generic;
using System.Linq;

namespace Altium.Geometry2D
{
    public class PathVertexIterator
    {
        private readonly IReadOnlyList<IPathSegment> segments;
        private readonly bool isClosed;

        public PathVertexIterator(IPath path)
            : this(path.Segments)
        {
        }

        public PathVertexIterator(IReadOnlyList<IPathSegment> segments)
        {
            this.segments = segments;
            this.isClosed = segments.Count > 0 && segments[0].StartPoint == segments[segments.Count - 1].EndPoint;
        }

        public IEnumerable<PathVertex> Vertices
        {
            get
            {
                var prevSegment = isClosed ? segments.Last() : null;
                foreach (var nextSegment in segments)
                {
                    yield return new PathVertex(prevSegment, nextSegment);
                    prevSegment = nextSegment;
                }

                if (!isClosed)
                    yield return new PathVertex(prevSegment, null);
            }
        }
    }
}
