using System;

namespace Svd.Geometry2D.Shapes
{
    public interface IShape : ICloneable
    {
        bool Equal(IShape obj);
    }
}
