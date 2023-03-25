using Svd.Geometry2D.Shapes;
using System.Collections.Generic;

namespace Svd.Geometry2D
{
    public interface IGeometry2DEngine
    {
        int Precision { get; }
        double Epsilon { get; }

        #region Primitive shapes factory

        Point2DD SnapToGrid(Point2DD point);
        IInfiniteLine CreateInfiniteLine(Point2DD point, Vector2DD vector);
        IInfiniteLine CreateInfiniteLine(ILinearShape refShape);
        IRay CreateRay(Point2DD point, Vector2DD vector);
        IRay CreateRay(ILinearShape refShape);
        ILine CreateLine(Point2DD startPoint, Point2DD endPoint);
        ILine CreateLine(ILinearShape refShape);
        IArc CreateArc(Point2DD startPoint, Point2DD endPoint, Point2DD pointOnArc);
        IArc CreateArc(Point2DD startPoint, Point2DD endPoint, double radius, bool cw = false, bool longest = false);
        IArc CreateArc(Point2DD center, double radius, double startAngle, double sweepAngle);
        IArc CreateArc(ICircularShape refShape, double startAngle, double sweepAngle);
        ICircle CreateCircle(Point2DD center, double radius);
        ICircle CreateCircle(ICircularShape refShape);
        IRectangle CreateRectangle(Point2DD center, double width, double height, double rotation = 0);
        IRectangle CreateRectangle(Point2DD vertex0, Point2DD vertex2, double rotation = 0);
        IRectangle CreateRectangle(IRectShape refShape);
        IRoundedRectangle CreateRoundedRectangle(Point2DD center, double width, double height, double rounding, ShapeRoundingMode roundingMode, double rotation = 0);
        IRoundedRectangle CreateRoundedRectangle(Point2DD vertex0, Point2DD vertex2, double rounding, ShapeRoundingMode roundingMode, double rotation = 0);
        IRoundedRectangle CreateRoundedRectangle(IRectShape refShape, double rounding, ShapeRoundingMode roundingMode);
        IBeveledRectangle CreateBeveledRectangle(Point2DD center, double width, double height, double beveling, ShapeBevelingMode bevelingMode, double rotation = 0);
        IBeveledRectangle CreateBeveledRectangle(Point2DD vertex0, Point2DD vertex2, double beveling, ShapeBevelingMode bevelingMode, double rotation = 0);
        IBeveledRectangle CreateBeveledRectangle(IRectShape refShape, double beveling, ShapeBevelingMode bevelingMode);
        IPolygon CreatePolygon(params Point2DD[] vertices);
        IPolygon CreatePolygon(IEnumerable<Point2DD> vertices);
        IRoundedPolygon CreateRoundedPolygon(double rounding, ShapeRoundingMode roundingMode, params Point2DD[] vertices);
        IRoundedPolygon CreateRoundedPolygon(double rounding, ShapeRoundingMode roundingMode, IEnumerable<Point2DD> vertices);
        IBeveledPolygon CreateBeveledPolygon(double beveling, ShapeBevelingMode bevelingMode, params Point2DD[] vertices);
        IBeveledPolygon CreateBeveledPolygon(double beveling, ShapeBevelingMode bevelingMode, IEnumerable<Point2DD> vertices);
        ILineTrace CreateLineTrace(ILine guide, double width);
        IArcTrace CreateArcTrace(IArc guide, double width);

        #endregion Primitive shapes factory

        #region Coincident check

        bool AreCoincident(Point2DD pt1, Point2DD pt2);
        bool AreCoincident(Point2DD pt, IInfiniteLine infLine);
        bool AreCoincident(Point2DD pt, IRay ray);
        bool AreCoincident(Point2DD pt, ILine line);
        bool AreCoincident(Point2DD pt, ICircle circle);
        bool AreCoincident(Point2DD pt, IArc arc);
        bool AreCoincident(Point2DD pt, IPathSegment segment);
        bool AreCoincident(Point2DD pt, IPath path);
        bool AreCoincident(Point2DD pt, IRegion region);

