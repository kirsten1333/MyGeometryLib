using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGeometryLib
{
    internal static class DoubleExtensions
    {
        public const double Delta = 0.000000001;
        public static bool EqualTo(this double value1, double value2, double epsilon)
        {
            return Math.Abs(value1 - value2) < epsilon;
        }
        public static bool EqualTo(this double value1, double value2)
            => value1.EqualTo(value2, Delta);
    }
}
