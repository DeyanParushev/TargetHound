namespace TargetHound.Calculations.Tests
{
    using NUnit.Framework;
    using TargetHound.Calcualtions;
    using TargetHound.Models;

    public class CurveExtrapolationCalculatorTests
    {
        private Collar collar = new Collar { Easting = 659_866.0000, Northing = 9_022_962.0000, Elevation = 530.0000 };

        [TestCase(659_300, 9_022_400, -811, 0.5)]
        public void GetDLSBetweenStationsTest(
            double targetEasting,
            double targetNorthing,
            double targetElevation,
            double expectedDLS)
        {
            Target target = new Target
            {
                Easting = targetEasting,
                Northing = targetNorthing,
                Elevation = targetElevation
            };
        }
    }
}
