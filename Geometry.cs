namespace MyGeometryLib
{
    namespace Geometry
    {
        public readonly struct Point
        {
            public static Point Zero { get; } = new(0, 0);
            public double X { get; }
            public double Y { get; }
            private int Hash { get; }
            // На самом деле не уверен, что это хороший способ реализации хэшкода,
            // с другой  стороны, зачем каждый раз считать Хэш, если его можно сохранить.
            // Моего опыта не хватает, чтобы решить что лучше, поэтому приведены оба способа

            public Point(double x, double y)
            {
                X = x;
                Y = y;
                Hash = HashCode(x, y);
            }

            public static bool operator ==(Point point, Point other) => point.X == other.X && point.Y == other.Y;
            public static bool operator !=(Point point, Point other) => point.X != other.X || point.Y != other.Y;

            public static Point operator +(Point point, Point other) => new(point.X + other.X, point.Y + other.Y);
            public static Point operator -(Point point, Point other) => new(point.X - other.X, point.Y - other.Y);
            public static Point operator *(Point point, double k) => new(point.X * k, point.Y * k);
            public static Point operator *(double k, Point point) => new(point.X * k, point.Y * k);
            public static Point operator /(Point point, double k) => new(point.X / k, point.Y / k);

            public double SqrDistanceTo(Point other) => (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y);

            public double DistanceTo(Point other) => Math.Sqrt(SqrDistanceTo(other));

            private static int HashCode(double x, double y) => (x.GetHashCode() * 397) ^ y.GetHashCode();

            public override int GetHashCode() => Hash;

            public override string ToString()
                => string.Format("({0, 5:f2}; {1, 5:f2})", X, Y);

            public bool Equals(Point other)
            {
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object? obj)
            {
                return obj is Point other && Equals(other);
            }
        }

        /* Функционал от которого отказался.
        public class Segment
        {
            public Point Start { get; }
            public Point End { get; }
            public Point Vector { get; }
            public double Length { get; }
            public bool IsScalar { get; }

            public Segment(Point start, Point end)
            {
                Start = start;
                End = end;
                Length = start.DistanceTo(end);
                Vector = end - start;
                IsScalar = false;
            }

            public Segment(double length)
            {
                Start = new Point(0, 0);
                End = new Point(length, 0);
                Vector = End;
                Length = length;
                IsScalar = true;
            }

            public static bool operator ==(Segment segment, Segment other) => segment.Start == other.Start && segment.End == other.End;
            public static bool operator !=(Segment segment, Segment other) => segment.Start != other.Start || segment.End != other.End;

            public bool Equals(Segment other) => Start == other.Start && End == other.End;

            public override bool Equals(object? obj) => obj is Segment other && Equals(other);


            public override int GetHashCode()
            {
                return Start.GetHashCode() * 397 ^ End.GetHashCode();
            }

            public override string ToString()
            {
                return string.Format($"S:{Start}, E:{End}, Len:{Length}");
            }
        }
        */

        /// <summary>
        /// Угол в радианах, представлен неотрицательной величиной в пределах [0, Tau).
        /// </summary>
        public class Angle
        {
            public double Radian { get; }
            public bool IsRight { get => Radian == Math.PI / 2; }

            public Angle(double radian)
            {
                Radian = Normilize(radian);
            }

            public Angle(double a, double b, double c) : this(GetABAngle(a, b, c)) { }

            /// <summary>
            /// Метод, который возвращает угол между сторонами с длиннами A и B,
            /// в треугольнике с длинами сторон a,b,c
            /// </summary>
            /// <param name="a">Первая сторона прилегающая к углу </param>
            /// <param name="b">Вторая сторона прилегающая к углу</param>
            /// <param name="c">Сторона не прилегающая к углу</param>
            /// <returns>Значение в радианах велечина угла между сторонами,
            /// если треугольник невозможный(вырожденный), то возвращает NaN,
            /// в случае плоского треугольника возвращает 0.0</returns>
            public static double GetABAngle(double a, double b, double c)
            {
                if (a <= 0 || b <= 0 || c < 0)
                    return double.NaN;
                if (c == 0 && a == b) return 0.0;
                if (a + b == c) return 0.0;
                if (b + c > a && a + c > b && a + b > c)
                {
                    double angle = (a * a + b * b - c * c) / (2 * a * b);
                    angle = Math.Acos(angle);
                    return angle;
                }
                return double.NaN;
            }

            private double Normilize(double value)
            {
                if (value < 0)
                    return Normilize(-value);
                if (value >= Math.Tau)
                    return value % Math.Tau;
                else
                    return value;
            }

            public static bool operator ==(Angle angle, Angle other) => angle.Radian == other.Radian;
            public static bool operator !=(Angle angle, Angle other) => angle.Radian != other.Radian;
            public static bool operator ==(Angle angle, double other) => angle.Radian == other;
            public static bool operator !=(Angle angle, double other) => angle.Radian != other;
            public static Angle operator +(Angle angle, Angle other) => new(angle.Radian + other.Radian);
            public static Angle operator -(Angle angle, Angle other) => new(angle.Radian - other.Radian);

            public bool Equals(Angle other) => Radian == other.Radian;

            public override bool Equals(object? obj) => obj is Angle other && Equals(other);

            public override int GetHashCode()
            {
                return Radian.GetHashCode();
            }

            public override string ToString()
            {
                return string.Format($"{Radian,4:f2}");
            }
        }
    }
}
