namespace TargetHound.Blazor.Services
{
    using TargetHound.DTOs;

    public interface IObjectCreator
    {
        public CollarDTO CreateCollar(string name, double easting, double norhing, double elevation, string projectId);

        public BoreholeDTO CreateCurvedBorehole(string name, CollarDTO collar, TargetDTO target, double azimuthChange, double dipChange);

        public BoreholeDTO CreateStraightBorehole(string name, CollarDTO collar, TargetDTO target);

        public TargetDTO CreateTarget(string name, double easting, double norhing, double elevation, string projectId);
    }
}
