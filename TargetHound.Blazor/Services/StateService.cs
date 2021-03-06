﻿namespace TargetHound.Blazor.Services
{
    using System;
    using TargetHound.DTOs;

    public class StateService : IStateService
    {
        public string UserId { get; set; }

        public ProjectDTO Project { get; set; }

        public bool IsInitialLoading { get; set; } = true;

        public string BoreholeToDisplayName { get; set; }

        public bool IsNewProject { get; set; } = false;
    }
}
