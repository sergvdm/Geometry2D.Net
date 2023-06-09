﻿using Svd.Geometry2D.Shapes;
using System.Collections.Generic;

namespace Svd.Geometry2D
{
    public interface IRegion : ILimitedShape
    {
        IReadOnlyList<IPath> Paths { get; }
        WindingInfo CalcWindingInfo(Point2DD point);
        bool IsInnerPoint(Point2DD point, bool includeCoincident);
        IRegion Invert();
        new IRegion Clone();
    }
}
