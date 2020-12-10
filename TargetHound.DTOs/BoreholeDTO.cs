namespace TargetHound.DTOs
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class BoreholeDTO : IMapFrom<Borehole>
    {
        public BoreholeDTO()
        {
            this.SurveyPoints = new List<SurveyPointDTO>();
        }

        public string Id { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Name has to be between 5 and 20 characters!")]
        [MaxLength(20, ErrorMessage = "Name has to be between 5 and 20 characters@")]
        public string Name { get; set; }

        public ContractorDTO Contractor { get; set; }

        public CollarDTO Collar { get; set; }

        public TargetDTO Target { get; set; }

        public IList<SurveyPointDTO> SurveyPoints { get; set; }
    }
}
