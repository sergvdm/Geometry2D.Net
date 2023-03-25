using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace Svd.Geometry2D.Serialization
{
    public enum GeometryDataType : byte
    {
        None = 0,
        Region = 1,
        Path = 2,
        Line = 3,
        Arc = 4
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GeometryDataHeader
    {
        public static readonly int PathIsClosedBit = BitVector32.CreateMask();
        public static readonly int ArcSweepDirBit = BitVector32.CreateMask(PathIsClosedBit);

        public long Size; // 8 bytes
        public GeometryDataType Type; // 1 byte
        private byte reserved1;
        private byte reserved2;
        private byte reserved3;
        public BitVector32 DataBits; // 4 bytes

        public bool ArcSweepDir
        {
            get => DataBits[ArcSweepDirBit];
            set => DataBits[ArcSweepDirBit] = value;
        }

        public bool PathIsClosed
        {
            get => DataBits[PathIsClosedBit];
            set => DataBits[PathIsClosedBit] = value;
        }
    } // 16 bytes

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RegionData
    {
        public GeometryDataHeader Header; // 16 bytes

        public void Init()
        {
            Header.Type = GeometryDataType.Region;
            Header.Size = Marshal.SizeOf<RegionData>();
        }
    } // 16 bytes

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PathData
    {
        public GeometryDataHeader Header; // 16 bytes
        public Point2DD StartPoint; // 16 bytes

        public void Init()
        {
            Header.Type = GeometryDataType.Path;
            Header.Size = Marshal.SizeOf<PathData>();
        }

        public void Init(Point2DD startPoint)
        {
            Init();
            StartPoint = startPoint;
        }
    } // 32 bytes

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LineSegmentData
    {
        public GeometryDataHeader Header; // 16 bytes
        public Point2DD Vertex; // 16 bytes

        public void Init()
        {
            Header.Type = GeometryDataType.Line;
            Header.Size = Marshal.SizeOf<LineSegmentData>();
        }

        public void Init(Point2DD vertex)
        {
            Init();
            Vertex = vertex;
        }
    } // 32 bytes

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ArcSegmentData
    {
        public GeometryDataHeader Header; // 16 bytes
        public Point2DD Vertex; // 16 bytes
        public Point2DD Center; // 16 bytes
        public double Radius; // 8 bytes
        public void Init()
        {
            Header.Type = GeometryDataType.Arc;
            Header.Size = Marshal.SizeOf<ArcSegmentData>();
        }

        public void Init(Point2DD vertex, Point2DD center, double radius, bool sweepDir)
        {
            Init();
            Vertex = vertex;
            Center = center;
            Radius = radius;
            Header.ArcSweepDir = sweepDir;
        }
    } // 56 bytes
}
