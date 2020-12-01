namespace TargetHound.Calcualtions
{
    using System;

    using TargetHound.DTOs;

    public class CoordinatesSetter
    {
        private CurveCalculator curveCalculator;
        private AngleConverter angleConverter;
        private double topInclinationRadians;
        private double bottomInclinationRadians;
        private double ratioFactor = 0;
        private double measuredDepth;

        public CoordinatesSetter()
        {
            this.curveCalculator = new CurveCalculator();
            this.angleConverter = new AngleConverter();
        }

        public void SetBottomStationUTMCoortinates(IPoint topStation, IPoint bottomStation)
        {
            this.SetCalculationParameterst(topStation, bottomStation);
            this.SetEasting(topStation, bottomStation);
            this.SetNorthing(topStation, bottomStation);
            this.SetElevation(topStation, bottomStation);
        }

        private void SetEasting(IPoint topStation, IPoint bottomStation)
        {
            double eastDisplacement = this.measuredDepth * this.ratioFactor *
                (Math.Sin(this.topInclinationRadians) * Math.Sin(this.angleConverter.ConvertAngleToRadians(topStation.Azimuth))
                + Math.Sin(this.bottomInclinationRadians) * Math.Sin(this.angleConverter.ConvertAngleToRadians(bottomStation.Azimuth)));

            bottomStation.Easting = topStation.Easting + eastDisplacement;
        }

        private void SetNorthing(IPoint topStation, IPoint bottomStation)
        {
            double northDspalcement = this.measuredDepth * this.ratioFactor *
                (Math.Sin(this.topInclinationRadians) * Math.Cos(this.angleConverter.ConvertAngleToRadians(topStation.Azimuth))
                + Math.Sin(this.bottomInclinationRadians) * Math.Cos(this.angleConverter.ConvertAngleToRadians(bottomStation.Azimuth)));

            bottomStation.Northing = topStation.Northing + northDspalcement;
        }

        private void SetElevation(IPoint topStation, IPoint bottomStation)
        {
            double elevationCahnge = this.measuredDepth * this.ratioFactor *
                (Math.Cos(this.topInclinationRadians) + Math.Cos(this.bottomInclinationRadians));

            bottomStation.Elevation = topStation.Elevation - elevationCahnge;
        }

        private void SetCalculationParameterst(IPoint topStation, IPoint bottomStation)
        {
            this.topInclinationRadians =
              this.angleConverter.ConvertAngleToRadians(this.angleConverter.ConverDipToInclination(topStation.Dip));
            this.bottomInclinationRadians =
                this.angleConverter.ConvertAngleToRadians(this.angleConverter.ConverDipToInclination(bottomStation.Dip));

            this.measuredDepth = (bottomStation.Depth - topStation.Depth) / 2.0;
            this.ratioFactor = this.curveCalculator.GetRatioFactor(topStation, bottomStation);
        }
    }
}
