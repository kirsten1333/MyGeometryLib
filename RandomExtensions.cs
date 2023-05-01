using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGeometryLib
{
    internal static class RandomExtensions
    {
        public static double NextDouble(this Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        public static double NextDouble(this Random random, double max)
        {
            return random.NextDouble(0, max);
        }

        public static Point NextPoint(this Random random, double min, double max)
        {
            return new(random.NextDouble(min, max), random.NextDouble(min, max));
        }

        public static Point NextPoint(this Random random, double max)
        {
            return NextPoint(random, -max, max);
        }

        public static Point NextPoint(this Random random)
        {
            return NextPoint(random, 0, 100000);
        }

        public static int NextSign(this Random random)
        {
            return Math.Sign(random.NextDouble(-1,1));
        }
    }
}
