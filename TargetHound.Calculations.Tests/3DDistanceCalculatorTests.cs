namespace TargetHound.Calculations.Tests
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using TargetHound.Calcualtions;
    using TargetHound.Models;
    using TargetHound.Models.Interfaces;

    public class _3DDistanceCalculatorTests
    {
        // TODO: Test for possitively inclined boreholes
        private IPoint collar = new Collar { Easting = 659_866, Northing = 9_022_962, Elevation = 530, Depth = 0 };
        private IPoint negativeTarget = new Target { Easting = 659_300, Northing = 9_022_400, Elevation = -811 };
        private Extrapolator curveExtrapolator = new Extrapolator();

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

        public void GetMinimumSpacialDistnceTests(double startAzimuth,
            double startDip,
            double azimuthChange,
            double dipChange,
            double distance)
        {
            collar.Azimuth = startAzimuth;
            collar.Dip = startDip;
            IList<IPoint> borehole = this.curveExtrapolator.GetCurvedExtrapolaton(collar, azimuthChange, dipChange);
            _3DDistanceCalculator distanceCalculator = new _3DDistanceCalculator(borehole, this.negativeTarget);

            double minimumSpacialDistance = distanceCalculator.GetMinimumSpacialDistance();

            Assert.AreEqual(distance, minimumSpacialDistance);
        }
    }
}
