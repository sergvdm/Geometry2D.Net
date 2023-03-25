namespace Svd.Geometry2D.Shapes
{
    public interface IClosedShape : ILimitedShape
    {
        double Perimeter();
        double Area();
    }
}
