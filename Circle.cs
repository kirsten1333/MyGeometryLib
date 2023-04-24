using MyGeometryLib.Geometry;

namespace MyGeometryLib
{
    internal class Circle : IFigure
    {
        public Point Center { get; }
        public double Radius { get; }

        public Circle(Point center, double radius)
        {
            Center = center;
            Radius = radius;
        }
        public Circle(double radius) : this (new(0,0), radius) { }

        /// <summary>
        /// Выдаёт точки пересечения двух окружностей
        /// </summary>
        /// <param name="other"> Окружность с которой выполняем пересечение</param>
        /// <returns> 
        /// Возвращает ноль, одну, две или три точки, в зависимости от того как пересекаются окружности.
        /// Если окружности не пересекаются - возвращает пустую последовательность
        /// Если окружности соприкасаются - возвращает последовательность из одной точки
        /// Если окружности пересекаются - возвращает две точки
        /// Если они идентичны, возвращает три точки.
        /// </returns>
        public IEnumerable<Point> Intersect(Circle other)
        {
            double dist = Center.DistanceTo(other.Center);
            double sqrDist = Center.SqrDistanceTo(other.Center);

            if (Center == other.Center && Radius != other.Radius) return Enumerable.Empty<Point>();
            else if (Center == other.Center && Radius == other.Radius) return GetThreePoints();
            else if (dist > Radius + other.Radius) return Enumerable.Empty<Point>();
            else
            {
                return IntersectImpl();
            }

            IEnumerable<Point> GetThreePoints() // Можно было решить в общем виде, но YAGNI
            {
                yield return new(Center.X, Center.Y + Radius);
                yield return new(Center.X, Center.Y - Radius);
                yield return new(Center.X + Radius, Center.Y);
            }

            IEnumerable<Point> IntersectImpl()
            {
                double r0 = Radius * Radius;
                double r1 = other.Radius * other.Radius;

                Point p0 = Center;
                Point p1 = other.Center;
                // Источник: http://algolist.manual.ru/maths/geom/intersect/circlecircle2d.php
                double a = (r0 - r1 + sqrDist) / (2 * dist); 
                double h = Math.Sqrt(r0 - a*a);
                Point p2 = p0 + a * (p1 - p0) / dist;

                double x31 = p2.X - h * (p1.Y - p0.Y) / dist;
                double y31 = p2.Y + h * (p1.X - p0.X) / dist;
                Point p31 = new(x31, y31);

                yield return p31;

                double x32 = p2.X + h * (p1.Y - p0.Y) / dist;
                double y32 = p2.Y - h * (p1.X - p0.X) / dist;
                Point p32 = new(x32, y32);
                
                if(p31 != p32) { yield return p32; }
            }
        }

        public double GetArea() => Math.PI * Radius * Radius;

        public double GetPerimeter() => 2 * Math.PI* Radius;

        public override bool Equals(object? obj)
        {
            return obj is Circle other 
                && other.Radius == Radius 
                && other.Center == Center;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Center, Radius);
        }

        public override string ToString()
        {
            return string.Format($"C:{Center}, Radius:{Radius}");
        }
    }
}
