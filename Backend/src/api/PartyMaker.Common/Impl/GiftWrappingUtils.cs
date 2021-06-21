using System;
using System.Collections.Generic;
using System.Text;

namespace PartyMaker.Common.Impl
{

    public class GiftWrappingUtils
    {

        private Stack<Point2D> _hull = new Stack<Point2D>();

        public GiftWrappingUtils(Point2D[] pts)
        {
            // defensive copy
            int N = pts.Length;
            Point2D[] points = new Point2D[N];
            for (int i = 0; i < N; i++)
                points[i] = pts[i];
            Array.Sort(points);

            Array.Sort(points, 1, N, new PolarOrder(points[0]));

            _hull.Push(points[0]); // p[0] is first extreme point
            int k1;
            for (k1 = 1; k1 < N; k1++)
                if (!points[0].Equals(points[k1]))
                    break;
            if (k1 == N)
                return; // all points equal

            int k2;
            for (k2 = k1 + 1; k2 < N; k2++)
                if (points[0].Ccw(points[k1], points[k2]) != 0)
                    break;
            _hull.Push(points[k2 - 1]); // points[k2-1] is second extreme point

            for (int i = k2; i < N; i++)
            {
                Point2D top = _hull.Pop();
                while (_hull.Peek().Ccw(top, points[i]) <= 0)
                {
                    top = _hull.Pop();
                }
                _hull.Push(top);
                _hull.Push(points[i]);
            }

            isConvex();
        }

        public IEnumerable<Point2D> Hull()
        {
            Stack<Point2D> s = new Stack<Point2D>();
            foreach (Point2D p in _hull)
                s.Push(p);
            return s;
        }

        private Boolean isConvex()
        {
            int N = _hull.Count;
            if (N <= 2)
                return true;

            Point2D[] points = new Point2D[N];
            int n = 0;
            foreach(Point2D p in _hull)
            {
                points[n++] = p;
            }

            for (int i = 0; i < N; i++)
            {
                if (points[i].Ccw(points[(i + 1) % N], points[(i + 2) % N]) <= 0)
                {
                    throw new Exception();
                }
            }
            return true;
        }
    }
}
