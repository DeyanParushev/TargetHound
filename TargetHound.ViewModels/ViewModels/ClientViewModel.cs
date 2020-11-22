﻿namespace TargetHound.SharedViewModels.ViewModels
{
    using TargetHound.Models;
    using TargetHound.Services.Automapper;

    public class ClientViewModel : IMapFrom<Client>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
