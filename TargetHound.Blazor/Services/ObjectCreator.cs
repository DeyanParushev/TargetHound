namespace TargetHound.Blazor.Services
{
    using System;
    using System.Collections.Generic;
    
    using TargetHound.Calcualtions;
    using TargetHound.DTOs;

    public class ObjectCreator : IObjectCreator
    {
        private readonly StraightExtrapolationCalculator straightExtrapolator;
        private readonly Extrapolator extrapolator;

        public ObjectCreator(
            StraightExtrapolationCalculator straightExtrapolator,
            Extrapolator extrapolator)
        {
            this.straightExtrapolator = straightExtrapolator;
            this.extrapolator = extrapolator;
        }

        public CollarDTO CreateCollar(string name, double easting, double norhing, double elevation, string projectId)
        {
            var collar = new CollarDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Easting = easting,
                Northing = norhing,
                Elevation = elevation,
                ProjectId = projectId,
            };

            return collar;
        }

        public TargetDTO CreateTarget(string name, double easting, double norhing, double elevation, string projectId)
        {
            var target = new TargetDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Easting = easting,
                Northing = norhing,
                Elevation = elevation,
                ProjectId = projectId,
            };

            return target;
        }

        public BoreholeDTO CreateStraightBorehole(string name, CollarDTO collar, TargetDTO target)
        {
            var borehole = new BoreholeDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Collar = collar,
                Target = target,
                ProjectId = collar.ProjectId,
            };

            borehole.Collar.Azimuth = this.straightExtrapolator.GetInitialAzimuthAngle(borehole.Collar, borehole.Target);
            borehole.Collar.Dip = this.straightExtrapolator.GetInitialDipAngle(borehole.Collar, borehole.Target);
            borehole.SurveyPoints = this.extrapolator.GetStraightExtrapolation(borehole.Collar, borehole.Target) as IList<SurveyPointDTO>;

            foreach (var point in borehole.SurveyPoints)
            {
                point.Id = Guid.NewGuid().ToString();
            }

            return borehole;
        }

        public BoreholeDTO CreateCurvedBorehole(string name, CollarDTO collar, TargetDTO target, double azimuthChange, double dipChange)
        {
            var borehole = new BoreholeDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Collar = collar,
                Target = target,
                ProjectId = collar.ProjectId,
            };

            var curveExtrapolationCalculator = new CurveExtrapolationCalculator(collar, target, azimuthChange, dipChange);
            borehole.Collar.Azimuth = curveExtrapolationCalculator.GetInitialAzimuth();
            borehole.Collar.Dip = curveExtrapolationCalculator.GetInitialDip();

            borehole.SurveyPoints = this.extrapolator.GetCurvedExtrapolaton(borehole.Collar, azimuthChange, dipChange);

            foreach (var point in borehole.SurveyPoints)
            {
                point.Id = Guid.NewGuid().ToString();
            }

            return borehole;
        }
    }
}
