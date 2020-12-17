namespace TargetHound.Blazor.Services
{
    using System;
    using TargetHound.DTOs;

    public interface IStateService
    {
        public string UserId { get; set; }

        public ProjectDTO Project { get; set; }

        public bool IsInitialLoading { get; set; }

        public string BoreholeToDisplayName { get; set; }
    }
}
