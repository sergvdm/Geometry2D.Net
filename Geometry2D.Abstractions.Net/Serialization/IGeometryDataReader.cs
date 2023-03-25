using Svd.Geometry2D.Shapes;

namespace Svd.Geometry2D.Serialization
{
    public interface IGeometryDataReader
    {
        GeometryDataHeader CurrentHeader { get; }
        bool HasContent { get; }
        IShape Read();
        void Skip();
        void Reset();
        void MoveNext();
        bool ResetRegion();
        bool SkipRegion();
        bool ResetPath();
        bool SkipPath();
    }
}
