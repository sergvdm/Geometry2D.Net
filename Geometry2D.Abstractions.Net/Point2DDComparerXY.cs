using System;
using System.Collections.Generic;

namespace Svd.Geometry2D
{
    public class Point2DDComparerXY : IComparer<Point2DD>
    {
        public static readonly Point2DDComparerXY Default = new Point2DDComparerXY();

        public int Compare(Point2DD pt1, Point2DD pt2)
        {
            var result = Math.Sign(pt1.X - pt2.X);
            if (result == 0)
                result = Math.Sign(pt1.Y - pt2.Y);
            return result;
        }
    }
}
