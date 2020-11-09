namespace TargetHound.Calcualtions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
   
    using TargetHound.Models;
    using TargetHound.Models.Interfaces;

    public class _3DDistanceCalculator : PlaneDistanceCalculator
    {
        private IPoint closestSpacialPoint;

        public _3DDistanceCalculator(IList<IPoint> borehole, IPoint target)
            : base(borehole, target)
        {
            this.ClosestSpacialPoint();
        }

        public IPoint ClosestSpacialPoint()
        {
            IPoint startStation = base.borehole.FirstOrDefault(x =>
                this.LengthOfA3DLine(x, base.target) == base.borehole.Min(y => this.LengthOfA3DLine(y, base.target)));
            int indexOfStartStation = borehole.IndexOf(startStation);

            IPoint endStation = this.GetNextNearestStation(base.borehole, base.target, indexOfStartStation);

            if (endStation == null)
            {
                this.closestHorizontalPoint = startStation;
            }

            if (startStation.Depth > endStation.Depth)
            {
                IPoint temp = new SurveyPoint
                {
                    Depth = startStation.Depth,
                    Azimuth = startStation.Azimuth,
                    Dip = startStation.Dip,
                    Easting = startStation.Easting,
                    Northing = startStation.Northing,
                    Elevation = startStation.Elevation,
                };

                startStation = new SurveyPoint
                {
                    Depth = endStation.Depth,
                    Azimuth = endStation.Azimuth,
                    Dip = endStation.Dip,
                    Easting = endStation.Easting,
                    Northing = endStation.Northing,
                    Elevation = endStation.Elevation
                };

                endStation = temp;
            }

            double depthChange = endStation.Depth - startStation.Depth;
            double totalAzimuthChange = endStation.Azimuth - startStation.Azimuth;
            double totalDipChange = endStation.Dip - startStation.Dip;

            double dipChangePerMeter = totalDipChange / depthChange;
            double azimuthChangePerMeter = totalAzimuthChange / depthChange;

            IPoint middleStation = new SurveyPoint();
            middleStation.Depth = startStation.Depth + (depthChange / 2);
            middleStation.Azimuth = startStation.Azimuth + (azimuthChangePerMeter * (depthChange / 2));
            middleStation.Dip = startStation.Dip + (dipChangePerMeter * (depthChange / 2));

            this.coordinateSetter.SetBottomStationUTMCoortinates(startStation, middleStation);

            while (Math.Abs(depthChange) > 0.001)
            {
                double startStationDistance = this.LengthOfA3DLine(startStation, target);
                double endStationDistance = this.LengthOfA3DLine(endStation, target);

                if (startStationDistance < endStationDistance)
                {
                    depthChange = middleStation.Depth - startStation.Depth;
                    endStation = middleStation;
                }
                else if (startStationDistance > endStationDistance)
                {
                    depthChange = endStation.Depth - middleStation.Depth;
                    startStation = middleStation;
                }
                else
                {
                    break;
                }

                middleStation = new SurveyPoint();
                middleStation.Depth = startStation.Depth + (depthChange / 2);
                middleStation.Azimuth = startStation.Azimuth + (azimuthChangePerMeter * (depthChange / 2));
                middleStation.Dip = startStation.Dip + (dipChangePerMeter * (depthChange / 2));

                base.coordinateSetter.SetBottomStationUTMCoortinates(startStation, middleStation);
            }

            this.closestSpacialPoint = middleStation;
            return middleStation;
        }

        public double GetMinimumSpacialDistance()
        {
            return this.LengthOfA3DLine(this.closestSpacialPoint, base.target);
        }

        public double LengthOfA3DLine(IPoint startPoint, IPoint endPoint)
        {
            double lineLength = Math.Sqrt(
                Math.Pow(startPoint.Easting - endPoint.Easting, 2) +
                Math.Pow(startPoint.Northing - endPoint.Northing, 2) +
                Math.Pow(startPoint.Elevation - endPoint.Elevation, 2));

            return lineLength;
        }

        protected override IPoint GetNextNearestStation(IList<IPoint> borehole, IPoint target, int indexOfNearestPoint)
        {
            IList<IPoint> boreholeCopy = borehole.Select(x => x).ToList();
            boreholeCopy.RemoveAt(indexOfNearestPoint);

            IPoint nextNearestPoint = boreholeCopy.FirstOrDefault(
                x => this.LengthOfA3DLine(x, target) ==
                boreholeCopy.Min(y => this.LengthOfA3DLine(y, target)));

            return nextNearestPoint;
        }
    }
}
