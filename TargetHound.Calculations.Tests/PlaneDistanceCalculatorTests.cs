﻿namespace TargetHound.Calculations.Tests
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;

    using TargetHound.Calcualtions;
    using TargetHound.DTOs;

    public class PlaneDistanceCalculatorTests
    {
        //// TODO: Test for positively inclined boreholes
        private PlaneDistanceCalculator planeDistanceCalculator;
        private Extrapolator extrapolator;
        private IPoint collar = new CollarDTO { Easting = 659_866, Northing = 9_022_962, Elevation = 530, Depth = 0 };
        private IPoint negativeTarget = new TargetDTO { Easting = 659_300, Northing = 9_022_400, Elevation = -811 };

        [TestCase(225.18, -59.2, 0, 0, 1.8038841065563065)]
        [TestCase(225.18, -59.2, 0.2, 0, 72.13142077526004)]
        [TestCase(225.18, -59.2, -0.3, 0, 108.85983307797925)]
        [TestCase(225.18, -59.2, 0, 0.2, 189.53118541462197)]
        [TestCase(225.18, -59.2, 0, -0.3, 215.24295871678984)]
        [TestCase(227, -59.2, 0, 0, 25.103407679718138)]
        [TestCase(220, -59.2, 0, 0, 72.511382684576233)]
        [TestCase(225.18, -61, 0, 0, 54.294307394399027)]
        [TestCase(225.18, -58, 0, 0, 40.328779201357534)]
        [TestCase(224, -60, 0.1, 0.1, 65.294315078495586)]
        [TestCase(224, -60, -0.1, -0.1, 111.0371891736599)]
        [TestCase(227, -58, -0.1, -0.1, 42.321055473618642)]
        [TestCase(227, -58, -0.1, -0.1, 42.321055473618642)]
        [TestCase(227, -58, 0.1, 0.1, 148.1932430401373)]
        [TestCase(5, -58, -0.2, 0.1, 1590.4746692288759)]
        [TestCase(150, -58, 0.2, -0.1, 893.84735554572319)]
        public void GetHirizontalDistanceOnTargetElevationTests(
            double startAzimuth,
            double startDip,
            double azimuthChange,
            double dipChange,
            double distance)
        {
            this.collar.Azimuth = startAzimuth;
            this.collar.Dip = startDip;
            this.Setup(negativeTarget, azimuthChange, dipChange);

            double horiozntalDistanceOtTargetElevation = this.planeDistanceCalculator.GetHorizontalDistanceOnTargetElevation();

            Assert.AreEqual(distance, horiozntalDistanceOtTargetElevation);
        }

        [TestCase(225.18, -59.25, 0, 0, 0.31931017214651547)]
        [TestCase(225.18, -59.25, 0.2, 0, 14.662508579980681)]
        [TestCase(225.18, -59.2, -0.3, 0, 34.689591539663638)]
        [TestCase(225.18, -59.2, 0, 0.2, 218.28922281577525)]
        [TestCase(227, -59.2, 0, 0, 3.635379017123455)]
        [TestCase(220, -59.2, 0, 0, 8.491406876816086)]
        [TestCase(225.18, -61, 0, 0, -97.94706713747496)]
        [TestCase(225.18, -58, 0, 0, 64.53919133952661)]
        [TestCase(224, -60, 0.1, 0.1, 88.899326852060199)]
        [TestCase(224, -60, -0.1, -0.1, -209.031370512124)]
        [TestCase(227, -58, -0.1, -0.1, -80.714633227580975)]
        [TestCase(227, -58, 0.1, 0.1, 179.82150510108147)]
        [TestCase(150, -58, 0.2, -0.1, 934.81646593782784)]
        public void GetVerticalDistnceTests(
            double startAzimuth,
            double startDip,
            double azimuthChange,
            double dipChange,
            double distance)
        {
            this.collar.Azimuth = startAzimuth;
            this.collar.Dip = startDip;
            this.Setup(this.negativeTarget, azimuthChange, dipChange);

            double verticalDistance = this.planeDistanceCalculator.GetVerticalDistance();

            Assert.AreEqual(distance, verticalDistance);
        }

        [TestCase(225.18, -59.25, 0, 0, 0.32263140573580229)]
        [TestCase(225.18, -59.25, 0.2, 0, 71.486162239917164)]
        [TestCase(225.18, -59.2, -0.3, 0, 106.80500538513944)]
        [TestCase(227, -59.2, 0, 0, 25.009708546168788)]
        [TestCase(220, -59.2, 0, 0, 72.334518180139924)]
        [TestCase(225.18, -61, 0, 0, 0.32263128162312998)]
        [TestCase(225.18, -58, 0, 0, 0.32263137642944889)]
        [TestCase(224, -60, 0.1, 0.1, 18.491026512238975)]
        [TestCase(227, -58, 0.1, 0.1, 58.298392356769064)]
        [TestCase(224, -60, -0.1, -0.1, 55.860214046518834)]
        [TestCase(227, -58, -0.1, -0.1, 11.7654836276279)]
        [TestCase(150, -58, 0.2, -0.1, 765.59156776651571)]
        [TestCase(45.18, -80, 0, -1, 0.32263131272401185)]

        public void GetMinimumHorizontalDistnceTests(
            double startAzimuth,
            double startDip,
            double azimuthChange,
            double dipChange,
            double distance)
        {
            this.collar.Azimuth = startAzimuth;
            this.collar.Dip = startDip;
            this.Setup(this.negativeTarget, azimuthChange, dipChange);

            double minimumHorizontalDistance = this.planeDistanceCalculator.GetMinimumHorizontalDistance();

            Assert.AreEqual(distance, minimumHorizontalDistance);
        }

        private void Setup(IPoint target, double azimuthChange, double dipChange)
        {
            var angleConverter = new Mock<AngleConverter>().Object;
            var curveCalculator = new Mock<CurveCalculator>().Object;
            var coordinateSetter = new Mock<CoordinatesSetter>(curveCalculator, angleConverter).Object;
            var straightExtrapolationCalculator = new Mock<StraightExtrapolationCalculator>(angleConverter).Object;

            this.extrapolator = new Extrapolator(straightExtrapolationCalculator, coordinateSetter);
            var borehole = this.extrapolator.GetCurvedExtrapolaton(this.collar, azimuthChange, dipChange);

            this.planeDistanceCalculator = new PlaneDistanceCalculator(borehole, target, coordinateSetter);
        }
    }
}
