namespace TargetHound.SharedViewModels.ViewModels
{
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class DrillRigViewModel : IMapFrom<DrillRig>
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
    }
}
