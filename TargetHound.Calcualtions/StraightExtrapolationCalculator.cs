namespace TargetHound.Calcualtions
{
    using System;

    using TargetHound.DTOs;

    public class StraightExtrapolationCalculator
    {
        private AngleConverter angleConverter;

        public StraightExtrapolationCalculator(AngleConverter angleConverter)
        {
            this.angleConverter = angleConverter;
        }

        public double GetStraightHoleLength(IPoint startPoint, IPoint endPoint)
        {
            double eastingDifference = this.GetCoordinateDifference(startPoint.Easting, endPoint.Easting);
            double northingDifference = this.GetCoordinateDifference(startPoint.Northing, endPoint.Northing);
            double elevationDifference = this.GetCoordinateDifference(startPoint.Elevation, endPoint.Elevation);

            double holeLength = Math.Sqrt(
                Math.Pow(eastingDifference, 2) +
                Math.Pow(northingDifference, 2) +
                Math.Pow(elevationDifference, 2));

            return holeLength;
        }

        public double GetInitialAzimuthAngle(IPoint collar, IPoint target)
        {
            if (this.IsVertical(collar, target))
            {
                return double.NaN;
            }

            double northingDifference = this.GetCoordinateDifference(collar.Northing, target.Northing);
            double eastingDifference = this.GetCoordinateDifference(collar.Easting, target.Easting);

            double hypothenuse = Math.Sqrt(
                Math.Pow(northingDifference, 2) + Math.Pow(eastingDifference, 2));

            double azimuth = Math.Acos(northingDifference / hypothenuse);
            azimuth = this.angleConverter.ConvertRadiansToAngle(azimuth);

            if (this.IsInSecondQuadrant(collar, target))
            {
                return 180 - azimuth;
            }
            else if (this.IsInThirdQuadrant(collar, target))
            {
                return 180 + azimuth;
            }
            else if (this.IsInForthQuadrant(collar, target))
            {
                return 360 - azimuth;
            }

            return azimuth;
        }

        public double GetInitialDipAngle(IPoint collar, IPoint target)
        {
            double eastingDifference = this.GetCoordinateDifference(collar.Easting, target.Easting);
            double northingDifference = this.GetCoordinateDifference(collar.Northing, target.Northing);

            double hypothenuseInHorizontalPlane = Math.Sqrt(
                Math.Pow(eastingDifference, 2) +
                Math.Pow(northingDifference, 2));

            double dipRadians = Math.Acos(hypothenuseInHorizontalPlane / this.GetStraightHoleLength(collar, target));
            double dipAngle = this.angleConverter.ConvertRadiansToAngle(dipRadians);

            if (this.IsBellowHorozontal(collar, target))
            {
                return 0 - dipAngle;
            }

            return dipAngle;
        }

        private double GetCoordinateDifference(double topStationCoordinate, double bottomStationCoordinate)
        {
            return Math.Abs(topStationCoordinate - bottomStationCoordinate);
        }

        private bool IsInSecondQuadrant(IPoint collar, IPoint target)
        {
            return collar.Northing > target.Northing && collar.Easting < target.Easting;
        }

        private bool IsInThirdQuadrant(IPoint collar, IPoint target)
        {
            return collar.Northing > target.Northing && collar.Easting > target.Easting;
        }

        private bool IsInForthQuadrant(IPoint collar, IPoint target)
        {
            return collar.Northing < target.Northing && collar.Easting > target.Easting;
        }

        private bool IsVertical(IPoint collar, IPoint target)
        {
            return collar.Northing == target.Northing && collar.Easting == target.Easting;
        }

        private bool IsBellowHorozontal(IPoint collar, IPoint target)
        {
            return collar.Elevation > target.Elevation;
        }
    }
}
