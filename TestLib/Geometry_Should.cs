using NUnit.Framework;
using System.Diagnostics;

namespace TestLib
{
    [TestFixture]
    public class Geometry_Should
    {
        private readonly Random random = new(6663628);

        [Test]
        public void CreationAngleTest()
        {
            int count = 200;
            for (int i = 0; i < count; i++)
            {
                double radian = random.NextDouble();
                double sign = random.NextDouble() > 0.5 ? 1 : -1;
                double sameRadian = radian + Math.Tau * random.Next(100) * sign;
                Angle a1 = new(radian);
                Angle a2 = new(sameRadian);

                Assert.IsTrue (a1.Equals(a2), 
                    $"Error on Equal test {i + 1}: A1 rad: {a1.Radian}, A2 rad: {a2.Radian}; rad1: {radian}, rad2: {sameRadian}.");

                Assert.IsTrue(a1 == a2,
                    $"Error on == test {i + 1}: A1 rad: {a1.Radian}, A2 rad: {a2.Radian}; rad1: {radian}, rad2: {sameRadian}.");

                Assert.IsTrue(a1 == radian,
                    $"Error on ==toDouble test {i + 1}: A1 rad: {a1.Radian}, A2 rad: {a2.Radian}; rad1: {radian}, rad2: {sameRadian}.");
            }
        }

        [TestCase(3, 4, 5, Math.PI / 2, TestName = "Normal Angle {a}")]
        [TestCase(3.0d, 3.0d, 3.0d, 1.0471975511966d, TestName = "Normal Angle {a}")]
        [TestCase(15.0d, 13.0d, 14.0d, 1.03829222849d, TestName = "Normal Angle {a}")]
        [TestCase(120.5d, 30.24d, 125.12d, 1.601030301068d, TestName = "Normal Angle {a}")]
        [TestCase(15.0d, 13.0d, 14.0d, 1.03829222849d, TestName = "Normal Angle {a}")]
        [TestCase(1, 1, 2, 0, TestName = "Flat Angle {a}")]
        [TestCase(1, 1, 0, 0, TestName = "Flat Angle {a}")]
        [TestCase(1, 1, 2, 0, TestName = "Flat Angle {a}")]
        [TestCase(0, 0, 0, double.NaN, TestName = "Degenerate Angle {a}")]
        [TestCase(-1, 0, 0, double.NaN, TestName = "Degenerate Angle {a}")]
        [TestCase(0, -1, 0, double.NaN, TestName = "Degenerate Angle {a}")]
        [TestCase(0, 0, -1, double.NaN, TestName = "Degenerate Angle {a}")]
        [TestCase(-1, -1, -1, double.NaN, TestName = "Degenerate Angle {a}")]
        [TestCase(0.1d, 0.1d, 1000.0d, double.NaN, TestName = "Degenerate Angle {a}")]
        public void AngleCalcTest(double a, double b, double c, double expectedAngle)
        {
            var actual = Angle.GetABAngle(a, b, c);
            Assert.AreEqual(expectedAngle, actual, 1e-5, $"Actual: {actual}, Expected: {expectedAngle}");
        }

        [TestCase(1, 1000, TestName = "Workability")]
        [TestCase(1000, 1000, TestName = "10^4")]
        [TestCase(10000, 1000, TestName = "10^5")]
        [TestCase(100000, 1000, TestName = "10^6")]
        [TestCase(1000000, 1000, TestName = "10^7")]
        [TestCase(10000000, 1000, TestName = "10^8")]
        public void AngleCalcPerfomanceTest(int repCount, double timelimitInSeconds)
        {
            var values = GenerateValidAnglesArray(repCount);
            var timeInMs = MeasureDurationInMs(CalcAngle, values);
            Assert.Less(timeInMs, timelimitInSeconds, $"Too slow, need {timeInMs / timelimitInSeconds} x faster.");
        }

        private (double, double, double)[] GenerateValidAnglesArray(int repCount)
        {
            var values = new (double, double, double)[repCount];
            for (int i = 0; i < repCount; i++)
            {
                (Point p1, Point p2, Point p3) = (GeneratePoint(), GeneratePoint(), GeneratePoint());
                double a = p1.DistanceTo(p2);
                double b = p2.DistanceTo(p3);
                double c = p3.DistanceTo(p1);
                values[i] = (a, b, c);
            }
            return values;
        }

        private Point GeneratePoint() => new(random.Next(), random.Next());
        //private double GenerateDouble() => random.NextDouble() * random.Next(1000) + 0.1;

        private static Action<(double, double, double)> CalcAngle => ((double, double, double) edges) => Angle.GetABAngle(edges.Item1, edges.Item2, edges.Item3);

        private static double MeasureDurationInMs<T>(Action<T> action, T[] values)
        {
            action(values[0]);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Stopwatch sw = new();
            sw.Start();
            for (int i = 0; i < values.Length; i++)
            {
                action(values[i]);
            }
            sw.Stop();
            return (double)sw.ElapsedMilliseconds;
            //return (double)sw.ElapsedMilliseconds / repetitionCount;
        }
    }
}