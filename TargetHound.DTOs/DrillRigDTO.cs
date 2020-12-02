namespace TargetHound.DTOs
{
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class DrillRigDTO : IMapFrom<DrillRig>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}