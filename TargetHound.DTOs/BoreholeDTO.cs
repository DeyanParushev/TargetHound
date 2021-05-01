namespace TargetHound.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class BoreholeDTO : IMapFrom<Borehole>, IMapTo<Borehole>, IEquatable<BoreholeDTO>
    {
        public BoreholeDTO()
        {
            this.SurveyPoints = new List<SurveyPointDTO>();
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Name has to be between 5 and 20 characters!")]
        [MaxLength(20, ErrorMessage = "Name has to be between 5 and 20 characters@")]
        public string Name { get; set; }

        public string ProjectId { get; set; }

        public ContractorDTO Contractor { get; set; }

        public CollarDTO Collar { get; set; }

        public TargetDTO Target { get; set; }

        public IList<SurveyPointDTO> SurveyPoints { get; set; }

        public bool Equals(BoreholeDTO other)
        {
            if (this.Collar == other.Collar && this.Target == other.Target)
            {
                return true;
            }

            return false;
        }
    }
}
