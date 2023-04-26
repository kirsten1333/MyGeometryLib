namespace MyGeometryLib.TestLib
{
    [TestFixture]
    internal class Circle_Should
    {
        private readonly Random random = new(6663628);

        #region AreaTest

        [Test]
        public void StressAreaTest()
        {
            for (int i = 0; i < 100000; i++)
            {
                double distance = GetRandomDouble();
                double expectedArea = distance * distance * Math.PI;
                CircleAreaTest(distance, expectedArea);
            }
        }

        [TestCase(0, 0)]
        [TestCase(1, Math.PI)]
        [TestCase(1.41421356237, Math.Tau)]
        [TestCase(123123.15153, 47624378320.20846)]
        [TestCase(99999, 31415298220.508804)]
        public void CircleAreaTest(double distance, double expectedArea)
        {
            Circle circle = new(distance);
            double area = circle.GetArea();
            Assert.AreEqual(expectedArea, area, 0.00001);
        }
        #endregion

        #region IntersectTest
        [Test]
        public void IntersectionContactStressTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                ContactCirclesTest();
            }
        }

        [Test]
        public void IntersectionStressTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                IntersectinonTest();
            }
        }

        public void ContactCirclesTest()
        {
            CreateContactCircles(out Circle circle1, out Circle circle2);
            if (!CheckContactCircles(circle1, circle2)) throw new ApplicationException("Generating contact circles don`t work");

            var points = circle1.Intersect(circle2);
            Assert.IsTrue(points.Count() == 1,
                $"Count of intersection points != 1\n" +
                $"On Circle: {circle1}\n" +
                $"And Circle: {circle2}\n" +
                $"Distance between circles: {circle1.Center.DistanceTo(circle2.Center)}\n" +
                $"Points: {string.Join(' ', points)}");
            CheckFirstPoint(circle1, points.First().DistanceTo(circle1.Center));
        }

        private void IntersectinonTest()
        {
            CreateIntersectCircle(out Circle circle1, out Circle circle2);

            var points = circle1.Intersect(circle2);

            Assert.IsTrue(points.Count() == 2,
                "Count of intersection points != 2");

            foreach (var point in points)
            {
                CheckFirstPoint(circle1, point.DistanceTo(circle1.Center));
                CheckFirstPoint(circle2, point.DistanceTo(circle2.Center));
            }
        }
        #endregion

        #region EqualityTests
        [Test]
        public void TestEquals()
        {
            for (int i = 0; i < 1000; i++)
            {
                CircleEquals();
                CircleNotEquals();
                CircleOpEquals();
                CircleNotOpEquals();
                CircleOpNotEquals();
                CircleNotOpNotEquals();
            }
        }

        [Test]
        public void CircleNotOpNotEquals()
        {
            CreateRandomNotSameCircles(out Circle circle, out Circle circle1);
            Assert.IsTrue(circle != circle1);
        }

        [Test]
        public void CircleNotOpEquals()
        {
            CreateRandomNotSameCircles(out Circle circle, out Circle circle1);
            Assert.IsFalse(circle1 == circle);
        }

        [Test]
        public void CircleNotEquals()
        {
            CreateRandomNotSameCircles(out Circle circle, out Circle circle1);
            Assert.IsFalse(circle1.Equals(circle));
        }

        [Test]
        public void CircleOpNotEquals()
        {
            CreateRandomSameCircles(out Circle circle, out Circle circle1);
            Assert.IsFalse(circle != circle1);
        }

        [Test]
        public void CircleOpEquals()
        {
            CreateRandomSameCircles(out Circle circle, out Circle circle1);
            Assert.IsTrue(circle1 == circle);
        }

        [Test]
        public void CircleEquals()
        {
            CreateRandomSameCircles(out Circle circle, out Circle circle1);
            Assert.IsTrue(circle1.Equals(circle));
        }
        #endregion

        static void CheckFirstPoint(Circle circle1, double distance)
        {
            Assert.AreEqual(distance, circle1.Radius, 0.000001,
                $"Wrong point in intertsection\n" +
                $"on Circle: {circle1}" +
                $"with distance {distance}");
        }

        private void CreateIntersectCircle(out Circle circle1, out Circle circle2)
        {
            do
            {
                Point center1 = GetRandomPoint();
                Point center2 = GetRandomPoint();
                Point IntersectionPoint = GetRandomPoint();

                circle1 = new(center1, IntersectionPoint);
                circle2 = new(center2, IntersectionPoint);
            }
            while (circle1 == circle2 || CheckContactCircles(circle1, circle2)); 
        }

        private static bool CheckContactCircles(Circle circle1, Circle circle2)
        {
            var dist = circle1.Center.DistanceTo(circle2.Center);
            return 0 == Math.Round(circle1.Radius + circle2.Radius - dist, 8);
        }

        private void CreateContactCircles(out Circle circle, out Circle circle1)
        {
            Point center1 = GetRandomPoint();
            Point center2 = GetRandomPoint();

            if (center1 == center2) center2 += new Point(2, 2);

            var dist = center1.DistanceTo(center2);

            var r1 = dist * (random.NextDouble()*(1-0.1) + 0.1);
            var r2 = dist - r1;

            circle = new(center1, r1);
            circle1 = new(center2, r2);
        }

        private void CreateRandomSameCircles(out Circle circle, out Circle circle1)
        {
            var radius = GetRandomDouble();
            circle = new(radius);
            circle1 = new(radius);
        }

        private void CreateRandomNotSameCircles(out Circle circle, out Circle circle1)
        {
            circle = new(GetRandomPoint(), GetRandomDouble());
            circle1 = new(circle.Radius + 2);
        }

        private Point GetRandomPoint() =>
            new(GetRandomDouble(), GetRandomDouble());

        private double GetRandomDouble() 
            => random.Next(100000) + random.NextDouble();
    }
}
