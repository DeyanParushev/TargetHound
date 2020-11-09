using System.Threading.Tasks;

namespace TargetHound.Services.Interfaces
{
    public interface IPointService
    {
        public Task<bool> AddPoint(string boreholeId, double depth, double azimuth, double dip);
        
        public Task<bool> ChangePointAzimuth(string pointId, double newAzimuth);
        
        public Task<bool> ChangePointDepth(string pointId, double newDepth);
        
        public Task<bool> ChangePointDip(string pointId, double newDip);
        
        public Task<bool> DeletePoint(string pointId);
    }
}