        bool AreCoincident(IInfiniteLine infLine, Point2DD pt);
        bool AreCoincident(IInfiniteLine infLine1, IInfiniteLine infLine2);
        bool AreCoincident(IInfiniteLine infLine, IRay ray);
        bool AreCoincident(IInfiniteLine infLine, ILine line);
        bool AreCoincident(IInfiniteLine infLine, ICircle circle);
        bool AreCoincident(IInfiniteLine infLine, IArc arc);
        bool AreCoincident(IInfiniteLine infLine, IPathSegment segment);
        bool AreCoincident(IInfiniteLine infLine, IPath path);
        bool AreCoincident(IInfiniteLine infLine, IRegion region);

        bool AreCoincident(IRay ray, Point2DD pt);
        bool AreCoincident(IRay ray, IInfiniteLine infLine);
        bool AreCoincident(IRay ray1, IRay ray2);
        bool AreCoincident(IRay ray, ILine line);
        bool AreCoincident(IRay ray, ICircle circle);
        bool AreCoincident(IRay ray, IArc arc);
        bool AreCoincident(IRay ray, IPathSegment segment);
        bool AreCoincident(IRay ray, IPath path);
        bool AreCoincident(IRay ray, IRegion region);

        bool AreCoincident(ILine line, Point2DD pt);
        bool AreCoincident(ILine line, IInfiniteLine infLine);
        bool AreCoincident(ILine line, IRay ray);
        bool AreCoincident(ILine line, ILine line2);
        bool AreCoincident(ILine line, ICircle circle);
        bool AreCoincident(ILine line, IArc arc);

        bool AreCoincident(ICircle circle, Point2DD pt);
        bool AreCoincident(ICircle circle, IInfiniteLine infLine);
        bool AreCoincident(ICircle circle, IRay ray);
        bool AreCoincident(ICircle circle, ILine line);
        bool AreCoincident(ICircle circle1, ICircle circle2);
        bool AreCoincident(ICircle circle, IArc arc);
        bool AreCoincident(ICircle circle, IPathSegment segment);
        bool AreCoincident(ICircle circle, IPath path);
        bool AreCoincident(ICircle circle, IRegion region);

        bool AreCoincident(IArc arc, Point2DD pt);
        bool AreCoincident(IArc arc, IInfiniteLine infLine);
        bool AreCoincident(IArc arc, IRay ray);
        bool AreCoincident(IArc arc, ILine line);
        bool AreCoincident(IArc arc, ICircle circle);
        bool AreCoincident(IArc arc1, IArc arc2);

        bool AreCoincident(IPathSegment segment, Point2DD point);
        bool AreCoincident(IPathSegment segment, IInfiniteLine infLine);
        bool AreCoincident(IPathSegment segment, IRay ray);
        bool AreCoincident(IPathSegment segment1, IPathSegment segment2);
        bool AreCoincident(IPathSegment segment, IPath path);
        bool AreCoincident(IPathSegment segment, IRegion region);

        bool AreCoincident(IPath path, Point2DD point);
        bool AreCoincident(IPath path, IInfiniteLine infLine);
        bool AreCoincident(IPath path, IRay ray);
        bool AreCoincident(IPath path, IPathSegment segment);
        bool AreCoincident(IPath path1, IPath path2);
        bool AreCoincident(IPath path, IRegion region);

        bool AreCoincident(IRegion region, Point2DD point);
        bool AreCoincident(IRegion region, IInfiniteLine infLine);
        bool AreCoincident(IRegion region, IRay ray);
        bool AreCoincident(IRegion region, IPathSegment segment);
        bool AreCoincident(IRegion region, IPath path);
        bool AreCoincident(IRegion region1, IRegion region2);

        #endregion Coincident check

        #region Calculating shortest line between objects

