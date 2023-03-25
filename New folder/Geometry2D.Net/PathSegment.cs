using Altium.Geometry2D.Shapes;

namespace Altium.Geometry2D
{
    internal abstract class PathSegment : OpenShape, IPathSegment
    {
        protected PathSegment()
        {
        }

        protected PathSegment(PathSegment other)
            : base(other)
        {
        }

        public abstract IPathSegment SnapToGrid(IGeometry2DEngine geometry2DEngine);
        public abstract PathVertex Split(Point2DD point);
        public abstract IPathSegment TryJoin(IGeometry2DEngine geometry2DEngine, IPathSegment otherSegment, PathSegmentJoinMode otherSegmentJoinMode);
        protected abstract PathSegment ReverseImpl();
        IPathSegment IPathSegment.Reverse() => ReverseImpl();
        public abstract int WindingNumber(IGeometry2DEngine geometry2DEngine, Point2DD point);
        IPathSegment IPathSegment.Clone() => (IPathSegment)CloneImpl();
    }
}
