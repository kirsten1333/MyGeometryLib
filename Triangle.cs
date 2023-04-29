using MyGeometryLib.Geometry;

namespace MyGeometryLib
{
    internal class Triangle : IFigure
    {
        public Point PointA { get; private set; }
        public Point PointB { get; private set; }
        public Point PointC { get; private set; }

        public double EdgeA { get; private set; }
        public double EdgeB { get; private set; }
        public double EdgeC { get; private set; }

        public Angle AngleA { get; private set; } = new(0);
        public Angle AngleB { get; private set; } = new(0);
        public Angle AngleC { get; private set; } = new(0);

        public bool IsAutoPoints { get; private set; }
        public bool IsDegenerate { get; private set; } = false;
        public bool IsFlat { get; private set; } = false;
        public bool IsRight { get; private set; } = false;

        private void CreateFromEdges(double edgeA, double edgeB, double edgeC)
        {
            EdgeA = edgeA;
            EdgeB = edgeB;
            EdgeC = edgeC;

            AngleA = new(edgeB, edgeC, edgeA);
            AngleB = new(edgeA, edgeC, edgeB);
            AngleC = new(edgeA, edgeB, edgeC);

            if (double.IsNaN(AngleA.Radian) || double.IsNaN(AngleB.Radian) || double.IsNaN(AngleC.Radian))
            {
                IsDegenerate = true;
                return;
            }
            else if (AngleA == 0 || AngleB == 0 || AngleC == 0)
            {
                IsFlat = true;
            }

            IsRight = AngleA.IsRight || AngleB.IsRight || AngleC.IsRight;
            if (PointA == Point.Zero && PointB == Point.Zero && PointC == Point.Zero)
            {
                PointA = Point.Zero;
                PointB = new(edgeC, 0);
                PointC = new Circle(PointA, EdgeB).Intersect(new(PointB, EdgeA)).FirstOrDefault();
            }
        }

        private Angle[] GetAngles()
        {
            Angle[] angles = new Angle[3];
            angles[0] = AngleA;
            angles[1] = AngleB;
            angles[2] = AngleC;
            return angles;
        }

        private double[] GetEdges()
        {
            double[] edges = new double[3];
            edges[0] = EdgeA;
            edges[1] = EdgeB;
            edges[2] = EdgeC;
            return edges;
        }

        private Point[] GetPoints()
        {
            Point[] points = new Point[3];
            points[0] = PointA;
            points[1] = PointB;
            points[2] = PointC;
            return points;
        }

        public Triangle(double edgeA, double edgeB, double edgeC)
        {
            IsAutoPoints = true;
            CreateFromEdges(edgeA, edgeB, edgeC);
        }

        public Triangle (Point pointA, Point pointB, Point pointC)
        {
            PointA = pointA;
            PointB = pointB;
            PointC = pointC;

            double edgeA = PointB.DistanceTo(PointC);
            double edgeB = PointA.DistanceTo(PointC);
            double edgeC = PointA.DistanceTo(PointB);

            CreateFromEdges(edgeA, edgeB, edgeC);
        }

        public double GetArea() => GetArea(TriangleAreaCalculator.Geron);

        public double GetArea(TriangleAreaCalculator option)
        {
            if (option == TriangleAreaCalculator.Geron)
            {
                return GetAreaGeron();
            }
            else if (option == TriangleAreaCalculator.AngleAndTwoEdges)
            {
                return GetAreaAngleAndTwoEdges();
            }
            else throw new InvalidOperationException();
        }

        private double GetAreaAngleAndTwoEdges()
        {
            double area = 0.5 * EdgeB * EdgeC * Math.Sin(AngleA.Radian);
            return area;
        }

        private double GetAreaGeron()
        {
            double p = GetPerimeter() / 2;
            double sqrArea = p * (p-EdgeA) * (p-EdgeB) * (p-EdgeC);
            return Math.Sqrt(sqrArea);
        }

        public enum TriangleAreaCalculator
        {
            Geron = 1,
            AngleAndTwoEdges = 2
        }

        public double GetPerimeter()
        {
            return EdgeA + EdgeB + EdgeC;
        }

        public override string ToString()
        {
            return string.Format("AA: {0}, AB: {1}, AC: {2}.\n" +
                                 "EA: {3, 4:f2}, EB: {4, 4:f2}, EC: {5, 4:f2}.",
                                 AngleA, AngleB, AngleC, EdgeA, EdgeB, EdgeC);
        }

        /// <summary>
        /// Устанавливает отношение конгруэнтности, не тождественного равенства (Равенство углов и сторон, но не координат)
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True при конгруэнтности треугольников</returns>
        public bool Equals(Triangle other)
        {
            bool isSameAngles = GetAngles().OrderBy(x=>x).SequenceEqual(other.GetAngles().OrderBy(x=>x));
            bool isSameEdges = GetEdges().OrderBy(x => x).SequenceEqual(other.GetEdges().OrderBy(x => x));
            return isSameAngles && isSameEdges;
        }

        public override bool Equals(object? obj)
        {
            return obj is Triangle triangle && triangle.Equals(this);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PointA, PointB, PointC, Math.Round(EdgeA, 8), Math.Round(EdgeB, 8), Math.Round(EdgeC, 8));
        }
    }
}
