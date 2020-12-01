namespace TargetHound.DTOs
{
    using TargetHound.DataModels.Interfaces;

    public class SurveyPointDTO : IPoint
    {
        public double Depth { get; set; }
        
        public double Azimuth { get; set; }
        
        public double Dip { get; set; }
        
        public double Easting { get; set; }
        
        public double Northing { get; set; }
        
        public double Elevation { get; set; }
    }
}