namespace TargetHound.Calculations.Tests
{
    using NUnit.Framework;
    using System;
    using TargetHound.Calcualtions;
    using TargetHound.DTOs;

    public class CurveExtrapolationCalculatorTests
    {
        private CollarDTO collar = new CollarDTO { Easting = 659_866.0000, Northing = 9_022_962.0000, Elevation = 530.0000 };

        // TODO: Finish the tests
        [TestCase(659_300, 9_022_400, -811, 0, 0)]
        [TestCase(659_300, 9_022_400, -811, 0.5, 0.5)]
        [TestCase(659_300, 9_022_400, -811, 1.5, -0.5)]
        [TestCase(659_300, 9_022_400, -811, 2.5, 0.5)]
        [TestCase(659_300, 9_022_400, -811, 0.5, -1.5)]
        [TestCase(659_300, 9_022_400, -811, -2.5, 1.5)]
        public void GetCollarForCurvedExtrapolation(
            double targetEasting,
            double targetNorthing,
            double targetElevation,
            double azimuthChange,
            double dipChange)
        {
            //// Arrange
            TargetDTO target = new TargetDTO
            {
                Easting = targetEasting,
                Northing = targetNorthing,
                Elevation = targetElevation
            };

            var angleConverter = new AngleConverter();
            var straightCalculator = new StraightExtrapolationCalculator(angleConverter);
            var curveCalculator = new CurveCalculator();
            var coordinatesSetter = new CoordinatesSetter(curveCalculator, angleConverter);
            var extrapolator = new Extrapolator(straightCalculator, coordinatesSetter);
            CurveExtrapolationCalculator curveExtrapolationCalculator = 
                new CurveExtrapolationCalculator(collar, target, azimuthChange, dipChange);
            
            //// Act
            var minimumDistance = 
                curveExtrapolationCalculator.FindInitialAzimuthAndDip(straightCalculator, extrapolator, coordinatesSetter);

            //// Assert
            Assert.AreEqual(1, Math.Round(minimumDistance, 4));
        }
    }
}
