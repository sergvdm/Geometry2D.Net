namespace Svd.Geometry2D
{
    public struct PathVertex
    {
        public PathVertex(IPathSegment prevSegment, IPathSegment nextSegment)
        {
            PrevSegment = prevSegment;
            NextSegment = nextSegment;
        }

        public IPathSegment PrevSegment;
        public IPathSegment NextSegment;
        public Point2DD Point => PrevSegment != null ? PrevSegment.EndPoint : NextSegment != null ? NextSegment.StartPoint : default(Point2DD);

        public override string ToString()
        {
            return $"PV({Point})";
        }
    }
}
