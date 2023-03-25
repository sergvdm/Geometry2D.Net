namespace Svd.Geometry2D.Shapes
{
    public interface ILine : IPathSegment, ILinearShape
    {
        IRay AsRay(bool reverse = false);
        IInfiniteLine AsInfinite();
        new ILine Clone();
        new ILine Reverse();
    }
}
