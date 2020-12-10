using TargetHound.DTOs;

namespace TargetHound.Blazor.Services
{
    public interface IStateService
    {
        public string UserId { get; set; }
        
        public ProjectDTO Project { get; set; }
    }
}
