using System;
using System.Collections.Generic;

namespace Altium.Geometry2D
{
    public class Point2DDComparerYX : IComparer<Point2DD>
    {
        public static readonly Point2DDComparerYX Default = new Point2DDComparerYX();

        public int Compare(Point2DD pt1, Point2DD pt2)
        {
            var result = Math.Sign(pt1.Y - pt2.Y);
            if (result == 0)
                result = Math.Sign(pt1.X - pt2.X);
            return result;
        }
    }
}
