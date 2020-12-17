namespace TargetHound.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class DrillRigDTO : IMapFrom<DrillRig>, IMapTo<DrillRig>
    {
        public DrillRigDTO()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Name must be between 3 and 20 characters.")]
        [MinLength(3, ErrorMessage = "Name must be between 3 and 20 characters.")]
        [MaxLength(20, ErrorMessage = "Name must be between 3 and 20 characters.")]
        public string Name { get; set; }
    }
}