using System.Collections.Generic;

namespace PartyMaker.Common.Impl
{
    public class Point2D
    {

        public double X { get; set; } // x coordinate
        public double Y { get; set; } // y coordinate

        public bool Equals(object other)
        {
            if (other == this)
                return true;
            if (other == null)
                return false;
            if (other.GetType() != this.GetType())
                return false;
            Point2D that = (Point2D)other;
            return this.X == that.X && this.Y == that.Y;
        }

        public int Ccw(Point2D b, Point2D c)
        {
            double area2 = (b.X - X) * (c.Y - Y) - (b.Y - Y) * (c.X - X);
            if (area2 < 0)
                return -1;
            else if (area2 > 0)
                return +1;
            else
                return 0;
        }

    }

    public class PolarOrder: IComparer<Point2D>
    {
        public Point2D Center { get; set; }
        public PolarOrder(Point2D center)
        {
            Center = center;
        }

        public int Compare(Point2D q1, Point2D q2)
        {
            double dx1 = q1.X - Center.X;
            double dy1 = q1.Y - Center.Y;
            double dx2 = q2.X - Center.X;
            double dy2 = q2.Y - Center.Y;

            if (dy1 >= 0 && dy2 < 0)
                return -1; // q1 above; q2 below
            else if (dy2 >= 0 && dy1 < 0)
                return +1; // q1 below; q2 above
            else if (dy1 == 0 && dy2 == 0)
            { // 3-collinear and horizontal
                if (dx1 >= 0 && dx2 < 0)
                    return -1;
                else if (dx2 >= 0 && dx1 < 0)
                    return +1;
                else
                    return 0;
            }
            else
                return -Center.Ccw(q1, q2); // both above or below
        }
    }
}
