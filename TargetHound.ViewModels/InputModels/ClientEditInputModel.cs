﻿namespace TargetHound.SharedViewModels.InputModels
{
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class ClientEditInputModel : IMapFrom<Client>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
