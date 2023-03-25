using Svd.Geometry2D.Shapes;
using System.Collections.Generic;

namespace Svd.Geometry2D
{
    internal class PathBuilder : IPathBuilder
    {
        private readonly IGeometry2DEngine geometry2DEngine;

        public PathBuilder(IGeometry2DEngine geometry2DEngine)
        {
            this.geometry2DEngine = geometry2DEngine;
        }

        private LinkedList<IPathSegment> originalSegmentList;
        private bool reverse = false;
        private bool allowReduce = true;

        public void BeginPath(bool reverse = false, bool allowReduce = true)
        {
            originalSegmentList = new LinkedList<IPathSegment>();
            this.allowReduce = allowReduce;
            this.reverse = reverse;
        }

        public void Add(Point2DD lineVertex)
        {
            if (originalSegmentList.Count > 0)
                Add(new Line(originalSegmentList.Last.Value.EndPoint, lineVertex));
            else
                Add(new Line(lineVertex, lineVertex));
        }

        public void Add(params Point2DD[] lineVertices) => Add((IEnumerable<Point2DD>)lineVertices);
        public void Add(IEnumerable<Point2DD> lineVertices)
        {
            foreach (var lineVertex in lineVertices)
                Add(lineVertex);
        }

        public void Add(IPathSegment segment)
        {
            if (reverse)
                AddReverse(originalSegmentList, segment, !allowReduce);
            else
                AddForward(originalSegmentList, segment, !allowReduce);
        }

        public void Add(params IPathSegment[] segments) => Add((IEnumerable<IPathSegment>)segments);
        public void Add(IEnumerable<IPathSegment> segments)
        {
            foreach (var segment in segments)
                Add(segment);
        }

        public bool IsClosed => originalSegmentList.Count > 0 && originalSegmentList.First.Value.StartPoint == originalSegmentList.Last.Value.EndPoint;

        public void ClosePath()
        {
            if (originalSegmentList.Count == 0) return;
            var startPoint = originalSegmentList.First.Value.StartPoint;
            var endPoint = originalSegmentList.Last.Value.EndPoint;
            if (startPoint != endPoint)
                AddForward(originalSegmentList, new Line(endPoint, startPoint), !allowReduce);
        }

        public IPath EndPath(bool close)
        {
            if (close && !IsClosed) ClosePath();
            if (!allowReduce)
                return new Path(geometry2DEngine, originalSegmentList);

            Reduce(originalSegmentList);
            var resultSegmentList = new LinkedList<IPathSegment>();
            foreach (var originalSegment in originalSegmentList)
                AddForward(resultSegmentList, originalSegment, true);
            if ((resultSegmentList.Count == 1 && resultSegmentList.First.Value.IsSinglePoint()) ||
                (resultSegmentList.Count == 2 && geometry2DEngine.AreCoincident(resultSegmentList.First.Value, resultSegmentList.Last.Value)))
                resultSegmentList.Clear();
            return new Path(geometry2DEngine, resultSegmentList);
        }

        private void AddForward(LinkedList<IPathSegment> segmentList, IPathSegment segment, bool snapToGrid)
        {
            segment = snapToGrid ? segment.SnapToGrid(geometry2DEngine) : segment;
            if (segmentList.Count > 0)
            {
                var last = segmentList.Last.Value;
                if (last.IsSinglePoint())
                    segmentList.RemoveLast();
                if (last.EndPoint != segment.StartPoint)
                    AddForward(segmentList, new Line(last.EndPoint, segment.StartPoint), false);
            }

            var isSinglePoint = segment.IsSinglePoint();
            if (isSinglePoint && !(segment is Line))
                segment = new Line(segment.StartPoint, segment.EndPoint);

            if (segmentList.Count > 0 && segmentList.Last.Value.StartPoint == segment.EndPoint && geometry2DEngine.AreCoincident(segmentList.Last.Value, segment))
            {
                // remove back step
                segmentList.RemoveLast();
            }
            else
            {
                if (!isSinglePoint || segmentList.Count == 0)
                    segmentList.AddLast(segment);
            }
        }

        private void AddReverse(LinkedList<IPathSegment> segmentList, IPathSegment segment, bool snapToGrid)
        {
            segment = (snapToGrid ? segment.SnapToGrid(geometry2DEngine) : segment).Reverse();
            if (segmentList.Count > 0)
            {
                var first = segmentList.First.Value;
                if (first.IsSinglePoint())
                    segmentList.RemoveFirst();
                if (first.StartPoint != segment.EndPoint)
                    AddReverse(segmentList, new Line(first.StartPoint, segment.EndPoint), false);
            }

            var isSinglePoint = segment.IsSinglePoint();
            if (isSinglePoint && !(segment is Line))
                segment = new Line(segment.StartPoint, segment.EndPoint);

            if (segmentList.Count > 0 && segmentList.First.Value.EndPoint == segment.StartPoint && geometry2DEngine.AreCoincident(segmentList.First.Value, segment))
            {
                // remove back step
                segmentList.RemoveFirst();
            }
            else
            {
                if (!isSinglePoint || segmentList.Count == 0)
                    segmentList.AddFirst(segment);
            }
        }

        private void Reduce(LinkedList<IPathSegment> segmentList)
        {
            LinkedListNode<IPathSegment> prev = null;
            var current = segmentList.First;
            var next = current?.Next;
            while (next != null)
            {
                var joined = current.Value.TryJoin(geometry2DEngine, next.Value, PathSegmentJoinMode.Start);
                if (joined != null)
                {
                    segmentList.Remove(current);
                    segmentList.Remove(next);
                    if (prev == null)
                        current = segmentList.AddFirst(joined);
                    else
                        current = segmentList.AddAfter(prev, joined);
                    next = current.Next;
                }
                else
                {
                    prev = current;
                    current = current.Next;
                    next = current?.Next;
                }
            }

            if (segmentList.First != null && segmentList.Last.Value != segmentList.First.Value)
            {
                var joined = segmentList.Last.Value.TryJoin(geometry2DEngine, segmentList.First.Value, PathSegmentJoinMode.Start);
                if (joined != null)
                {
                    segmentList.RemoveFirst();
                    segmentList.RemoveLast();
                    segmentList.AddLast(joined);
                }
            }
        }
    }
}
