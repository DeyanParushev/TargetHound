namespace TargetHound.Blazor.Services
{
    using TargetHound.DTOs;

    public class StateService : IStateService
    {
        public string UserId { get; set; }

        public ProjectDTO Project { get; set; }
    }
}
