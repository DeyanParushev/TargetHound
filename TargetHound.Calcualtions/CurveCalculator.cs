namespace TargetHound.Calcualtions
{
    using System;
    using TargetHound.DTOs;

    public class CurveCalculator
    {
        private AngleConverter angleConverter;

        public CurveCalculator()
        {
            this.angleConverter = new AngleConverter();
        }

        public double GetDoglegAngle(IPoint topStation, IPoint bottomStation)
        {
            if(this.IsStraightSection(topStation, bottomStation))
            {
                return 0;
            }

            SurveyPointDTO preparedTopStation = this.PrepareAngles(topStation);
            SurveyPointDTO preparedBottomStation = this.PrepareAngles(bottomStation);

            // acos(cos(I2 – I1) – sinI1 sinI2 (1-cos(A2-A1 DL acos( cos(I ))) 
            double doglegInRadians = Math.Acos(
                Math.Sin(preparedTopStation.Dip) * Math.Sin(preparedBottomStation.Dip)
                * Math.Cos(preparedBottomStation.Azimuth - preparedTopStation.Azimuth)
                + Math.Cos(preparedTopStation.Dip) * Math.Cos(preparedBottomStation.Dip));

            double dogleg = this.angleConverter.ConvertRadiansToAngle(doglegInRadians);
            return dogleg;
        }

        public double GetDoglegSeverity(IPoint topStation, IPoint bottomStation)
        {
            if (this.IsStraightSection(topStation, bottomStation))
            {
                return 0;
            }

            double dogleg = this.GetDoglegAngle(topStation, bottomStation);
            return (dogleg / (bottomStation.Depth - topStation.Depth)) * 30;
        }

        public double GetToolFace(IPoint topStation, IPoint bottomStation)
        {
            if(topStation.Dip == 90 || topStation.Dip == -90)
            {
                return bottomStation.Azimuth;
            }

            SurveyPointDTO preparedTopStation = this.PrepareAngles(topStation);
            SurveyPointDTO preparedBottomStation = this.PrepareAngles(bottomStation);

            double doglegAngle = this.GetDoglegAngle(topStation, bottomStation);
            double doglegInRadians = this.angleConverter.ConvertAngleToRadians(doglegAngle);

            //double topEquationPart =
            //    Math.Sin(preparedBottomStation.Dip) - (Math.Sin(preparedTopStation.Dip) * Math.Cos(doglegInRadians));
            //double bottomEquationPart = Math.Cos(preparedTopStation.Dip) * Math.Sin(doglegInRadians);
            
            // Working Excel formulae
            double topEquationPart = Math.Cos(preparedTopStation.Dip) * Math.Cos(doglegInRadians)
                - Math.Cos(preparedBottomStation.Dip);

            double bottomEquationPart = Math.Sin(preparedTopStation.Dip) * Math.Sin(doglegInRadians);

            double toolFaceInRadians = Math.Acos(Math.Round(topEquationPart / bottomEquationPart, 10));
            double toolFaceInAngle = this.angleConverter.ConvertRadiansToAngle(toolFaceInRadians);

            if(toolFaceInAngle == 360)
            {
                toolFaceInAngle = 0;
                return toolFaceInAngle;
            }

            if(this.IsLeftTurn(topStation, bottomStation))
            {
                if(this.AzimuthChangeIsGreaterThanHalfTurn(topStation, bottomStation))
                {
                    return toolFaceInAngle;
                }
                else
                {
                    return 360 - toolFaceInAngle;
                }
            }
            else
            {
                if(this.AzimuthChangeIsGreaterThanHalfTurn(topStation, bottomStation))
                {
                    return 360 - toolFaceInAngle;
                }
                else
                {
                    return toolFaceInAngle;
                }
            }
        }

        public double GetRatioFactor(IPoint topStation, IPoint bottomStation)
        {
            if(this.IsStraightSection(topStation, bottomStation))
            {
                return 1;
            }

            double doglegAngle = this.GetDoglegAngle(topStation, bottomStation);
            double doglegRadians = this.angleConverter.ConvertAngleToRadians(doglegAngle);

            double ratioFactor = (2 * Math.Tan(this.angleConverter.ConvertAngleToRadians(doglegAngle / 2))) / doglegRadians;
            return ratioFactor;
        }

        private SurveyPointDTO PrepareAngles(IPoint point)
        {
            SurveyPointDTO convertedPoint = new SurveyPointDTO();
            convertedPoint.Dip = this.angleConverter.ConvertAngleToRadians(
                this.angleConverter.ConverDipToInclination(point.Dip));

            convertedPoint.Azimuth = this.angleConverter.ConvertAngleToRadians(point.Azimuth);

            return convertedPoint;
        }

        private bool AzimuthChangeIsGreaterThanHalfTurn(IPoint topStation, IPoint bottomStation)
        {
            return Math.Abs(topStation.Azimuth - bottomStation.Azimuth) > 180;
        }

        private bool IsLeftTurn(IPoint topStation, IPoint bottomStation)
        {
            return bottomStation.Azimuth < topStation.Azimuth;
        }

        private bool IsStraightSection(IPoint topStation, IPoint bottomStation)
        {
            return topStation.Azimuth == bottomStation.Azimuth &&  topStation.Dip == bottomStation.Dip;
        }
    }
}
