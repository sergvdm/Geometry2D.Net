using Svd.Geometry2D.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Svd.Geometry2D.Serialization
{
    internal class GeometryDataReader : IGeometryDataReader
    {
        private readonly IGeometry2DEngine geometry2DEngine;

        public GeometryDataReader(IGeometry2DEngine geometry2DEngine, byte[] geometryData)
        {
            this.geometry2DEngine = geometry2DEngine;
            buffer = geometryData;
            Reset();
        }

        public GeometryDataReader(MemoryStream memoryStream)
        {
            buffer = memoryStream.GetBuffer();
            Reset();
        }

        public GeometryDataHeader CurrentHeader { get; private set; }
        public bool HasContent => CurrentHeader.Type != GeometryDataType.None;

        public IShape Read()
        {
            switch (CurrentHeader.Type)
            {
                case GeometryDataType.None:
                    Skip();
                    return null;
                case GeometryDataType.Region:
                    return ReadRegion();
                case GeometryDataType.Path:
                    return ReadPath();
                case GeometryDataType.Line:
                    return ReadLine(lastVertex);
                case GeometryDataType.Arc:
                    return ReadArc(lastVertex);
                default:
                    throw new Exception($"Unknown geometry type: {CurrentHeader.Type}");
            }
        }

        public void Skip()
        {
            offset += CurrentHeader.Size;
            PeekCurrent();
        }

        public void Reset()
        {
            offset = 0;
            regionOffset = -1;
            regionEnd = -1;
            pathOffset = -1;
            pathEnd = -1;
            PeekCurrent();
        }

        public void MoveNext()
        {
            switch (CurrentHeader.Type)
            {
                case GeometryDataType.None:
                    break;
                case GeometryDataType.Region:
                    offset += Marshal.SizeOf<RegionData>();
                    break;
                case GeometryDataType.Path:
                    offset += Marshal.SizeOf<PathData>();
                    break;
                case GeometryDataType.Line:
                    offset += Marshal.SizeOf<LineSegmentData>();
                    break;
                case GeometryDataType.Arc:
                    offset += Marshal.SizeOf<ArcSegmentData>();
                    break;
                default:
                    throw new Exception($"Unknown geometry type: {CurrentHeader.Type}");
            }

            PeekCurrent();
        }

        public bool ResetRegion()
        {
            if (regionOffset < 0) return false;
            offset = regionOffset;
            PeekCurrent();
            return true;
        }

        public bool SkipRegion()
        {
            if (regionEnd < 0) return false;
            offset = regionEnd;
            PeekCurrent();
            return true;
        }

        public bool ResetPath()
        {
            if (pathOffset < 0) return false;
            offset = pathOffset;
            PeekCurrent();
            return true;
        }

        public bool SkipPath()
        {
            if (pathEnd < 0) return false;
            offset = pathEnd;
            PeekCurrent();
            return true;
        }

        private IRegion ReadRegion()
        {
            var regionData = GetCurrent<RegionData>();
            var regionDataEnd = offset + regionData.Header.Size;
            var paths = new LinkedList<IPath>();
            while (offset < regionDataEnd)
            {
                switch (CurrentHeader.Type)
                {
                    case GeometryDataType.Path:
                        var path = ReadPath();
                        if (path != null) paths.AddLast(path);
                        break;
                    default:
                        MoveNext();
                        break;
                }
            }

            return new Region(geometry2DEngine, paths);
        }

        private IPath ReadPath()
        {
            var pathData = GetCurrent<PathData>();
            var pathDataEnd = offset + pathData.Header.Size;
            var segments = new LinkedList<IPathSegment>();
            var prevVertex = pathData.StartPoint;
            while (offset < pathDataEnd)
            {
                switch (CurrentHeader.Type)
                {
                    case GeometryDataType.Line:
                        var line = ReadLine(prevVertex);
                        if (line != null) segments.AddLast(line);
                        break;
                    case GeometryDataType.Arc:
                        var arc = ReadArc(prevVertex);
                        if (arc != null) segments.AddLast(arc);
                        break;
                    default:
                        MoveNext();
                        break;
                }
            }

            return new Path(geometry2DEngine, segments);
        }

        private ILine ReadLine(Point2DD prevVertex)
        {
            var lineData = GetCurrent<LineSegmentData>();
            MoveNext();
            return new Line(prevVertex, lineData.Vertex);
        }

        private IArc ReadArc(Point2DD prevVertex)
        {
            var arcData = GetCurrent<ArcSegmentData>();
            MoveNext();
            return new Arc(prevVertex, arcData.Vertex, arcData.Radius, arcData.Center);
        }

        private T GetCurrent<T>()
            where T : struct
        {
            if ((buffer.Length - offset) < Marshal.SizeOf<T>()) return default(T);
            var bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var bufferPtr = bufferHandle.AddrOfPinnedObject();
                return Marshal.PtrToStructure<T>(new IntPtr(bufferPtr.ToInt64() + offset));
            }
            finally
            {
                bufferHandle.Free();
            }
        }

        private void PeekCurrent()
        {
            if (regionEnd > 0 && regionEnd <= offset)
            {
                regionOffset = -1;
                regionEnd = -1;
            }

            if (pathEnd > 0 && pathEnd <= offset)
            {
                pathOffset = -1;
                pathEnd = -1;
                lastVertex = default(Point2DD);
            }

            CurrentHeader = GetCurrent<GeometryDataHeader>();
            switch (CurrentHeader.Type)
            {
                case GeometryDataType.Region:
                    regionOffset = offset;
                    regionEnd = offset + CurrentHeader.Size;
                    pathOffset = -1;
                    break;
                case GeometryDataType.Path:
                    pathOffset = offset;
                    pathEnd = offset + CurrentHeader.Size;
                    lastVertex = GetCurrent<PathData>().StartPoint;
                    break;
                case GeometryDataType.Line:
                    lastVertex = GetCurrent<LineSegmentData>().Vertex;
                    break;
                case GeometryDataType.Arc:
                    lastVertex = GetCurrent<ArcSegmentData>().Vertex;
                    break;
            }
        }

        private readonly byte[] buffer;
        private long offset;
        private long regionOffset;
        private long regionEnd;
        private long pathOffset;
        private long pathEnd;
        private Point2DD lastVertex;
    }
}