        ILine ShortestLineBetween(Point2DD pt1, Point2DD pt2);
        ILine ShortestLineBetween(Point2DD pt, IInfiniteLine infLine);
        ILine ShortestLineBetween(Point2DD pt, IRay ray);
        ILine ShortestLineBetween(Point2DD pt, ILine line);
        ILine ShortestLineBetween(Point2DD pt, ICircle circle);
        ILine ShortestLineBetween(Point2DD pt, IArc arc);
        ILine ShortestLineBetween(Point2DD pt, IPathSegment segment);
        ILine ShortestLineBetween(Point2DD pt, IPath path);
        ILine ShortestLineBetween(Point2DD pt, IRegion region);

        ILine ShortestLineBetween(IInfiniteLine infLine, Point2DD pt);
        ILine ShortestLineBetween(IInfiniteLine infLine1, IInfiniteLine infLine2);
        ILine ShortestLineBetween(IInfiniteLine infLine, IRay ray);
        ILine ShortestLineBetween(IInfiniteLine infLine, ILine line);
        ILine ShortestLineBetween(IInfiniteLine infLine, ICircle circle);
        ILine ShortestLineBetween(IInfiniteLine infLine, IArc arc);
        ILine ShortestLineBetween(IInfiniteLine infLine, IPathSegment segment);
        ILine ShortestLineBetween(IInfiniteLine infLine, IPath path);
        ILine ShortestLineBetween(IInfiniteLine infLine, IRegion region);

        ILine ShortestLineBetween(IRay ray, Point2DD pt);
        ILine ShortestLineBetween(IRay ray, IInfiniteLine infLine);
        ILine ShortestLineBetween(IRay ray1, IRay ray2);
        ILine ShortestLineBetween(IRay ray, ILine line);
        ILine ShortestLineBetween(IRay ray, ICircle circle);
        ILine ShortestLineBetween(IRay ray, IArc arc);
        ILine ShortestLineBetween(IRay ray, IPathSegment segment);
        ILine ShortestLineBetween(IRay ray, IPath path);
        ILine ShortestLineBetween(IRay ray, IRegion region);

        ILine ShortestLineBetween(ILine line, Point2DD pt);
        ILine ShortestLineBetween(ILine line, IInfiniteLine infLine);
        ILine ShortestLineBetween(ILine line, IRay ray);
        ILine ShortestLineBetween(ILine line1, ILine line2);
        ILine ShortestLineBetween(ILine line, ICircle circle);
        ILine ShortestLineBetween(ILine line, IArc arc);

        ILine ShortestLineBetween(ICircle circle, Point2DD pt);
        ILine ShortestLineBetween(ICircle circle, IInfiniteLine infLine);
        ILine ShortestLineBetween(ICircle circle, IRay ray);
        ILine ShortestLineBetween(ICircle circle, ILine line);
        ILine ShortestLineBetween(ICircle circle1, ICircle circle2);
        ILine ShortestLineBetween(ICircle circle, IArc arc);

        ILine ShortestLineBetween(IArc arc, Point2DD pt);
        ILine ShortestLineBetween(IArc arc, IInfiniteLine infLine);
        ILine ShortestLineBetween(IArc arc, IRay ray);
        ILine ShortestLineBetween(IArc arc, ILine line);
        ILine ShortestLineBetween(IArc arc, ICircle circle);
        ILine ShortestLineBetween(IArc arc1, IArc arc2);

        ILine ShortestLineBetween(IPathSegment segment, Point2DD point);
        ILine ShortestLineBetween(IPathSegment segment, IInfiniteLine infLine);
        ILine ShortestLineBetween(IPathSegment segment, IRay ray);
        ILine ShortestLineBetween(IPathSegment segment1, IPathSegment segment2);

