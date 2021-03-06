﻿namespace TargetHound.Calcualtions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
   
    using TargetHound.DTOs;

    public class PlaneDistanceCalculator
    {
        protected CoordinatesSetter coordinateSetter;
        protected IList<SurveyPointDTO> borehole;
        protected IPoint target;
        protected IPoint pointOnTargetElevation;
        protected IPoint closestHorizontalPoint;

        public PlaneDistanceCalculator(IList<SurveyPointDTO> borehole, IPoint target, CoordinatesSetter coordinatesSetter)
        {
            this.coordinateSetter = coordinatesSetter;
            this.target = target;
            this.borehole = borehole;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.SetClosestHorizontalPointAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.SetPointOnTargetElevationAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public double GetHorizontalDistanceOnTargetElevation()
        {
            double horizontalDistanceOnTargetElevation = this.GetPlaneLineBetweenPoints(this.pointOnTargetElevation, this.target);

            return horizontalDistanceOnTargetElevation;
        }

        public double GetMinimumHorizontalDistance()
        {
            double minimuHorizontalDistance = this.GetPlaneLineBetweenPoints(this.closestHorizontalPoint, this.target);

            return minimuHorizontalDistance;
        }

        public double GetVerticalDistance()
        {
            double verticalDistance = this.closestHorizontalPoint.Elevation - this.target.Elevation;

            return verticalDistance;
        }

        protected virtual SurveyPointDTO GetNextNearestStation(IList<SurveyPointDTO> borehole, IPoint target, int indexOfNearestPoint)
        {
            IList<SurveyPointDTO> boreholeCopy = borehole.Select(x => x).ToList();
            boreholeCopy.RemoveAt(indexOfNearestPoint);

            SurveyPointDTO nextNearestPoint = boreholeCopy.FirstOrDefault(
                x => this.GetPlaneLineBetweenPoints(x, target) ==
                boreholeCopy.Min(y => this.GetPlaneLineBetweenPoints(y, target)));

            return nextNearestPoint;
        }

        private async Task SetPointOnTargetElevationAsync()
        {
            IPoint startPoint = new SurveyPointDTO();
            IPoint endPoint = new SurveyPointDTO();

            bool isPositivelyInclined = this.borehole[0].Elevation < this.target.Elevation;

            if (isPositivelyInclined)
            {
                startPoint = this.borehole.LastOrDefault(x => x.Elevation <= this.target.Elevation);
                endPoint = this.borehole.FirstOrDefault(x => x.Elevation >= this.target.Elevation);
            }
            else
            {
                startPoint = this.borehole.LastOrDefault(x => x.Elevation >= this.target.Elevation);
                endPoint = this.borehole.FirstOrDefault(x => x.Elevation <= this.target.Elevation);
            }

            if (startPoint == null)
            {
                throw new ArgumentException("Borehole does not pass target elevation!");
            }

            if (endPoint == null)
            {
                throw new ArgumentException("Borehole does not pass target elevation!");
            }

            double totalAzimuthChange = startPoint.Azimuth - endPoint.Azimuth;
            double totalDipChange = startPoint.Dip - endPoint.Dip;
            double totalDepthChange = endPoint.Depth - startPoint.Depth;

            double azimuthChangePerMeter = totalAzimuthChange / totalDepthChange;
            double dipChangePerMeter = totalDipChange / totalDepthChange;

            double elevationFromStartToTarget = startPoint.Elevation - this.target.Elevation;

            if (isPositivelyInclined)
            {
                elevationFromStartToTarget = this.target.Elevation - startPoint.Elevation;
            }

            IPoint nextStation = new SurveyPointDTO
            {
                Depth = startPoint.Depth + elevationFromStartToTarget,
                Azimuth = startPoint.Azimuth + azimuthChangePerMeter * elevationFromStartToTarget,
                Dip = startPoint.Dip + dipChangePerMeter * elevationFromStartToTarget
            };

            this.coordinateSetter.SetBottomStationUTMCoortinates(startPoint, nextStation);

            while (Math.Abs(elevationFromStartToTarget) > 0.01)
            {
                elevationFromStartToTarget = nextStation.Elevation - this.target.Elevation;

                if (isPositivelyInclined)
                {
                    elevationFromStartToTarget = this.target.Elevation - nextStation.Elevation;
                }

                IPoint tempStation = new SurveyPointDTO
                {
                    Depth = nextStation.Depth + elevationFromStartToTarget,
                    Azimuth = nextStation.Azimuth + (azimuthChangePerMeter * elevationFromStartToTarget),
                    Dip = nextStation.Dip + (dipChangePerMeter * elevationFromStartToTarget),
                };

                this.coordinateSetter.SetBottomStationUTMCoortinates(nextStation, tempStation);

                nextStation = tempStation;
            }

            this.pointOnTargetElevation = nextStation;
        }

        private async Task SetClosestHorizontalPointAsync()
        {
            SurveyPointDTO startStation = this.borehole.FirstOrDefault(x =>
            this.GetPlaneLineBetweenPoints(x, this.target) == this.borehole.Min(y => this.GetPlaneLineBetweenPoints(y, this.target)));
            int indexOfStartStation = this.borehole.IndexOf(startStation);

            SurveyPointDTO endStation = this.GetNextNearestStation(this.borehole, this.target, indexOfStartStation);

            if (endStation == null)
            {
                this.closestHorizontalPoint = startStation;
            }

            if (startStation.Depth > endStation.Depth)
            {
                SurveyPointDTO temp = new SurveyPointDTO
                {
                    Depth = startStation.Depth,
                    Azimuth = startStation.Azimuth,
                    Dip = startStation.Dip,
                    Easting = startStation.Easting,
                    Northing = startStation.Northing,
                    Elevation = startStation.Elevation,
                };

                startStation = new SurveyPointDTO
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
            double totalDipChange = startStation.Dip - endStation.Dip;

            double dipChangePerMeter = totalDipChange / depthChange;
            double azimuthChangePerMeter = totalAzimuthChange / depthChange;

            SurveyPointDTO middleStation = new SurveyPointDTO();
            middleStation.Depth = startStation.Depth + (depthChange / 2);
            middleStation.Azimuth = startStation.Azimuth + (azimuthChangePerMeter * (depthChange / 2));
            middleStation.Dip = startStation.Dip + (dipChangePerMeter * (depthChange / 2));

            this.coordinateSetter.SetBottomStationUTMCoortinates(startStation, middleStation);

            while (Math.Abs(depthChange) > 0.001)
            {
                double startStationDistance = this.GetPlaneLineBetweenPoints(startStation, this.target);
                double endStationDistance = this.GetPlaneLineBetweenPoints(endStation, this.target);

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
                    startStation = middleStation;
                    break;
                }

                middleStation = new SurveyPointDTO();
                middleStation.Depth = startStation.Depth + (depthChange / 2.0);
                middleStation.Azimuth = startStation.Azimuth + (azimuthChangePerMeter * (depthChange / 2));
                middleStation.Dip = startStation.Dip + (dipChangePerMeter * (depthChange / 2));

                this.coordinateSetter.SetBottomStationUTMCoortinates(startStation, middleStation);
            }

            this.closestHorizontalPoint = middleStation;
        }

        private double GetPlaneLineBetweenPoints(IPoint firstStation, IPoint secondStation)
        {
            double horizontalDistance =
                Math.Sqrt(Math.Pow(firstStation.Easting - secondStation.Easting, 2) + Math.Pow(firstStation.Northing - secondStation.Northing, 2));

            return horizontalDistance;
        }
    }
}
