namespace TargetHound.Calculations.Tests
{
    using NUnit.Framework;

    using TargetHound.Calcualtions;
    using TargetHound.DTOs;

    public class CoordinatesSetterTests
    {
        private SurveyPointDTO topStation = new SurveyPointDTO
        {
            Depth = 0,
            Azimuth = 90,
            Dip = -89.6,
            Easting = 659_866.00,
            Northing = 9_022_962.00,
            Elevation = 530.00,
        };
        private readonly CoordinatesSetter coordinatesSetter;

        public CoordinatesSetterTests(CoordinatesSetter coordinatesSetter)
        {
            this.coordinatesSetter = coordinatesSetter;
        }

        [TestCase(15, 331.20, -89.7, 659_866.0334)]
        [TestCase(15, 270, -88, 659_865.7906)]
        [TestCase(15, 95, -89, 659_866.1828)]
        [TestCase(15, 85, -89, 659_866.1828)]
        [TestCase(15, 95, -89.8, 659_866.0784)]
        [TestCase(15, 85, -89.8, 659_866.0784)]

        public void SetEastingTest(double bottomDepth, double bottomAzimuth, double bottomDip, double expectedBottomEasting)
        {
            SurveyPointDTO bottomStation = new SurveyPointDTO { Depth = bottomDepth, Azimuth = bottomAzimuth, Dip = bottomDip };
           
            this.coordinatesSetter.SetBottomStationUTMCoortinates(this.topStation, bottomStation);

            double bottomEasting = bottomStation.Easting;
            Assert.AreEqual(expectedBottomEasting.ToString(), bottomEasting.ToString("F4"));
        }

        [TestCase(15, 331.20, -89.7, 9_022_962.0344)]
        [TestCase(15, 270, -88, 9_022_962.00001)]
        [TestCase(15, 95, -89, 9_022_961.9886)]
        [TestCase(15, 85, -89, 9_022_962.0114)]
        [TestCase(15, 85, -89.8, 9_022_962.0023)]
        [TestCase(15, 95, -89.8, 9_022_961.9977)]

        public void SetNorthingTest(double bottomDepth, double bottomAzimuth, double bottomDip, double expectedBottomEasting)
        {
            SurveyPointDTO bottomStation = new SurveyPointDTO { Depth = bottomDepth, Azimuth = bottomAzimuth, Dip = bottomDip };
           
            this.coordinatesSetter.SetBottomStationUTMCoortinates(this.topStation, bottomStation);

            double bottomNorthing = bottomStation.Northing;
            Assert.AreEqual(expectedBottomEasting.ToString("F4"), bottomNorthing.ToString("F4"));
        }

        [TestCase(15, 331.20, -89.7, 515.0003)]
        [TestCase(15, 270, -88, 515.0048)]
        [TestCase(15, 95, -89, 515.0013)]
        [TestCase(15, 85, -89, 515.0013)]
        [TestCase(15, 85, -89.8, 515.0002)]
        [TestCase(15, 95, -89.8, 515.0002)]

        public void SetElevationTest(double bottomDepth, double bottomAzimuth, double bottomDip, double expectedBottomEasting)
        {
            SurveyPointDTO bottomStation = new SurveyPointDTO { Depth = bottomDepth, Azimuth = bottomAzimuth, Dip = bottomDip };
           
            this.coordinatesSetter.SetBottomStationUTMCoortinates(this.topStation, bottomStation);

            double bottomElevation = bottomStation.Elevation;
            Assert.AreEqual(expectedBottomEasting.ToString("F2"), bottomElevation.ToString("F2"));
        }
    }
}
