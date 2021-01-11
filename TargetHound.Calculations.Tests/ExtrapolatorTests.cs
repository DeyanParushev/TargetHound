namespace TargetHound.Calculations.Tests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;
    using Moq;

    using TargetHound.Calcualtions;
    using TargetHound.DTOs;

    public class ExtrapolatorTests
    {
        private Extrapolator extrapolator;
        private IPoint collar = new CollarDTO { Easting = 659_866, Northing = 9_022_962, Elevation = 530, Depth = 0, Azimuth = 45, Dip = -75 };

        [TestCase(1300)]
        [TestCase(300)]
        [TestCase(30)]
        [TestCase(564)]
        public void GetStraightExtrapolationAngleTests(double depth)
        {
            this.Setup(depth);
            List<IPoint> extrapolation = (List<IPoint>)this.extrapolator.GetStraightExtrapolation(this.collar, depth);
            
            Assert.AreEqual(extrapolation[0].Azimuth, this.collar.Azimuth);
            Assert.AreEqual(extrapolation[0].Dip, this.collar.Dip);
            Assert.AreEqual(extrapolation[extrapolation.Count - 1].Azimuth, this.collar.Azimuth);
            Assert.AreEqual(extrapolation[extrapolation.Count - 1].Dip, this.collar.Dip);
        }

        [TestCase(1300)]
        [TestCase(300)]
        [TestCase(30)]
        [TestCase(564)]
        public void GetStraightExtrapolationDepthTests(double depth)
        {
            this.Setup(depth);
            List<IPoint> extrapolation = (List<IPoint>)this.extrapolator.GetStraightExtrapolation(this.collar, depth);
           
            Assert.AreEqual(depth, extrapolation[extrapolation.Count - 1].Depth);
        }

        [TestCase(1300)]
        [TestCase(300)]
        [TestCase(30)]
        [TestCase(564)]
        public void GetStraightExtrapolationMultipleVariablesOverloadTests(double depth)
        {
            this.Setup(depth);
            IList<IPoint> extrapolation = this.extrapolator.GetStraightExtrapolation(
                this.collar.Easting,
                this.collar.Northing,
                this.collar.Elevation,
                this.collar.Azimuth,
                this.collar.Dip);
            
            Assert.AreEqual(extrapolation[0].Azimuth, this.collar.Azimuth);
            Assert.AreEqual(extrapolation[0].Dip, this.collar.Dip);
            Assert.AreEqual(depth, extrapolation[extrapolation.Count - 1].Depth);
            Assert.AreEqual(extrapolation[extrapolation.Count - 1].Azimuth, this.collar.Azimuth);
            Assert.AreEqual(extrapolation[extrapolation.Count - 1].Dip, this.collar.Dip);
        }

        [TestCase(1200, 1, 1)]
        [TestCase(300, 0.5, 0.5)]
        public void GetCurvedExtrapolationObjectVariableOverload(double depth, double aziChange, double dipChange)
        {
            this.Setup(depth);
            List<IPoint> extrapolation = (List<IPoint>)this.extrapolator.GetCurvedExtrapolaton(this.collar, aziChange, dipChange);
            double expectedTotalAziChange = Math.Abs(extrapolation.Count * aziChange);
            double actualAzimuthChange = Math.Abs(extrapolation[0].Azimuth - extrapolation[extrapolation.Count - 1].Azimuth) + aziChange;

            double expectedTotalDipChange = Math.Abs(extrapolation.Count * dipChange);
            double actualDipChange = Math.Abs(extrapolation[0].Dip - extrapolation[extrapolation.Count - 1].Dip) + dipChange;

            Assert.AreEqual(expectedTotalAziChange, actualAzimuthChange);
            Assert.AreEqual(expectedTotalDipChange, actualDipChange);
            Assert.AreEqual(depth, extrapolation[extrapolation.Count - 1].Depth);
        }

        public void Setup(double depth)
        {
            var angleConverter = new Mock<AngleConverter>().Object;
            var curveCalculator = new Mock<CurveCalculator>().Object;
            var coordinateSetter = new Mock<CoordinatesSetter>(curveCalculator, angleConverter).Object;
            var straightExtrapolationCalculator = new Mock<StraightExtrapolationCalculator>(angleConverter).Object;
            this.extrapolator = new Extrapolator(straightExtrapolationCalculator, coordinateSetter, depth);
        }
    }
}