        ILine ShortestLineBetween(IPath path, Point2DD point);
        ILine ShortestLineBetween(IPath path, IInfiniteLine infLine);
        ILine ShortestLineBetween(IPath path, IRay ray);
        ILine ShortestLineBetween(IPath path, IPathSegment segment);
        ILine ShortestLineBetween(IPath path1, IPath path2);

        ILine ShortestLineBetween(IRegion region, Point2DD point);
        ILine ShortestLineBetween(IRegion region, IInfiniteLine infLine);
        ILine ShortestLineBetween(IRegion region, IRay ray);
        ILine ShortestLineBetween(IRegion region, IPathSegment segment);
        ILine ShortestLineBetween(IRegion region, IPath path);
        ILine ShortestLineBetween(IRegion region1, IRegion region2);

        #endregion Calculating shortest line between objects

        #region Calculating all intersections between objects

        IEnumerable<Point2DD> Intersections(Point2DD pt1, Point2DD pt2);
        IEnumerable<Point2DD> Intersections(Point2DD pt, IInfiniteLine infLine);
        IEnumerable<Point2DD> Intersections(Point2DD pt, IRay ray);
        IEnumerable<Point2DD> Intersections(Point2DD pt, ILine line);
        IEnumerable<Point2DD> Intersections(Point2DD pt, ICircle circle);
        IEnumerable<Point2DD> Intersections(Point2DD pt, IArc arc);
        IEnumerable<Point2DD> Intersections(Point2DD pt, IPathSegment segment);
        IEnumerable<Point2DD> Intersections(Point2DD pt, IPath path);
        IEnumerable<Point2DD> Intersections(Point2DD pt, IRegion region);

        IEnumerable<Point2DD> Intersections(IInfiniteLine infLine, Point2DD pt);
        IEnumerable<Point2DD> Intersections(IInfiniteLine infLine1, IInfiniteLine infLine2);
        IEnumerable<Point2DD> Intersections(IInfiniteLine infLine, IRay ray);
        IEnumerable<Point2DD> Intersections(IInfiniteLine infLine, ILine line);
        IEnumerable<Point2DD> Intersections(IInfiniteLine infLine, ICircle circle);
        IEnumerable<Point2DD> Intersections(IInfiniteLine infLine, IArc arc);
        IEnumerable<Point2DD> Intersections(IInfiniteLine infLine, IPathSegment segment);
        IEnumerable<Point2DD> Intersections(IInfiniteLine infLine, IPath path);
        IEnumerable<Point2DD> Intersections(IInfiniteLine infLine, IRegion region);

        IEnumerable<Point2DD> Intersections(IRay ray, Point2DD pt);
        IEnumerable<Point2DD> Intersections(IRay ray, IInfiniteLine infLine);
        IEnumerable<Point2DD> Intersections(IRay ray1, IRay ray2);
        IEnumerable<Point2DD> Intersections(IRay ray, ILine line);
        IEnumerable<Point2DD> Intersections(IRay ray, ICircle circle);
        IEnumerable<Point2DD> Intersections(IRay ray, IArc arc);
        IEnumerable<Point2DD> Intersections(IRay ray, IPathSegment segment);
        IEnumerable<Point2DD> Intersections(IRay ray, IPath path);
        IEnumerable<Point2DD> Intersections(IRay ray, IRegion region);

        IEnumerable<Point2DD> Intersections(ILine line, Point2DD pt);
        IEnumerable<Point2DD> Intersections(ILine line, IInfiniteLine infLine);
        IEnumerable<Point2DD> Intersections(ILine line, IRay ray);
        IEnumerable<Point2DD> Intersections(ILine line1, ILine line2);
        IEnumerable<Point2DD> Intersections(ILine line, ICircle circle);
        IEnumerable<Point2DD> Intersections(ILine line, IArc arc);

