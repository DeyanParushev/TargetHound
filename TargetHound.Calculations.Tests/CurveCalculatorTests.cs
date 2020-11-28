namespace TargetHound.Calculations.Tests
{
    using NUnit.Framework;
    using System;
    using TargetHound.Calcualtions;
    using TargetHound.DataModels;

    public class CurveCalculatorTests
    {
        // Zero case
        [TestCase(90, -73, 90, -73, 0)]
        // Dip gain and drop
        [TestCase(90, -73, 90, -72, 1)]
        [TestCase(90, -73, 90, -74, 1)]
        // Azimuth gain and drop
        [TestCase(90, -73, 94, -73, 1.169)]
        [TestCase(90, -73, 86, -73, 1.169)]
        // Passing vertical
        [TestCase(270, -89, 90, -87, 4.000)]
        // Passing through north
        [TestCase(0, -73, 359, -73, 0.292)]
        [TestCase(0, -73, 1, -73, 0.292)]
        [TestCase(359, -73, 1, -73, 0.585)]
        [TestCase(1, -73, 359, -73, 0.585)]
        // Passing north and horizontal
        [TestCase(359, -1, 1, 2, 3.605)]
        // Passing north and vertical
        [TestCase(359, -89, 1, -87, 2.001)]
        public void DoglegCalculationTest(
            double topAzimuth,
            double topDip,
            double bottomAzimuth,
            double bottomDip,
            double expectedDogleg)
        {
            SurveyPoint topStation = new SurveyPoint { Dip = topDip, Azimuth = topAzimuth };
            SurveyPoint bottomStation = new SurveyPoint { Dip = bottomDip, Azimuth = bottomAzimuth };
            CurveCalculator curveCalculator = new CurveCalculator();

            double actualDoglegAngle = curveCalculator.GetDoglegAngle(topStation, bottomStation);

            Assert.AreEqual(expectedDogleg.ToString("F3"), actualDoglegAngle.ToString("F3"));
        }

        // Zero case
        [TestCase(0, 90, -73, 30, 90, -73, 0)]
        // Standart dip and azimuth changes
        [TestCase(0, 20, -75, 30, 45, -65, 12.952)]
        [TestCase(0, 75, -59, 45, 75, -62, 2.000)]
        [TestCase(0, 359, -60, 45, 2, -60, 1.000)]
        [TestCase(0, 1, -60, 45, 359, -60, 0.667)]
        [TestCase(0, 20, -75, 35, 45, -65, 11.101)]
        // Passing vertical
        [TestCase(15, 90, -89, 45, 270, -86, 5.000)]
        [TestCase(15, 270, -89, 45, 90, -86, 5.000)]
        // Passing horizontal
        [TestCase(15, 270, -2, 45, 270, 2, 4.000)]
        // Passing north
        [TestCase(15, 90, -2, 45, 270, -2, 176.000)]
        public void DoglegSeverityClaculatonTest(
            double topDepth,
            double topAzimuth,
            double topDip,
            double bottomDepth,
            double bottomAzimuth,
            double bottomDip,
            double doglegSeverity
            )
        {
            SurveyPoint topStation = new SurveyPoint { Depth = topDepth, Azimuth = topAzimuth, Dip = topDip };
            SurveyPoint bottomStation = new SurveyPoint { Depth = bottomDepth, Azimuth = bottomAzimuth, Dip = bottomDip };
            CurveCalculator curveCalculator = new CurveCalculator();

            double actualDoglegSeverity = curveCalculator.GetDoglegSeverity(topStation, bottomStation);

            Assert.AreEqual(doglegSeverity.ToString("F3"), actualDoglegSeverity.ToString("F3"));
        }

        // Azimuth turn
        [TestCase(75, -59, 78, -59, 91.286)]
        [TestCase(78, -59, 75, -59, 268.714)]
        // Dip gain/drop
        [TestCase(75, -59, 75, -58, 0)]
        [TestCase(75, -59, 75, -60, 180)]
        // Simultanieus turn
        [TestCase(75, -59, 76, -60, 153.520)]
        [TestCase(75, -59, 74, -58, 331.986)]
        [TestCase(75, -59, 76, -58, 28.014)]
        [TestCase(75, -59, 74, -60, 206.480)]
        // Pass through north
        [TestCase(2, -59, 355, -59, 266.999)]
        [TestCase(355, -59, 2, -59, 93.001)]
        // Pass through horizontal
        [TestCase(0, -2, 0, 2, 0)]
        [TestCase(0, 2, 0, -2, 180)]
        // Pass through horizontal and north 
        [TestCase(3, -2, 355, 2, 296.523)]
        [TestCase(355, 2, 3, -2, 116.523)]
        // Vertucal orientation
        [TestCase(3, -90, 15, -89, 15)]
        public void ToolFaceCalculationTest(
            double topAzimuth,
            double topDip,
            double bottomAzimuth,
            double bottomDip,
            double toolFace)
        {
            SurveyPoint topStation = new SurveyPoint { Azimuth = topAzimuth, Dip = topDip };
            SurveyPoint bottomStation = new SurveyPoint { Azimuth = bottomAzimuth, Dip = bottomDip };
            CurveCalculator curveCalculator = new CurveCalculator();

            double actualToolFace = curveCalculator.GetToolFace(topStation, bottomStation);

            Assert.AreEqual(toolFace.ToString("F3"), actualToolFace.ToString("F3"));
        }

        [TestCase(20, -75, 45, -65, 1.0042800000000001)]
        [TestCase(45, -75, 45, -75, 1)]
        public void RatioFactorCalculationTests(
            double topAzimuth,
            double topDip,
            double bottomAzimuth,
            double bottomDip,
            double expectedRatioFactor)
        {
            SurveyPoint topStation = new SurveyPoint { Azimuth = topAzimuth, Dip = topDip };
            SurveyPoint bottomStation = new SurveyPoint { Azimuth = bottomAzimuth, Dip = bottomDip };
            CurveCalculator curveCalculator = new CurveCalculator();

            double actualRatioFactor = Math.Round(curveCalculator.GetRatioFactor(topStation, bottomStation), 5);

            Assert.AreEqual(expectedRatioFactor, actualRatioFactor);
        }
    }
}