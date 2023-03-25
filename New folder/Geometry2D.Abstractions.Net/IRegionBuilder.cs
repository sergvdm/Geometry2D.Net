using System.Collections.Generic;

namespace Altium.Geometry2D
{
    public interface IRegionBuilder
    {
        void BeginRegion();
        void Add(IPath path);
        void Add(params IPath[] paths);
        void Add(IEnumerable<IPath> paths);
        IRegion EndRegion();
    }
}