        IEnumerable<Point2DD> Intersections(ICircle circle, Point2DD pt);
        IEnumerable<Point2DD> Intersections(ICircle circle, IInfiniteLine infLine);
        IEnumerable<Point2DD> Intersections(ICircle circle, IRay ray);
        IEnumerable<Point2DD> Intersections(ICircle circle, ILine line);
        IEnumerable<Point2DD> Intersections(ICircle circle1, ICircle circle2);
        IEnumerable<Point2DD> Intersections(ICircle circle, IArc arc);

        IEnumerable<Point2DD> Intersections(IArc arc, Point2DD pt);
        IEnumerable<Point2DD> Intersections(IArc arc, IInfiniteLine infLine);
        IEnumerable<Point2DD> Intersections(IArc arc, IRay ray);
        IEnumerable<Point2DD> Intersections(IArc arc, ILine line);
        IEnumerable<Point2DD> Intersections(IArc arc, ICircle circle);
        IEnumerable<Point2DD> Intersections(IArc arc1, IArc arc2);

        IEnumerable<Point2DD> Intersections(IPathSegment segment, Point2DD point);
        IEnumerable<Point2DD> Intersections(IPathSegment segment, IInfiniteLine infLine);
        IEnumerable<Point2DD> Intersections(IPathSegment segment, IRay ray);
        IEnumerable<Point2DD> Intersections(IPathSegment segment1, IPathSegment segment2);

        IEnumerable<Point2DD> Intersections(IPath path, Point2DD point);
        IEnumerable<Point2DD> Intersections(IPath path, IInfiniteLine infLine);
        IEnumerable<Point2DD> Intersections(IPath path, IRay ray);
        IEnumerable<Point2DD> Intersections(IPath path, IPathSegment segment);
        IEnumerable<Point2DD> Intersections(IPath path1, IPath path2);

        IEnumerable<Point2DD> Intersections(IRegion region, Point2DD point);
        IEnumerable<Point2DD> Intersections(IRegion region, IInfiniteLine infLine);
        IEnumerable<Point2DD> Intersections(IRegion region, IRay ray);
        IEnumerable<Point2DD> Intersections(IRegion region, IPathSegment segment);
        IEnumerable<Point2DD> Intersections(IRegion region, IPath path);
        IEnumerable<Point2DD> Intersections(IRegion region1, IRegion region2);

        #endregion Calculating all intersections between objects

        #region Calculating first intersection point between objects

        Point2DD? FirstIntersection(Point2DD pt1, Point2DD pt2);
        Point2DD? FirstIntersection(Point2DD pt, IInfiniteLine infLine);
        Point2DD? FirstIntersection(Point2DD pt, IRay ray);
        Point2DD? FirstIntersection(Point2DD pt, ILine line);
        Point2DD? FirstIntersection(Point2DD pt, ICircle circle);
        Point2DD? FirstIntersection(Point2DD pt, IArc arc);
        Point2DD? FirstIntersection(Point2DD pt, IPathSegment segment);
        Point2DD? FirstIntersection(Point2DD pt, IPath path);
        Point2DD? FirstIntersection(Point2DD pt, IRegion region);

        Point2DD? FirstIntersection(IInfiniteLine infLine, Point2DD pt);
        Point2DD? FirstIntersection(IInfiniteLine infLine1, IInfiniteLine infLine2);
        Point2DD? FirstIntersection(IInfiniteLine infLine, IRay ray);
        Point2DD? FirstIntersection(IInfiniteLine infLine, ILine line);
        Point2DD? FirstIntersection(IInfiniteLine infLine, ICircle circle);
        Point2DD? FirstIntersection(IInfiniteLine infLine, IArc arc);
        Point2DD? FirstIntersection(IInfiniteLine infLine, IPathSegment segment);
        Point2DD? FirstIntersection(IInfiniteLine infLine, IPath path);
        Point2DD? FirstIntersection(IInfiniteLine infLine, IRegion region);

