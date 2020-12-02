namespace TargetHound.DTOs
{
    using System.Collections.Generic;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class BoreholeDTO : IMapFrom<Borehole>
    {
        public BoreholeDTO()
        {
            this.SurveyPoints = new List<SurveyPointDTO>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public ContractorDTO Contractor { get; set; }

        public string ProjectId { get; set; }

        public CollarDTO Collar { get; set; }

        public TargetDTO Targets { get; set; }

        public IList<SurveyPointDTO> SurveyPoints { get; set; }
    }
}
