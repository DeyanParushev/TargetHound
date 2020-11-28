namespace TargetHound.Calcualtions
{
    using System;
    using System.Collections.Generic;

    using TargetHound.DataModels;
    using TargetHound.DataModels.Interfaces;

    public class Extrapolator
    {
        private CoordinatesSetter coordinatesSetter;
        private const double StationSeparationDistance = 30.0;
        private const double VerticalDip = -90;
        private const double FullCircle = 360;
        private double extrapolationLength;

        public Extrapolator(double extrapolation = 4000)
        {
            this.coordinatesSetter = new CoordinatesSetter();
            this.extrapolationLength = extrapolation;
        }

        public ICollection<IPoint> GetStraightExtrapolation(
            double startEasting,
            double startNorthing,
            double startElevation,
            double startAzimuth,
            double startDip)
        {
            int extrapolationPointsCount = (int)Math.Ceiling((extrapolationLength / 30.0));
            double endSectionLength = extrapolationLength % 30.0;
            List<IPoint> extrapolation = new List<IPoint>(extrapolationPointsCount + 1);

            for (int i = 0; i < extrapolation.Capacity - 1; i++)
            {
                IPoint surveyStation = new SurveyPoint();

                if (i == 0)
                {
                    surveyStation.Easting = startEasting;
                    surveyStation.Northing = startNorthing;
                    surveyStation.Elevation = startElevation;
                    surveyStation.Depth = 0;
                    surveyStation.Azimuth = startAzimuth;
                    surveyStation.Dip = startDip;
                }
                else
                {
                    surveyStation.Depth = extrapolation[i - 1].Depth + 30;
                    surveyStation.Azimuth = startAzimuth;
                    surveyStation.Dip = startDip;

                    this.coordinatesSetter.SetBottomStationUTMCoortinates(extrapolation[i - 1], surveyStation);
                }

                extrapolation.Add(surveyStation);
            }

            IPoint lastStation = new SurveyPoint
            {
                Depth = extrapolation[extrapolation.Count - 1].Depth + (endSectionLength == 0 ? 30 : endSectionLength),
                Azimuth = startAzimuth,
                Dip = startDip,
            };

            this.coordinatesSetter.SetBottomStationUTMCoortinates(extrapolation[extrapolation.Count - 1], lastStation);
            extrapolation.Add(lastStation);

            return extrapolation;
        }

        public IList<IPoint> GetStraightExtrapolation(IPoint collar, double extrapolationLength)
        {
            int extrapolationPointsCount = (int)Math.Ceiling((extrapolationLength / StationSeparationDistance));
            double endSectionLength = extrapolationLength % StationSeparationDistance;
            List<IPoint> extrapolation = new List<IPoint>(extrapolationPointsCount + 1);

            for (int i = 0; i < extrapolationPointsCount; i++)
            {
                IPoint surveyStation = new SurveyPoint();

                if (i == 0)
                {
                    surveyStation.Easting = collar.Easting;
                    surveyStation.Northing = collar.Northing;
                    surveyStation.Elevation = collar.Elevation;
                    surveyStation.Depth = 0;
                    surveyStation.Azimuth = collar.Azimuth;
                    surveyStation.Dip = collar.Dip;
                }
                else
                {
                    surveyStation.Depth = extrapolation[i - 1].Depth + 30;
                    surveyStation.Azimuth = extrapolation[i - 1].Azimuth;
                    surveyStation.Dip = extrapolation[i - 1].Dip;

                    this.coordinatesSetter.SetBottomStationUTMCoortinates(extrapolation[i - 1], surveyStation);
                }

                extrapolation.Add(surveyStation);
            }

            IPoint lastStation = new SurveyPoint
            {
                Depth = extrapolation[extrapolation.Count - 1].Depth + (endSectionLength == 0 ? 30 : endSectionLength),
                Azimuth = extrapolation[extrapolation.Count - 1].Azimuth,
                Dip = extrapolation[extrapolation.Count - 1].Dip,
            };

            this.coordinatesSetter.SetBottomStationUTMCoortinates(extrapolation[extrapolation.Count - 1], lastStation);
            extrapolation.Add(lastStation);

            return extrapolation;
        }

        public ICollection<IPoint> GetCurvedExtrapolaton(
            double startEasting,
            double startNorthing,
            double startElevation,
            double startAzimuth,
            double startDip,
            double azimuthChange,
            double dipChange)
        {
            int extrapolationPointsCount = (int)Math.Ceiling((extrapolationLength / StationSeparationDistance));
            double endSectionLength = extrapolationLength % StationSeparationDistance;
            List<IPoint> extrapolation = new List<IPoint>(extrapolationPointsCount + 1);

            for (int i = 0; i < extrapolation.Capacity - 1; i++)
            {
                IPoint surveyStation = new SurveyPoint();

                if (i == 0)
                {
                    surveyStation.Easting = startEasting;
                    surveyStation.Northing = startNorthing;
                    surveyStation.Elevation = startElevation;
                    surveyStation.Depth = 0;
                    surveyStation.Azimuth = startAzimuth;
                    surveyStation.Dip = startDip;
                }
                else
                {
                    surveyStation.Depth = extrapolation[i - 1].Depth + 30;
                    surveyStation.Azimuth = startAzimuth + azimuthChange;
                    surveyStation.Dip = startDip + dipChange;

                    this.coordinatesSetter.SetBottomStationUTMCoortinates(extrapolation[i - 1], surveyStation);
                }

                if (surveyStation.Dip < -90)
                {
                    surveyStation.Dip += Math.Abs(2 * dipChange);
                    surveyStation.Azimuth += FullCircle / 2;
                    dipChange = Math.Abs(dipChange);
                }

                if (surveyStation.Azimuth > FullCircle)
                {
                    surveyStation.Azimuth -= FullCircle;
                }

                if(surveyStation.Azimuth < 0)
                {
                    surveyStation.Azimuth += FullCircle;
                }

                extrapolation.Add(surveyStation);
            }

            IPoint lastStation = new SurveyPoint
            {
                Depth = extrapolation[extrapolation.Count - 1].Depth + (endSectionLength == 0 ? 30 : endSectionLength),
                Azimuth = extrapolation[extrapolation.Count - 1].Azimuth + azimuthChange,
                Dip = extrapolation[extrapolation.Count - 1].Dip + dipChange,
            };

            this.coordinatesSetter.SetBottomStationUTMCoortinates(extrapolation[extrapolation.Count - 1], lastStation);
            extrapolation.Add(lastStation);

            return extrapolation;
        }

        public IList<IPoint> GetCurvedExtrapolaton(
            IPoint collar,
            double azimuthChange,
            double dipChange)
        {
            int extrapolationPointsCount = (int)Math.Ceiling((extrapolationLength / StationSeparationDistance));
            double endSectionLength = extrapolationLength % StationSeparationDistance;
            List<IPoint> extrapolation = new List<IPoint>(extrapolationPointsCount + 1);

            for (int i = 0; i < extrapolation.Capacity - 1; i++)
            {
                IPoint surveyStation = new SurveyPoint();

                if (i == 0)
                {
                    surveyStation.Easting = collar.Easting;
                    surveyStation.Northing = collar.Northing;
                    surveyStation.Elevation = collar.Elevation;
                    surveyStation.Depth = 0;
                    surveyStation.Azimuth = collar.Azimuth;
                    surveyStation.Dip = collar.Dip;
                }
                else
                {
                    surveyStation.Depth = extrapolation[i - 1].Depth + 30;
                    surveyStation.Azimuth = extrapolation[i - 1].Azimuth + azimuthChange;
                    surveyStation.Dip = extrapolation[i - 1].Dip + dipChange;

                    this.coordinatesSetter.SetBottomStationUTMCoortinates(extrapolation[i - 1], surveyStation);
                }

                if(surveyStation.Dip < -90)
                {
                    surveyStation.Dip += Math.Abs(2 * dipChange);
                    surveyStation.Azimuth += FullCircle / 2;
                    dipChange = Math.Abs(dipChange);
                }

                if (surveyStation.Azimuth > FullCircle)
                {
                    surveyStation.Azimuth -= FullCircle;
                }

                if(surveyStation.Azimuth < 0)
                {
                    surveyStation.Azimuth += FullCircle;
                }

                extrapolation.Add(surveyStation);
            }

            IPoint lastStation = new SurveyPoint
            {
                Depth = extrapolation[extrapolation.Count - 1].Depth + (endSectionLength == 0 ? 30 : endSectionLength),
                Azimuth = extrapolation[extrapolation.Count - 1].Azimuth + azimuthChange,
                Dip = extrapolation[extrapolation.Count - 1].Dip + dipChange,
            };

            this.coordinatesSetter.SetBottomStationUTMCoortinates(extrapolation[extrapolation.Count - 1], lastStation);
            extrapolation.Add(lastStation);

            return extrapolation;
        }
    }
}
