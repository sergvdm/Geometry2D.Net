using System.Collections.Generic;

namespace Altium.Geometry2D
{
    internal class RegionBuilder : IRegionBuilder
    {
        private readonly IGeometry2DEngine geometry2DEngine;

        public RegionBuilder(IGeometry2DEngine geometry2DEngine)
        {
            this.geometry2DEngine = geometry2DEngine;
        }

        private LinkedList<IPath> pathList;

        public void BeginRegion()
        {
            pathList = new LinkedList<IPath>();
        }

        public void Add(IPath path)
        {
            pathList.AddLast(path);
        }

        public void Add(params IPath[] paths) => Add((IEnumerable<IPath>)paths);
        public void Add(IEnumerable<IPath> paths)
        {
            foreach (var path in paths)
                Add(path);
        }

        public IRegion EndRegion()
        {
            var region = new Region(geometry2DEngine, pathList);
            pathList = null;
            return region;
        }
    }
}
