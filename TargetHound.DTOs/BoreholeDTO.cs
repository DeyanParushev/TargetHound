namespace TargetHound.DTOs
{
    using System.Collections.Generic;

    public class BoreholeDTO
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