        Point2DD? FirstIntersection(IRay ray, Point2DD pt);
        Point2DD? FirstIntersection(IRay ray, IInfiniteLine infLine2);
        Point2DD? FirstIntersection(IRay ray1, IRay ray2);
        Point2DD? FirstIntersection(IRay ray, ILine line);
        Point2DD? FirstIntersection(IRay ray, ICircle circle);
        Point2DD? FirstIntersection(IRay ray, IArc arc);
        Point2DD? FirstIntersection(IRay ray, IPathSegment segment);
        Point2DD? FirstIntersection(IRay ray, IPath path);
        Point2DD? FirstIntersection(IRay ray, IRegion region);

        Point2DD? FirstIntersection(ILine line, Point2DD pt);
        Point2DD? FirstIntersection(ILine line, IInfiniteLine infLine);
        Point2DD? FirstIntersection(ILine line, IRay ray);
        Point2DD? FirstIntersection(ILine line1, ILine line2);
        Point2DD? FirstIntersection(ILine line, ICircle circle);
        Point2DD? FirstIntersection(ILine line, IArc arc);

        Point2DD? FirstIntersection(ICircle circle, Point2DD pt);
        Point2DD? FirstIntersection(ICircle circle, IInfiniteLine infLine);
        Point2DD? FirstIntersection(ICircle circle, IRay ray);
        Point2DD? FirstIntersection(ICircle circle, ILine line);
        Point2DD? FirstIntersection(ICircle circle1, ICircle circle2);
        Point2DD? FirstIntersection(ICircle circle, IArc arc);

        Point2DD? FirstIntersection(IArc arc, Point2DD pt);
        Point2DD? FirstIntersection(IArc arc, IInfiniteLine infLine);
        Point2DD? FirstIntersection(IArc arc, IRay ray);
        Point2DD? FirstIntersection(IArc arc, ILine line);
        Point2DD? FirstIntersection(IArc arc, ICircle circle);
        Point2DD? FirstIntersection(IArc arc1, IArc arc2);

        Point2DD? FirstIntersection(IPathSegment segment, Point2DD point);
        Point2DD? FirstIntersection(IPathSegment segment, IInfiniteLine infLine);
        Point2DD? FirstIntersection(IPathSegment segment, IRay ray);
        Point2DD? FirstIntersection(IPathSegment segment1, IPathSegment segment2);

        Point2DD? FirstIntersection(IPath path, Point2DD point);
        Point2DD? FirstIntersection(IPath path, IInfiniteLine infLine);
        Point2DD? FirstIntersection(IPath path, IRay ray);
        Point2DD? FirstIntersection(IPath path, IPathSegment segment);
        Point2DD? FirstIntersection(IPath path1, IPath path2);

        Point2DD? FirstIntersection(IRegion region, Point2DD point);
        Point2DD? FirstIntersection(IRegion region, IInfiniteLine infLine);
        Point2DD? FirstIntersection(IRegion region, IRay ray);
        Point2DD? FirstIntersection(IRegion region, IPathSegment segment);
        Point2DD? FirstIntersection(IRegion region, IPath path);
        Point2DD? FirstIntersection(IRegion region1, IRegion region2);

        #endregion Calculating first intersection point between objects

        #region Paths and regions

        IPathBuilder CreatePathBuilder();
        IRegionBuilder CreateRegionBuilder();
        IRegionComposer CreateRegionComposer();
        IPath Reduce(IPath path);
        IRegion Reduce(IRegion region);
        IEnumerable<IPath> FindCommonPaths(IPath path1, IPath path2);
        IEnumerable<IPath> FindCommonPaths(IRegion region1, IRegion region2);
        IRectangle FindMinimalRectangleAroundPath(IPath path, Vector2DD? direction = null);
        IPolygon FindMinimalPoly4AroundPath(IPath path);
        IPath Offset(IPath path, double offset, OffsetMode mode);
        double ComputeMaxDiff(IPath path1, IPath path2);
        IPath SnapPathToRefPath(IPath pathToSnap, IPath refPath, double distanceThreshold);

        #endregion Paths and regions
    }
}
