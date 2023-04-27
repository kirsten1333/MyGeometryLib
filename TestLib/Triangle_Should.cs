namespace MyGeometryLib.TestLib
{
    internal class Triangle_Should
    {
        private Random random = new(6663628);

        [TestCase(10000)]
        public void EqualityStressTest(int itteration)
        {
            for (int i = 0; i < itteration; i++)
            {
                EqualToHimSelf();
                EqualityNotSameTest();
                EqualitySameTest();
                EqualityExactSameTest();
            }
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
            NotEqualAssert(t1, t2);
            NotEqualAssert(t2, t1);
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
            Assert.That(t1.Equals(t1), $"t1: {t1}\n are equal to \nt2: {t2}");
        }

        private static void NotEqualAssert(Triangle t1, Triangle t2)
        {
            Assert.That(!t1.Equals(t1), $"t1: {t1}\n are not equal to \nt2: {t2}");
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

        private void CreateThreeRandomPoints(out Point p1, out Point p2, out Point p3)
        {
            p1 = random.NextPoint();
            p2 = random.NextPoint();
            p3 = random.NextPoint();
        }
    }
}
