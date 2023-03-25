using System.Collections.Generic;

namespace Svd.Geometry2D
{
    public interface IRegionComposer
    {
        void Reset();
        void Add(IRegion region);
        void Add(params IRegion[] regions);
        void Add(IEnumerable<IRegion> regions);
        IRegion Compose(RegionCompositionMode mode);
    }
}
