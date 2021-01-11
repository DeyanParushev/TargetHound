namespace TargetHound.Calculations.Tests
{
    using Moq;
    using NUnit.Framework;

    using TargetHound.Calcualtions;
    using TargetHound.DTOs;

    public class StraightExtrapolationCalculatorTests
    {
        private StraightExtrapolationCalculator straightExtrapolation;
        private IPoint collar = new CollarDTO { Easting = 659_866.0000, Northing = 9_022_962.0000, Elevation = 530.0000 };

        [TestCase(660_011.77, 9_023_008.37, -811.72, 1350.4117)]
        //// Only east change
        [TestCase(659_976.45, 9_023_008.37, -811.72, 1347.0568)]
        //// Only north change
        [TestCase(660_011.77, 9_022_941.31, -811.72, 1349.7739)]
        //// Only elevationChange
        [TestCase(660_011.77, 9_023_008.37, -483.39, 1024.8699)]
        //// Above horizontal extrapolation 
        [TestCase(659_866.0000, 9_022_962.0000, 850, 320.0000)]
        [TestCase(659_950.0000, 9_022_962.0000, 850, 330.8414)]
        [TestCase(659_866.0000, 9_022_600.0000, 850, 483.1604)]
        [TestCase(659_550.0000, 9_022_600.0000, 850, 577.3214)]
        public void GetStraightHoleLengthTest(
            double targetEasting,
            double targetNorthing,
            double targetElevation,
            double expectedLength)
        {
            this.Setup();
            TargetDTO target = new TargetDTO { Easting = targetEasting, Northing = targetNorthing, Elevation = targetElevation };

            Assert.AreEqual(expectedLength.ToString("F4"), this.straightExtrapolation.GetStraightHoleLength(this.collar, target).ToString("F4"));
        }

        //// Vertical hole 
        [TestCase(659_866.0000, 9_022_962.0000, -811, double.NaN)]
        //// 1st quadrant
        [TestCase(660_011.77, 9_023_008.37, -811, 72.3539)]
        //// 2nd quadrant
        [TestCase(660_011.77, 9_022_662.000, -811, 154.0849)]
        //// 3rd quadrant
        [TestCase(659_000.000, 9_022_100.000, -811, 225.1326)]
        //// 4th quadrant
        [TestCase(659_000.000, 9_023_500.37, -811, 301.8682)]
        public void GetAzimuthAngleTest(
            double targetEasting,
            double targetNorthing,
            double targetElevation,
            double expectedAzimuth)
        {
            this.Setup();
            TargetDTO target = new TargetDTO { Easting = targetEasting, Northing = targetNorthing, Elevation = targetElevation };

            this.collar.Azimuth = this.straightExtrapolation.GetInitialAzimuthAngle(this.collar, target);

            Assert.AreEqual(expectedAzimuth.ToString("F4"), this.collar.Azimuth.ToString("F4"));
        }

        //// Vertical hole 
        [TestCase(659_866.0000, 9_022_962.0000, -811, -90)]
        [TestCase(659_866.0000, 9_022_962.0000, 811, 90)]
        //// Hozizontal hole
        [TestCase(659_500.0000, 9_022_962.0000, 530, 0)]
        [TestCase(659_866.0000, 9_022_300.0000, 530, 0)]
        [TestCase(659_400.0000, 9_022_300.0000, 530, 0)]
        [TestCase(660_400.0000, 9_023_300.0000, 530, 0)]
        //// 1st quadrant
        [TestCase(660_011.77, 9_023_008.37, -811, -83.4924)]
        //// 2nd quadrant
        [TestCase(660_011.77, 9_022_662.000, -811, -76.0325)]
        //// 3rd quadrant
        [TestCase(659_000.000, 9_022_100.000, -811, -47.6610)]
        //// 4th quadrant
        [TestCase(659_000.000, 9_023_500.37, -811, -52.7504)]
        public void GetDipAngleTest(
            double targetEasting,
            double targetNorthing,
            double targetElevation,
            double expectedDip)
        {
            this.Setup();
            TargetDTO target = new TargetDTO { Easting = targetEasting, Northing = targetNorthing, Elevation = targetElevation };

            this.collar.Dip = this.straightExtrapolation.GetInitialDipAngle(this.collar, target);

            Assert.AreEqual(expectedDip.ToString("F4"), this.collar.Dip.ToString("F4"));
        }

        private void Setup()
        {
            var angleConverter = Mock.Of<AngleConverter>();
            this.straightExtrapolation = new StraightExtrapolationCalculator(angleConverter);
        }
    }
}
