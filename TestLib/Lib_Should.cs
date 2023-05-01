using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGeometryLib.TestLib
{
    internal class Lib_Should
    {
        private readonly Circle_Should circle_Should = new();
        private readonly Triangle_Should triangle_Should = new();
        private readonly Random random = new(6663628);

        [Test]
        public void StressAreaTest() 
        {
            for (int i = 0; i < 1000; i++)
            {
                TestCircleArea();
                TestTriangleArea();
            }
        }

        [Test]
        public void TestCircleArea()
        {
            var radius = random.NextDouble(10000);
            var area = Facade.CalculateArea(radius);
            circle_Should.CircleAreaTest(radius, area);
        }

        [Test]
        public void TestTriangleArea()
        {
            triangle_Should.CreateThreeRandomPoints(out Point a, out Point b, out Point c);
            var area = Facade.CalculateArea(a.DistanceTo(c), a.DistanceTo(b), c.DistanceTo(b));
            triangle_Should.SimpleAreaTest(a.DistanceTo(c), a.DistanceTo(b), c.DistanceTo(b), area);
        }
    }
}
