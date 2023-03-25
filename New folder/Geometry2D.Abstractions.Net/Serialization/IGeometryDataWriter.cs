namespace Altium.Geometry2D.Serialization
{
    public interface IGeometryDataWriter
    {
        void WriteRegion(IRegion region);
        void WritePath(IPath path);
    }
}
