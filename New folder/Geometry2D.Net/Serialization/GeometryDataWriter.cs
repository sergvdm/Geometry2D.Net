using Altium.Geometry2D.Shapes;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Altium.Geometry2D.Serialization
{
    internal class GeometryDataWriter : IGeometryDataWriter
    {
        private readonly MemoryStream memoryStream;

        public GeometryDataWriter(MemoryStream memoryStream)
        {
            this.memoryStream = memoryStream;
        }

        public void WriteRegion(IRegion region)
        {
            if (region.Paths.Count == 0) return;
            BeginRegion();
            foreach (var path in region.Paths)
                WritePath(path);
            EndRegion();
        }

        public void WritePath(IPath path)
        {
            if (path.Segments.Count == 0) return;
            var firstSegment = path.Segments[0];
            BeginPath(firstSegment.StartPoint, path.IsClosed);
            for (int i = 0; i < path.Segments.Count; i++)
                WritePathSegment(path.Segments[i]);
            EndPath();
        }

        private void WritePathSegment(IPathSegment segment)
        {
            switch (segment)
            {
                case ILine line:
                    AddLineSegment(line.EndPoint);
                    break;
                case IArc arc:
                    AddArcSegment(arc.EndPoint, arc.Midpoint());
                    break;
                default:
                    throw new ArgumentException($"Unsupported path segment type: {segment.GetType()}", nameof(segment));
            }
        }

        private void BeginRegion()
        {
            EndRegion();
            regionData = new RegionData();
            regionData.Init();
            regionOffset = Append(regionData);
        }

        private void BeginPath(Point2DD startPoint, bool closed)
        {
            EndPath();
            if (!closed && regionOffset.HasValue)
                throw new InvalidOperationException("Region paths must be closed");
            pathData = new PathData();
            pathData.Init(startPoint);
            pathOffset = Append(pathData);
        }

        private void AddLineSegment(Point2DD vertex)
        {
            var line = new LineSegmentData();
            line.Init(vertex);
            Append(line);
            lastPathVertex = line.Vertex;
        }

        private void AddArcSegment(Point2DD vertex, Point2DD pointOnArc)
        {
            var arc = new LineSegmentData();
            arc.Init(vertex);
            Append(arc);
            lastPathVertex = arc.Vertex;
        }

        private void EndPath()
        {
            if (pathOffset.HasValue)
            {
                // auto close path if need
                if (pathData.Header.PathIsClosed && pathData.StartPoint != lastPathVertex)
                    AddLineSegment(pathData.StartPoint);

                pathData.Header.Size = memoryStream.Length - pathOffset.Value;
                Write(pathData, pathOffset.Value);
                pathOffset = null;
            }
        }

        private void EndRegion()
        {
            EndPath();
            if (regionOffset.HasValue)
            {
                regionData.Header.Size = memoryStream.Length - regionOffset.Value;
                Write(regionData, regionOffset.Value);
                regionOffset = null;
            }
        }

        private long Append<T>(T data)
            where T : struct
        {
            var result = memoryStream.Length;
            Write(data, result);
            return result;
        }

        private void Write<T>(T data, long offset)
            where T : struct
        {
            var dataSize = Marshal.SizeOf<T>();
            if (memoryStream.Length >= (offset + dataSize))
            {
                var bufferHandle = GCHandle.Alloc(memoryStream.GetBuffer(), GCHandleType.Pinned);
                try
                {
                    var bufferPtr = bufferHandle.AddrOfPinnedObject();
                    Marshal.StructureToPtr(data, new IntPtr(bufferPtr.ToInt64() + offset), false);
                }
                finally
                {
                    bufferHandle.Free();
                }
            }
            else
            {
                var buffer = new byte[dataSize];
                var bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    var bufferPtr = bufferHandle.AddrOfPinnedObject();
                    Marshal.StructureToPtr(data, bufferPtr, false);
                }
                finally
                {
                    bufferHandle.Free();
                }

                var positionSave = memoryStream.Position;
                try
                {
                    memoryStream.Position = offset;
                    memoryStream.Write(buffer, 0, buffer.Length);
                }
                finally
                {
                    memoryStream.Position = positionSave;
                }
            }
        }

        private long? regionOffset;
        private long? pathOffset;
        private PathData pathData;
        private RegionData regionData;
        private Point2DD lastPathVertex;
    }
}
