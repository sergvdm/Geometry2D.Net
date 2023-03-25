using System.Collections.Generic;
using System.Linq;

namespace Svd.Geometry2D
{
    public static class Extensions
    {
        public static IPath CreatePath(this IGeometry2DEngine geometry2DEngine, bool autoClosePaths, params Point2DD[] points)
        {
            return CreatePath(geometry2DEngine, autoClosePaths, (IEnumerable<Point2DD>)points);
        }

        public static IPath CreatePath(this IGeometry2DEngine geometry2DEngine, bool autoClosePaths, IEnumerable<Point2DD> points)
        {
            var pathBuilder = geometry2DEngine.CreatePathBuilder();
            pathBuilder.BeginPath();
            pathBuilder.Add(points);
            return pathBuilder.EndPath(autoClosePaths);
        }

        public static IPath CreatePath(this IGeometry2DEngine geometry2DEngine, bool autoClosePaths, params IPathSegment[] segments)
        {
            return CreatePath(geometry2DEngine, autoClosePaths, (IEnumerable<IPathSegment>)segments);
        }

        public static IPath CreatePath(this IGeometry2DEngine geometry2DEngine, bool autoClosePaths, IEnumerable<IPathSegment> segments)
        {
            var pathBuilder = geometry2DEngine.CreatePathBuilder();
            pathBuilder.BeginPath();
            pathBuilder.Add(segments);
            return pathBuilder.EndPath(autoClosePaths);
        }

        public static IRegion CreateRegion(this IGeometry2DEngine geometry2DEngine, bool autoClosePaths, params Point2DD[] points)
        {
            return CreateRegion(geometry2DEngine, autoClosePaths, (IEnumerable<Point2DD>)points);
        }

        public static IRegion CreateRegion(this IGeometry2DEngine geometry2DEngine, bool autoClosePaths, IEnumerable<Point2DD> points)
        {
            var regionBuilder = geometry2DEngine.CreateRegionBuilder();
            regionBuilder.BeginRegion();
            regionBuilder.Add(CreatePath(geometry2DEngine, autoClosePaths, points));
            return regionBuilder.EndRegion();
        }

        public static IRegion CreateRegion(this IGeometry2DEngine geometry2DEngine, bool autoClosePaths, params IPathSegment[] segments)
        {
            return CreateRegion(geometry2DEngine, autoClosePaths, (IEnumerable<IPathSegment>)segments);
        }

        public static IRegion CreateRegion(this IGeometry2DEngine geometry2DEngine, bool autoClosePaths, IEnumerable<IPathSegment> segments)
        {
            var regionBuilder = geometry2DEngine.CreateRegionBuilder();
            regionBuilder.BeginRegion();
            regionBuilder.Add(CreatePath(geometry2DEngine, autoClosePaths, segments));
            return regionBuilder.EndRegion();
        }

        public static IRegion CreateRegion(this IGeometry2DEngine geometry2DEngine, params IPath[] paths)
        {
            return CreateRegion(geometry2DEngine, (IEnumerable<IPath>)paths);
        }

        public static IRegion CreateRegion(this IGeometry2DEngine geometry2DEngine, IEnumerable<IPath> paths)
        {
            var regionBuilder = geometry2DEngine.CreateRegionBuilder();
            regionBuilder.BeginRegion();
            regionBuilder.Add(paths);
            return regionBuilder.EndRegion();
        }

        public static IRegion CreateSliceRegion(this IGeometry2DEngine geometry2DEngine, params IPath[] paths)
        {
            return CreateSliceRegion(geometry2DEngine, (IEnumerable<IPath>)paths);
        }

        public static IRegion CreateSliceRegion(this IGeometry2DEngine geometry2DEngine, IEnumerable<IPath> paths)
        {
            var regionBuilder = geometry2DEngine.CreateRegionBuilder();
            var pathBuilder = geometry2DEngine.CreatePathBuilder();
            regionBuilder.BeginRegion();
            foreach (var path in paths)
            {
                if (path.IsClosed)
                {
                    // create two opened paths
                    var longestSegment = path.Segments.OrderBy(x => x.Length()).First();
                    var longestSegmentSplit = longestSegment.Split(longestSegment.Midpoint());
                    pathBuilder.BeginPath(false, false);
                    pathBuilder.Add(path.Segments.TakeWhile(x => x != longestSegment));
                    pathBuilder.Add(longestSegmentSplit.PrevSegment);
                    var path1 = pathBuilder.EndPath(false);
                    regionBuilder.Add(path1);
                    pathBuilder.BeginPath(false, false);
                    pathBuilder.Add(longestSegmentSplit.NextSegment);
                    pathBuilder.Add(path.Segments.SkipWhile(x => x != longestSegment).Skip(1));
                    var path2 = pathBuilder.EndPath(false);
                    regionBuilder.Add(path2);
                    regionBuilder.Add(path1.Reverse());
                    regionBuilder.Add(path2.Reverse());
                }
                else
                {
                    regionBuilder.Add(path);
                    regionBuilder.Add(path.Reverse());
                }
            }

            return regionBuilder.EndRegion();
        }
    }
}
