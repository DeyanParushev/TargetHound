﻿namespace TargetHound.Calculations.Tests
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;

    using TargetHound.Calcualtions;
    using TargetHound.DTOs;

    public class _3DDistanceCalculatorTests
    {
        //// TODO: Test for possitively inclined boreholes
        private _3DDistanceCalculator distanceCalculator;
        private IPoint collar = new CollarDTO { Easting = 659_866, Northing = 9_022_962, Elevation = 530, Depth = 0 };
        private IPoint negativeTarget = new TargetDTO { Easting = 659_300, Northing = 9_022_400, Elevation = -811 };
        private Extrapolator extrapolator;

        [TestCase(225.18, -59.25, 0, 0, 0.36150725910973086)]
        [TestCase(225.18, -59.25, 0.2, 0, 71.882843672247134)]
        [TestCase(225.18, -59.2, -0.3, 0, 108.31071641023574)]
        [TestCase(227, -59.2, 0, 0, 25.078890141225006)]
        [TestCase(220, -59.2, 0, 0, 72.465065136186055)]
        [TestCase(225.18, -61, 0, 0, 47.486789564466868)]
        [TestCase(225.18, -58, 0, 0, 34.201946354296517)]
        [TestCase(224, -60, 0.1, 0.1, 54.365085767365109)]
        [TestCase(227, -58, 0.1, 0.1, 122.97161776033722)]
        [TestCase(224, -60, -0.1, -0.1, 103.38785898816293)]
        [TestCase(227, -58, -0.1, -0.1, 38.13620405303049)]
        [TestCase(150, -58, 0.2, -0.1, 872.62627134310048)]
        [TestCase(45.18, -80, 0, -1, 344.78388275388062)]

        public void GetMinimumSpacialDistnceTests(
            double startAzimuth,
            double startDip,
            double azimuthChange,
            double dipChange,
            double distance)
        {
            this.collar.Azimuth = startAzimuth;
            this.collar.Dip = startDip;
            this.Setup(azimuthChange, dipChange);

            double minimumSpacialDistance = this.distanceCalculator.GetMinimumSpacialDistance();

            Assert.AreEqual(distance, minimumSpacialDistance);
        }

        private void Setup(double azimuthChange, double dipChange)
        {
            var angleConverter = new Mock<AngleConverter>().Object;
            var curveCalculator = new Mock<CurveCalculator>().Object;
            var coordinateSetter = new Mock<CoordinatesSetter>(curveCalculator, angleConverter).Object;
            var straightExtrapolationCalculator = new Mock<StraightExtrapolationCalculator>(angleConverter).Object;

            this.extrapolator = new Extrapolator(straightExtrapolationCalculator, coordinateSetter);
            var borehole = this.extrapolator.GetCurvedExtrapolaton(this.collar, azimuthChange, dipChange);
            this.distanceCalculator = new _3DDistanceCalculator(borehole, this.negativeTarget, coordinateSetter);
        }
    }
}
