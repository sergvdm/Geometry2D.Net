﻿namespace Svd.Geometry2D.Shapes
{
    public interface IRay : IShape, ILinearShape
    {
        new IRay Clone();
    }
}
