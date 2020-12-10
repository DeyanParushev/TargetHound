namespace TargetHound.Calcualtions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TargetHound.DTOs;

    public class CurveExtrapolationCalculator
    {
        private IList<IPoint> borehole;
        private IPoint target;
        private readonly double azimuthChange;
        private readonly double dipChange;
        private StraightExtrapolationCalculator straightCalculator;
        private readonly CoordinatesSetter coordinatesSetter;
        private readonly Extrapolator extrapolator;
        private IPoint sameVectorLengthPoint;

        public CurveExtrapolationCalculator(IList<IPoint> borehole, IPoint target)
        {
            this.borehole = borehole;
            this.target = target;
        }

        public CurveExtrapolationCalculator(
            StraightExtrapolationCalculator straightExtrapolationCalculator,
            CoordinatesSetter coordinatesSetter,
            Extrapolator extrapolator,
            IPoint collar,
            IPoint target,
            double azimuthChange,
            double dipChange)
        {
            this.target = target;
            this.straightCalculator = straightExtrapolationCalculator;
            this.coordinatesSetter = coordinatesSetter;
            this.extrapolator = extrapolator;
            if (collar.Dip == 0 && collar.Azimuth == 0)
            {
                collar.Azimuth = this.straightCalculator.GetInitialAzimuthAngle(collar, target);
                collar.Dip = this.straightCalculator.GetInitialDipAngle(collar, target);
            }

            this.borehole = extrapolator.GetCurvedExtrapolaton(collar, azimuthChange, dipChange);
            this.dipChange = dipChange;
            this.azimuthChange = azimuthChange;
            this.sameVectorLengthPoint = this.GetSameVectorLengthPoint();
        }

        public double GetInitialAzimuth()
        {
            double collarAzimuth = this.straightCalculator.GetInitialAzimuthAngle(this.borehole[0], this.sameVectorLengthPoint);
            return collarAzimuth;
        }

        public double GetDistance()
        {
            double vectorLength = this.straightCalculator.GetStraightHoleLength(this.borehole[0], this.sameVectorLengthPoint);
            return vectorLength;
        }

        public double GetInitialDip()
        {
            double collarDip = this.straightCalculator.GetInitialDipAngle(this.borehole[0], this.sameVectorLengthPoint);
            return collarDip;
        }

        private IPoint GetSameVectorLengthPoint()
        {
            double straightLineToTarget = this.straightCalculator.GetStraightHoleLength(this.borehole[0], this.target);
            IPoint startStation =
                borehole.LastOrDefault(x =>
                    this.straightCalculator.GetStraightHoleLength(this.borehole[0], x) <= straightLineToTarget);
            IPoint endStation =
                borehole.FirstOrDefault(x =>
                    this.straightCalculator.GetStraightHoleLength(this.borehole[0], x) >= straightLineToTarget);

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


            this.coordinatesSetter.SetBottomStationUTMCoortinates(startStation, midStation);


            while (Math.Abs(depthChange) > 0.001)
            {
                double straightDistanceToMidPoitn = this.straightCalculator.GetStraightHoleLength(this.borehole[0], midStation);
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
                straightDistanceToMidPoitn = this.straightCalculator.GetStraightHoleLength(this.borehole[0], midStation);
            }

            return midStation;
        }

        // TODO: finish the algorithm for curve boreholes
        private void FindInitialAzimuthAndDip(IPoint collar, IPoint target)
        {
            double initialAzimuthToSameVector =
                this.straightCalculator.GetInitialAzimuthAngle(collar, this.sameVectorLengthPoint);
            double initialDipToSameVector =
                this.straightCalculator.GetInitialDipAngle(collar, this.sameVectorLengthPoint);

            double straightAzimuth = this.straightCalculator.GetInitialAzimuthAngle(collar, target);
            double straightDip = this.straightCalculator.GetInitialDipAngle(collar, target);

            double collarDipChange = straightDip - initialDipToSameVector;
            double collarAzimuthChange = straightAzimuth - initialAzimuthToSameVector;


            while (true)
            {
                collar.Azimuth = straightAzimuth - collarAzimuthChange;
                collar.Dip = straightDip - collarDipChange;

                this.extrapolator.GetCurvedExtrapolaton(collar, this.azimuthChange, this.dipChange);

            }
        }
    }
}
