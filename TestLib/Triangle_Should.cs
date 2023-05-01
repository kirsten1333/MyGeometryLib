namespace MyGeometryLib.TestLib
{
    internal class Triangle_Should
    {
        private readonly Random random = new(6663628);
        private const double maxRandom = 10000;
        private const double accuracy = 0.001;

        #region AreaTest

        [Test]
        public void StressAreaTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                CreateSameTriangles(out Triangle t, out _);

                double pp = t.GetPerimeter() / 2;
                double sqrExpectedArea = pp * (pp - t.EdgeA) * (pp - t.EdgeB) * (pp - t.EdgeC);
                double expectedArea = Math.Sqrt(sqrExpectedArea);

                SimpleAreaTest(t.EdgeA, t.EdgeB, t.EdgeC, expectedArea);
            }
        }

        [TestCase(3, 4, 5, 6)]
        [TestCase(10, 11, 12, 51.5212334867)]
        [TestCase(95, 31, 123, 717.70689525)]
        [TestCase(1.5, 2.5, 3.5, 1.62379763209)]
        public void SimpleAreaTest(double edgeA, double edgeB, double edgeC, double expectedArea)
        {
            Triangle t = new(edgeA, edgeB, edgeC);
            foreach (var item in Enum.GetValues(typeof(Triangle.TriangleAreaCalculator)))
            {
                Triangle.TriangleAreaCalculator areaCalculator = (Triangle.TriangleAreaCalculator)item;
                var actual = t.GetArea(areaCalculator);
                Assert.AreEqual(expectedArea, actual, accuracy,
                    $"Expected: {expectedArea} +/- {accuracy}\n" +
                    $"\tActual: {actual}\n" +
                    $"\tDifference: {expectedArea - actual}\n" +
                    $"\tWith {item} AreaCalculator");
            }
        }
        #endregion

        #region EqualityTests

        [TestCase(10000)]
        public void EqualityStressTest(int itteration)
        {
            for (int i = 0; i < itteration; i++)
            {
                EqualToHimSelf();
                EqualityNotSameTest();
                EqualitySameTest();
                EqualityExactSameTest();
                NotEqualityTest();
            }
        }

        [Test]
        public void NotEqualityTest()
        {
            CreateDifferentTriangles(out Triangle t1, out Triangle t2);
            NotEqualAssert(t1, t2);
            NotEqualAssert(t2, t1);
        }

        [Test]
        public void EqualToHimSelf()
        {
            CreateNotSameTrianglesWithEqualEdges(out Triangle t1, out Triangle t2);
            EqualAssert(t1, t1);
            EqualAssert(t2, t2);
        }

        [Test]
        public void EqualityNotSameTest()
        {
            CreateNotSameTrianglesWithEqualEdges(out Triangle t1, out Triangle t2);
            EqualAssert(t1, t1);
            EqualAssert(t2, t2);
        }

        [Test]
        public void EqualitySameTest()
        {
            CreateSameTriangles(out Triangle t1, out Triangle t2);
            EqualAssert(t1, t2);
            EqualAssert(t2, t1);
        }

        [Test]
        public void EqualityExactSameTest()
        {
            CreateExactSameTriangles(out Triangle t1, out Triangle t2);
            EqualAssert(t1, t2);
            EqualAssert(t2, t1);
        }

        private static void EqualAssert(Triangle t1, Triangle t2)
        {
            Assert.That(t1.Equals(t2), $"t1: {t1}\n are equal to \nt2: {t2}");
        }

        private static void NotEqualAssert(Triangle t1, Triangle t2)
        {
            Assert.That(!t1.Equals(t2), $"t1: {t1}\n are not equal to \nt2: {t2}");
        }

        private void CreateDifferentTriangles(out Triangle t1, out Triangle t2)
        {
            CreateThreeRandomPoints(out Point p11, out Point p21, out Point p31);
            CreateThreeRandomPoints(out Point p12, out Point p22, out Point p32);

            if ((p11, p11, p11) == (p12, p22, p32))
                p11 += new Point(2, 2);
            t1 = new(p11, p21, p31);
            t2 = new(p12, p22, p32);
        }

        private void CreateNotSameTrianglesWithEqualEdges(out Triangle t1, out Triangle t2)
        {
            CreateThreeRandomPoints(out Point p1, out Point p2, out Point p3);

            t1 = new Triangle(p2, p1, p3);
            t2 = new Triangle(p1.DistanceTo(p2), p2.DistanceTo(p3), p3.DistanceTo(p1));
        }

        private void CreateSameTriangles(out Triangle t1, out Triangle t2)
        {
            CreateThreeRandomPoints(out Point p1, out Point p2, out Point p3);

            t1 = new Triangle(p1, p2, p3);
            t2 = new Triangle(p2, p3, p1);
        }

        private void CreateExactSameTriangles(out Triangle t1, out Triangle t2)
        {
            CreateThreeRandomPoints(out Point p1, out Point p2, out Point p3);

            t1 = new Triangle(p1, p2, p3);
            t2 = new Triangle(p1, p2, p3);
        }

        internal void CreateThreeRandomPoints(out Point p1, out Point p2, out Point p3)
        {
            p1 = random.NextPoint(maxRandom);
            p2 = random.NextPoint(maxRandom);
            p3 = random.NextPoint(maxRandom);
        }
        #endregion
    }
}
