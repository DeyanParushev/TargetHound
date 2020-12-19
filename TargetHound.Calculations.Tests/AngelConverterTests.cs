namespace TargetHound.Calculations.Tests
{
    using System;
    using NUnit.Framework;

    using TargetHound.Calcualtions;

    public class AngelConverterTests
    {
        [TestCase(-45, 45)]
        [TestCase(-90, 0)]
        [TestCase(-0, 90)]
        [TestCase(-79, 11)]
        public void ConvertDipToInclinationTests(double dip, double expectedResult)
        {
            AngleConverter angelConverter = new AngleConverter();

            Assert.AreEqual(expectedResult, angelConverter.ConverDipToInclination(dip));
        }

        [TestCase(90, 0)]
        [TestCase(45, -45)]
        [TestCase(0, -90)]
        [TestCase(13.5, -76.5)]
        public void ConvertInclinationToDipTests(double inclination, double expectedDip)
        {
            AngleConverter angleConverter = new AngleConverter();

            Assert.AreEqual(expectedDip, angleConverter.ConvertInclinationToDip(inclination));
        }

        [TestCase(90, 1.571)]
        [TestCase(45, 0.785)]
        [TestCase(0, 0)]
        [TestCase(13.5, 0.236)]
        public void ConvertAngleToRadiansTests(double angle, double expectedRadians)
        {
            AngleConverter angleConverter = new AngleConverter();
            Assert.AreEqual(expectedRadians.ToString("F3"), angleConverter.ConvertAngleToRadians(angle).ToString("F3"));
        }

        [TestCase(1.571, 90)]
        [TestCase(0.785, 45)]
        [TestCase(0, 0)]
        [TestCase(0.236, 13.5)]
        public void ConvertRadiansToAngle(double radians, double expectedAngle)
        {
            AngleConverter angleConverter = new AngleConverter();
            Assert.AreEqual(expectedAngle, Math.Round(angleConverter.ConvertRadiansToAngle(radians), 1));
        }
    }
}
