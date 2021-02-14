namespace TargetHound.Calcualtions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TargetHound.DTOs;

    public class CurveExtrapolationCalculator
    {
        private readonly IPoint collar;
        private readonly IPoint target;
        private readonly double azimuthChange;
        private readonly double dipChange;
        private IPoint sameVectorLengthPoint;

        public CurveExtrapolationCalculator(IPoint collar, IPoint target, double azimuthChange, double dipchange)
        {
            this.target = target;
            this.collar = collar;
            this.target = target;
            this.azimuthChange = azimuthChange;
            this.dipChange = dipchange;
        }

        public double GetInitialAzimuth(StraightExtrapolationCalculator straightCalculator, Extrapolator extrapolator)
        {
            var borehole = extrapolator.GetCurvedExtrapolaton(this.collar, this.azimuthChange, this.dipChange);
            double collarAzimuth = straightCalculator.GetInitialAzimuthAngle(borehole[0], this.sameVectorLengthPoint);
            return collarAzimuth;
        }

        //public double GetDistance()
        //{
        //    double vectorLength = this.straightCalculator.GetStraightHoleLength(this.borehole[0], this.sameVectorLengthPoint);
        //    return vectorLength;
        //}

        //public double GetInitialDip()
        //{
        //    double collarDip = this.straightCalculator.GetInitialDipAngle(this.borehole[0], this.sameVectorLengthPoint);
        //    return collarDip;
        //}

        private IPoint GetSameVectorLengthPoint(
            StraightExtrapolationCalculator straightCalculator,
            Extrapolator extrapolator,
            CoordinatesSetter coordinatesSetter)
        {
            if(this.collar.Azimuth == 0 || this.collar.Dip == 0)
            {
                this.collar.Azimuth = straightCalculator.GetInitialAzimuthAngle(this.collar, this.target);
                this.collar.Dip = straightCalculator.GetInitialDipAngle(this.collar, this.target);
            }

            var borehole = extrapolator.GetCurvedExtrapolaton(this.collar, this.azimuthChange, this.dipChange);

            double straightLineToTarget = straightCalculator.GetStraightHoleLength(this.collar, this.target);
            IPoint startStation =
                borehole.LastOrDefault(x =>
                    straightCalculator.GetStraightHoleLength(borehole[0], x) <= straightLineToTarget);
            IPoint endStation =
                borehole.FirstOrDefault(x =>
                    straightCalculator.GetStraightHoleLength(borehole[0], x) >= straightLineToTarget);

            double depthChange = endStation.Depth - startStation.Depth;
            double azimuthChange = startStation.Azimuth - endStation.Azimuth;
            double dipChange = startStation.Dip - endStation.Dip;

            double azimuthChangePerMeter = azimuthChange / depthChange;
            double dipChangePerMeter = dipChange / depthChange;

            IPoint midStation = new SurveyPointDTO
            {
                Depth = startStation.Depth + (depthChange / 2),
                Azimuth = startStation.Azimuth + (azimuthChangePerMeter * (depthChange / 2)),
                Dip = startStation.Dip + (dipChangePerMeter * (depthChange / 2)),
            };

            coordinatesSetter.SetBottomStationUTMCoortinates(startStation, midStation);

            while (Math.Abs(depthChange) > 0.001)
            {
                double straightDistanceToMidPoitn =
                    straightCalculator.GetStraightHoleLength(borehole[0], midStation);

                if (straightDistanceToMidPoitn < straightLineToTarget)
                {
                    depthChange = endStation.Depth - midStation.Depth;
                    startStation = midStation;
                }
                else if (straightDistanceToMidPoitn > straightLineToTarget)
                {
                    depthChange = midStation.Depth - startStation.Depth;
                    endStation = midStation;
                }
                else
                {
                    break;
                }

                midStation = new SurveyPointDTO();
                midStation.Depth = startStation.Depth + (depthChange / 2);
                midStation.Azimuth = startStation.Azimuth + (azimuthChangePerMeter * (depthChange / 2));
                midStation.Dip = startStation.Dip + (dipChangePerMeter * (depthChange / 2));

                coordinatesSetter.SetBottomStationUTMCoortinates(startStation, midStation);
                //straightDistanceToMidPoitn = 
                //    this.straightCalculator.GetStraightHoleLength(this.borehole[0], midStation);
            }

            return midStation;
        }

        // TODO: finish the algorithm for curve boreholes
        public double FindInitialAzimuthAndDip(
            StraightExtrapolationCalculator straightCalculator,
            Extrapolator extrapolator,
            CoordinatesSetter coordinatesSetter)
        {
            double straightAzimuth = straightCalculator.GetInitialAzimuthAngle(this.collar, this.target);
            double straightDip = straightCalculator.GetInitialDipAngle(this.collar, this.target);

            double minimumSpacial = 10;
            CollarDTO newCollar = new CollarDTO
            {
                Easting = this.collar.Easting,
                Northing = this.collar.Northing,
                Elevation = this.collar.Elevation,
                Depth = this.collar.Depth,
            };

            while (minimumSpacial > 0.3)
            {
                this.sameVectorLengthPoint = this.GetSameVectorLengthPoint(
                    straightCalculator, extrapolator, coordinatesSetter);

                double initialAzimuthToSameVector =
                    straightCalculator.GetInitialAzimuthAngle(newCollar, this.sameVectorLengthPoint);
                double initialDipToSameVector =
                    straightCalculator.GetInitialDipAngle(newCollar, this.sameVectorLengthPoint);

                double collarAzimuthChange = straightAzimuth - initialAzimuthToSameVector;
                double collarDipChange = straightDip - initialDipToSameVector;

                this.collar.Azimuth += collarAzimuthChange;
                this.collar.Dip += collarDipChange;

                IList<IPoint> borehole = extrapolator.GetCurvedExtrapolaton(this.collar, this.azimuthChange, this.dipChange);
                _3DDistanceCalculator distanceCalculator = new _3DDistanceCalculator(borehole, this.target, coordinatesSetter);
                minimumSpacial = distanceCalculator.GetMinimumSpacialDistance();
            }

            return minimumSpacial;
        }

        public double Backup(
            StraightExtrapolationCalculator straightCalculator,
            Extrapolator extrapolator,
            CoordinatesSetter coordinatesSetter)
        {
            this.sameVectorLengthPoint = this.GetSameVectorLengthPoint(
                straightCalculator, extrapolator, coordinatesSetter);
            CollarDTO newCollar = new CollarDTO
            {
                Easting = this.collar.Easting,
                Northing = this.collar.Northing,
                Elevation = this.collar.Elevation,
                Azimuth = this.collar.Azimuth,
                Dip = this.collar.Dip,
                Depth = this.collar.Depth,
            };

            double initialAzimuthToSameVector =
                straightCalculator.GetInitialAzimuthAngle(newCollar, this.sameVectorLengthPoint);
            double initialDipToSameVector =
                straightCalculator.GetInitialDipAngle(newCollar, this.sameVectorLengthPoint);

            double straightAzimuth = straightCalculator.GetInitialAzimuthAngle(this.collar, this.target);
            double straightDip = straightCalculator.GetInitialDipAngle(this.collar, this.target);

            double collarAzimuthChange = straightAzimuth - initialAzimuthToSameVector;
            double collarDipChange = straightDip - initialDipToSameVector;

            newCollar.Azimuth = straightAzimuth + collarAzimuthChange;
            newCollar.Dip = straightDip + collarDipChange;

            IList<IPoint> borehole = extrapolator.GetCurvedExtrapolaton(newCollar, this.azimuthChange, this.dipChange);
            _3DDistanceCalculator distanceCalculator = new _3DDistanceCalculator(borehole, this.target, coordinatesSetter);
            double minimumSpacial = distanceCalculator.GetMinimumSpacialDistance();

            return minimumSpacial;
        }
    }
}
