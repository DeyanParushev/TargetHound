namespace TargetHound.Calcualtions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TargetHound.DataModels;
    using TargetHound.DataModels.Interfaces;

    public class CurveExtrapolationCalculator
    {
        private IList<IPoint> borehole;
        private IPoint target;
        private readonly double azimuthChange;
        private readonly double dipChange;
        private StraightExtrapolationCalculator straightCalculator;

        public CurveExtrapolationCalculator(IList<IPoint> borehole, IPoint target)
        {
            this.borehole = borehole;
            this.target = target;
        }

        public CurveExtrapolationCalculator(IPoint collar, IPoint target, double azimuthChange, double dipChange)
        {
            this.target = target;
            this.straightCalculator = new StraightExtrapolationCalculator();

            if (collar.Dip == 0 && collar.Azimuth == 0)
            {
                collar.Azimuth = this.straightCalculator.GetInitialAzimuthAngle(collar, target);
                collar.Dip = this.straightCalculator.GetInitialDipAngle(collar, target);
            }

            Extrapolator extrapolator = new Extrapolator();
            this.borehole = extrapolator.GetCurvedExtrapolaton(collar, azimuthChange, dipChange);
            this.dipChange = dipChange;
            this.azimuthChange = azimuthChange;
        }

        public double GetInitialAzimuth()
        {
            IPoint sameVectorLengthPoint = this.GetSameVectorLengthPoint();

            double collarAzimuth = this.straightCalculator.GetInitialAzimuthAngle(this.borehole[0], sameVectorLengthPoint);
            return collarAzimuth;
        }

        public double GetDistance()
        {
            IPoint sameVectorLengthPoint = this.GetSameVectorLengthPoint();

            double vectorLength = this.straightCalculator.GetStraightHoleLength(this.borehole[0], sameVectorLengthPoint);
            return vectorLength;
        }

        public double GetInitialDip()
        {
            IPoint sameVectorLengthPoint = this.GetSameVectorLengthPoint();

            double collarDip = this.straightCalculator.GetInitialDipAngle(this.borehole[0], sameVectorLengthPoint);
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

            IPoint midStation = new SurveyPoint
            {
                Depth = startStation.Depth + (depthChange / 2),
                Azimuth = startStation.Azimuth + (azimuthChangePerMeter * (depthChange / 2)),
                Dip = startStation.Dip + (dipChangePerMeter * (depthChange / 2)),
            };

            CoordinatesSetter coordinatesSetter = new CoordinatesSetter();
            coordinatesSetter.SetBottomStationUTMCoortinates(startStation, midStation);


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

                midStation = new SurveyPoint();
                midStation.Depth = startStation.Depth + (depthChange / 2);
                midStation.Azimuth = startStation.Azimuth + (azimuthChangePerMeter * (depthChange / 2));
                midStation.Dip = startStation.Dip + (dipChangePerMeter * (depthChange / 2));

                coordinatesSetter.SetBottomStationUTMCoortinates(startStation, midStation);
                straightDistanceToMidPoitn = this.straightCalculator.GetStraightHoleLength(this.borehole[0], midStation);
            }

            return midStation;
        }
    }
}
