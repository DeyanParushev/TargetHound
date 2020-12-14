namespace TargetHound.DTOs
{
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class SurveyPointDTO : IPoint, IMapFrom<SurveyPoint>, IMapTo<SurveyPoint>
    {
        public string Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Depth must be a possitive number.")]
        public double Depth { get; set; }
        
        [Range(0, 360, ErrorMessage = "Azimuth must be between 0 and 360 degrees.")]
        public double Azimuth { get; set; }
        
        [Range(-90, 90, ErrorMessage = "Dip must be bewtween -90 and 90 degrees.")]
        public double Dip { get; set; }
        
        public double Easting { get; set; }
        
        public double Northing { get; set; }
        
        public double Elevation { get; set; }
    }
}