namespace Svd.Geometry2D.Shapes
{
    internal abstract class LimitedShape : Shape, ILimitedShape
    {
        protected LimitedShape()
        {
        }

        protected LimitedShape(LimitedShape other)
            : base(other)
        {
            boundingRect = other.boundingRect;
        }

        private AARect2DD? boundingRect;
        public AARect2DD BoundingRect
        {
            get
            {
                if (!boundingRect.HasValue)
                    boundingRect = CalcBoundingRect();
                return boundingRect.Value;
            }
        }

        protected void InvalidateBoundingRect()
        {
            boundingRect = null;
        }

        public virtual IRegion CreateRegion(IGeometry2DEngine geometry2DEngine, bool invert = false)
        {
            var regionBuilder = geometry2DEngine.CreateRegionBuilder();
            var pathBuilder = geometry2DEngine.CreatePathBuilder();
            regionBuilder.BeginRegion();
            BuildRegion(geometry2DEngine, regionBuilder, pathBuilder, invert);
            return regionBuilder.EndRegion();
        }

        protected abstract AARect2DD CalcBoundingRect();

        protected virtual void BuildRegion(IGeometry2DEngine geometry2DEngine, IRegionBuilder regionBuilder, IPathBuilder pathBuilder, bool invert)
        {
        }
    }
}