﻿namespace TargetHound.MVC.Models.ViewModels
{
    using TargetHound.Models;
    using TargetHound.Services.Automapper;

    public class BoreholeViewModel : IMapFrom<Borehole>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
