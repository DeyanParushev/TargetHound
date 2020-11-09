namespace TargetHound.Calcualtions
{
    using System;

    public class AngleConverter
    {
        public double ConvertAngleToRadians(double angleInDegrees)
        {
            return angleInDegrees * (Math.PI / 180);
        }

        public double ConvertRadiansToAngle(double angleInRadians)
        {
            return angleInRadians * (180 / Math.PI);
        }

        public double ConverDipToInclination(double dipAngle)
        {
            return 90 + dipAngle;
        }

        public double ConvertInclinationToDip(double inclinationAngle)
        {
            return inclinationAngle - 90;
        }
    }
}
